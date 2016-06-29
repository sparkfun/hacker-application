import re


def clean_phone_number(phone, user_phone=None):
    """ clean a phone number for db storage, remove non-numerics and
    verify a country code -> TODO: Internationalize/area code cleanup """
    numerics = re.compile(r'[^\d.]+')
    c_phone = numerics.sub('', phone)
    if len(c_phone) == 10:
        # TODO: internationalize/user_phone
        return '1{0}'.format(c_phone)
    # TODO: use the user_phone number to determine area code if it is missing
    return c_phone
