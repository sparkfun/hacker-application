<?php 

session_start();

//if user is logged in
if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id') {

	//is the users profile complete?
	if(!(isset($_SESSION['profile']))) {
		Header('Location: edit-profile');
	}

	include('includes/user.header.php');
	
	//set user_id
	$user_id = intval($_SESSION['user_id']);
	
	//istantiate database object
	$db = new DBConnection;
	//istantiate user object
	$user = new User($db);
	$validate = new data_validation;
	
	if(!(isset($_SESSION['company']))) {
		
		//get the user header
		$userData = $user->getUserInfo($user_id);
		
		/* 
			piece user information together properly for display
		*/
			$fullName = $userData['first_name']. " " .$userData['last_name'];
			
			if(strlen($userData['photo_path']) <= 0) {
				$photo_path = '/images/default_image.gif';
			} else {
				$photo_path = $userData['photo_path'];
			}

			
			$major = stripslashes($userData['major']);

			$todays_date = date("Y-m-d");
			$todays_date = strtotime($todays_date);
			$graduationDate = strtotime($userData['graduation_date']);
			
			$graduated_expression = '';
			//if user has not graduated, expression is "attends" else expression is "Attended"
			if($graduationDate < $todays_date) {
				$graduated_expression = "Attended " .$userData['school']. " for " .$major. " <span class='tip'>(graduated)</span>";
			} else {
				$graduated_expression = "Attends " .$userData['school']. " for " .$major;
			}

		
			//if user has not supplied work info, do not display anything
			if(strlen($userData['employer']) > 0) {
				$employerExpression = $userData['position']. " at " .$userData['employer'];
			} else {
				$employerExpression = '';
			}
			
			$twitter = $userData['twitter'];
			//check if twitter handle has @ sign infront of it
			if(!(substr($userData['twitter'], 0, 1)==='@')) {
				$twitter = '@'.$userData['twitter']; //add the @ sign if it doesnt exist for the url text
			} 
			if(substr($userData['twitter'], 0, 1) === '@') {
				$twitter = trim($twitter, '@'); // trim the @ sign from supplied twitter handle to get twitter url
				$twitter_url = "http://www.twitter.com/".$twitter;
			}

	?>
	
		<div class='user_specific_header'>
		
			<div class='user_img'>
				<img src='<?php echo $photo_path; ?>'>
			</div>
			
			<div class='user_info'>
			
				<span class='user_name'><?php echo $fullName; ?></span>
				<span class='user_specs'><?php echo $graduated_expression; ?><br />
					<?php echo $employerExpression; ?>
				</span>
				
				<div class='user_social'>
					<ul>
						<li><a href='<?php echo $twitter_url; ?>' target='_blank'><?php echo $userData['twitter']; ?></a></li>
						<li><a href='<?php echo $userData['blog']; ?>' target='_blank'><?php echo $userData['blog']; ?></a></li>
					</ul>
				</div>
			
			</div>
		
			<div class='user_specific_header_right'>
				<a class='btn' rel='tooltip' title='View your profile as others see it' href='user/<?php echo $userData['username']; ?>'>Public Profile</a>
				<a class='btn' rel='tooltip' title='Edit Profile Information' href='edit-profile'>Edit Profile</a>
			</div>
		</div>
	
	<?php
	} else {
		
		//get the company header
		$userData = $user->getUserInfo($user_id);
		$ind = new Industry($db);
		
		$companySlogan = stripslashes($userData['slogan']);
		
		$indName = $ind->getIndustry($userData['industry']);
		
		if(strlen($userData['logo_path']) === 0) {
			$logo = "/images/company_default.png";
		} else {
			$logo = $userData['logo_path'];
		}
		
		if(strlen($userData['twitter']) === 0) {
			$twitter_expression = "";
		} else {
		
			//check if twitter handle has @ sign infront of it
			if(!(substr($userData['twitter'], 0, 1)==='@')) {
				$userData['twitter'] = '@'.$userData['twitter']; //add the @ sign if it doesnt exist for the url text
			} 
			if(substr($userData['twitter'], 0, 1) === '@') {
				$twitter_url = trim($userData['twitter'], '@'); // trim the @ sign from supplied twitter handle to get twitter url
			}
			//set full twitter URL
			$twitter_expression = "<a href='http://twitter.com/".$twitter_url."'>" .$userData['twitter']. "</a>";
		}
		
		if(strlen($userData['blog']) === 0) {
			$blog_url = "";
		} else {
			$blog_url = "<a href='".$userData['blog']."' target='_blank'>" .$userData['blog']. "</a>";
		}
	
	?>
	
		<div class='user_specific_header'>
			<div class='user_img'>
				<img src='<?php echo $logo; ?>'>
			</div>
			<div class='user_info'>
				<span class='user_name'><?php echo $userData['company_name']; ?></span>
				<span class='company_slogan'> - "<?php echo $companySlogan; ?>"</span>
				<span class='user_specs'>
					Located in:<a href='#'><?php echo $userData['location']; ?></a> - <a href='/members?industry=<?php echo $userData['industry']; ?>'><?php echo $indName; ?></a><br />
				</span>
				<div class='user_social'>
					<ul>
						<li><a href='http://twitter.com/<?php echo $twitter_url; ?>' target='_blank'><?php echo $twitter_url; ?></a></li>
						<li><?php echo $blog_url; ?></li>
					</ul>
				</div>
			</div>
			<div class='user_specific_header_right'>
				<a class='btn' rel='tooltip' title='View your profile as others see it' href='company/<?php echo $username; ?>'>Public Profile</a>
				<a class='btn' rel='tooltip' title='Edit Profile Information' href='edit-profile'>Edit Profile</a>
			</div>
		</div>
	
	<?php
	
	}
	
	echo "<div class='blockContainer'>";
		
		$limit = "8";
		//istantiate Blocks object
		$blocks = new Blocks($db);		
		$start = $blocks->getRecentPost();
		echo $blocks->getBlocks($start, $limit);
			
	echo "</div>";

	?>
	<script type="text/javascript">
	//block infinite scroll
	var loading = false;
    $(window).scroll(function(){
        var h = $('.blockContainer').height();
        var st = $(window).scrollTop();
 
        if(st >= 0.4*h && !loading && h > 300){
            loading = true;
            $('#ajaxLoader').show();
            $.ajax({
                url: "/ajax/blocks.process.php?lastid=" + $(".masonryBlock:last").attr("id"),
                success: function(html){
					console.log(html);
                    if(html){
                        $(".blockContainer").append(html).masonry('reload');
                        $('div#ajaxLoader').hide();
                    }else{
                        $('div#ajaxLoader').html('<center>No more posts to show.</center>');
                    }
                    loading = false;
                }
            });
        }
    });
	</script>
	<div id="ajaxLoader"></div>
