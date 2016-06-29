import json
from app import app, db
from auth import User, UserSchema, hash_pass
from models import Peep, PeepEmail, PeepsContacts, ContactName, PeepPhone
from schemas import PeepSchema, ContactsSchema
from flask import jsonify, request
from flask.ext.login import login_user, logout_user, current_user, login_required
from rq import Queue
from rq.job import Job
from worker import conn
from datetime import datetime
from helpers import clean_phone_number

# queue manager for background worker task management
# manages the process of syncing all contacts of user
q = Queue(connection=conn)


@app.route('/logout', methods=['GET'])
def logout():
    try:
        logout_user()  # destroy the token and log the user out
        return jsonify({'message': 'Logged out'}), 200
    except Exception as err:
        app.logger.error('routes.logout() [exception]: ' + err.message)
        return jsonify({'message': err.message}), 400


@app.route('/login', methods=['POST', 'GET'])
def login(remember_me=True):
    try:
        serializer = PeepSchema()
        if request.method == "POST":
            # check for the presence of an email and password
            if 'email' not in request.form:
                return jsonify({'message': 'Email is required'}), 400
            if 'password' not in request.form:
                return jsonify({'message': 'Password is required'}), 400
            password = hash_pass(request.form['password'])
            # determine if a user meets the login criteria
            user = User.query.filter_by(email=request.form['email'], password=password).all()
            if len(user) > 0:
                # set the status of the login cookie
                if 'remember_me' in request.form:
                    remember_me = bool(request.form['remember_me'])
                # log the user in and return their peep data
                login_user(user[0], remember=remember_me)
                return jsonify(serializer.dump(current_user.peep).data), 200
            return jsonify({'message': 'Authentication failed'}), 400
        if request.method == "GET":
            if current_user.is_authenticated:
                return jsonify(serializer.dump(current_user.peep).data), 200
            return jsonify({'message': 'Authentication failed'}), 400
    except Exception as err:
        app.logger.error('routes.login(' + remember_me + ') [exception]: ' + err.message)
        return jsonify({'message': err.message}), 400


@app.route('/signup', methods=['POST'])
def signup(remember_me=True):
    success = False
    new_user = None
    try:
        # check for an email
        if 'email' not in request.form:
            return jsonify({'message': 'No email provided'}), 400
        email = request.form['email']
        # validate email is not already registered
        cu = User.query.filter_by(email=email).all()
        if (len(cu) > 0):
            return jsonify({'message': 'Email is already registered'}), 400
        # a user is already logged in deny registration
        if not current_user.is_anonymous:
            return jsonify({'message': 'Existing user currently logged in'}), 400
        # make sure a password was sent to register with
        if 'password' not in request.form:
            return jsonify({'message': 'No password provided'}), 400
        password = request.form['password']
        # check if the user would like to stay logged in
        if 'remember_me' in request.form:
            remember_me = bool(request.form['remember_me'])

        # generate our new user now and validate with marshmallow
        new_user = User(email=email, password=password)
        user_serialized = UserSchema().dumps(new_user)
        if len(user_serialized.errors) > 0:
            app.logger.error('routes.signup() [exception]: ' + str(user_serialized.errors[0]))
            return jsonify({'message': str(user_serialized.errors[0])}), 400

        # store the new user to database after hashing their password
        new_user.password = hash_pass(new_user.password)
        db.session.add(new_user)
        db.session.commit()
        login_user(new_user, remember=remember_me)  # authenticate our user
        success = True  # bool to rollback user creation if peep creation fails

        # find peep or create a new peep and affiliate them with user peep
        peep = Peep.by_email(email)

        # TODO: add validations | reduce redundancy of updates when not needed
        if not peep:
            peep = Peep()  # create a new peep
            # create a new peep_email for this peep
            pmail = PeepEmail(email)
            pmail.peep = peep
            # assign all default peep preferences
            PeepPreference(name='post_to_stream', peep=peep, value=u'True')
            PeepPreference(name='allow_comments', peep=peep, value=u'True')
            PeepPreference(name='allow_love_hate', peep=peep, value=u'True')
            PeepPreference(name='photo_public', peep=peep, value=u'True')

        peep.registration = new_user
        peep.first_name = request.form['first_name']
        peep.last_name = request.form['last_name']
        peep.gender = request.form['gender']
        # convert javascript/json isostring into python utc date
        peep.birth_date = datetime.strptime(request.form['birth_date'], '%Y-%m-%dT%H:%M:%S.%fZ')

        peep_serialized = PeepSchema().dump(peep)
        if len(peep_serialized.errors) > 0:
            logout_user()  # logout the user as peep creation failed
            db.session.delete(new_user)  # delete the user as well
            db.session.commit()
            app.logger.error('routes.signup() [exception]: ' + str(peep_serialized.errors[0]))
            return jsonify({'message': str(peep_serialized.errors[0])}), 400

        db.session.commit()
        # return the signed cookie token and user profile as json
        return jsonify(peep_serialized.data)
    except Exception as err:
        if (success):  # user was created but peep failed, remove user from db
            logout_user()
            db.session.delete(new_user)
            db.session.commit()
        app.logger.error('routes.signup() [exception]: ' + err.message)
        return jsonify({'message': err.message}), 400


