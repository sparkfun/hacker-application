<?php

require('includes/db.config.php');

//check for user id in the url
if(!(isset($_GET['id']))) {
	echo "No user found ID detected.";
} else {
	//sql injection security
	$blogID = intval($_GET['id']);
	

	$deleteSQL = $link->query("DELETE FROM all_posts WHERE item_id = '".$blogID."'");
	$deleteSQL2 = $link->query("DELETE FROM blogs WHERE blog_id = '".$blogID."'");

	
	if($deleteSQL === FALSE) {
		echo "Blog could not be deleted from the 'all_posts' table. Please contact your network administrator (Ty) to fix this error.";
	} elseif($deleteSQL2 === FALSE) {
		echo "Blog could not be deleted from the 'blogs' OR 'seekers' table. Please contact your network administrator (Ty) to fix this error.";
	} 
}


?>