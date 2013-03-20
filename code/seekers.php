<?php 

session_start();

//if user is logged in
if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id' && isset($_SESSION['admin'])) {
	
	//set user_id
	$user_id = $_SESSION['user_id'];
	
	include_once('includes/user.header.php');
	
	$db = new DBConnection;
	$admin = new Admin($db);
	$user = new User($db);
	$str = new str_format;
	
	$userData = $user->getUserInfo($user_id);
		
?>
	
	<?php include('includes/admin.nav.php'); ?>
	
		<div class="sidebar">
			<div class="side-search">
				<form name="adminSearch" method="GET" action="<?php echo $_SERVER['PHP_SELF']; ?>">
					<input type="text" name="q" placeholder="Search all users" />
					<input type="submit" name="search" class="btn btn-success" value="Search" />
				</form>
			</div>
			<?php
			
				if(isset($_GET['q'])) {
					$q = $_GET['q'];
					
					page_redirect("/members?q=$q");
				}
			
			?>
			<div class="user_stats">
				<span class="title">Registered Users</span>
				<ul style="margin-top: 10px;">
					<li>
						<span class="stat_title">Companies: </span> 
						<?php
							echo $user->getNumCompanies();				
						?>
					</li>
					
					<li>
						<span class="stat_title">Job Seekers: </span>
						<?php					
							echo $user->getNumSeekers();				
						?>
					</li>
					
					<li>
						<span class="stat_title">Total Users: </span>
						<?php					
							echo $user->getNumUsers();
						?>
					</li>					   
				</ul>
			</div>
		</div>
	
	<div class="post-container">
	
	
	
	</div>


<?php

} else {
	header("Location: index.php");
}//close if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'user_id')

include('includes/footer.php');

?>