@app.route('/api/sync', methods=['POST'])
@login_required
def mobile_sync():
    try:
        # queue contact sync for background worker processing
        contacts = json.loads(request.data)
        job = q.enqueue_call(func=sync_contacts, args=(current_user.peep.id, contacts,), result_ttl=5000)
        return jsonify({'job_id': job.get_id()}), 200
    except Exception as err:
        app.logger.error('routes.mobile_sync() [exception]: ' + err.message)
        return jsonify({'message': err.message}), 400


@app.route('/api/sync/<string:job_key>', methods=['GET'])
@login_required
def sync_results(job_key):
    try:
        job = Job.fetch(job_key, connection=conn)
        if job.is_finished:
            serializer = ContactsSchema()
            # return json of all current_users contacts (using local names)
            return jsonify(serializer.dump(current_user.peep).data), 200
        else:
            return jsonify({'status': 'processing'}), 202
    except Exception as err:
        app.logger.error('routes.sync_results() [exception]: ' + err.message)
        return jsonify({'message': err.message}), 400


# used only by the web view | mobile is bootstrapped with above sync functions
@app.route('/api/peep', methods=['GET'])
@login_required
def peeps():
    """ returns a list of all the current_user contacts using local names """
    try:
        serializer = ContactsSchema()
        return jsonify(serializer.dump(current_user.peep).data), 200
    except Exception as err:
        app.logger.error('routes.peeps() [exception]: ' + err.message)
        return jsonify({'message': err.message}), 400


@app.route('/api/peep/<int:id>', methods=['GET'])
@login_required
def peep(id):
    try:
        # determine if the current_user is fetching their own info ie. my_profile
        if (int(current_user.peep.id) == int(id)):
            serializer = PeepSchema()
            return jsonify(serializer.dump(current_user.peep).data), 200
        # verify the peep is a contact of the current_user (has rights to view)
        peep = Peep.query.get(id)
        if (peep is None):
            return jsonify({'message': 'No such peep: id:' + str(id)}), 404
        if current_user.peep.has_contact(peep):
            # only include fairly generic information
            serializer = PeepSchema(only=('id', 'first_name', 'middle_name',
                'last_name', 'birth_date', 'city', 'state', 'affiliation',
                'gender', 'relationship_status', 'job_title', 'created',
                'edited', 'preferences', 'display_name', 'registered'))
            # get the contact_name model for this contact and use current_users local_name
            cn = current_user.peep.contact_name(peep)
            if (cn is not None):
                peep.first_name = cn.first_name
                peep.middle_name = cn.middle_name
                peep.last_name = cn.last_name
            return jsonify(serializer.dump(peep).data), 200
        else:
            return jsonify({'message': 'Peep is not a contact of the current user: id:' + str(id)}), 403
    except Exception as err:
        app.logger.error('routes.peep(' + str(id) + ') [exception]: ' + err.message)
        return jsonify({'message': err.message}), 400


