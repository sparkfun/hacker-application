<?php
include 'connect.php';

//Prevent SQL inject
function cleaninject($string){
	$string = strip_tags($string);
	return mysql_real_escape_string($string);
}
//Grab all values from POST
$strValue1= cleaninject($_POST["value1"]);
$strValue2= cleaninject($_POST["value2"]);
$strValue3= cleaninject($_POST["value3"]);

//Generate Query
$query="INSERT INTO testTable (value1, value2, value3) VALUES ('$strValue1', '$strValue2', '$strValue3')";
$result = mysql_query($query);

//Run Query if fails then produce
if(!mysql_query($query))
{
	die('Error: ' . mysql_error($result));
}else{
echo "Values are now stored.";
}
//Close mySQL Session
mysql_close($connect);

//Redirecting
header( 'Refresh: 2; URL=inputs.html' );
exit;
?>