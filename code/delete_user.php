<?php

require('includes/db.config.php');

//check for user id in the url
if(!(isset($_GET['id']))) {
	echo "No user found ID detected.";
} else {
	//sql injection security
	$userID = intval($_GET['id']);
	
	//check if user is a company
	$companySQL = $link->query("SELECT company FROM users WHERE user_id = '".$userID."'");
	$companyRow = mysqli_fetch_row($companySQL);
	$isCompany = $companyRow['company'];
	//if user is company, delete from the companies table
	if($isCompany === 1) {
		$deleteSQL = $link->query("DELETE FROM users WHERE user_id = '".$userID."'");
		$deleteSQL2 = $link->query("DELETE FROM companies WHERE id = '".$userID."'");
	//if user is not a company, delete from the seekers table
	} else {	
		$deleteSQL = $link->query("DELETE FROM users WHERE user_id = '".$userID."'");
		$deleteSQL2 = $link->query("DELETE FROM seekers WHERE id = '".$userID."'");
	}
	
	if($deleteSQL === FALSE) {
		echo "User could not be deleted from the 'users' table. Please contact your network administrator (Ty) to fix this error.";
	} elseif($deleteSQL2 === FALSE) {
		echo "User could not be deleted from the 'companies' OR 'seekers' table. Please contact your network administrator (Ty) to fix this error.";
	} else {
		page_redirect('/seekers');
	}
}


?>