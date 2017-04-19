<?php
include 'connect.php';
//Prevent SQL inject pass string to function
function cleaninject($string){
	$string = strip_tags($string);
	return mysql_real_escape_string($string);
}
//Get JSON from index
$strSearch= cleaninject($_GET["search"]);

//Run only if there is valid content
if ($strSearch){
//Generate query and request results from DB
	$query="SELECT * FROM testTable WHERE value1 LIKE '$strSearch%' OR value2 LIKE '$strSearch%' OR value3 LIKE '%$strSearch%'";
	$result = mysql_query($query);
	$rows = array();

//If mysql query errors then produce error
	if (!$result) {
		echo "Could not successfully run query ($sql) from DB: " . mysql_error();
		exit;
	}

//If no values are pulled
	if (mysql_num_rows($result) == 0) {
		echo "No rows found, nothing to print so am exiting";
		exit;
	}
//Parse through mysql fetch array then echo encoded JSON response
	while ($r = mysql_fetch_array($result, MYSQL_ASSOC)) {
		$rows[] = $r;
	}
	echo json_encode($rows);
}
//Close mySQL Session
mysql_close($connect);
?>