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
	
	include('includes/user.header.php');
	
	//istantiate all the required objects
	$db = new DBConnection;
	$cats = new Category($db);
	$blogs = new Blog($db);
	$user = new User($db);
	$comments = new Comment($db);
	$str = new str_format;
	$validate = new data_validation;

	
	$start = $blogs->getLastBlogId();
	$limit = "10";
		
?>
	<div class='sidebar'>
		<form class='side-search form-search' name='blog-search' method='GET' action='<?php echo $_SERVER['PHP_SELF']; ?>'>
			<input type='text' name='q' class='job_search search-query' placeholder='Search by title, author, or keyword' />
			<input type='hidden' name='token' value='<?php echo $_SESSION['token']; ?>' />
			<input type='submit' value='Search' class='job_search_btn btn'>
		</form>

		<?php
			//if user is an administrator, show the "Post Blog" button
			if(isset($_SESSION['admin']) || isset($_SESSION['blogger'])) {
				echo "<div class='sideButton'><a href='new_blog'><button class='btn btn-primary'>Post Blog</button></a></div>";
			}
		?>
		
		<ul class='sideList'>
			<?php
				//get all the categories from the database and display them
				$catArray = $cats->get_allCats();
				foreach($catArray as $key => $val) {
					echo "<li><a href='blog?cat=" .$key. "'>" .$val. "</a></li>";
				}
			?>
		
		</ul>
	</div>

	
	
	<div class='post-container'>
		<?php
			/* If Search Query Is Set */
			if(isset($_GET['q'])) {
				
				//call the searchBlogs() function and search blogs -- pass the search query
				$blogData = $blogs->searchBlogs($_GET['q']);
				
				if($blogData === FALSE) {
					echo "<span class='error'>Could not find any results with your criteria.</span>";
					die();
				}
				
				//foreach blog returned from searchBlogs()
				foreach($blogData as $blog => $val) {
					
					//set variables for display purposes
					$isVid = $val['video'];
					$blogTitle = $val['blog_title'];
					$blogCat = $val['category'];
					$authorID = $val['author'];
					$blogImage = $val['image'];
					$blogContent = $val['content'];
					
					//set SEO friendly URL
					$blogURL = $str->Seo($blogTitle);
					
					//format the date, blog title, and content			
					$pubDate = date('F jS, Y', strtotime($val['pub_date']));
					$blogTitle = stripslashes($blogTitle);
					
					//format the blog content
					$blogContent = nl2br(str_replace('\\r\\n', "\r\n", $blogContent));
					$blogContent = html_entity_decode(stripslashes($str->truncate($blogContent, 325)));
					
					//get the author information
					$userData = $user->getUserInfo($authorID);
					//get the author type (company or seeker)
					$userType = $user->getUserType($authorID);

					if(!($userType === '4')) {
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

			<div class='post' id="<?php echo $val['blog_id']; ?>">
					<span class='post-photo'>
						<img src='<?php echo $blogImage; ?>'>
					</span>
					<span class='post-title'><a href='/post/<?php echo $val['blog_id']."/".$blogURL; ?>'><?php echo $blogTitle; ?></a></span><br />
					<span class='post-author'>By <a href='<?php echo $profile; ?>'><?php echo $blogAuthor; ?></a></span><br />
					<span class='post-date'><img src='/images/icon_date.gif' class='icon'><?php echo $pubDate; ?></span>
					<span class='comment'><img src='/images/icon_comments.gif' class='icon'><a href='/post/<?php echo $val['blog_id']."/".$blogURL; ?>'>
						<?php
							//get the number of comments
							echo $comments->getNumComments($val['blog_id']);
						?>
										
					</a></span>
					<p class='post-content'>
						<p>
							<?php 
								echo $blogContent;
								echo "<span class='read_more'><a href='/post/".$val['blog_id']."/".$blogURL."'> Read More</a></span>";
							?>
						</p>
					</p>
				</div>
		
		<?php
				
				} //close foreach(blog search)
				
			/* No Query (Standard Blog View) */
			} elseif(!(isset($_GET['cat']))) {
				
				//get the blog data using getBlogs() function
				$blogData = $blogs->getBlogs($start, $limit);
				
				//foreach blog returned from getBlogs()
				foreach($blogData as $blog => $val) {
					
					//set variables for display purposes
					$isVid = $val['video'];
					$blogTitle = $val['blog_title'];
					$blogCat = $val['category'];
					$authorID = $val['author'];
					$blogImage = $val['image'];
					$blogContent = $val['content'];
					
					//set SEO friendly URL
					$blogURL = $str->Seo($blogTitle);
					
					//format the date, blog title, and content			
					$pubDate = date('F jS, Y', strtotime($val['pub_date']));
					$blogTitle = stripslashes($blogTitle);
					
					//format the blog content
					$blogContent = nl2br(str_replace('\\r\\n', "\r\n", $blogContent));
					$blogContent = html_entity_decode(stripslashes($str->truncate($blogContent, 325)));
					
					//get the author information
					$userData = $user->getUserInfo($authorID);
					//get the author type (company or seeker)
					$userType = $user->getUserType($authorID);

					if($userType === '4') {
						//if the author is a company
						$blogAuthor = $userData['company_name'];
						$profile = "/company/".$authRow['username'];
					} else {
						//if the author is not a company
						$firstName = $userData['first_name'];
						$lastName = $userData['last_name'];
						$blogAuthor = $firstName. " " .$lastName;				
						$profile = "/user/".$userData['username'];
					}	
		?>
		
				<div class='post' id="<?php echo $val['blog_id']; ?>">
					<span class='post-photo'>
						<img src='<?php echo $blogImage; ?>'>
					</span>
					<span class='post-title'><a href='/post/<?php echo $val['blog_id']."/".$blogURL; ?>'><?php echo $blogTitle; ?></a></span><br />
					<span class='post-author'>By <a href='<?php echo $profile; ?>'><?php echo $blogAuthor; ?></a></span><br />
					<span class='post-date'><img src='/images/icon_date.gif' class='icon'><?php echo $pubDate; ?></span>
					<span class='comment'><img src='/images/icon_comments.gif' class='icon'><a href='/post/<?php echo $val['blog_id']."/".$blogURL; ?>'>
						<?php
							//get the number of comments
							echo $comments->getNumComments($val['blog_id']);
						?>
										
					</a></span>
					<p class='post-content'>
						<p>
							<?php 
								echo $blogContent;
								echo "<span class='read_more'><a href='/post/".$val['blog_id']."/".$blogURL."'> Read More</a></span>";
							?>
						</p>
					</p>
				</div>
		
		<?php
				
				} //close foreach(standard blog view)
					
			/* If category isset */
			} else {
				$cat = intval($_GET['cat']);
				
				//get the blogs for the specific category using the get_catBlogs() function -- passing the category
				$blogData = $blogs->get_catBlogs($cat);
				
				//foreach blog
				foreach($blogData as $blog => $val) {
					
					//set variables for display
					$isVid = $val['video'];
					$blogTitle = $val['blog_title'];
					$blogCat = $val['category'];
					$authorID = $val['author'];
					$blogImage = $val['image'];
					$blogContent = $val['content'];
					
					//set SEO friendly URL
					$blogURL = $str->Seo($blogTitle);
					
					//format the date, blog title, and content			
					$pubDate = date('F jS, Y', strtotime($val['pub_date']));
					$blogTitle = stripslashes($blogTitle);
					
					//format the blog content
					$blogContent = nl2br(str_replace('\\r\\n', "\r\n", $blogContent));
					$blogContent = html_entity_decode(stripslashes($str->truncate($blogContent, 325)));
					
					//get the author information
					$userData = $user->getUserInfo($authorID);
					//get the author type (company or seeker)
					$userType = $user->getUserType($authorID);

					if(!($userType === '4')) {
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
				

				<div class='post' id="<?php echo $val['blog_id']; ?>">
					<span class='post-photo'>
						<img src='<?php echo $blogImage; ?>'>
					</span>
					<span class='post-title'><a href='/post/<?php echo $val['blog_id']."/".$blogURL; ?>'><?php echo $blogTitle; ?></a></span><br />
					<span class='post-author'>By <a href='<?php echo $profile; ?>'><?php echo $blogAuthor; ?></a></span><br />
					<span class='post-date'><img src='/images/icon_date.gif' class='icon'><?php echo $pubDate; ?></span>
					<span class='comment'><img src='/images/icon_comments.gif' class='icon'><a href='/post/<?php echo $val['blog_id']."/".$blogURL; ?>'>
						<?php
							//get the number of comments
							echo $comments->getNumComments($val['blog_id']);
						?>
										
					</a></span>
					<p class='post-content'>
						<p>
							<?php 
								echo $blogContent;
								echo "<span class='read_more'><a href='/post/".$val['blog_id']."/".$blogURL."'> Read More</a></span>";
							?>
						</p>
					</p>
				</div>

		<?php
				}//close foreach(blog categories)
			} //close if(cat isset)	
		?>
</div>				
<script type="text/javascript">
	//block infinite scroll
	var loading = false;
    $(window).scroll(function(){
 
        if($(window).scrollTop() == $(document).height() - $(window).height()){
            loading = true;
            $('#ajaxLoader').show();
            $.ajax({
                url: "/ajax/blog.process.php?lastid=" + $(".post:last").attr("id"),
                success: function(html){
                    if(html){
                        $(".post-container").append(html);
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
	//user is not logged in
	} else {		
		header("Location: index");
	}//close if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'user_id')

include('includes/footer.php');

?>