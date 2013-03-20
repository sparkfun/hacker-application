<?php

include_once('../classes/db.class.php');
include_once('../classes/user.class.php');
include_once('../classes/industry.class.php');

//if there is a query in the URL
if(isset($_GET['lastid'])) {

	//istantiate required objects
	$db = new DBConnection;
	$user = new User($db);
	$industry = new Industry($db);

		$start = intval($_GET['lastid']);
		$limit = "15";
		//get all users from database using the getAllUsers() function
		$users = $user->getAllUsersInfo($start, $limit);
				
		foreach($users as $u => $val) {
			$userType = $user->getUserType($val['id']);
					
			if($userType !== '4') {
				//get user data into variables for display purposes
				$firstName = $val['first_name'];
				$lastName = $val['last_name'];
				$fullName = $firstName. " " .$lastName;
						
				//if user has not uploaded a photo, use the default image
				if(strlen($val['photo_path']) === 0) {
					$userPhoto = "/images/default_image.gif";
				} else {
					$userPhoto = $val['photo_path'];
				}
				//explode the skills by commas into an array	
				$skills = explode(",", $val['skills']);

		?>
		
			<div class='user-block'>
				<span class='user-photo'>
					<img src='<?php echo $userPhoto; ?>'>
				</span>
				<span class='user-info'>
					<span class='username'><?php echo "<a href='/user/".$val['username']."'>".$fullName."</a>"; ?></span><br />
					<span class='user-school'><?php echo "<a href='/members?q=".$val['school']."'>".$val['school']."</a>"; ?> </span><br />
					<span class='user-skills'>
						<ul class='tags'>
							<?php
								$i = 0;
								foreach($skills as $skill) if($i<6) {
									echo "<li class='tag'><a href='/members?q=".$skill."' style='color: #333;'>".$skill."</a></li>";
									$i += 1;
								}
							?>
						</ul>
					</span><br />
				</span>		
			</div>
		
		
	<?php 
					
			} else {  
				
				//stripslashes from company name
				$companyName = stripslashes($val['company_name']);
								
				//if company has not uploaded a photo, use the default image
				if(strlen($val['logo_path']) === 0) {
					$logo = "/images/company_default.png";
				} else {
					$logo = $val['logo_path'];
				}
				//get the company industry
				$indName = $industry->getIndustry($val['industry'])
				
	?>

			<div class='user-block'>
				<span class='user-photo'>
					<img src='<?php echo $logo; ?>'>
				</span>
				<span class='user-info'>
					<span class='username'><?php echo "<a href='/company/".$val['username']."'>".$companyName."</a>"; ?></span><br />
					<span class='user-school'><b>Industry: </b><?php echo "<a href='/members?q=".$indName."'>".$indName."</a>"; ?><br /><b>Location: </b><?php echo $val['location']; ?></span>
					<span class='user-skills'>
									
					</span><br />
				</span>
						
			</div>

<?php 		
		}//close if(not company)
	}//close foreach($users)
}//close if(isset(lastid))


?>