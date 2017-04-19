<?php 

session_start();

//if user is logged in
if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id' && isset($_SESSION['admin'])) {
	
	//set user_id
	$user_id = $_SESSION['user_id'];
	
	include('includes/user.header.php');
		
	echo "<div class='container' style='margin-top: 100px;'>";
		
	include('includes/admin.nav.php'); 
		
	if(isset($_POST['addUser'])) {

		$errorCount = 0;
		
		// get form values, escape them and apply the check_input function
		$firstName = $link->real_escape_string(check_input($_POST['firstName'], "Please enter a first name!"));
		$lastName = $link->real_escape_string(check_input($_POST['lastName'], "Please enter a last name!"));
		$username = $link->real_escape_string(check_input($_POST['username'], "Please enter a custom url!"));
		$email = $link->real_escape_string(check_input($_POST['email'], "Please enter an email!"));
		$password = check_input($_POST['password'], "Please enter a password!");
		$confirmedPass = check_input($_POST['password_confirm'], "Please confirm the password");
		
		if(!(validate_alphanumeric($firstName))) {
			echo("Invalid characters found in first name.");
			$errorCount += 1;
		}
		
		if(!(validate_alphanumeric($lastName))) {
			echo("Invalid characters found in last name.");
			$errorCount += 1;
		}
		
		if(!(validate_username($username))) {
			echo("The custom URL is invalid.");
			$errorCount += 1;
		}
		
		
		validate_email($email);
		//match passwords
		match_passwords($password, $confirmedPass);

		if($errorCount > 0) {
			echo "Please correct errors before continuing";
		} else {
		
			//make sure there isn't duplicate emails
			$emailQuery = $link->query("SELECT email, username FROM users WHERE email ='".$email."'");

			if ($emailQuery->num_rows >= 1) {
				echo "An email is already registered with that address";
				return false;
			}

			//make sure there isn't duplicate usernames
			$userQuery = $link->query("SELECT username FROM users WHERE username ='".$username."'");

			if ($userQuery->num_rows >= 1) {
				echo "That username is already registered.";
				return false;
			}

			// generate a random salt for converting passwords into MD5
			$salt = $link->real_escape_string(bin2hex(mcrypt_create_iv(32, MCRYPT_DEV_URANDOM)));
			$saltedPW =  $password . $salt;
			$hashedPW = sha1($saltedPW);

			mysqli_connect($db_host, $db_user, $db_pass) OR DIE (mysqli_error());
			// select the db
			mysqli_select_db ($link, $db_name) OR DIE ("Unable to select db".mysqli_error($db_name));

			// our sql query
			$sql = "INSERT INTO users (email, password, salt, username, company, admin, session, register_date) VALUES ('$email', '$hashedPW', '$salt', '$username', '0', '0', 'NULL', Now());";

			//save the updated information to the database			
			$result = mysqli_query($link, $sql) or die("Error in Query: " . mysqli_error($link));
			if (!mysqli_error($link)) {

				$row = mysqli_fetch_assoc($result);
				$_SESSION['user_id'] = mysqli_insert_id($link);
					
				$seekerQuery = $link->query("INSERT INTO seekers (id, first_name, last_name, username, email) VALUES ('".$_SESSION['user_id']."', '".$firstName."', '".$lastName."', '".$username."', '".$email."');");
					
				echo "User successfully added!";
			}
		}//close if($errorCount > 0)	
	}//close if(isset($_POST['userRegister']))
	
	?>
	
	<form name="addUser" method="post" action="<?php $_SERVER['PHP_SELF']; ?>" onSubmit="return checkForm(this);">
		<div>
			<label class="add_label">First Name: </label>
			<input type="text" id="firstName" name="firstName" required pattern="\w{2,}" onchange="
  this.setCustomValidity(this.validity.patternMismatch ? 'First name can only contain letters, numbers, underscores/hyphens.' : '');" />
		</div>
		
		<div>
			<label class="add_label">Last Name: </label>
			<input type="text" id="lastName" name="lastName" required pattern="\w{2,} ?\w+ ?\w+ ?\w+" onchange="
  this.setCustomValidity(this.validity.patternMismatch ? 'Last name can only contain letters, numbers, underscores/hyphens.' : '');" />
		</div>
		
		<div>
			<label class="add_label">Email: </label>
			<input type="text" id="email" name="email" required pattern="([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,6})" onchange="
  this.setCustomValidity(this.validity.patternMismatch ? 'Invalid email format.' : '');" />
		</div>
		
		<div>
			<label class="add_label">Custom URL: </label>
			<input type="text" id="username" name="username" required pattern="\w{3,}" onchange="
  this.setCustomValidity(this.validity.patternMismatch ? 'Custom URL must be 3 characters or longer and only contain letters, numbers, underscores/hyphens.' : '');" />
		</div>
		
		<div>
			<label class="add_label">Password: </label>
			<input type="password" id="password" name="password" style="height: 18px;" required pattern="(?=.*\d)(?=.*[a-z])\w{6,}" onchange="
  this.setCustomValidity(this.validity.patternMismatch ? 'Password must contain at least 6 characters, 1 UPPER CASE character, 1 lower case character, and 1 number.' : '');
  if(this.checkValidity()) form.pwd2.pattern = this.value;" />
		</div>
		
		<div>
			<label class="add_label">Confirm Password: </label>
			<input type="password" id="password_confirm" name="password_confirm" style="height: 18px;" required pattern="(?=.*\d)(?=.*[a-z])\w{6,}" name="pwd2" onchange="
  this.setCustomValidity(this.validity.patternMismatch ? 'Please enter the same Password as above.' : '');" />
		</div>
		
		<div>
			<input type="submit" name="addUser" class="btn btn-primary" value="Add User" />
		</div>
	</form>
</div>

<?php

} else {
	header("Location: index.php");
}//close if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'user_id')

include('includes/footer.php');

?>