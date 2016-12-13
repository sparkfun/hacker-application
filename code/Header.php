<?php
//
// Header.php
// 
// This class is responsible to render header portion of every page.
//
class Header {

    public $parameters = array(
                               'main_title' => 'Surfboard',
                               'help_page' => '',
                               'page' => '',
                               'param_str' => '',
                               'sid' => '',
                               'sname' => '',
                               'allow_edit' => false, // does user's role allow them to edit
                               'wptype' => '',
                               );

    
    // get the logouot URL
    public function ssoLogoutURL() {
        $config = parse_ini_file("config.ini");
        $logout_link = $config['logout_link'];
      
        $redirect_url = $_SERVER['PHP_SELF'] . '?' . urlencode($this->parameters["param_str"]);
        $DONE_URL = "https://" . $_SERVER["HTTP_HOST"] . $redirect_url;
        $SSO_LOGOUT_URL = $logout_link . $DONE_URL;
        return $SSO_LOGOUT_URL;
    }
   
    public function setHeaderParameters($parameters) {
        foreach ($parameters as $key => $value) {
            $this->parameters[$key] = $value;
        }
    }

    // display page title
    public function displayPageTitle($break) {
        $title = $this->parameters["main_title"];  
        print $title;
    }
    
    // Get user's email who is logged in.  Currently hard-coded for a demo purpose.
    public function displayLoginLink() {
        $email = getUserEmail();
        print "<a class=\"coltitle_link\" href=\"" . $this->ssoLogoutURL() . "\">\n";
        print "  Logout ";
        print "</a>\n";
        print "<span class=\"coltitle\">" . $email . "</span>\n";
        print "&nbsp;&nbsp;\n";
        
    }
    // Display user's timezone.  Currently hard-coded for a demo purpose.
    public function displayTimezone() {
        
        $offset = '-07:00';
        $user_timezone = getDisplayTimezone($offset);
        print 'Timezone: ' . $user_timezone . ' (GMT ' . $offset . ')';
    }

    // Display database name 
    public function displayDatabaseName() {
        $config = parse_ini_file("config.ini");
        print $config['surf_name'];
    }
   
    
    public function displayTabs() {
        print '<table id="surfboard_tab_table" style="border-bottom:1px solid gray" border="0" cellpadding="0" cellspacing="0" align="left" width="100%">'."\n";
        print "<tr>\n";


        $tab_width = "width: 230px; ";
        $tabs = array();
        
        $tabs["mysystem"] = "My Systems";
        $tabs["myres"] = "My Reservations";
        $tabs["mgmt"] = "Management";
        $tabs["avail"] = "Availability";
        $tabs["report"] = "Reporting";
        $tabclass = array("suntab", "suntabactive");

        $active_tab = $this->parameters["wptype"];
        $top_active_tab = $active_tab;
        
        if (startsWith($active_tab, 'sys_')) {
            $top_active_tab = 'mysystem';
        }
        if (startsWith($active_tab, 'mgmt')) {
            $top_active_tab = 'mgmt';
        }
        if (startsWith($active_tab, 'avail')) {
            $top_active_tab = 'avail';
        }
        foreach ($tabs as $tab_id => $tab_label) {
            $active = $tabclass[0];
            if ($top_active_tab == $tab_id) {
                $active = $tabclass[1];
            }
            print "<td class=\"" . $active . "\" id=\"" . $tab_id . "\" style=\"" . $tab_width . " vertical-align: bottom; text-align: center;\" onmousedown=\"surfboard_switch_tab('$tab_id')\">";
            print "  <div>";
            print "    <span class=" . $active . "_label>";
            print "$tab_label</span>";
            print '  </div>';
            print '</td>';
        }
        print '<td></td>';
        print "</tr>\n";

        print "</table>\n";
        
        print "<br/>\n";
        if (startsWith($active_tab, 'sys_')) {
            $this->displaySystemSubTabs();
        }
        if (startsWith($active_tab, 'mgmt')) {
            $this->displayManageSubTabs();
        }
        if (startsWith($active_tab, 'avail')) {
            $this->displayAvailSubTabs();
        }
    }
    
    public function displaySystemSubTabs() {

        print '<table id="surfboard_subtab_table" style="border-bottom:1px solid gray" border="0" cellpadding="0" cellspacing="0" align="left" width="100%">'."\n";
        print "<tr>\n";


        $tab_width = "width: 200px; ";
        $tabs = array();
        
        $tabs["sys_info"] = "System Info";
        $tabs["sys_monitor"] = "Monitoring";
        $tabs["sys_res"] = "Reservations";
        $tabs["sys_activity"] = "Recent Activity";
        
        $tabclass = array("suntab", "suntabactive");

        $active_tab = $this->parameters["wptype"];
        $sid = $this->parameters["sid"];
        foreach ($tabs as $tab_id => $tab_label) {
            $active = $tabclass[0];
            if ($active_tab == $tab_id) {
                $active = $tabclass[1];
            }
            print "<td class=\"" . $active . "\" id=\"" . $tab_id . "\" style=\"" . $tab_width . " vertical-align: bottom; text-align: center;\" onmousedown=\"surfboard_switch_subtab('$tab_id', '$sid')\">";
            print "  <div>";
            print "    <span class=" . $active . "_label>";
            print "$tab_label</span>";
            print '  </div>';
            print '</td>';
        }
        print '<td></td>';
        print "</tr>\n";

        print "</table>\n";
    }
    
}

?>
