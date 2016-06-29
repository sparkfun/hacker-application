import hashlib
from app import app, db
from datetime import datetime
from flask.ext.login import LoginManager, UserMixin
from itsdangerous import URLSafeTimedSerializer
from marshmallow import Schema, fields, ValidationError, post_load

# setup login_serializer with application secret key
login_serializer = URLSafeTimedSerializer(app.secret_key)

# setup login manager for flask-login
login_manager = LoginManager()
login_manager.init_app(app)
login_manager.session_protection = "Strong"


def hash_pass(password):
    """
    Return the hash of the password+salt
    """
    salted_password = str(password).strip() + app.secret_key
    return str(hashlib.md5(salted_password).hexdigest())


class EmailField(fields.Field):
    """
    Email field 'type' for validations
    """
    def _serialize(self, value, attr, obj):
        if not value or '@' not in value:
            raise ValidationError('Email address is not in correct format.')
        return str(value).strip()


class PasswordField(fields.Field):
    """
    Password field 'type' for validations'
    """
    def _serialize(self, value, attr, obj):
        if not value or len(value) < 8:
            raise ValidationError('Password must be longer than 8 characters.')
        return str(value).strip()


class User(db.Model, UserMixin):
    """
    User model using Flask-Login UserMixin
    """
    id = db.Column(db.Integer, autoincrement=True, primary_key=True)
    email = db.Column(db.String(160), index=True, unique=True, nullable=False)
    phone = db.Column(db.String(16), unique=True)
    password = db.Column(db.String(256))
    created = db.Column(db.DateTime, default=datetime.utcnow())
    edited = db.Column(db.DateTime, onupdate=datetime.utcnow())
    peep_id = db.Column(db.Integer, db.ForeignKey('peep.id'))

    def __init__(self, email, password):
        self.email = str(email).strip()
        self.password = str(password).strip()

    def get_auth_token(self):
        # encode secure cookie token
        data = [self.email, self.password]
        return login_serializer.dumps(data)


class UserSchema(Schema):
    """
    User model serializer
    """
    email = EmailField()
    password = PasswordField()

    @post_load
    def make_user(self, data):
        return User(**data)

    class Meta:
        fields = ('peep_id', 'id', 'email', 'phone', 'password')


@login_manager.user_loader
def load_user(user_id):
    """
    Flask-Login user loader
    """
    return User.query.get(user_id)


@login_manager.token_loader
def load_token(token):
    """
    Flask-Login token loader
    """
    # set max age of token cookie
    max_age = app.config["REMEMBER_COOKIE_DURATION"].total_seconds()
    # decrypt the security token, data = [email, hash_pass]
    data = login_serializer.loads(token, max_age=max_age)
    # find our user verify they exist and return them
    return User.query.filter_by(email=data[0], password=data[1]).one()
