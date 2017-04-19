<?php 

session_start();

include('includes/db.config.php');
include('includes/functions.php'); 
	
//include all PHP classes
foreach (glob("classes/*.php") as $filename) {
	include_once($filename);
}

	echo "<?xml version='1.0' encoding='UTF-8'?>";

	$token = token();

	$_SESSION['token'] = $token;
?>

<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
<html xmlns='http://www.w3.org/1999/xhtml' xml:lang='en' lang='en'>
<head>

<meta http-equiv='Content-type' content='text/html;charset=UTF-8'> 
<meta name='viewport' content='width=device-width' />
<meta name='author' content='Tyler Bailey - HireStarts, LLC' />

<!-- Stylesheets -->
<link rel='stylesheet' href='styles/style.css'>
<link rel='stylesheet' href='styles/custom.bootstrap.css'>
<link rel='stylesheet' href='styles/beta.style.css'>
<link href='http://fonts.googleapis.com/css?family=Podkova:400,700' rel='stylesheet' type='text/css'>

<script type='text/javascript' src='http://ajax.googleapis.com/ajax/libs/jquery/1/jquery.min.js'></script>

<title><?php echo $site_name; ?></title> 

<script type="text/javascript">

  var _gaq = _gaq || [];
  _gaq.push(['_setAccount', 'UA-39025630-1']);
  _gaq.push(['_setDomainName', 'beta.hirestarts.com']);
  _gaq.push(['_trackPageview']);

  (function() {
    var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
    ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
  })();

</script>

</head>

<?php
	//flush the html document to send all compiled html to the browser
	flush();

	if(isset($_POST['submitLogin'])) {
		$db = new DBConnection;
		$user = new User($db);
		
		$login = $user->Login();
		
	}
?>
<body class='anon'>
	<header class='anon'>
		<div class='fixed_menu'>
			<div class='anon-logo'>
				<a href='index.php'><img src='images/logo.png'></a>
			</div>
			<div class='menu_login'>
				<form name='userLogin' method='POST' action='<?php $_SERVER['PHP_SELF'] ?>'>
					<label class='login'>Email:</label> <input type='text' name='email' maxlength='50' placeholder='Email Address' autofocus required />
					<label class='login'>Password:</label> <input type='password' name='password' maxlength='50' placeholder='Password' required/>
					<input type='hidden' name='token' value='".$token."' />
					<input type='submit' class='btn btn-primary' name='submitLogin' value='Login' style='margin-top: -3px;'/><br />
					<!--<span class='remember'><label class='login'>Remember Me?</label> <input type='checkbox' name='rememberMe' /></span>-->
				</form>
			</div>
		</div>
</header>

<div class="container">
