<?php

include_once('../classes/db.class.php');
include_once("../classes/blog.class.php");
include_once("../classes/string.class.php");
include_once("../classes/user.class.php");
include_once("../classes/comment.class.php");

//if there is a query in the URL
if(isset($_GET['lastid'])) {
	
	$db = new DBConnection;
	$blogs = new Blog($db);
	$str = new str_format;
	$user = new User($db);
	$comments = new Comment($db);
	
	$start = intval($_GET['lastid']);
	$limit = "5";
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

		if(!($userType === '4')) {
			//if the author is not a company
			$blogAuthor = $userData['first_name']. " " .$userData['last_name'];				
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
	} //close foreach
			
}
?>
