<?php

session_start();

//if user is logged in
if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id') {

	//is the users profile complete?
	if(!(isset($_SESSION['profile']))) {
		Header('Location: /edit-profile');
	}

	//set user_id
	$user_id = $_SESSION['user_id'];
	
	include('includes/user.header.php');
	
	$userProfile = $_GET['user'];
	
	//istantiate database connection
	$db = new DBConnection;
	$industry = new Industry($db);
	//istantiate User object and echo out the user profile
	$user = new User($db);
	$string = new str_format;
	$jobs = new Job($db);
	
	$userData = $user->getProfile($userProfile);
	$ind = $industry->getIndustryByID($userData['industry']);
	
	$twitter = $userData['twitter'];
	//check if twitter handle has @ sign infront of it
	if(!(substr($userData['twitter'], 0, 1)==='@')) {
		$twitter = '@'.$userData['twitter']; //add the @ sign if it doesnt exist for the url text
	} 
	if(substr($userData['twitter'], 0, 1) === '@') {
		$twitteru = trim($twitter, '@'); // trim the @ sign from supplied twitter handle to get twitter url
		$twitter_url = "http://www.twitter.com/".$twitteru;
	}
	
	if(strlen($userData['logo_path']) > 5) {
		$logo = $userData['logo_path'];
	} else {
		$logo = "/images/company_default.png";
	}
	

?>
		
	<div class='user_specific_header'>
		<div class='user_img'>
			<img src='<?php echo $logo; ?>'>
		</div>
		<div class='user_info'>
			<span class='user_name'><?php echo stripslashes($userData['company_name']); ?></span>
			<span class='company_slogan'> - <?php echo stripslashes($userData['slogan']); ?></span>
			<span class='user_specs'>
				Located in: <a href='#'><?php echo $userData['location']; ?></a> - Industry: <a href='/members?industry=<?php echo $ind['name']; ?>'> <?php echo $ind['name']; ?></a><br />
			</span>
			<div class='user_social'>
				<ul>
					<li><a href='<?php echo $twitter_url; ?>' target='_blank'><?php echo $twitter; ?></a></li>
					<li><a href='<?php echo $userData['blog']; ?>' target='_blank'><?php echo $userData['blog']; ?></a></li>
				</ul>
			</div>
		</div>
		<div class='user_specific_header_right'>
			<a class='btn' rel='tooltip' title='Message <?php echo stripslashes($userData['company_name']); ?>' href='#'>Private Message</a>
			<a class='btn' rel='tooltip' title='View <?php echo stripslashes($userData['company_name']); ?> " Jobs' href='/opportunities.php?company=<?php echo stripslashes($userData['company_name']); ?>'>View Jobs</a>
		</div>
	</div>
			
			<div class='quick-intro'>
				<h2>About: </h2>
				<?php echo html_entity_decode(stripslashes($userData['description'])); ?>
			</div>
			
			<div class='second-block'>			
				<h2>Mission Statement: </h2>
				<?php echo html_entity_decode(stripslashes($userData['m_statement'])); ?>		
			</div>	
			
			<div class='third-block'>
				<h2>Job Openings:</h2>
					
					<?php
						$jobQuery = $jobs->getCompanyJobs($userData['id']);
						foreach($jobQuery as $job => $jobRow) {
							$jobID = $db->real_escape_string($jobRow['job_id']);
							$jobTitle = $db->real_escape_string($jobRow['job_title']);
							$jobLocation = $db->real_escape_string($jobRow['location']);
							$jobIndustry = $db->real_escape_string(intval($jobRow['industry']));
							
							//current industry
							$indRow = $industry->getIndustryById($jobIndustry);
							
							$jobURL = $string->Seo($jobTitle);
							
							/* Stylize The Job Posts */
							echo "<p>";
							echo "<a href='/job/".$jobID."/".$jobURL."'>".$jobTitle."</a> - <a href='/opportunities?industry=".$indRow['name']."'>".$indRow['name']."</a><br />";
							echo $jobLocation."<br />";
							echo "</p>";
							
						}
					?>
			</div>

<?php
	
} else {
	header("Location: index");
} //close if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id')


include("includes/footer.php");
?>