# all assistance functions are below this point
def update_contact(user_peep_id, peep_set, emails, phones, contact):
    """ update a contact of the current user """
    peep = peep_set.pop()  # there is only one item in the set
    # check if this contact is the current user
    if (user_peep_id != peep.id):
        # affiliate this contact with the current user
        user_peep = Peep.query.get(user_peep_id)
        # generate a contact name for this peep
        contact_name = ContactName(
                mobile_id=contact['mobile_id'],
                first_name=contact['first_name'],
                middle_name=contact['middle_name'],
                last_name=contact['last_name'],
                display_name=contact['display_name'],
                peep=user_peep,
                contact=peep)
        # check if they are contact of current user
        if not user_peep.has_contact(peep):
            # add this peep as a contact of the user
            peeps_contacts = PeepsContacts(peepa_id=user_peep.id, peepb_id=peep.id)
            db.session.add(peeps_contacts)
            db.session.add(contact_name)
            db.session.commit()
        else:
            # TODO: add check instead of redundant assignment
            pcontact_name = user_peep.contact_name(peep)
            pcontact_name = contact_name

    for email in emails:
        pemails = PeepEmail.query.filter_by(email=email).all()
        if len(pemails) == 0:
            pemail = PeepEmail(email)
            pemail.peep = peep
            db.session.add(pemail)
        else:
            pemail = pemails[0]
            if not (pemail.peep):
                pemail.peep = peep  # assign our peep to this email
            else:
                if pemail.peep_id != peep.id:
                    raise Exception('routes.update_contact() [exception]: ' +
                            str(pemail.email) + ' peep_email id: ' +
                            str(pemail.id) + ' is already affiliated with peep ' +
                            str(pemail.peep_id))

    for phone in phones:
        pphones = PeepPhone.query.filter_by(phone=phone).all()
        if len(pphones) == 0:
            pphone = PeepPhone(phone)
            pphone.peep = peep
            db.session.add(pphone)
        else:
            pphone = pphones[0]
            if not (pphone.peep):
                pphone.peep = peep  # assign our peep to this phone
            else:
                if pphone.peep_id != peep.id:
                    raise Exception('routes.update_contact() [exception]: ' +
                            str(pphone.phone) + ' peep_phone id: ' + str(pphone.id) +
                            ' is already affiliated with peep ' + str(pphone.peep_id))

    db.session.commit()
    return


def create_contact(user_peep_id, emails, phones, contact):
    # get the peep (current_user) performing the sync
    user_peep = Peep.query.get(user_peep_id)
    # no peep exists go ahead and create a new one
    peep = Peep()
    # set the default prefs
    PeepPreference(name='post_to_stream', peep=peep, value=u'True')
    PeepPreference(name='allow_comments', peep=peep, value=u'True')
    PeepPreference(name='allow_love_hate', peep=peep, value=u'True')
    PeepPreference(name='photo_public', peep=peep, value=u'True')

    # generate all emails and phone numbers
    for email in emails:
        pemails = PeepEmail.query.filter_by(email=email).all()
        if len(pemails) == 0:  # no peep_email yet exists
            pemail = PeepEmail(email)
            pemail.peep = peep
        else:  # check if a peep affiliation has yet been made
            pemail = pemails[0]
            if not (pemail.peep):
                pemail.peep = peep  # no affil, assign our peep to this email
            else:
                if pemail.peep_id != peep.id:
                    raise Exception('routes.create_contact() [exception]: ' + str(pemail.email) +
                            ' peep_email id: ' + str(pemail.id) +
                            ' is already affiliated with peep ' + str(pemail.peep_id))

    for phone in phones:
        pphones = PeepPhone.query.filter_by(phone=phone).all()
        if len(pphones) == 0:  # no peep_phone yet exists
            pphone = PeepPhone(phone)
            pphone.peep = peep
        else:  # check if a peep affiliation is made to peep_phone yet
            pphone = pphones[0]
            if not (pphone.peep):
                pphone.peep = peep  # no affil, assign our peep to this phone
            else:
                if pphone.peep_id != peep.id:
                    raise Exception('routes.create_contact() [exception]: ' + str(pphone.phone) +
                            ' peep_phone id: ' + str(pphone.id) +
                            ' is already affiliated with peep ' + str(pphone.peep_id))

    db.session.add(peep)
    db.session.commit()

    # generate the peeps contacts connection
    peeps_contacts = PeepsContacts(peepa_id=user_peep.id,  peepb_id=peep.id)
    db.session.add(peeps_contacts)

    # generate a contact name for the user of this contact
    contact_name = ContactName(
            mobile_id=contact['mobile_id'],
            first_name=contact['first_name'],
            middle_name=contact['middle_name'],
            last_name=contact['last_name'],
            display_name=contact['display_name'],
            peep=user_peep,
            contact=peep)

    db.session.add(contact_name)
    db.session.commit()
    return


