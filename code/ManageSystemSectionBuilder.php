<?php
//
// ManageSystemSectionBuilder.php
// 
// This class is responsible to render html to the browser for the available systems.
//
/**
 * Display all the systems available. 
 * 
 * @param type $sort list of columns to sort in order, separated by a semicolon
 * @param type $desc list of sort order, separated by a semicolon
 * @param type $system_arr list of systems
 */
function displaySystemTable($sort, $desc, $system_arr) {

    print "  <tr>\n";
    
    $columns = array("HW_NAME" => "System",        
                     "HW_MODEL" => "Type",
                     "TOTAL_DAY1" => "72 Hr<br/>Avail.",
                     "POOL_NAME" => "Pool",
                     );

    foreach ($columns as $column => $title) {
        formatColumnHeadings($column, $title, $sort, $desc, 'system');
    }

    print "  </tr>\n";
    print "  <tr>\n";
    
    // display the filter row.
    $colidx = 0;
    foreach ($columns as $column => $title) {
        displayFilterRow($column, $title, $colidx, 'ManageSystem');
        $colidx++;
    }
    
    print "  </tr>\n";
    $spage = 'SystemWebPage.php';
    $rpage = 'ReservationWebPage.php';    
    
    foreach ($system_arr as $system) {      
        $sid = $system["sid"];
        $poolname = $system['poolname'];
        $poolowner = $system['poolowner'];
        $name = $system["name"];
        $type = $system["type"];
        $status = $system["status"];
        $cid = $system["cid"];
        if ($status == 'INACTIVE') {
            continue;
        }
        print "<tr>\n";
        $a_class = "projlink";
        $class = '';
        // System Name
        print "<td nowrap>";
        if ($status == 'ACTIVE') {
            print "<div style=\"float:left;text-align:center;\">";
            print "<a class=\"".$a_class."\" href=\"" . $spage .
                "?"."sid=".$sid."\">".$name."</a>&nbsp;";    
            print "</div>";
            print "<div style=\"float:right;text-align:center;\"><a class=\"coltitle_link\" href=\"" . $rpage .
                    "?" . "sid=" . $sid . "&cid=" . $cid . "\"><img src=\"images/scheduler_ena.png\" title=\"Reserve\" alt=\"Reserve\"></a></div>";
        } 
        print "</td>\n";

        print "<td" . $class . " nowrap>";    
        print $type;
        print "</td>\n";
        
        displayAvailabilityColumn($system['day1']);

        print "<td" . $class . ">";  
        print $poolname;
        print "</td>\n";

        print "</tr>\n";
    }
}

/**
 * Create a table header with sorting icons.  
 * 
 * @param type $column name of the column
 * @param type $title  display name of the column
 * @param type $sort list of columns to sort in order, separated by a semicolon
 * @param type $desc list of sort order, separated by a semicolon
 * @param type $page current page
 */
function formatColumnHeadings($column, $title, $sort, $desc, $page) 
{
    print "<td style=\"text-align: center;\">";
    if (strpos($column, 'TIME') === false && strpos($column, 'TOTAL_DAY') === false) {
        $next_desc = 1;
        
        $col_list = preg_split("/[;]+/", $sort);
        $desc_list = preg_split("/[;]+/", $desc);
        $found = false;
        $new_desc = '0';
        $sortnum = 0;
        $current_sort = 
        $idx = 0;
        foreach ($col_list as $col) {              
            if ($col == $column) {
                $found = true;
                $current_sort = $desc_list[$idx];
                if ($current_sort == '0') {
                    $new_desc = '1';
                } else {
                    $new_desc = '0';
                }
                $sortnum = $idx+1;
                break;
            } 
            $idx ++;
        }
        print "<a class=\"coltitle_link\" href=\"" . $_SERVER['PHP_SELF'] .
            "?sort=" . $column . "&desc=" . $new_desc . "\">"; 
        print $title;
        print "</a>";

        
        print "<div style=\"float:right;\">";
        $img = "images/sort_ena.png"; 
        if ($found) {
            if ($current_sort == '1') {
                $img = "images/sort_column_descending.png"; 
            } else if ($current_sort == '0') {
                $img = "images/sort_column_ascending.png"; 
            }
        }
        print "<img src=\"" . $img . "\" title=\"Sort\" alt=\"Sort\" onclick=\"displaySortMenu(event, '" . $column . "', '" . $page . "')\">";
        
        if ($found) {
            print strval($sortnum);
        }
        print "</div>";
        
    } else {
        print "<span class=\"coltitle\">";
        print $title;
        print "</span>";
    }
  
    print "</td>\n";

}

/**
 * Display the filter column.
 * 
 * @param type $column name of the column
 * @param type $title  display name of the column
 * @param type $colidx column index, starting from 0.
 * @param type $page   page name
 */
//
function displayFilterRow($column, $title, $colidx, $page) 
{
    
    $colname = 'server_filter_' . $column;
   
    $cookiename="sgd_system_filter_" . $page . "_". $colidx;
    $value =  isset($_COOKIE[$cookiename]) ? $_COOKIE[$cookiename] : '';
    print "    <td name=\"" . $colname . "\">\n";
    print '      <input type="text" class="sgd_filter_ed_tx" value="' . $value . '" onkeyup="filterSystems()">';
    print "    </td>\n";

}


/**
 * Display availability percentage.  Show in red if greater than and equal to 75, 
 * show in yellow if greater than and equals to 50, show in green is less than 50.
 * 
 * @param type $percentage availability percentage in integer ranging from 0 to 100
 */
function displayAvailabilityColumn($percentage) {
        $percent = intval(floatval($percentage));
        $bgcolor = '';
        $wcolor = 'black';
        if ($percent < 50) {
            $bgcolor = lightGreen();
        } else if ($percent < 75) {
            $bgcolor = lightYellow();
        } else {
            $bgcolor = lightRed();       
        }
        $avbl_value = $percent . '%';
        print "<td align=\"center\" style=\"width:10px;background-color: " . $bgcolor . "\">";  
        print "<font color=\"" . $wcolor . "\"><b>";
        print $avbl_value;
        print "</b></font>";
        print "</td>\n";
}

?>

