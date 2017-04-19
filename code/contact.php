<?php

session_start();

//if user is logged in
if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id') {

	//set user_id
	$user_id = $_SESSION['user_id'];
	
	include('includes/user.header.php');	

} else {
	include('includes/anon.header.php');
} //close if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id')

$token = token();
?>

<div class="container" style="margin: 80px auto 15px auto; width: 80%;">
	<p>
	<h3>Contact HireStarts</h3>
	</p>
	<div class="container_left">
		<form name="contact" method="post" action="<?php echo $_SERVER['PHP_SELF']; ?>">
			<div>
				<label class="add_label">Name: </label>
				<input type="text" class="add_form" name="name" maxlength="55" placeholder="Enter your name" required/>
			</div>
			
			<div>
				<label class="add_label">Email: </label>
				<input type="text" class="add_form" name="email" maxlength="65" placeholder="Enter your email address" required/>
			</div>
			
			<div>
				<label class="add_label">Subject: </label>
				<select name="subject" class="add_select" required>
					<option value="default">Select Subject:</option>
					<option value="1">Network Support</option>
					<option value="2">Developer Support</option>
					<option value="3">Sales Inquiry</option>
					<option value="4">Feedback</option>
					<option value="5">General</option>
				</select>
			</div>
			
			<div>
				<label class="add_label">Message:</label>
				<textarea name="message" maxlength="600" class="add_txt" placeholder="Enter your message" required></textarea>
			</div>
			<input type="hidden" name="token" value="<?php echo $token; ?>" />
			
			<div>
				<input type="submit" class="btn btn-primary" name="submitContact" value="Send" />
			</div>
		</form>
	</div>
	<div class="container_right">
		<h3>This is where 6 social icons will go</h3>
		(Facebook) (Pinterest) (Twitter)<br />
		(LinkedIn) (StumbleUpon) (Reddit)
		<div style="margin-top: 25px; border-top: 1px solid #333; padding-top: 5px;">
		<h3>This is where a google map will go of our location with our office phone #, address, etc..</h3>
		</div>
	</div>
</div>


<?php

if(isset($_POST['submitContact'])) {
	
	$name = $link->real_escape_string($_POST['name']);
	$email = $link->real_escape_string($_POST['email']);
	
	if(!validate_email($email)) {
		echo "Invalid Email Address";
		die();
	}
	
	$userMessage = $link->real_escape_string($_POST['message']);
	
	$type = intval($_POST['subject']);
	
	if($type === 1) {
		$type = 'Network Support';
	} elseif ($type === 2) {
		$type = 'Developer Support';
	} else {
		if($type === 3) {
			$type = 'Sales Inquiry';
		} elseif ($type === 4) {
			$type = 'Feedback';
		} else {
			$type = 'General';
		}
	}
	
	$userMessage = $link->real_escape_string($_POST['message']);
	$formToken = $_POST['token'];
	
	if(!$formToken === $token) {
		echo "Invalid form token";
		die();
	}

	/* SEND USER EMAIL */
	$to = "info@hirestarts.com";
	$subject = "HireStarts Beta Contact Form - Type: ".$type;
	$headers = "From: no-reply@hirestarts.com". "\r\n";
	$headers .= "Reply-To: ".$email."\r\n";
	$headers .= "Cc: chase@hirestarts.com" . "\r\n";
	$headers .= "Cc: tyler@hirestarts.com" . "\r\n";
	$headers .= "MIME-Version: 1.0\r\n";
	$headers .= "Content-Type: text/html; charset=ISO-8859-1\r\n";
	$message = "<html><body style='color: #333; font-family: Verdana, sans-serif;'>";
	$message .= "<h1 style='color: #333; font-size: 1.5em;'> </h1>";
	$message .= "<b>Message:</b> ".stripslashes($userMessage);		
			
	$mail = mail($to, $subject, $message, $headers);
			
	echo "<span class='success'>Thank you for contacting us. We will be in touch with your shortly.</span>";
}

include("includes/footer.php");

?>