<?php

session_start();

if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id') {
	header('location: /index');
} else {

	include('includes/anon.header.php');

	$errors = 0;

	if(isset($_GET['error'])) {
		echo "<div class='errors'>";
		echo "Incorrect email or password.";
		echo "</div>";
	}
	
	if(isset($_POST['submitLogin'])) {
		$db = new DBConnection;
		$user = new User($db);
	
		$login = $user->Login();
	
	}
	
	if(isset($_GET['newuser'])) {
		echo "<span class='success'>Please login with your new credentials</span>";
	}
?>
	<div class="login-form">
		<form name="userLogin" action="<?php echo $_SERVER['PHP_SELF']; ?>" method="post">
			<label>Email:</label><br />
			<input type="text" id="email" name="email" value="<?php if(isset($_POST['submitLogin'])) echo $_POST['email']; ?>" required pattern="([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,})" onchange="
  this.setCustomValidity(this.validity.patternMismatch ? 'Invalid email format.' : '');" /><br />
			<label>Password:</label><br />
			<input type="password" name="password" maxlength="20" style="height: 18px;" required/><br />
			<input type="hidden" name="token" value="<?php echo $token; ?>" />
			<input type="submit" name="submitLogin" class="btn btn-success" value="Login" />
		</form>
	</div>

<?php

} //close the session conditional

include('includes/footer.php');

?>