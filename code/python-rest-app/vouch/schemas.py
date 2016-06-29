from app import ma
from marshmallow import fields


class PeepSchema(ma.ModelSchema):
    display_name = fields.Method("format_name")
    registered_email = fields.Method("user_email")
    registered_phone = fields.Method("user_phone")
    emails = fields.Method("peep_emails")
    phones = fields.Method("peep_phones")
    preferences = fields.Nested('PeepPreferenceSchema', many=True)
    created = fields.DateTime(dump_only=True)
    edited = fields.DateTime(dump_only=True)
    registered = fields.Method("peep_registered")

    def peep_registered(self, peep):
        if peep.registration:
            return True
        else:
            return False

    def format_name(self, peep):
        fn = (peep.first_name if peep.first_name is not None else '')
        ln = (peep.last_name if peep.last_name is not None else '')
        dn = (fn + " " + ln).strip()
        return "%s" % (dn)

    def user_email(self, peep):
        if peep.registration:
            return "%s" % (peep.registration.email)

    def user_phone(self, peep):
        if peep.registration:
            return "%s" % (peep.registration.phone)

    def peep_emails(self, peep):
        emails = [str(e.email) for e in peep.peep_emails]
        return emails

    def peep_phones(self, peep):
        phones = [str(p.phone) for p in peep.peep_phones]
        return phones

    class Meta:
        fields = ('id', 'first_name', 'middle_name', 'last_name', 'birth_date',
                'address', 'city', 'state', 'post_code', 'affiliation',
                'gender', 'relationship_status', 'job_title', 'school',
                'children', 'created', 'edited', 'registered_email',
                'registered_phone', 'emails', 'phones', 'preferences',
                'display_name', 'registered')


class ContactsSchema(ma.ModelSchema):
    peeps = fields.Nested('ContactNameSchema', many=True, attribute="contacts_names")
    class Meta:
        fields = ('peeps',)


class ContactNameSchema(ma.ModelSchema):
    registered = fields.Method("peep_registered")
    id = fields.Int(dump_only=True, attribute="contact_id")

    def peep_registered(self, contact_name):
        if contact_name.contact.registration:
            return True
        else:
            return False

    class Meta:
        fields = ('mobile_id', 'id', 'visible', 'display_name', 'registered')
