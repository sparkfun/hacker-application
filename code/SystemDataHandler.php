<?php
//
// SystemDataHandler.php
// 
// This class retrieves data from database and return data in a data structure.
//

/**
 * Get the list of systems available in the sort order specified.  
 * For the demo purpose, the list of systems is hard-coded and 
 * sorting is not working.
 * 
 * @param type $db  database connection
 * @param type $sort list of columns to sort in order, separated by a semicolon
 * @param type $desc list of sort order, separated by a semicolon
 * @param type $userid  current user logged in
 * @return array list of map with system information.
 */
function getSystemSummary($db, $sort='', $desc='', $userid=NULL) {
    $system_arr = array();
    $new_sort = '';
    if ($sort == '') {
        $new_sort = 'HW_NAME ASC';
    } else {
        $col_list = preg_split("/[;]+/", $sort);
        $desc_list = preg_split("/[;]+/", $desc);
        $idx = 0;

        foreach ($col_list as $col) {
            if ($new_sort != '') {
                $new_sort .= ', ';
            }
            $sort_order = 'ASC';
            if ($desc_list[$idx] == '1') {
                $sort_order = 'DESC';
            }
            $new_sort .= $col;
            $new_sort .= ' ' . $sort_order;
            $idx ++;
        }
    }
            
//    $param = 'P_USER=>\'' . $userid . '\'';
//    $sql_str = 'select * from table(SURF_HARDWARE_INFO.GET_SYSTEM_DETAILS( ' . $param . ')) ORDER BY HW_STATUS ASC, ' . $new_sort;
    
//    error_log($sql_str);
//    $sql = oci_parse($db, $sql_str);
//    oci_execute($sql);
//    while ($row =  oci_fetch_array($sql, OCI_ASSOC + OCI_RETURN_NULLS)) {
//
//        $sid = $row['HW_ID'];
//        $name = $row['HW_NAME'];
//        $type = $row['HW_MODEL'];
//        $poolname = $row['POOL_NAME'];
//        $status = $row['HW_STATUS'];
//        $cid = $row['CONTROL_ID'];
//        $day1 = $row['TOTAL_DAY1'];
//        $arr = array('sid' => $sid,
//                     'name' => $name,
//                     'type' => $type,
//                     'poolname' => $poolname,
//                     'poolowner' => '',
//                     'status' => $status,
//                     'day1' => $day1,
//                     'cid' => $cid, 
//                     );
//
//        array_push($system_arr, $arr);      
//    }
    
    $arr = array('sid' => '1',
                     'name' => 'scr-app-01',
                     'type' => 'M3000',
                     'poolname' => 'POOL1',
                     'poolowner' => 'toshiko.sesselmann@gmail.com',
                     'status' => 'ACTIVE',
                     'day1' => '100',
                     'cid' => '1', 
                     );
    array_push($system_arr, $arr);     
    $arr = array('sid' => '2',
                     'name' => 'scr-app-02',
                     'type' => 'M3000',
                     'poolname' => 'POOL2',
                     'poolowner' => 'toshiko.sesselmann@gmail.com',
                     'status' => 'ACTIVE',
                     'day1' => '50',
                     'cid' => '1', 
                     );
    array_push($system_arr, $arr);     
    
    $arr = array('sid' => '3',
                     'name' => 'scr-app-03',
                     'type' => 'M8-2',
                     'poolname' => 'POOL3',
                     'poolowner' => 'toshiko.sesselmann@gmail.com',
                     'status' => 'ACTIVE',
                     'day1' => '25',
                     'cid' => '1', 
                     );
    array_push($system_arr, $arr); 
    
    $arr = array('sid' => '4',
                     'name' => 'scr-app-04',
                     'type' => 'T7',
                     'poolname' => 'POOL4',
                     'poolowner' => 'toshiko.sesselmann@gmail.com',
                     'status' => 'ACTIVE',
                     'day1' => '0',
                     'cid' => '1', 
                     );
    array_push($system_arr, $arr); 
    
    $arr = array('sid' => '5',
                     'name' => 'scr-app-05',
                     'type' => 'SN2',
                     'poolname' => 'POOL5',
                     'poolowner' => 'toshiko.sesselmann@gmail.com',
                     'status' => 'ACTIVE',
                     'day1' => '70',
                     'cid' => '1', 
                     );
    array_push($system_arr, $arr); 
    
    //oci_free_statement($sql);
    return $system_arr;
}
