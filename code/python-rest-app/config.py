import os
from datetime import timedelta


class Config(object):
    ADMINS = frozenset(['jpenka@gmail.com'])
    DEBUG = False
    TESTING = False
    CSRF_ENABLED = True
    CSRF_SESSION_KEY = os.urandom(24)
    DATABASE_CONNECT_OPTIONS = {}
    PERMANENT_SESSION_LIFETIME = timedelta(minutes=30)
    THREADS_PER_PAGE = 8
    REMEMBER_COOKIE_DURATION = timedelta(days=14)
    SECRET_KEY = 'silliesupursekrutkee'
    SQLALCHEMY_DATABASE_URI = 'postgresql://vouch:silliesupursekrutkee@localhost:5432/vouch'


class ProductionConfig(Config):
    DEBUG = False


class StagingConfig(Config):
    DEVELOPMENT = True
    DEBUG = True


class DevelopmentConfig(Config):
    DEVELOPMENT = True
    DEBUG = True


class TestingConfig(Config):
    TESTING = True
