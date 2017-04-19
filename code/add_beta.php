<?php 

session_start();



//if user is logged in
if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id' && isset($_SESSION['admin'])) {
	
	//set user_id
	$user_id = $_SESSION['user_id'];
	
	include('includes/user.header.php');
	
	$db = new DBConnection;
		
	echo "<div class='container' style='margin-top: 100px;'>";
		
	include('includes/admin.nav.php'); 
		
	if(isset($_POST['addUser'])) {
		
		$email = $_POST['email'];
		
		//make sure there isn't duplicate emails
		$emailQuery = $db->query("SELECT email FROM users WHERE email ='".$email."'");

		if ($emailQuery->num_rows >= 1) {
			echo "An email is already registered with that address";
			die();
		}
		
		$actCode = sha1($email);
		
		// our sql query
		$sql = $db->query("INSERT INTO beta_u(email, activation) VALUES ('".$email."', '".$actCode."')");
	
		if (!$sql === FALSE) {
		
			//url to send the user in the email
			$actURL = "http://beta.hirestarts.com/beta-register?e=".$email."&c=".$actCode;
			
			/* SEND USER EMAIL */
			$to = $email;

			$subject = 'HireStarts Private Beta';

			$headers = "From: no-reply@hirestarts.com\r\n";
			$headers .= "Reply-To: info@hirestarts.com\r\n";
			$headers .= "MIME-Version: 1.0\r\n";
			$headers .= "Content-Type: text/html; charset=ISO-8859-1\r\n";
			$message = '<html><body style="color: #333; font-family: Verdana, sans-serif;">';
			$message .= '<h1 style="color: #333; font-size: 1.5em;">Thank you for your interest in <a href="http://www.hirestarts.com" style="color: #333;">HireStarts</a></h1>';
			$message .= '<p>You are receiving this email because you have requested an invitation to the <a href="http://beta.hirestarts.com">HireStarts Beta</a>.</p>';
			$message .= '<p>In order to activate your account on the new network, please click the link below, or copy and paste it into your web browser.</p>';
			$message .= '<p><a href="'.$actURL.'">'.$actURL.'</a></p>';
			$message .= '<p>Upon clicking the link you will be prompted to create a new password and activate your account. After you set your password you may login to the network freely. To login to the network after activation, please visit: <a href="http://beta.hirestarts.com" style="color: #333;">http://beta.hirestarts.com</a></p>';
			$message .= '<p>Please note that the current HireStarts network is still active, you may login to that site using your old credentials as normal.</p>';
			$message .= '<p>The purpose of this beta group is to test the network functionality with a small amount of users before releasing it to the public. Your job is to report any errors or bugs you find throughout the testing period. You may report any bugs or errors you come across using the provided link under the "You!" tab on the network navigation. </p>';
			$message .= '<p>Remember that this is a test group. You WILL come across some things on the network that are not yet functional. Do not be alarmed by this, please just copy and paste the error or take a screen shot (Print Screen on PC & Command+4 on Mac) and send it to us using the provided form.</p>';
			$message .= '<p>We hope that you will take advantage of this service we are offering and provide us with constructive feedback based upon your experience. <b>Help us, help you.</b></p>';
			$message .= '<p>Thank you for your time.</p>';
			$message .= '<p>HireStarts Management<br /><a href="mailto:info@hirestarts.com" style="color: #333;">info@hirestarts.com</a><br /><a href="http://www.hirestarts.com" style="color: #333;">www.HireStarts.com</a></p>';
			$message .= '</body></html>';
			
			$mail = mail($to, $subject, $message, $headers);
			
			echo "Email Successfully Sent!";
		}
					
	}
	
	?>
	
	<h1>Send user activation email: </h1>
	<form name="addUser" method="post" action="<?php $_SERVER['PHP_SELF']; ?>" onSubmit="return checkForm(this);">

		<div>
			<label class="add_label">Email: </label>
			<input type="text" id="email" name="email" required pattern="([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,6})" onchange="
  this.setCustomValidity(this.validity.patternMismatch ? 'Invalid email format.' : '');" />
		</div>
		<div>
			<input type="submit" name="addUser" class="btn btn-primary" value="Send Email" />
		</div>
	</form>
</div>

<?php

} else {
	header("Location: index.php");
}//close if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'user_id')

include('includes/footer.php');

?>