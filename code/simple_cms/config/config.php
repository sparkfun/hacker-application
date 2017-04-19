<?php

$config['db_info']['driver']    = 'pdo_mysql';
$config['db_info']['host']      = 'localhost';
$config['db_info']['dbname']    = 'simple_cms';
$config['db_info']['user']      = 'root';
$config['db_info']['password']  = '';

$config['template']['base_url'] = '/cms/';

define('CONTACT_TO',        'email@test.com');
define('CONTACT_SUBJECT',   'SUBJECT GOES HERE');

// This is by no means a recommended best practice for authentication.
// In this case it was used for simplicity sake and normally would not
// be stored in a config file and would be one with hashed with a
// salt.
define('ADMIN_USER',        '');
define('ADMIN_PASS',        '');

?>
