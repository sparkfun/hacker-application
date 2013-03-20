<?php 

session_start();

//if user is logged in
if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id') {
	
	//set user_id
	$user_id = intval($_SESSION['user_id']);
	
	include('includes/user.header.php');
	
	$db = new DBConnection;
	$user = new User($db);
	
	//get the user data from the database
	$userData = $user->getUserInfo($user_id);
	if(isset($_SESSION['company'])) {
		$name = $userData['company_name'];
		$email = $userData['email'];
	} else {
		$name = $userData['first_name'];
		$email = $userData['email'];
	}
?> 

	<div class="user_specific_header">
		<h2>Welcome <?php echo $name; ?></h2><br />
		<h4>Please fill out the form below to complete your profile.</h4>
		<div class="edit-profile-pages">
			<ul>
				<li><a href="edit-profile">Edit Profile</a></li>
				<li><a class="active" href="account-settings">Account Settings</a></li>
				<li><a href="#">Privacy Settings</a></li>
			</ul>
		
		</div>
	</div>
	
	<div class="edit-profile-form">	
	
		<form id="changePass" method="post" action="<?php echo $_SERVER['PHP_SELF']; ?>">

			<h3>Change Password: </h3>
			<div>
				<div style='width: 150px; text-align: right; margin-right: 25px; display: inline-block;'><b>Current Password:</div><input type="password" class='add_form' name="currentPass" placeholder="Current Password" required />
			</div>
			<div>
				<div style='width: 150px; text-align: right; margin-right: 25px; display: inline-block;'><b>New Password:</div><input type="password" class='add_form' name="newPass" placeholder="New Password" required pattern="(?=.*\d)(?=.*[a-z])\w{6,}" onchange="
  this.setCustomValidity(this.validity.patternMismatch ? 'Password must contain at least 6 characters and 1 number.' : '');
  if(this.checkValidity()) form.confirmPass.pattern = this.value;" />
			</div>
			<div>
				<div style='width: 150px; text-align: right; margin-right: 21px; display: inline-block;'><b>Confirm Password:</div> <input type="password" class='add_form' name="confirmPass" placeholder="Confirm Password" required pattern="(?=.*\d)(?=.*[a-z])\w{6,}" onchange="
  this.setCustomValidity(this.validity.patternMismatch ? 'Please enter the same Password as above.' : '');" />
			</div>
			<div style='margin-left: 175px'>
				<input type="submit" value="Change Password" name="changePass" class="btn btn-primary" />
			</div>

		</form>
		<div style='margin-top: 20px;'>
			<form id="changeEmail" method="post" action="<?php echo $_SERVER['PHP_SELF']; ?>">

				<h3>Update Email: </h3>
				<div>
					<div style='width: 150px; text-align: right; margin-right: 25px; display: inline-block;'><b>New Email:</div><input type="text" class='add_form' name="newEmail" placeholder="New Email Address" value="<?php echo $email; ?>" required pattern="([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,6})" onchange="
	  this.setCustomValidity(this.validity.patternMismatch ? 'Invalid email format.' : ''); 
	  if(this.checkValidity()) form.confirmEmail.pattern = this.value;" />
				</div>
				<div>
					<div style='width: 150px; text-align: right; margin-right: 21px; display: inline-block;'><b>Confirm Email:</div> <input type="text" class='add_form' name="confirmEmail" placeholder="Confirm Email" required pattern="([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,6})" onchange="
	  this.setCustomValidity(this.validity.patternMismatch ? 'Please enter the same Email Address as above.' : '');" />
				</div>
				<div style='margin-left: 175px'>
					<input type="submit" value="Update Email" name="changeEmail" class="btn btn-primary" />
				</div>

			</form>
		</div>

	</div>


	
<?php	

	if(isset($_POST['changePass'])) {
		$changePass = $user->changePass($user_id);
		
		if($changePass === FALSE) {
			echo "<span class='error'>Change Password Failed. Please contact network administrator.</span>";
		} else {
			echo "<span class='success'>Password successfully changed!</span>";
		}
	}
	
	//user is not logged in at all
} else {

	header('Location: login.php');

}//close if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'user_id')

include('includes/footer.php');

?>