<?php
	
//if the user is NOT LOGGED IN
} else {

	include_once('includes/anon.header.php');
	
	$validate = new data_validation;
	
?>
		<iframe width="450" height="271" src="http://www.youtube.com/embed/_i3C2RuKpBw?feature=player_detailpage" frameborder="0" allowfullscreen></iframe>

			<div style='float: right;' class='beta_signup'>
				<span class='beta_text_lrg'>We're in beta!</span><br />
				<span class='beta_text_med'>Care to join us?</span>
				<form class="" method="POST" action="<?php echo $_SERVER['PHP_SELF']; ?>">
					<input type="text" class="betaEmail" name="email" maxlength="55" placeholder="Enter your email address" />
					<input type="submit" name="submitEmail" class="btn-large btn-primary" style="height: 38px; position: relative; bottom: 1px; padding-top: 7px; cursor:pointer;" value="Submit" />
				</form>
				<div class="small">
					Already have an account? <a href="/login">Login!</a>
				</div>
			</div>
</div>

<?php

	/* SEND USER EMAIL */
		if(isset($_POST['submitEmail'])) {
			$to = "tyler@hirestarts.com, chase@hirestarts.com";
			$email = $_POST['email'];
			
			if(!$validate->validate_email($email)) {
				echo "Invalid email address";
				die();
			}
			$subject = 'Beta Invitation Request';

			$headers = "From: no-reply@hirestarts.com\r\n";
			$headers .= "Reply-To: info@hirestarts.com\r\n";
			$headers .= "MIME-Version: 1.0\r\n";
			$headers .= "Content-Type: text/html; charset=ISO-8859-1\r\n";
			$message = '<html><body style="color: #333; font-family: Verdana, sans-serif;">';
			$message .= '<h1 style="color: #333; font-size: 1.5em;">New Beta Invite Request!</h1>';
			$message .= '<p>'.$email.' is requesting to become a beta tester!</p>';
			
			$mail = mail($to, $subject, $message, $headers);
			
			echo "<center><span class='success'>Email Successfully Sent!</span></center>";
		}
	}
	

include('includes/footer.php');

?>