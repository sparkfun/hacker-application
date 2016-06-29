from app import app, db, ma
from auth import *
from models import *
from routes import *
from schemas import *

if __name__ == '__main__':
    app.run()
