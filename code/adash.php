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
		<div class="admin_message">
			<form name="admin_message" action="<?php echo $_SERVER['PHP_SELF']; ?>" method="POST">
				<label class="message_label">Leave Message: </label>
				<input type="text" class="large" name="message" maxlength="140" placeholder="Leave a 140 character message." />
				<input type="hidden" name="poster" value="<?php echo $userData['first_name']; ?>" />
				<input type="submit" name="addMessage" class="btn btn-primary" value="Submit" />
			</form>
			<div class="message_viewer">
			<?php
				
				//if the message form is submitted, save the message to db
				if(isset($_POST['addMessage'])) {
					$addMessage = $admin->postAdminMessage();
					
					if($addMessage === FALSE) {
						echo "<span class='error'>Add Message Failed.</span>";
					}
				} 
				
				$messages = $admin->getAdminMessages();
				
				foreach($messages as $message => $data) {
					$date = date('F jS, Y - g:ia', strtotime($data['date']));
					$message = html_entity_decode(stripslashes($data['message']));
				
		
			?>
				<p class="a_messages">
					<span class="date"><?php echo $date; ?>: </span>
					<span class="message">
						<?php echo $message; ?>
					</span>
					<span class="poster">
						- <?php echo $data['poster']; ?>
					</span>
				</p>
				
			<?php
				}//close while($row = mysqli_fetch_assoc($messageSQL))
			?>
			
		</div>
	</div>
	
	<div class="go_analytics">
		<h4>Site Analytics</h4>
		<?php
			
			//google analytics data
			$ga_email = 'hirestarts@gmail.com';
			$ga_password = 'college111';
			$ga_profile_id = '69806077';
			$ga_url = $_SERVER['REQUEST_URI'];
			
			/* Create a new Google Analytics request and pull the results */
			$ga = new gapi($ga_email,$ga_password);
			$ga->requestReportData($ga_profile_id, array('date'),array('pageviews'), 'date');    

			$results = $ga->getResults();
	
			foreach($results as $result) {
				//do nothing
			}
			$ga->requestReportData($ga_profile_id, 'date', array('pageviews', 'uniquePageviews', 'avgPageLoadTime', 'avgTimeOnPage', 'percentNewVisits'), 'date');
			$results = $ga->getResults();
			
			$pageViews = number_format($result->getPageviews());
			
			//covert the time to minutes:seconds
			function secondMinute($seconds) {
				$minResult = floor($seconds/60);
				
				if($minResult < 10) {
					$minResult = 0 . $minResult;
				}
				
				$secResult = ($seconds/60 - $minResult)*60;
				
				if($secResult < 10) {
					$secResult = 0 . round($secResult);
				} else { 
					$secResult = round($secResult); 
				}
				
				return $minResult.":".$secResult;
			}
			
			echo '<div id="page-analtyics">';
			
			$results = array_reverse($results);
			
			foreach($results as $result) {
			
				echo '<div class="metric"><span class="label">Pageviews</span><br /><strong>'.number_format($result->getPageviews()).'</strong></div>';
				echo '<div class="metric"><span class="label">Unique pageviews</span><br /><strong>'.number_format($result->getUniquepageviews()).'</strong></div>';		
				echo '<div class="metric"><span class="label">% New Visits</span><br /><strong>'.round($result->getPercentNewVisits(), 2).'%</strong></div>';
				
				echo '<div class="metric"><span class="label">Avg time on page</span><br /><strong>'.secondMinute($result->getAvgtimeonpage()).'</strong></div>';		
				echo '<div class="metric"><span class="label">Avg Page Load</span><br /><strong>'.secondMinute($result->getAvgPageLoadTime()).'</strong></div>';			
				echo '<div style="clear: left;"></div>';
			}
			echo '</div>';
	?>
	
	</div><!-- close .post-container -->

<?php

} else {
	header("Location: index.php");
}//close if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'user_id')

include('includes/footer.php');

?>