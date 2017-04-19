<?php 

session_start();

//if user is logged in
if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id' && isset($_SESSION['admin'])) {
	
	//set user_id
	$user_id = $_SESSION['user_id'];
	
	include('includes/user.header.php');
		
	echo "<div class='container' style='margin-top: 100px;'>";
		
	include('includes/admin.nav.php'); 
	
	echo "<h1>Pending Beta Activations: </h1>";
	
?>

	<table width="75%" border="0">
		<?php
			
			$compare = $link->query("SELECT u_beta.email, IF(u_beta.email != users.email, 1, 0) AS is_different FROM u_beta LEFT JOIN users ON users.email = u_beta.email");

			/* PRINT OUT PENDING ACTIVATIONS */
			
			
		?>
	</table>

<?php
	
	echo "</div>"; //close container div

} else {
	header("Location: index.php");
}//close if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'user_id')

include('includes/footer.php');

?>