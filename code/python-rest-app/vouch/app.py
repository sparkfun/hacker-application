import os
import logging
from flask import Flask
from flask.ext.cors import CORS
from flask_sqlalchemy import SQLAlchemy
from flask_marshmallow import Marshmallow


# create flask application from config object
app = Flask(__name__)
if not os.environ.get('APP_SETTINGS'):
    app.config.from_object('config.DevelopmentConfig')
else:
    app.config.from_object(os.environ['APP_SETTINGS'])
    app.config.update(SECRET_KEY=os.environ['SECRET_KEY'])
    app.config.update(SQLALCHEMY_DATABASE_URI=os.environ['DATABASE_URL'])

# initialize sqlalchemy, marshmallow and CORS
db = SQLAlchemy(app)
ma = Marshmallow(app)
CORS(app)

# set logging output to stdout (heroku requirement)
stream_handler = logging.StreamHandler()
if app.debug:
    stream_handler.setLevel(logging.DEBUG)
else:
    stream_handler.setLevel(logging.WARN)
stream_handler.setFormatter(logging.Formatter('%(asctime)s %(levelname)s: %(message)s [in %(pathname)s:%(lineno)d]'))
app.logger.addHandler(stream_handler)
