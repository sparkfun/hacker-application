<?php

/*****
 * Example Include File
 *
 * This is a main include file that I wrote to allow for a central place to add
 * things like config vars, constants, includes for things like composer and
 * database connections.
 *
 * Thank you for your consideration!
 *****/

// Disable error reporting by default
ini_set('display_errors', 0);
error_reporting(0);

// Running on test domain, show errors by default
if (isset($_SERVER['HTTP_HOST'])) {
    if (substr($_SERVER['HTTP_HOST'], 0, strlen('comit-test')) == 'comit-test' ||
        substr($_SERVER['HTTP_HOST'], 0, strlen('comit-beta')) == 'comit-beta' ||
        substr($_SERVER['HTTP_HOST'], 0, strlen('comit-dev')) == 'comit-dev') {

        ini_set('display_errors', 1);
        error_reporting(E_ALL & ~E_DEPRECATED);

        define('TEST_SERVER', true);
    }
}

if (!defined('TEST_SERVER')) {
    define('TEST_SERVER', false);
}

// Paths
if (PHP_SAPI !== 'cli') {
    // Redirect to ssl versions
    if (trim(substr($_SERVER['HTTP_HOST'], -19)) == 'greekgods.com') {
        if (empty($_SERVER['HTTPS']) || $_SERVER['HTTPS'] == 'off') {
            header('Location: https://' . $_SERVER['HTTP_HOST'] . $_SERVER['REQUEST_URI']);
            exit();
        }
    }

    // Base URL
    if (isset($_SERVER['HTTPS']) && $_SERVER['HTTPS'] !== 'off' ? 's' : '') {
        $http_prefix = 'https://';
    } else {
        $http_prefix = 'http://';
    }

    define('BASE_URL', $http_prefix.$_SERVER['HTTP_HOST']);

    // Check if we are on a greekgods.com
    if (substr($_SERVER['HTTP_HOST'], strlen('greekgods.com')*-1) == 'greekgods.com') {
        // Set the cookies to the whole domain
        define('COOKIE_DOMAIN', '.greekgods.com');
    } else {
        // Set the cookies to only this domain
        define('COOKIE_DOMAIN', $_SERVER['HTTP_HOST']);
    }
} else {
    // Show errors in CLI
    ini_set('display_errors', 1);
    error_reporting(E_ALL & ~E_DEPRECATED);

    define('BASE_URL', 'https://comit.greekgods.com');
    define('COOKIE_DOMAIN', '.greekgods.com');
}

// General Constants
define('COOKIE_LIFETIME', strtotime(date('Y-m-d 02:00:00', strtotime("+1 day")))-time());
define('BASE_PATH', realpath(dirname(__FILE__).'/..'));
define('DOC_ROOT', realpath(dirname(__FILE__).'/../documentroot'));

// Check if on windows
if (strtoupper(substr(PHP_OS, 0, 3)) === 'WIN') {
    define('PHP_WIN', true);
} else {
    define('PHP_WIN', false);
}

// Set Include Path
set_include_path(get_include_path() . PATH_SEPARATOR . BASE_PATH . '/include');


// Base Requirements
require_once BASE_PATH.'/vendor/autoload.php';
require_once BASE_PATH.'/include/db.inc.php';
require_once BASE_PATH.'/include/mandrill.inc.php';

if (PHP_SAPI !== 'cli') {
    require_once BASE_PATH.'/include/error.inc.php';
    require_once BASE_PATH.'/include/session.inc.php';
}
