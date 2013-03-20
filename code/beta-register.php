<?php 

session_start();

//if user is logged in
if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id') {
	header("Location: index.php");
}

include_once('includes/anon.header.php');


$db = new DBConnection;
$user = new User($db);
$validate = new data_validation;

$actEmail = $db->real_escape_string($_GET['e']);
$actCode = $db->real_escape_string($_GET['c']);

$actQuery = "SELECT * FROM beta_u WHERE email = '".$actEmail."' AND activation = '".$actCode."'";
//save the updated information to the database			
$actRes = $db->query($actQuery);
if (!$actRes === FALSE) {
			
	if($actRes->num_rows < 1) {
		echo "<div style='margin-top: 15%; margin-left: 15%;'><span class='error'>Invalid activation code.</span>";
		die();
	}


	if(!(isset($_GET['employer']))) {


		/***************************************************
		************* JOB SEEKER REGISTRATION **************
		****************************************************/

		if(isset($_POST['userRegister'])) {
			
			$register = $user->userRegister();
			
			if($register === FALSE) {
				echo "<span class='error'>Error with registration</span>";
			} else {
				page_redirect("/login?newuser");
			}
			
		}//close if(isset($_POST['userRegister']))

	?>


	<div class="anon-top">
		<div class="register-form">
			<h1>Welcome to the HireStarts Beta!</h1>
			<h4>Please fill out the form below to complete your activation.</h4>
			<table border="0" style="margin-top: 25px;">
				<form name="register" id="register" method="post" action="<?php $_SERVER['PHP_SELF']; ?>" onSubmit="return checkForm(this);">
					<tr>
						<td>
							<label>First Name:</label><br />
							<input type="text" rel="tooltip" title="What is your first name?" id="firstName" name="firstName" required pattern="\w{2,}" onchange="
	  this.setCustomValidity(this.validity.patternMismatch ? 'First name can only contain letters, numbers, underscores/hyphens.' : '');" /><br />
							<label>Last Name:</label><br />
							<input type="text" rel="tooltip" title="What is your last name?" id="lastName" name="lastName" required pattern="\w{2,} ?\w+ ?\w+ ?\w+" onchange="
	  this.setCustomValidity(this.validity.patternMismatch ? 'Last name can only contain letters, numbers, underscores/hyphens.' : '');" /><br />
							<label>Email Address:</label><br />
							<input type="text" rel="tooltip" title="What is your email address?" id="email" name="email" value="<?php echo $actEmail; ?>" required pattern="([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,6})" onchange="
	  this.setCustomValidity(this.validity.patternMismatch ? 'Invalid email format.' : '');" />
						</td>
					
						<td>
							<label>Custom URL:</label><br />
							<input type="text" rel="tooltip-left" title="Enter a username for your profile: <br />EX: hirestarts.com/YourCustomURL <br />(without the hirestarts.com/)" id="username" name="username" placeholder="Enter your custom URL here"  required pattern="\w{3,}" onchange="
	  this.setCustomValidity(this.validity.patternMismatch ? 'Custom URL must be 3 characters or longer and only contain letters, numbers, underscores/hyphens.' : '');" /><br />
							<label>Password:</label><br />
							<input type="password" rel="tooltip" title="Passwords must be at least 6 characters and contain a number" id="password" name="password" style="height: 18px;" required pattern="(?=.*\d)(?=.*[a-z])\w{6,}" onchange="
	  this.setCustomValidity(this.validity.patternMismatch ? 'Password must contain at least 6 characters and 1 number.' : '');
	  if(this.checkValidity()) form.pwd2.pattern = this.value;" /><br />
							<label>Confirm Password:</label><br />
							<input type="password" rel="tooltip" title="Enter your password again!" id="password_confirm" name="password_confirm" style="height: 18px;" required pattern="(?=.*\d)(?=.*[a-z])\w{6,}" name="pwd2" onchange="
	  this.setCustomValidity(this.validity.patternMismatch ? 'Please enter the same Password as above.' : '');" /><br />
						</td>
					</tr>
					<tr>
						<td>
							<p>
								I agree to the <a href="/terms">Terms of Service</a> 
								<input type="checkbox" name="agree" />
							</p>
						</td>
					</tr>
				
					<tr>
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
			$companyName = $db->real_escape_string(check_input($_POST['companyName'], "Please enter your company name!"));
			$contactEmail = $db->real_escape_string(check_input($_POST['email'], "Please provide a contact email!"));
			$username = $db->real_escape_string(check_input($_POST['username'], "Please enter a custom url!"));
			$password = check_input($_POST['password'], "Please enter a password!");
			$confirmedPass = check_input($_POST['password_confirm'], "Please confirm your password!");
			$industry = $db->real_escape_string(check_input($_POST['industry'], "Please enter your work industry"));
			$agree = intval($_POST['agree']);

			echo("<div class='errors'>");
			
			if($agree === FALSE) {
				echo "You must agree to the terms of service";
				$errorCount += 1;
			}
			
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
				$emailQuery = $db->query("SELECT email FROM users WHERE email ='".$contactEmail."'");
				
				//error indicators
				$emailError = false;
				$usernameError = false;
				
				if (mysqli_num_rows($emailQuery) >= 1) {
					$emailError = true;
					$emailTaken = "That email address is already registered in our database.";
					return false;
				}

				//make sure there isn't duplicate usernames
				$userQuery = $db->query("SELECT username FROM users WHERE username ='".$username."'");

				if (mysqli_num_rows($userQuery) >= 1) {
					$usernameError = true;
					$usernameTaken = "That custom URL is already registered in our database.";
					return false;
				}

				// generate a random salt for converting passwords into MD5
				$salt = $db->real_escape_string(bin2hex(mcrypt_create_iv(32, MCRYPT_DEV_URANDOM)));
				$saltedPW =  $password . $salt;
				$hashedPW = sha1($saltedPW);

				mysqli_connect($db_host, $db_user, $db_pass) OR DIE (mysqli_error());
				// select the db
				mysqli_select_db ($db, $db_name) OR DIE ("Unable to select db".mysqli_error($db_name));

				// our sql query
				$sql = "INSERT INTO users (email, password, salt, username, company, register_date, last_login, complete_prof) VALUES ('$contactEmail', '$hashedPW', '$salt', '$username', '1', Now(), Now(), '0');";

				//save the updated information to the database			
				$result = mysqli_query($db, $sql) or die("Error in Query: " . mysqli_error($db));
				if (!mysqli_error($db)) {

					$row = mysqli_fetch_assoc($result);
					$_SESSION['user_id'] = mysqli_insert_id($db);
					$_SESSION['loggedin'] = TRUE;
					$_SESSION['company'] = TRUE;
						
					$userID = $_SESSION['user_id'];
					$companyQuery = $db->query("INSERT INTO companies (id, company_name, email, password, salt, username, industry) VALUES ('$userID', '$companyName', '$contactEmail', '$hashedPW', '$salt', '$username', '$industry');");
						
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
							<input type="text" id="email" name="email" value="<?php echo $actEmail; ?>" required pattern="([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,6})" onchange="
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
}
include('includes/footer.php'); 
?>