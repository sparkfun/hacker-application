from app import app, db
from helpers import clean_phone_number
from datetime import datetime
from sqlalchemy.orm.exc import NoResultFound
from sqlalchemy.orm.exc import MultipleResultsFound


class Peep(db.Model):
    id = db.Column(db.Integer, autoincrement=True, primary_key=True)
    # attributes are only populated if the peep is also a registered user
    first_name = db.Column(db.String(255), index=True)
    middle_name = db.Column(db.String(255), index=True)
    last_name = db.Column(db.String(255), index=True)
    birth_date = db.Column(db.DateTime)
    address = db.Column(db.String(2048))
    city = db.Column(db.String(255))
    state = db.Column(db.String(16))
    post_code = db.Column(db.String(16))
    affiliation = db.Column(db.String(255))
    gender = db.Column(db.Enum('Male', 'Female', name='gender_enum'))
    relationship_status = db.Column(db.Enum('Single', 'Casual', 'Dating',
                                            'Hitched', 'Married', 'Separated', 'Divorced',
                                            name='relationship_status_enum'))
    job_title = db.Column(db.String(255))
    school = db.Column(db.String(255))
    children = db.Column(db.String(255))

    created = db.Column(db.DateTime, default=datetime.utcnow())
    edited = db.Column(db.DateTime, onupdate=datetime.utcnow())

    # a one to one relationship with the user/auth class
    registration = db.relationship('User', backref=db.backref('peep'), uselist=False)

    # join to get the user unique name class
    contacts_names = db.relationship("ContactName", primaryjoin="ContactName.peep_id==Peep.id")

    # self referential contacts through peeps_contacts
    contacts = db.relationship("Peep",
                               secondary="peeps_contacts",
                               backref=db.backref("contacted_by"),
                               primaryjoin="Peep.id==PeepsContacts.peepa_id",
                               secondaryjoin="Peep.id==PeepsContacts.peepb_id")

    def has_contact(self, contact):
        for c in self.contacts:
            if c.id == contact.id:
                return True
        return False

    def contact_name(self, contact):
        for cn in self.contacts_names:
            if cn.contact.id == contact.id:
                return cn
        return None

    def contacts_in_common(self, op):
        my_contact_ids = set([c.id for c in self.contacts])
        common_contacts = [c for c in op.contacts if c.id in my_contact_ids]
        return common_contacts

    @classmethod
    def by_phone(cls, phone):
        try:
            p = db.session.query(cls, PeepPhone).join('peep_phones').filter_by(phone=phone).one()
            return p[0]
        except NoResultFound, ex:
            app.logger.exception("Peep.by_phone: No PeepPhone found", ex)
            return None
        except MultipleResultsFound, ex:
            app.logger.exception("Peep.by_phone: Multiple PeepPhones found", ex)

    @classmethod
    def by_email(cls, email):
        try:
            e = db.session.query(cls, PeepEmail).join('peep_emails').filter_by(email=email).one()
            return e[0]
        except NoResultFound, ex:
            app.logger.exception("Peep.by_email: No PeepEmail found", ex)
            return None
        except MultipleResultsFound, ex:
            app.logger.exception("Peep.by_email: Multiple PeepEmails found", ex)


class ContactName(db.Model):
    id = db.Column(db.Integer, autoincrement=True, primary_key=True)
    # attributes are populated using the contacts on users phone
    mobile_id = db.Column(db.Integer, index=True)
    display_name = db.Column(db.String(255))
    first_name = db.Column(db.String(255), index=True)
    middle_name = db.Column(db.String(255), index=True)
    last_name = db.Column(db.String(255), index=True)
    visible = db.Column(db.Boolean, default=True)

    peep_id = db.Column(db.Integer, db.ForeignKey('peep.id'))
    contact_id = db.Column(db.Integer, db.ForeignKey('peep.id'))

    peep = db.relationship("Peep", foreign_keys=[peep_id])
    contact = db.relationship("Peep", foreign_keys=[contact_id])


class PeepEmail(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    peep_id = db.Column(db.Integer, db.ForeignKey('peep.id'))
    peep = db.relationship('Peep', backref=db.backref('peep_emails'))
    email = db.Column(db.String(160), unique=True, nullable=False)

    def __init__(self, email):
        self.email = str(email).strip()


class PeepPhone(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    peep_id = db.Column(db.Integer, db.ForeignKey('peep.id'))
    peep = db.relationship('Peep', backref=db.backref('peep_phones'))
    phone = db.Column(db.String(16), unique=True, nullable=False)

    def __init__(self, phone):
        self.phone = clean_phone_number(str(phone).strip())


class PeepsContacts(db.Model):
    peepa_id = db.Column(
        db.Integer, db.ForeignKey('peep.id'), primary_key=True)
    peepb_id = db.Column(
        db.Integer, db.ForeignKey('peep.id'), primary_key=True)
