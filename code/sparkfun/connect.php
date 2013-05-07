<?php
//Create Connection
$connect=mysql_connect("localhost","sparkfun","passwordhere");

//Make Connection
if (!mysql_errno($connect))
	{
		echo "Failed to connect to DB:" . mysqli_connect_error();
	}

if (!mysql_select_db("sparkfun")) {
    echo "Unable to select db sparkfun: " . mysql_error();
    exit;
}

?>
