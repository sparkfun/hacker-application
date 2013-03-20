<?php 

session_start();
//check if there is a cookie on users pc
if(isset($_COOKIE['username']) && isset($_COOKIE['logged'])) {
	
	$cookieUser = $_COOKIE['username'];
	$cookieSalt = $_COOKIE['logged'];
	
	include_once('includes/config.php');
	
	//match the cookie information with the database information
	$cookieSQL = $link->query("SELECT user_id, username, salt FROM users WHERE username = '".$cookieUser."' AND salt = '" .$cookieSalt. "'");
	
	if(mysqli_num_rows($cookieSQL) === 1) {
	
		$cookieRow = mysqli_fetch_assoc($cookieSQL);
		
		$user_id = intval($cookieRow['user_id']);
		$_SESSION['loggedin'] === TRUE;
		$_SESSION['user_id'] === $user_id;
	} else {
		echo "Invalid cookie information. Please login again.";
	}
	
}

//if user is logged in
if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id' && isset($_SESSION['admin'])) {
	
	//set user_id
	$user_id = $_SESSION['user_id'];
	
	include_once('includes/user.header.php');
		
?>

<div class="container" style="margin-top: 100px; min-height: 1200px;">
	
	<?php include('includes/admin.nav.php'); ?>
	
	<h2>This page is currently under construction.</h2>
	
</div><!-- close container -->

<?php

} else {
	header("Location: index.php");
}//close if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'user_id')

include('includes/footer.php');

?>