<?php 

session_start();

//if user is logged in
if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id') {
	
	//set user_id
	$user_id = $_SESSION['user_id'];
	
	include('includes/user.header.php');
	
	//istantiate database object
	$db = new DBConnection;
	//istantiate user object
	$user = new User($db);
	$upload = new Upload($db);
	
	//if the user is NOT a company
	if(!(isset($_SESSION['company']))) {
	
	/***************************************************
	************* SEEKER EDIT PROFILE ******************
	****************************************************/
		
		//get the user data from the database
		$userData = $user->getUserInfo($user_id);
		
		$gradDate = date('F jS, Y', strtotime($userData['graduation_date']));
		$background = nl2br(stripslashes($userData['background']));
		

?> 

	<div class="user_specific_header">
		<h2>Welcome <?php echo $userData['first_name']; ?></h2><br />
		<h4>Please fill out the form below to complete your profile.</h4>
		<div class="edit-profile-pages">
			<ul>
				<li><a class="active" href="#">Edit Profile</a></li>
				<li><a href="account-settings">Account Settings</a></li>
				<li><a href="#">Privacy Settings</a></li>
			</ul>
		
		</div>
	</div>
	
		<?php
		
			if(isset($_GET['complete'])) {
				if(!(isset($_SESSION['company']))) {
					echo "<div style='margin: 5px;'><span class='success'>Profile update complete. <a href='/user/".$username."'>View Profile</a></span></div>";
				} else {
					echo "<div style='margin: 5px;'><span class='success'>Profile update complete. <a href='/company/".$username."'>View Profile</a></span></div>";
				}
			}
		
		?>
            <div class="upload-form">
			
				<?php
				
					if(isset($_POST['uploadImg'])) {
						$uploadPhoto = $user->updatePhoto($user_id);
						
						if($uploadPhoto === FALSE) {
							echo "<span class='error'>Photo upload failed</span>";
						} else {
							echo "<div style='margin: 5px;'><span class='success'>Photo Sucessfully Uploaded!</span></div>";
						}
					}
				
				?>
                <form id="upload_form" name="upload_form" enctype="multipart/form-data" method="post" action="<?php echo $_SERVER['PHP_SELF']; ?>">
                    <div>
						<label class="edit_label">Upload Photo: </label>
                        <input type="file" name="photo" id="image_file" onchange="PreviewImage();" />
						<input type="hidden" name="MAX_FILE_SIZE" value="1048000" />
                    </div>
                    <div>
                        <input type="submit" name="uploadImg" id="userImgUpload" class="btn btn-large btn-primary" value="Upload" />
                    </div>
                </form>
				<div class='image-preview'><img id="uploadPreview" /></div>
            </div>
		<hr />
<?php
	
	
	if(isset($_POST['submitPersonal'])) {

		$update = $user->updateProfile($user_id);
		
		if($update === FALSE) {
			echo "<span class='error'>Error updating profile.</span>";
		} else {
			
		}
	
	}

?>	
	
		<form name="editProfile" method="POST" action="<?php echo $_SERVER['PHP_SELF']; ?>">
		
			<div class="edit-profile-form">
				<div class="edit-profile-field">
					<label class="add_label">School Name: </label>
					<input type="text" rel="tooltip" title="Where do you (or did you) go to school?" class="add_form" name="school" maxlength="50" placeholder="i.e. University of Michigan" value="<?php echo $userData['school']; ?>" required />
				</div>
				
				<div class="edit-profile-field">
					<label class="add_label">Area of Study: </label>
					<input type="text" rel="tooltip" title="What is/was your major?" class="add_form" name="major" maxlength="50" placeholder="i.e. Business Marketing" value="<?php echo $userData['major']; ?>" required />
				</div>
				
				<div class="edit-profile-field">
					<label class="add_label">Graduation Date: </label>
					<input rel="tooltip" rel="tooltip" title="When do you (or did you) graduate?" title="yyyy/mm/dd" type="date" id="gradDate" class="add_form" name="gradDate" placeholder="i.e. 2012-12-25" maxlength="50" value="<?php echo $gradDate; ?>" required /><span class="tip"> (mm/dd/yyy)</span>
				</div>
				
				<div class="edit-profile-field">
					<label class="add_label">Employer: </label>
					<input type="text" rel="tooltip" title="Most recent employer" class="add_form" name="employer" maxlength="50" placeholder="i.e. Ted's Books" value="<?php echo $userData['employer']; ?>" /><span class="tip"> Optional</span>
				</div>
				
				<div class="edit-profile-field">
					<label class="add_label">Position: </label>
					<input type="text" rel="tooltip" title="Most recent position" class="add_form" name="position" maxlength="50" placeholder="i.e. Store Manager" value="<?php echo $userData['position']; ?>" /><span class="tip"> Optional</span>
				</div>
			</div>
			
			
			<div class="edit-profile-form">
			
				<div class="edit-profile-field">
					<label class="add_label">Background: </label>
					<textarea name="background" class="add_txt" placeholder="Tell us about yourself!" required><?php echo $background; ?></textarea>
				</div>
				
				<div class="edit-profile-field">
					<label class="add_label">Interests: </label>
					<input type='text' rel="tooltip" title="Separate interests by a comma please" id="interests" class='add_form' name='interests' maxlength='100' placeholder='Separate interests with a comma please.' value="<?php echo $userData['interests'] ?>" required />
					<ul id="interests" data-name="interests"></ul>
				</div>

				<div class="edit-profile-field" style="margin: 10px 0 10px 0;">
					<label class="add_label">Skills: </label>
					<input type='text' rel="tooltip" title="List all of them! Employers search by skills! Separate skills by a comma please" id="skills" class='add_form' name='skills' maxlength='100' placeholder='Separate skills with a comma please.' value="<?php echo $userData['skills'] ?>" required />
				</div>
				
				<div class="edit-profile-field">
					<label class="add_label">Twitter Handle: </label>
					<input type="text" class="add_form" rel="tooltip" title="Not required" name="twitter" maxlength="50" placeholder="@yourHandle" value="<?php echo $userData['twitter']; ?>" /><span class="tip"> Optional</span>
				</div>
				
				<div class="edit-profile-field">
					<label class="add_label">Blog URL: </label>
					<input type="text" rel="tooltip" title="Not required" class="add_form" name="blog" maxlength="50" placeholder="http://you.blogspot.com" value="<?php echo $userData['blog']; ?>" /><span class="tip"> Optional</span>
				</div>
				<div>
					<input type="submit" class="btn btn-primary" name="submitPersonal" value="Update Profile" />
				</div>
			</div>
		</form>

	</div>


	
<?php	
	
	/***************************************************
	************* COMPANY EDIT PROFILE *****************
	****************************************************/	
	
	//else user IS a company
	} else {
	
		//get the company info
		$userData = $user->getUserInfo($user_id);
		$ind = new Industry($db);
		
		$companySlogan = stripslashes($userData['slogan']);
		$companyName = stripslashes($userData['company_name']);
		
		$indName = $ind->getIndustry($userData['industry']);
		
		if(strlen($userData['logo_path']) === 0) {
			$logo = "/images/company_default.png";
		} else {
			$logo = $userData['logo_path'];
		}
		
		if(strlen($userData['twitter']) === 0) {
			$twitter_expression = "";
		} else {
		
			//check if twitter handle has @ sign infront of it
			if(!(substr($userData['twitter'], 0, 1)==='@')) {
				$userData['twitter'] = '@'.$userData['twitter']; //add the @ sign if it doesnt exist for the url text
			} 
			if(substr($userData['twitter'], 0, 1) === '@') {
				$twitter_url = trim($userData['twitter'], '@'); // trim the @ sign from supplied twitter handle to get twitter url
			}
			//set full twitter URL
			$twitter_expression = "<a href='http://twitter.com/".$twitter_url."'>" .$userData['twitter']. "</a>";
		}
		
		if(strlen($userData['blog']) === 0) {
			$blog_url = "";
		} else {
			$blog_url = "<a href='".$userData['blog']."' target='_blank'>" .$userData['blog']. "</a>";
		}
?>

	<div class="user_specific_header">
		<h2>Welcome <?php echo $companyName; ?></h2><br />
		<h4>Please fill out the form below to complete your profile.</h4>
	</div>
	
	<div class="container">
	
		<div class="edit-profile-pages">
			<ul>
				<li><a class="active" href="#">Edit Profile</a></li>
				<li><a href="account-settings">Account Settings</a></li>
				<li><a href="#">Privacy Settings</a></li>
			</ul>
		
		</div>
		<?php
		
			if(isset($_GET['complete'])) {
				echo "<div style='margin: 5px;'><span class='success'>Profile update complete. <a href='/user/".$username."'>View Profile</a></span></div>";
			}
		
		?>
            <div class="upload_form_cont">
                <form id="upload_form" enctype="multipart/form-data" method="post" action="process/upload.process.php">
                    <div>
						<label class="edit_label">Upload Photo: </label>
                        <input type="file" name="image_file" id="image_file" />
						<input type="hidden" name="MAX_FILE_SIZE" value="1048000" />
                    </div>
					<?php
				
						if($_GET['photo'] === "success") {
							echo "<div style='margin: 5px;'><span class='success'>Photo Sucessfully Uploaded!</span></div>";
						}
				
					?>
                    <div>
                        <input type="submit" class="btn btn-primary" value="Upload" />
                    </div>
                </form>
            </div>
			
		<hr />
		<?php

	
	if(isset($_POST['submitPersonal'])) {
		
		$motto = $link->real_escape_string(check_input($_POST['motto'], "Please enter your company motto"));
		$location = $link->real_escape_string(check_input($_POST['location'], "Please enter your company location"));	
		$contactEmail = $link->real_escape_string(check_input($_POST['email'], "Please enter a contact email"));	
		$industry = $link->real_escape_string(intval(check_input($_POST['industry'], "Please select an industry")));
		$missionStatement = $link->real_escape_string(check_input($_POST['mission'], "Please enter a mission statement"));
		$description = $link->real_escape_string(htmlentities(check_input($_POST['description'], "Pleae enter a company description")));	
		$twitterHandle = $link->real_escape_string($_POST['twitter']);
		$blogURL = $link->real_escape_string($_POST['blog']);	
		
		if(!validate_email($contactEmail)) {
			echo "Invalid Email";
			die();
		}

		$updateQuery = $link->query("UPDATE companies SET email = '".$contactEmail."', industry = '".$industry."', slogan = '".$motto."', m_statement = '".$missionStatement."', location = '".$location."', twitter = '".$twitterHandle."', blog = '".$blogURL."', description = '".$description."' WHERE id = '".$user_id."'");
		
		if($updateQuery === FALSE) {
			printf("Query Failed: %s\n", $link->error);
		} else {
				
			//update the complete_prof field in the users table
			$profileQuery = $link->query("UPDATE users SET complete_prof = '1' WHERE user_id = '".$user_id."'");
			
			//set the profile session 
			$_SESSION['profile'] = TRUE;
			page_redirect('edit-profile?complete=1');
		}	
	}


?>	
	
		<form name="editProfile" method="POST" action="<?php echo $_SERVER['PHP_SELF']; ?>">
		
			<div class="edit-profile-form">
				<div>
					<label class="add_label">Motto: </label>
					<input type="text" class="add_form" name="motto" maxlength="140" placeholder="Company Slogan" value="<?php echo $companySlogan; ?>" required/>
				</div>
				
				<div>
					<label class="add_label">Location: </label>
					<input type="text" class="add_form" name="location" maxlength="50" placeholder="i.e. Kalamazoo, MI" value="<?php echo $location; ?>" required/>
				</div>
				
				<div>
					<label class="add_label">Contact Email: </label>
					<input type="text" class="add_form" name="email" maxlength="50" placeholder="Primary email address" value="<?php echo $email; ?>" required pattern="([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,6})" onchange="
	  this.setCustomValidity(this.validity.patternMismatch ? 'Invalid email format.' : ''); 
	  if(this.checkValidity()) form.confirmEmail.pattern = this.value;" />
				</div>
				
				<div>
					<label class="add_label">Industry: </label>
					<select class="add_select" name="industry" required>
						<option selected>Select Industry:</option>
						<?php
							$allInd = $link->query("SELECT * FROM industries");
							while ($allIndRow = $allInd->fetch_assoc()) {
								$indID = $link->real_escape_string(intval($allIndRow['ind_id']));
								$indName = $link->real_escape_string($allIndRow['name']);
								
								echo "<option value='".$indID."'>".$indName."</option>";
							}
						
						?>
					</select>
				</div>
				
				<div>
					<label class="add_label">Description: </label>
					<textarea name="description" class="add_txt" placeholder="Enter a company description" required><?php echo $description; ?></textarea>
				</div>
				
				<div>
					<label class="add_label">Mission Statement: </label>
					<textarea name="mission" class="add_txt" placeholder="Enter your mission statement" required><?php echo $m_statement; ?></textarea>
				</div>
			</div>
			
			
			<div class="edit-profile-form">
			
				<div>
					<label class="add_label">Twitter Handle: </label>
					<input type="text" class="add_form" name="twitter" maxlength="50" placeholder="@yourHandle" value="<?php echo $twitter; ?>" />
				</div>
				
				<div>
					<label class="add_label">Blog URL: </label>
					<input type="text" class="add_form" name="blog" maxlength="50" placeholder="http://you.blogspot.com" value="<?php echo $blog; ?>" />
				</div>
				<div>
					<input type="submit" class="btn btn-primary" name="submitPersonal" value="Update Profile" />
				</div>
			</div>
		</form>


<?php
	
	}//close if(!(isset($_SESSION['company'])))


if(!(isset($_SESSION['profile']))) {
	echo "<div class='modal-notice in' id='profileComplete'>
		<div class='modal-header'>
			<a class='close' data-dismiss='modal'>×</a>
			<h2 style='color: red; margin-top: 8px;'>Notice!</h3>
		</div>
		
		<div class='modal-body'>
			<p style='font-family: Verdana, sans-serif;'><b>Everything on this page (except your profile photo and fields labeled <em>optional</em> ) must be filled out in order to move on to the network. You may return to this page at anytime to edit your profile.</b></p>
			<p style='font-family: Verdana, sans-serif;'>Click \"<b>Ok</b>\" to continue or \"<b>Not Now</b>\" to logout.</p>
		</div>
		
		<div class='modal-footer'>
			<a href='#' class='btn btn-primary' data-dismiss='modal' style='width: 100px;'>OK</a>
			<a href='/logout' class='btn'>Not Now</a>
		</div>
	</div>";
}



//user is not logged in at all
} else {

	header('Location: login');

}//close if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'user_id')

include('includes/footer.php');

?>