def sync_contacts(user_peep_id, contacts):
    """ sync all contacts of the user with the database """
    for contact in contacts:
        t_peep = None  # temp peep for validations and checks
        peep_set = set()  # set to hold all matched peeps
        phone_set = set()  # all peep phones
        email_set = set()  # all peep emails
        # all mobile peep information
        contact_d = {'mobile_id': contact['id'],
                'first_name': contact['name'].get('givenName'),
                'middle_name': contact['name'].get('middleName'),
                'last_name': contact['name'].get('familyName'),
                'display_name': contact['displayName']}
        try:
            # check contact for a phone number, if not, discard this contact
            phoneNumbers = contact.get('phoneNumbers')
            if phoneNumbers is not None:
                # grab the emails and check if any of them are registered
                emails = contact.get('emails')
                if emails is not None:
                    for email in emails:
                        if email is not None:
                            email_set.add(str(email['value']).strip())
                            t_peep = Peep.by_email(str(email['value']).strip())
                            if (t_peep):
                                peep_set.add(t_peep)

                for phone in phoneNumbers:
                    if phone is not None:
                        # TODO: pass active user to validate area code if missing
                        c_phone = clean_phone_number(phone['value'])
                        if len(c_phone) == 11:
                            phone_set.add(c_phone)
                            t_peep = Peep.by_phone(c_phone)
                            if (t_peep):
                                peep_set.add(t_peep)

                if len(peep_set) > 1:
                    # multiple contacts meet the mobile criteria (sync failure)
                    p_list = []
                    for p in peep_set:
                        p_list.append('peep_id: ' + str(p.id) + ' ')
                    raise Exception('routes.sync_contacts() [exception]: mobile_info links multiple contacts: '
                            + ''.join(p_list) + ', sync not possible')
                elif len(peep_set) == 1:
                    # update peep or user info with mobile contact data
                    update_contact(user_peep_id, peep_set, list(email_set), list(phone_set), contact_d)
                else:
                    # make sure a phone or email exists for contact creation
                    if len(email_set) > 0 or len(phone_set) > 0:
                        create_contact(user_peep_id, list(email_set), list(phone_set), contact_d)

        except Exception as err:  # log errors and data, then soldier on...
            m_list = ['user_peep_id:' + str(user_peep_id) + ', ']
            m_list.append('mobile_contact:' + json.dumps(contact_d) + ', ')
            m_list.append('mobile_emails:[' + json.dumps(', '.join(email_set)) + '], ')
            m_list.append('mobile_phones:[' + json.dumps(', '.join(phone_set)) + '], ')
            e_set = set()
            p_set = set()
            for peep in peep_set:
                for peep_email in peep.peep_emails:
                    e_set.add(str(peep_email.email))
                for peep_phone in peep.peep_phones:
                    p_set.add(str(peep_phone.phone))
                m_list.append('matched_peep_id:' + str(peep.id) + ', ')
                m_list.append('matched_peep_emails:[' + json.dumps(', '.join(e_set)) + '], ')
                m_list.append('matched_peep_phones:[' + json.dumps(', '.join(p_set)) + ']')
            app.logger.error(err.message + ' [data]: {' + ''.join(m_list) + '}')
