<?php 

session_start();

//if user is logged in
if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id') {

	//is the users profile complete?
	if(!(isset($_SESSION['profile']))) {
		Header('Location: /edit-profile');
	}
	
	//set user_id
	$user_id = $_SESSION['user_id'];
	
	include('includes/user.header.php');
	
	//istantiate all the required objects
	$db = new DBConnection;
	$tag = new Tag($db);
	$user = new User($db);
	$industry = new Industry($db);
	$str = new str_format;
	$validate = new data_validation;
	
	//get the start user id and set a limit for the mysql query for ajax infinite scroll
	$start = $user->getLastUserId();
	$limit = '20';

	
	?>
		<div class='sidebar'>
			<form class='side-search form-search' name='user-search' method='GET' action='<?php $_SERVER['PHP_SELF']; ?>'>
				<input type='text' class='job_search search-query' name='q' placeholder='Search by name, school, major, skills' />
				<input type='hidden' name='t' value="<?php echo token(); ?>" />
				<input type='submit' class='job_search_btn btn' value='Search'>
			</form>
			
			<b>Most Popular Tags:</b>
			<ul class='tags'>
			<?php
				//get the top 10 most recently added tags
				$tags = $tag->getTagLimit('15');
				foreach($tags as $tag => $val) {
					$tagURL = strtolower($val['tag']);
					echo "<li class='tag'><a href='/members?q=".$tagURL."'>".$val['tag']."</a></li>";
				}
			?>
			
			</ul>
		</div>
		
		<div class='post-container'>
	
		<?php
				
			//if isset(search query) 
			if(isset($_GET['q'])) {
				//get the search query from the URL
				$q = $_GET['q'];				
				
				//search through the users using the searchUser() function passing the search query
				$users = $user->searchUsers($q);
				foreach($users as $u => $val) {
					
					//get the users type
					$userType = $user->getUserType($val['id']);
					
					if($userType !== '4') {
						//get user data into variables for display purposes
						$firstName = $val['name1'];
						$lastName = $val['name2'];
						$fullName = $firstName. " " .$lastName;
						
						//if user has not uploaded a photo, use the default image
						if(strlen($val['photo_path']) === 0) {
							$userPhoto = "/images/default_image.gif";
						} else {
							$userPhoto = $val['photo_path'];
						}
						//explode the skills by commas into an array	
						$skills = explode(",", $val['skills']);

		?>
		
					<div class='user-block'>
						<span class='user-photo'>
							<img src='<?php echo $userPhoto; ?>'>
						</span>
						<span class='user-info'>
							<span class='username'><?php echo "<a href='/user/".$val['user_industry']."'>".$fullName."</a>"; ?></span><br />
							<span class='user-school'><?php echo "<a href='/members?q=".$val['school_slogan']."'>".$val['school_slogan']."</a>"; ?> </span><br />
							<span class='user-skills'>
								<ul class='tags'>
									<?php
										$i = 0;
										foreach($skills as $skill) if($i<8) {
											echo "<li class='tag'><a href='/members?q=".$skill."' style='color: #333;'>".$skill."</a></li>";
											$i += 1;
										}
									?>
								</ul>
							</span><br />
						</span>		
					</div>
		
		
			<?php 
					
					} else {  
				
						//stripslashes from company name
						$companyName = stripslashes($val['name1']);
						$url = $val['name2'];
						$profile = "/company/".$val['name2'];

						//if company has not uploaded a photo, use the default image
						if(strlen($val['photo_path']) === 0) {
							$logo = "/images/company_default.png";
						} else {
							$logo = $val['photo_path'];
						}
						//get the company industry
						$indName = $industry->getIndustry($val['user_industry'])
				
			?>

					<div class='user-block'>
						<span class='user-photo'>
							<img src='<?php echo $logo; ?>'>
						</span>
						<span class='user-info'>
							<span class='username'><?php echo "<a href='/company/".$url."'>".$companyName."</a>"; ?></span><br />
							<span class='user-school'><b>Industry: </b><?php echo "<a href='/members?q=".$indName."'>".$indName."</a>"; ?><br /><b>Location: </b><?php echo $val['major_location']; ?></span>
							<span class='user-skills'>
									
							</span><br />
						</span>
						
					</div>

		<?php 		
					}//close if(not company)
				}//close foreach($users)
				
			//if no search query, display the standard view
			} else {
			
				//get all users from database using the getAllUsers() function
				$users = $user->getAllUsersInfo($start, $limit);
				
				foreach($users as $u => $val) {
					$userType = $user->getUserType($val['id']);
					
					if($userType !== '4') {
						//get user data into variables for display purposes
						$firstName = $val['first_name'];
						$lastName = $val['last_name'];
						$fullName = $firstName. " " .$lastName;
						
						//if user has not uploaded a photo, use the default image
						if(strlen($val['photo_path']) === 0) {
							$userPhoto = "/images/default_image.gif";
						} else {
							$userPhoto = $val['photo_path'];
						}
						//explode the skills by commas into an array	
						$skills = explode(",", $val['skills']);

		?>
		
					<div class='user-block' id='<?php echo $val['id']; ?>'>
						<span class='user-photo'>
							<img src='<?php echo $userPhoto; ?>'>
						</span>
						<span class='user-info'>
							<span class='username'><?php echo "<a href='/user/".$val['username']."'>".$fullName."</a>"; ?></span><br />
							<span class='user-school'><?php echo "<a href='/members?q=".$val['school']."'>".$val['school']."</a>"; ?> </span><br />
							<span class='user-skills'>
								<ul class='tags'>
									<?php
										$i = 0;
										foreach($skills as $skill) if($i<6) {
											echo "<li class='tag'><a href='/members?q=".$skill."' style='color: #333;'>".$skill."</a></li>";
											$i += 1;
										}
									?>
								</ul>
							</span><br />
						</span>		
					</div>
		
		
			<?php 
					
					} else {  
				
						//stripslashes from company name
						$companyName = stripslashes($val['company_name']);
								
						//if company has not uploaded a photo, use the default image
						if(strlen($val['logo_path']) === 0) {
							$logo = "/images/company_default.png";
						} else {
							$logo = $val['logo_path'];
						}
						//get the company industry
						$indName = $industry->getIndustry($val['industry'])
				
			?>

					<div class='user-block' id='<?php echo $val['id']; ?>'>
						<span class='user-photo'>
							<img src='<?php echo $logo; ?>'>
						</span>
						<span class='user-info'>
							<span class='username'><?php echo "<a href='/company/".$val['username']."'>".$companyName."</a>"; ?></span><br />
							<span class='user-school'><b>Industry: </b><?php echo "<a href='/members?q=".$indName."'>".$indName."</a>"; ?><br /><b>Location: </b><?php echo $val['location']; ?></span>
							<span class='user-skills'>
									
							</span><br />
						</span>
						
					</div>
		
		<?php 		
					}//close if(not company)
				}//close foreach($users)
			} //close the members page
			
		?>
					<script type="text/javascript">
						//member infinite scroll
						var loading = false;
						$(window).scroll(function(){
					 
							if($(window).scrollTop() == $(document).height() - $(window).height()){
								loading = true;
								$('#ajaxLoader').show();
								$.ajax({
									url: "/ajax/member.process.php?lastid=" + $(".user-block:last").attr("id"),
									success: function(html){
										if(html){
											$(".post-container").append(html);
											$('div#ajaxLoader').hide();
										}else{
											$('div#ajaxLoader').html('<center>There are no more members.</center>');
										}
										loading = false;
									}
								});
							}
						});
					</script>
			</div>
			
			
		<div id="ajaxLoader"></div>

<?php

} else {
	header("Location: /index");
}//close if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'user_id')

include('includes/footer.php');

?>