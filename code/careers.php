<?php

session_start();

//if user is logged in
if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id') {

	//set user_id
	$user_id = $_SESSION['user_id'];
	
	include('includes/user.header.php');	

} else {
	include('includes/anon.header.php');
} //close if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id')
?>

<div class="container" style="margin: 80px auto 15px auto; width: 80%;">
	<p>
		<h3>Join us!</h3>
	</p>
	<p>
		Do you want to work for HireStarts?
		<br />... <b>too bad.</b>
	</p>
	<p>
	No, just kidding. This page is in the works, but we do have open positions listed under our <a href="/opportunities/" title="HireStarts Job Listings" alt="HireStarts Opportunities Page">Opportunities</a> page.
	</p>
</div>


<?php

include("includes/footer.php");

?>