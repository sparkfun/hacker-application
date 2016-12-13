<?php
//
// SdgSystemName.php
// 
// This class is called via AJAX to check if the system being added has a unique
// name.  Return an error to be handled by javascript.

include 'prep_db_oracle.php';
include 'Utilities.php';

session_start();
header("Cache-control: private");	// IE6 fix

function returnSnameCheck($sname, $exist) {
    // This returns data via the AJAX interface to the pname_checkNewContent()
    // Javascript function
    global $SGD_ROW_DELIM;

    if ($exist == 'E') {
        $err_msg = 'The system name "' . $sname . '" already exists.  Please choose a different system name.';
        print "ERROR".$SGD_ROW_DELIM.$err_msg;
    } else {
        print $sname.$SGD_ROW_DELIM;
    }
}

$sname = isset($_REQUEST['sname']) ? $_REQUEST["sname"]   : "";

$db_oracle = getSurfboardConnection();
$sname_sql_str = 'select * from SURF_HARDWARE where HW_NAME = \''.$sname . '\'';
error_log($sname_sql_str);
$sname_sql = oci_parse($db_oracle, $sname_sql_str);
oci_execute($sname_sql);
$exist = "";
while ($returnval = oci_fetch_array($sname_sql, OCI_ASSOC+OCI_RETURN_NULLS+OCI_RETURN_LOBS)) {
    $rsname = $returnval['HW_NAME'];
    $exist = 'E';
    break;
}
oci_free_statement($sname_sql);
returnSnameCheck($sname, $exist);

?>
