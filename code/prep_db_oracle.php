<?php

putenv("NLS_LANG=American_America.UTF8");
$config = parse_ini_file("config.ini");
putenv("ORACLE_HOME=" . $config['oracle_home']);
putenv("LD_LIBRARY_PATH=" . $config['library_path']);
putenv("TNS_ADMIN=" . $config['tns_admin']);

function getConnection($ptype) {
    $config = parse_ini_file("config.ini");
    $user = $config[$ptype . '_dbuser'];
    $passwd = $config[$ptype . '_dbpasswd'];
    $conn = $config[$ptype . '_conn'];
    $db = oci_connect($user, $passwd, $conn);

    if (!$db) {
        trigger_error("Could not connect to OATS database", E_USER_ERROR);
    }
    return $db;
}

function getSurfboardConnection() {
    return getConnection('surf');
}

?>
