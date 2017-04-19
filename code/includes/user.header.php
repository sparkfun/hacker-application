<?php 

session_start();

//include the required files
include('includes/functions.php');

//include all PHP classes
foreach (glob("classes/*.php") as $filename) {
	include_once($filename);
}

//if user is an administrator, include the gapi class
if(isset($_SESSION['admin'])) {
	include('classes/gapi.class.php');
	include('classes/admin.class.php');
}

$_SESSION['token'] = token();

//set user_id variable
$user_id = $_SESSION['user_id'];

//istantiate database and user object
$db = new DBConnection;
$user = new User($db);

//get the user data
$userData = $user->getUserInfo($user_id);
//set the username for profile under the 'You!' tab
$username = $userData['username'];

echo "<?xml version='1.0' encoding='UTF-8'?>";
?>
<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
<html xmlns='http://www.w3.org/1999/xhtml' xml:lang='en' lang='en'>
<head>
<meta http-equiv='Content-type' content='text/html;charset=UTF-8'> 
<meta name='viewport' content='width=device-width, initial-scale=1.0'>
<meta name='author' content='Tyler Bailey - HireStarts, LLC' />

<!-- Stylesheets -->
<link type='text/css' rel='stylesheet' href='/styles/style.css'>
<link type='text/css' rel='stylesheet' href='/styles/responsive.css'>
<link type='text/css' rel='stylesheet' href='/styles/custom.bootstrap.css'>
<link type='text/css' rel='stylesheet' href='http://ajax.googleapis.com/ajax/libs/jqueryui/1.9.0/themes/cupertino/jquery-ui.css'>
<link href='http://fonts.googleapis.com/css?family=Podkova:400,700' rel='stylesheet' type='text/css'>

<script type='text/javascript' src='http://ajax.googleapis.com/ajax/libs/jquery/1/jquery.min.js'></script>


<!--[if lte IE 9]>
	<script src="http://css3-mediaqueries-js.googlecode.com/svn/trunk/css3-mediaqueries.js"></script>
	<script type="text/javascript">
        $(document).ready(function() {
			$('.masonryBlock').css('opacity', 1);
		});
    </script>
<![endif]-->



<title>HireStarts</title>

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
?>

<body>
	<div id='wrapper'>
		<header>
			<div class='fixed_menu'>
				<div class='logo'>
					<a href='/index.php'><img src='/images/logo.png'></a>
				</div>
				<div class='menu_right'>
				<?php
					//is the users profile complete?
					if(!(isset($_SESSION['profile']))) {
						echo "<ul>";
						echo "<li class='nav_item'>You must complete your profile before moving on.</li>";
						echo "</ul>";
					} else {
				?>
					<ul>
						<li class='nav_item'><a href='/index'>Home</a></li>
						<li class='nav_item'><a href='/blog'>Blog</a></li>
						<li class='nav_item'><a href='/members'>Members</a></li>
						<li class='nav_item'><a href='/opportunities'>Opportunities</a></li>
						<li class='nav_item dropdown'>
							<a data-toggle='dropdown' class='dropdown-toggle'>You!
								<b class='caret'></b>
							</a>
							<ul class='dropdown-menu'>
								<?php
									if(!(isset($_SESSION['company']))) {
								?>
								<li><a href='/user/<?php echo $username; ?>'>Public Profile</a></li>
								<li><a href='/account-settings'>Account Settings</a></li>
								<li><a href='/edit-profile'>Edit Profile</a></li>
								<li><a href='/edit-profile'>Privacy Settings</a></li>
								<?php
									} else {
								?>
								<li><a href='/company/<?php echo $username; ?>'>Public Profile</a></li>
								<li><a href='/opportunities'>Post Opportunity</a></li>
								<li><a href='/account-settings'>Account Settings</a></li>
								<li><a href='/edit-profile'>Edit Profile</a></li>
								<li><a href='/edit-profile'>Privacy Settings</a></li>
								<?php
									}
								?>
								<li><a href='/feedback'>Beta Feedback</a></li>
								<?php
									if(isset($_SESSION['admin'])) {
										echo "<li><a href='/adash'>Admin Dashboard</a></li>";
									}
								?>

								<li class='divider'></li>
								<li><a href='/logout'>Logout</a></li>
							</ul>
						</li>
					
						<li><a href='#'><img rel='tooltip' title='Coming Soon!' class='jewel' src='/images/mail.png' 
											onmouseover='this.src="/images/mail2.png"'
											onmouseout='this.src="/images/mail.png"'></a>
						</li>
					</ul>
						
				<?php	
					}//close if(!(isset($_SESSION['profile'])))	
				?>
					
				</div>
			</div>

		</header>
		<div class="container">
	<noscript>
		<div id='NoScript'>
			<h1>You need to enable Javascript to view this site.</h1>
		</div>
	</noscript>
