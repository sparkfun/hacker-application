<?php
//
// SystemCommon.php
// 
// This class contains some utility functions that are common in all pages.
//

function displayActions() {
    print "<table border=\"0\" width=\"100%\">\n";
    print "<tr>\n";
    print "<td>\n";
    // Used one the sustain/archive, edit project and add project web pages

    $actions = array(
                      "sep1" => "separator",
                      "create_system" => "Create a New System",
                      "manage_pools" => "Manage Pools",
                      "sep2" => "separator",
                      "availability" => "Search Availability",
                      "sep3" => "separator",
                      "view_reservation" => "View Reservations",
                      "view_system" => "View Systems",   
                      "sep4" => "separator",
                      "reports" => "Utilization Reports",
                      );

    print "<select id=\"system_action\" name=\"system_action\" size=\"1\" onChange=\"handleSurfboardAction(this)\">\n";
    print '<option value="">Actions</option>' . "\n";
    //print '<option value="" disabled>----------</option>' . "\n";
    foreach ($actions as $action_id => $action) {
        $disabled = '';
        //print '<option value="' . $action_id . '">' . $action . '</option>' . "\n";
        if ($action == 'separator') {
            $disabled = " disabled";
            $action_id = '';
            $action = '----------';
            //print "<optgroup label=\"----------\"></optgroup>\n";
        } 
        print '<option value="' . $action_id . '"' . $disabled . '>' . $action . '</option>' . "\n";
    }
    print "</select>";
    print "</td>\n";
    print "</tr>\n";
    print "</table>\n";
}

function displayErrorPopup($error_msg) {
    print '<script type="text/javascript">' . "\n";
    print 'window.alert("' . $error_msg . '");' . "\n";
    print '</script>' . "\n";
}

function displaySystemActionMenu($webpage, $sid) {
    print "<table border=\"0\" width=\"100%\">\n";
    print "<tr>\n";
    print "<td>\n";
    print "<select name=\"surfboard_system_actions\"  width=\"200\" style=\"width: 200px\" onclick=\"handleSystemAction(this, '" . $sid . "')\">\n";
    print "<option value=\"\">System Actions</option>\n";
    print "<option value=\"\" disabled>----------</option>\n";
    // webpage can be either 'system', 'activity', 'reservation'
    if ($webpage != 'system') {
        print "<option value=\"system\">System Management</option>\n";
    }
    if ($webpage != 'reservation') {
        print "<option value=\"reservation\">Reservation</option>\n";
    }
    if ($webpage != 'activity') {
        print "<option value=\"activity\">Activity Logs</option>\n";
    }
    
    print "<option value=\"\" disabled>----------</option>\n";
    print "</select>";
    print "</td>\n";
    print "</tr>\n";
    print "</table>\n";
}

?>

