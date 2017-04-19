<?php

session_start();

//if user is logged in
if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id') {

	include('includes/user.header.php');
	
	//set user_id
	$user_id = $_SESSION['user_id'];
	
	$db = new DBConnection;
	$user = new User($db);
	
	$userData = $user->getUserInfo($user_id);
		
	//get username & email
	$username = $userData['username'];
	$email = $userData['email'];	

} else {
	include('includes/anon.header.php');
} //close if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id')

$token = token();
?>
	<p>
	<h3>We want to hear from you!</h3>
	</p>

	<form name="feedback" method="post" action="<?php echo $_SERVER['PHP_SELF']; ?>">
		<div class="edit-profile-field">
			<label class="add_label">Type: </label>
			<select name="subject" class="add_select" required>
				<option value="default">Select Type:</option>
				<option value="1">Error Report</option>
				<option value="2">Suggestion</option>
				<option value="3">Comment</option>
			</select>
		</div>
			
		<div class="edit-profile-field">
			<label class="add_label">Message:</label>
			<textarea name="message" maxlength="600" class="add_txt" placeholder="Enter your message" required></textarea>
		</div>
		<input type="hidden" name="token" value="<?php echo $token; ?>" />
			
		<div class="edit-profile-field">
			<input type="submit" class="btn btn-primary" name="submitFeedback" value="Send" />
		</div>
	</form>




<?php

if(isset($_POST['submitFeedback'])) {
	
	$type = intval($_POST['subject']);
	
	if($type === 1) {
		$type = 'Error Report';
	} elseif ($type === 2) {
		$type = 'Suggestion';
	} else {
		$type = 'Comment';
	}
	
	$userMessage = $db->real_escape_string($_POST['message']);
	$formToken = $_POST['token'];
	
	if(!$formToken === $token) {
		echo "Invalid form token";
		die();
	}

	/* SEND USER EMAIL */
	$to = "tyler@hirestarts.com";
	$subject = "HireStarts Beta Feedback - Type: ".$type;
	$headers = "From: no-reply@hirestarts.com". "\r\n";
	$headers .= "Reply-To: ".$email."\r\n";
	$headers .= "Cc: chase@hirestarts.com" . "\r\n";
	$headers .= "MIME-Version: 1.0\r\n";
	$headers .= "Content-Type: text/html; charset=ISO-8859-1\r\n";
	$message = "<html><body style='color: #333; font-family: Verdana, sans-serif;'>";
	$message .= "<h1 style='color: #333; font-size: 1.5em;'>You have network feedback from <a href='http://beta.hirestarts.com/user/".	$username."''>".$username."</a></h1>";
	$message .= "<b>Message:</b> ".stripslashes($userMessage);		
			
	$mail = mail($to, $subject, $message, $headers);
			
	echo "<span class='success'>Thank you for your feedback.</span>";
}


include("includes/footer.php");

?>