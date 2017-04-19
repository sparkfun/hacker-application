<?php

session_start();

//if user is logged in
if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id') {

	//is the users profile complete?
	if(!(isset($_SESSION['profile']))) {
		Header('Location: edit-profile.php');
	}

	//set user_id
	$user_id = $_SESSION['user_id'];
	//who's profile?
	$userProfile = $_GET['user'];
	
	include('includes/user.header.php');
	
	//istantiate database connection
	$db = new DBConnection;

	//istantiate User object and echo out the user profile
	$user = new User($db);
	$userData = $user->getProfile($userProfile);
	
		/* 
			piece user information together properly for display
		*/
			$fullName = $userData['first_name']. " " .$userData['last_name'];
			
			if(strlen($userData['photo_path']) <= 0) {
				$photo_path = '/images/default_image.gif';
			} else {
				$photo_path = $userData['photo_path'];
			}

			
			$major = stripslashes($userData['major']);

			$todays_date = date("Y-m-d");
			$todays_date = strtotime($todays_date);
			$graduationDate = strtotime($userData['graduation_date']);
			$graduationDate = date('F jS, Y', $graduationDate);
			
			$graduated_expression = '';
			
			//Graduation Expression
			if($graduationDate < $todays_date) {
				$graduated_expression = "Attended " .$userData['school']. " for " .$major. " <span class='tip'>(graduated)</span>";
				$gradStatement = $graduationDate. " <span class='tip'>(graduated)</span>";
			} else {
				$graduated_expression = "Attends " .$userData['school']. " for " .$major;
				$gradStatement  = $graduationDate;
			}

			//if user has not supplied work info, do not display anything
			if(strlen($userData['employer']) > 0) {
				$employerExpression = $userData['position']. " at " .$userData['employer'];
			} else {
				$employerExpression = '';
			}
			
			$twitter = $userData['twitter'];
			//check if twitter handle has @ sign infront of it
			if(!(substr($userData['twitter'], 0, 1)==='@')) {
				$twitter = '@'.$userData['twitter']; //add the @ sign if it doesnt exist for the url text
			} 
			if(substr($userData['twitter'], 0, 1) === '@') {
				$twitter = trim($twitter, '@'); // trim the @ sign from supplied twitter handle to get twitter url
				$twitter_url = "http://www.twitter.com/".$twitter;
			}
			
			$interests = explode(',', $userData['interests']);
			$skills = explode(',', $userData['skills']);
			
			$background = nl2br(stripslashes($userData['background']));

	?>
	
		<div class='user_specific_header'>
		
			<div class='user_img'>
				<img src='<?php echo $photo_path; ?>'>
			</div>
			
			<div class='user_info'>
			
				<span class='user_name'><?php echo $fullName; ?></span>
				<span class='user_specs'><?php echo $graduated_expression; ?><br />
					<?php echo $employerExpression; ?>
				</span>
				
				<div class='user_social'>
					<ul>
						<li><a href='<?php echo $twitter_url; ?>' target='_blank'><?php echo $userData['twitter']; ?></a></li>
						<li><a href='<?php echo $userData['blog']; ?>' target='_blank'><?php echo $userData['blog']; ?></a></li>
					</ul>
				</div>
			
			</div>
			<div class='user_specific_header_right'>
				<a class='btn' rel='tooltip' title='Private message <?php echo $userData['first_name']; ?> - Coming Soon!' href='#'>Send Message</a>
				<a class='btn' rel='tooltip' title='Print <?php echo $userData['first_name']; ?>s resume - Coming Soon!' href='#'>Print Resume</a>
			</div>
		</div>
			
		<div class='quick-intro'>
			<?php echo $background; ?>
		</div>
				
			<div class='second-block'>
					<div class='education-block'>
						<h2>Education: </h2>
						<ul>
							<li><span class='title'><?php echo "<a href='/members?q=".$userData['school']."'>".$userData['school']; ?></a></span></li>
							<li><span class='title'>Area of Study: </span><?php echo $userData['major']; ?></li>
							<li><span class='title'>Graduation Date: </span><?php echo $gradStatement; ?></li>
						</ul>
					</div>


				<div class='skills-block'>
					<h2>Skills: </h2>
					<ul class='tags'>
						<?php
							foreach($skills as $skill) {
								echo "<li class='tag'><a href='/members?q=".$skill."' style='color: #333;'>".$skill."</a></li>";
							}
						?>
					</ul>
				</div>		
			</div>
			
			<div class='third-block'>
					<h2>Interests: </h2>
				<ul class='tags'>
					<?php
						foreach($interests as $interest) {
							echo "<li class='tag'><a href='/members?q=".$interest."' style='color: #333;'>".$interest."</a></li>";
						}
					?>
				</ul>
			</div>
			
			<div class='fourth-block'>
				<h2>Recommendations: </h2>
					<h3>Coming soon!</h3>
			</div>
			

<?php

	
} else {
	header("Location: /index.php");
} //close if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id')


include("includes/footer.php");
?>