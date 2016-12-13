<?php
/**
 *  SURFBOARD
 * 
 *  This is the main page to list all the systems available for the user.  
 *  The list can be sorted by columns.
 */
include 'Header.php';
include 'Utilities.php';

// ----- Display the Page Header
$page_header = New Header();   
$parameters = array("allow_edit" => false,
                    "help_page" => 'surfboard',
                    "wptype" => 'mysystem'
                    );
$page_header->setHeaderParameters($parameters);
include "Header.html";
include 'SystemDataHandler.php';
include 'SystemCommon.php';
include 'ManageSystemSectionBuilder.php';
include 'prep_db_oracle.php';

//-------------------------------- MAIN -----------------------------

$sort     = isset($_REQUEST['sort'])  ? $_REQUEST["sort"]  : "HW_NAME";
$desc     = isset($_REQUEST['desc'])  ? $_REQUEST["desc"]  : '0';
$action = isset($_REQUEST['action']) ? $_REQUEST['action']  : '';
//$db_oracle = getSurfboardConnection();
$userid = getUserEmail();
//$valid_user = validateUser($db_oracle, $userid);
$valid_user = true;

// always display in edit mode.
$mode = 'edit';

if ($valid_user == false) {
    // ----- Invalid user exit the program
    header("Location:NoAccess.html");
    exit();

}

print '<script language="JavaScript" src="./SgdAjaxBase.js"></script>';
print '<script language="JavaScript" src="./Utilities.js"></script>';
print '<script language="JavaScript" src="./ManageSystem.js"></script>';
print '<script language="JavaScript" src="./SystemCommon.js"></script>';
if ($mode == 'edit') {
    print '<script type="text/javascript">';
    print 'document.body.className = "page_edit";'."\n";
    print "</script>\n";
}
   
displayActions();

print "<form name=\"system_form\" method=\"POST\" action=\"".$_SERVER["PHP_SELF"]."\">\n";
print '<input type="hidden" id="action" name="action" value="">' . "\n";
print '<input type="hidden" id="server_ui_activate_sid" name="server_ui_activate_sid">';
print '<div class="ftc_action_menu" id="sort_action_menu" style="display: none;"></div>' . "\n";

// get the systems available for the user.
$system_arr = getSystemSummary($db_oracle, $sort, $desc, $userid);

// display the System Table
print "<table id=\"system_table\" border=\"1\" cellpadding=\"2\" width=\"100%\" style=\"border-collapse: collapse;\">\n";
displaySystemTable($sort, $desc, $system_arr);//, $user_prefs->preferences['show_inactive_systems']);
print "</table>\n";

print "</form>\n";

include "Footer.html";

?>

