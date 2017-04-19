<?php 

session_start();

//check if there is a cookie on users pc
if(isset($_COOKIE['username']) && isset($_COOKIE['logged'])) {
	
	$cookieUser = $_COOKIE['username'];
	$cookieSalt = $_COOKIE['logged'];
	
	include_once('includes/config.php');
	
	//match the cookie information with the database information
	$cookieSQL = $link->query("SELECT user_id, username, salt FROM users WHERE username = '".$cookieUser."' AND salt = '" .$cookieSalt. "'");
	
	if(mysqli_num_rows($cookieSQL) === 1) {
	
		$cookieRow = mysqli_fetch_assoc($cookieSQL);
		
		$user_id = intval($cookieRow['user_id']);
		$_SESSION['loggedin'] === TRUE;
		$_SESSION['user_id'] === $user_id;
	} else {
		echo "Invalid cookie information. Please login again.";
	}
	
}

//if user is logged in
if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id') {
	header("Location: index.php");
}

include_once('includes/anon.header.php');

if(!(isset($_GET['employer']))) {


	/***************************************************
	************* JOB SEEKER REGISTRATION **************
	****************************************************/

	if(isset($_POST['userRegister'])) {

		$errorCount = 0;
		
		// get form values, escape them and apply the check_input function
		$firstName = $link->real_escape_string(check_input($_POST['firstName'], "Please enter your first name!"));
		$lastName = $link->real_escape_string(check_input($_POST['lastName'], "Please enter your last name!"));
		$username = $link->real_escape_string(check_input($_POST['username'], "Please enter a custom url!"));
		$email = $link->real_escape_string(check_input($_POST['email'], "Please enter an email!"));
		$password = check_input($_POST['password'], "Please enter a password!");
		$confirmedPass = check_input($_POST['password_confirm'], "Please confirm your password");
		
		echo("<div class='errors'>");
		
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
		
		if(!(validate_email($email))) {
			echo("Invalid email format.");
			$errorCount += 1;
		}
		
		if(!(match_passwords($password, $confirmedPass))) {
			echo("Your passwords do not match!");
			$errorCount += 1;
		}

		if($errorCount > 0) {
			echo "Please correct errors before continuing";
		} else {
		
			//make sure there isn't duplicate emails
			$emailQuery = $link->query("SELECT email FROM users WHERE email ='".$email."'");

			//error indicators
			$emailError = false;
			$usernameError = false;
			
			//no duplicates?
			if (mysqli_num_rows($emailQuery) >= 1) {
				$emailError = true;
				$emailTaken = "That email address is already registered in our database.";
				return false;
			}

			//make sure there isn't duplicate usernames
			$userQuery = $link->query("SELECT username FROM users WHERE username ='".$username."'");

			if (mysqli_num_rows($userQuery) >= 1) {
				$usernameError = true;
				$usernameTaken = "That custom URL is already registered in our database.";
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
			$sql = "INSERT INTO users (email, password, salt, username, company) VALUES ('$email', '$hashedPW', '$salt', '$username', '0');";

			//save the updated information to the database			
			$result = mysqli_query($link, $sql) or die("Error in Query: " . mysqli_error($link));
			if (!mysqli_error($link)) {

				$row = mysqli_fetch_assoc($result);
				$_SESSION['user_id'] = mysqli_insert_id($link);
				$_SESSION['loggedin'] = TRUE;
				
				setcookie('username', sha1($username), time()+8460);// 1 day
				setcookie('logged', $salt, time()+8460);// 1 day
					
				$userID = $_SESSION['user_id'];
				$seekerQuery = $link->query("INSERT INTO seekers (id, first_name, last_name, username, email) VALUES ('$userID', '$firstName', '$lastName', '$username', '$email');");
					
				header("Location: edit-profile.php");
			}
		}//close if($errorCount > 0)	
		echo("</div>"); //close errors <div>
	}//close if(isset($_POST['userRegister']))

?>


<div class="anon-top">
	<div class="register-form">
		<table border="0">
			<form name="register" id="register" method="post" action="<?php $_SERVER['PHP_SELF']; ?>" onSubmit="return checkForm(this);">
				<tr>
					<td>
						<label>First Name:</label><br />
						<input type="text" id="firstName" name="firstName" value="<?php echo $firstName; ?>" required pattern="\w{2,}" onchange="
  this.setCustomValidity(this.validity.patternMismatch ? 'First name can only contain letters, numbers, underscores/hyphens.' : '');" /><br />
						<label>Last Name:</label><br />
						<input type="text" id="lastName" name="lastName" value="<?php echo $lastName; ?>" required pattern="\w{2,} ?\w+ ?\w+ ?\w+" onchange="
  this.setCustomValidity(this.validity.patternMismatch ? 'Last name can only contain letters, numbers, underscores/hyphens.' : '');" /><br />
						<label>Email Address:</label><br />
						<input type="text" id="email" name="email" value="<?php echo $email; ?>" required pattern="([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,6})" onchange="
  this.setCustomValidity(this.validity.patternMismatch ? 'Invalid email format.' : '');" />
					</td>
				
					<td>
						<label>Custom URL:</label><br />
						<input type="text" id="username" name="username" value="<?php echo $username; ?>"  required pattern="\w{3,}" onchange="
  this.setCustomValidity(this.validity.patternMismatch ? 'Custom URL must be 3 characters or longer and only contain letters, numbers, underscores/hyphens.' : '');" /><br />
						<label>Password:</label><br />
						<input type="password" id="password" name="password" style="height: 18px;" required pattern="(?=.*\d)(?=.*[a-z])\w{6,}" onchange="
  this.setCustomValidity(this.validity.patternMismatch ? 'Password must contain at least 6 characters and 1 number.' : '');
  if(this.checkValidity()) form.pwd2.pattern = this.value;" /><br />
						<label>Confirm Password:</label><br />
						<input type="password" id="password_confirm" name="password_confirm" style="height: 18px;" required pattern="(?=.*\d)(?=.*[a-z])\w{6,}" name="pwd2" onchange="
  this.setCustomValidity(this.validity.patternMismatch ? 'Please enter the same Password as above.' : '');" /><br />
					</td>
				</tr>
				
				<tr>
					<td>
					<?php 
						/* CAPTCHA
						require_once('recaptchalib.php');
						$publickey = "6LehxNkSAAAAAPcSi6br9FtzYKC_DVOn_B-LcgwH"; // you got this from the signup page
						echo recaptcha_get_html($publickey); 	
						*/
					?>
					</td>
				</tr>
			
				<tr>
					<td>
						<p>
							Already registered? <a href="login.php">Click here to login!</a>
						</p>
					</td>
					<td>
						<p>
							<input type="submit" name="userRegister" value="Register" style="margin-left: 18px;">
						</p>
					</td>
				</tr>
			</form>
		</table>
	</div>
</div>

<div class="anon-middle">

</div>

<div class="anon-bottom">

</div>

<?php 
} else {

	/***************************************************
	************* COMPANY REGISTRATION *****************
	****************************************************/

	if(isset($_POST['companyRegister'])) {
	
		$errorCount = 0;
			
		// get form values, escape them and apply the check_input function
		$companyName = $link->real_escape_string(check_input($_POST['companyName'], "Please enter your company name!"));
		$contactEmail = $link->real_escape_string(check_input($_POST['email'], "Please provide a contact email!"));
		$username = $link->real_escape_string(check_input($_POST['username'], "Please enter a custom url!"));
		$password = check_input($_POST['password'], "Please enter a password!");
		$confirmedPass = check_input($_POST['password_confirm'], "Please confirm your password!");
		$industry = $link->real_escape_string(check_input($_POST['industry'], "Please enter your work industry"));

		echo("<div class='errors'>");
		
		if(!(validate_string($companyName))) {
			echo("Invalid characters found in your company name.");
			$errorCount += 1;
		}
		
		if(!(validate_username($username))) {
			echo("The custom URL is invalid.");
			$errorCount += 1;
		}
		
		if(!(validate_string($industry))) {
			echo("Invalid characters found in your industry.");
			$errorCount += 1;
		}
		
		if(!(validate_email($contactEmail))) {
			echo("Invalid email format.");
			$errorCount += 1;
		}
		
		if(!(match_passwords($password, $confirmedPass))) {
			echo("Your passwords do not match!");
			$errorCount += 1;
		}
		
		if($errorCount > 0) {
			echo "Please correct errors before continuing.";
		} else {
		
			//make sure there isn't duplicate emails
			$emailQuery = $link->query("SELECT email FROM users WHERE email ='".$contactEmail."'");
			
			//error indicators
			$emailError = false;
			$usernameError = false;
			
			if (mysqli_num_rows($emailQuery) >= 1) {
				$emailError = true;
				$emailTaken = "That email address is already registered in our database.";
				return false;
			}

			//make sure there isn't duplicate usernames
			$userQuery = $link->query("SELECT username FROM users WHERE username ='".$username."'");

			if (mysqli_num_rows($userQuery) >= 1) {
				$usernameError = true;
				$usernameTaken = "That custom URL is already registered in our database.";
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
			$sql = "INSERT INTO users (email, password, salt, username, company) VALUES ('$contactEmail', '$hashedPW', '$salt', '$username', '1');";

			//save the updated information to the database			
			$result = mysqli_query($link, $sql) or die("Error in Query: " . mysqli_error($link));
			if (!mysqli_error($link)) {

				$row = mysqli_fetch_assoc($result);
				$_SESSION['user_id'] = mysqli_insert_id($link);
				$_SESSION['loggedin'] = TRUE;
				$_SESSION['company'] = TRUE;
					
				$userID = $_SESSION['user_id'];
				$companyQuery = $link->query("INSERT INTO companies (id, company_name, email, password, salt, username, industry) VALUES ('$userID', '$companyName', '$contactEmail', '$hashedPW', '$salt', '$username', '$industry');");
					
				header("Location: edit-profile.php");
			}
		}//close if($errorCount > 0)
		echo("</div>"); //close errors <div>
	}//close if(isset($_POST['companyRegister']))

?>
<div class="anon-top">
	<div class="register-form">
				<table border="0">
			<form name="register" id="register" method="post" action="<?php $_SERVER['PHP_SELF']; ?>">
				<tr>
					<td>
						<label>Company Name:</label><br />
						<input type="text" name="companyName" value="<?php echo $companyName; ?>" required pattern="\w+? ?\w+ ?\w+ ?\w+" onchange="
  this.setCustomValidity(this.validity.patternMismatch ? 'Company name can only contain letters, numbers, underscores/hyphens.' : '');" /><br />
						<label>Industry:</label><br />
						<input type="text" name="industry" value="<?php echo $industry; ?>" required pattern="\w+? ?\w+ ?\w+ ?\w+" onchange="
  this.setCustomValidity(this.validity.patternMismatch ? 'Company industry can only contain letters, numbers, underscores/hyphens.' : '');" /><br />
						<label>Contact Email:</label><br />
						<input type="text" id="email" name="email" value="<?php echo $email; ?>" required pattern="([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,6})" onchange="
  this.setCustomValidity(this.validity.patternMismatch ? 'Invalid email format.' : '');" /><?php if($usernameError === TRUE) { echo $usernameTaken; } ?><br />
					</td>
				
					<td>
						<label>Custom URL:</label><br />
						<input type="text" id="username" name="username" value="<?php echo $username; ?>" required pattern="\w{3,}" onchange="
  this.setCustomValidity(this.validity.patternMismatch ? 'Custom URL must be 3 characters or longer and only contain letters, numbers, underscores/hyphens.' : '');" /><?php if($usernameError === TRUE) echo($usernameTaken); ?><br />
						<label>Password:</label><br />
						<input type="password" id="password" name="password" value="<?php echo $password; ?>" style="height: 18px;" required pattern="(?=.*\d)(?=.*[a-z])\w{6,}" onchange="
  this.setCustomValidity(this.validity.patternMismatch ? 'Password must contain at least 6 characters and 1 number.' : '');
  if(this.checkValidity()) form.pwd2.pattern = this.value;" /><br />
						<label>Confirm Password:</label><br />
						<input type="password" id="password_confirm" name="password_confirm" value="<?php echo $password_confirm; ?>" style="height: 18px;" required pattern="(?=.*\d)(?=.*[a-z])\w{6,}" name="pwd2" onchange="
  this.setCustomValidity(this.validity.patternMismatch ? 'Please enter the same Password as above.' : '');" /><br />
					</td>
				</tr>
				
				<tr>
					<td>
					<?php 
						/* CAPTCHA
						require_once('recaptchalib.php');
						$publickey = "6LehxNkSAAAAAPcSi6br9FtzYKC_DVOn_B-LcgwH"; // you got this from the signup page
						echo recaptcha_get_html($publickey); 	
						*/
					?>
					</td>
				</tr>
				
				<tr>
					<td>
						<p>
							Already registered? <a href="login.php">Click here to login!</a>
						</p>
					</td>
					<td>
						<p>
							<input type="submit" name="companyRegister" value="Register"  style="margin-left: 18px;">
						</p>
					</td>
				</tr>
			</form>
		</table>
	</div>
</div>

<div class="anon-middle">

</div>

<div class="anon-bottom">

</div>
<?php

} //end employer conditional

include('includes/footer.php'); 
?>