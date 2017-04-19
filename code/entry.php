<?php 

session_start();

//if user is logged in
if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id') {
	
	//is the users profile complete?
	if(!(isset($_SESSION['profile']))) {
		Header('Location: edit-profile');
	}
	
	//set user_id
	$user_id = intval($_SESSION['user_id']);
	$blogID = intval($_GET['id']);
	
	include('includes/user.header.php');

	
		$db = new DBConnection;
		$blogs = new Blog($db);
		$user = new User($db);
		$comments = new Comment($db);
		$str = new str_format;
		
		$blogData = $blogs->get_oneBlog($blogID);

		//set variables for display
		$isVid = $blogData['video'];
		$blogTitle = $blogData['blog_title'];
		$blogCat = $blogData['category'];
		$authorID = $blogData['author'];
		$blogImage = $blogData['image'];
		$blogContent = $blogData['content'];
		
					
		//set SEO friendly URL
		$blogURL = $str->Seo($blogTitle);
					
		//format the date, blog title	
		$pubDate = date('F jS, Y', strtotime($blogData['pub_date']));
		$blogTitle = stripslashes($blogTitle);
		
		//format the blog content
		$blogContent = nl2br(str_replace('\\r\\n', "\r\n", $blogContent));
		$blogContent = html_entity_decode(stripslashes($blogContent));
					
		//get the author information
		$userData = $user->getUserInfo($authorID);
		//get the author type (company or seeker)
		$userType = $user->getUserType($authorID);

		if($userType !== '4') {
			//if the author is not a company
			$firstName = $userData['first_name'];
			$lastName = $userData['last_name'];
			$blogAuthor = $firstName. " " .$lastName;				
			$profile = "/user/".$userData['username'];
		} else {
			//if the author is a company
			$blogAuthor = $userData['company_name'];
			$profile = "/company/".$authRow['username'];
		}	

		?>
		
		<div class='post'>
			<span class='full-post-photo'>
				<img src='<?php echo $blogImage; ?>'>
			</span>
			<span class='post-title'><a href='/post/<?php echo $blogData['blog_id']."/".$blogURL; ?>'><?php echo $blogTitle; ?></a></span><br />
			<span class='post-author'>By <a href='<?php echo $profile; ?>'><?php echo $blogAuthor; ?></a></span><br />
			<span class='post-date'><img src='/images/icon_date.gif' class='icon'><?php echo $pubDate; ?></span>
			<span class='comment'><img src='/images/icon_comments.gif' class='icon'><a href='/post/<?php echo $blogData['blog_id']."/".$blogURL; ?>'>
				<?php									
					$commentSQL = $db->query("SELECT * FROM b_comments WHERE blog_id = '".$blogData['blog_id']."'");
											
					echo $commentSQL->num_rows. " comment(s)";
				?>
										
			</a></span>
			<?php
			
				if($isVid === '1') {
					$vidURL = html_entity_decode($blogData['url']);
					echo "<p style='margin-top: 75px;'>";
					echo $vidURL;
					echo "</p>";
				}
			
			?>
			<p class='post-content'>
				<p>
					<?php 
						echo $blogContent;
					?>
				</p>
			</p>
		</div>
		
		
		<?php
		
		//get the commenting user information
		$comUserType = $user->getUserType($user_id);
		$comUserData = $user->getUserInfo($user_id);
		
		if($comUserType === '4') {
			$photo_path = $comUserData['logo_path'];
			if(strlen($photo_path) < 5) {
				$photo_path = "/images/company_default.png";
			}
		} else {
			$photo_path = $comUserData['photo_path'];
			if(strlen($photo_path) < 5) {
				$photo_path = "/images/default_image.gif";
			}
		}	
		?>
		
		<div class='comment-container'>
			<div class='comment-form'>
				<form name='commentForm' id='commentForm' action="" method='post'>
					<span class='com-pic'><img src='<?php echo $photo_path; ?>'/></span>
					<input type='text' id='blogComment' class='comment-input' name='blogComment' maxlength='140' placeholder='Write comment here...' required/>
					<input type='hidden' id='blogID' name='blogID' value='<?php echo $blogID; ?>'/>
					<input type='hidden' id='photo' name='comPhoto' value='<?php echo $photo_path; ?>' />
					<input type='hidden' id='userID' name='userID' value='<?php echo $user_id; ?>' />
					<input type='submit' name='submitComment' id="submitComment" class='btn btn-primary' value='Submit' style='position: relative; bottom: 2px;' />
				</form>
			</div>
			
			

		<?php

			//echo blog comments
			$c = $comments->comMarkup($blogID);
			echo $c;
			
		?>
		
		</div>
	</div>

<?php

} else {
	header("Location: index");
}//close if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'user_id')

include('includes/footer.php');

?>