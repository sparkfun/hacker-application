<?php 

session_start();

//if user is logged in
if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id' && isset($_SESSION['admin'])) {
	
	//set user_id
	$user_id = $_SESSION['user_id'];
	
	include('includes/user.header.php');
		
	//select user information according to their logged in user_id
	$userSQL = $link->query('SELECT first_name, last_name FROM seekers WHERE id = "'.$user_id.'"');
	$row = mysqli_fetch_assoc($userSQL);
		
	//create piece name together
	$firstName = $link->real_escape_string($row['first_name']);
	$lastName = $link->real_escape_string($row['last_name']);
	$author = $firstName. " " .$lastName;
	
	
	$todays_date = strtotime($todays_date);
	
	$token = token();
	
	echo "<div style='margin-top: 100px;'>";
		include('includes/admin.nav.php');
	echo "</div>";	
	
			echo "<div id='dynamic_container'>";
			
			//get blog posts
			$blogID = $link->real_escape_string(intval($_GET['id']));
			
			$sql = $link->query("SELECT * FROM blogs WHERE blog_id = '" .$blogID. "'");
					
			$row = mysqli_fetch_assoc($sql);
			
			$isVid = $link->real_escape_string(intval($row['video']));
			
				$blogTitle = $link->real_escape_string($row['blog_title']);
				$pubDate = $link->real_escape_string($row['pub_date']);
				$blogCat = $link->real_escape_string($row['category']);
				$blogAuthor = $link->real_escape_string($row['author']);
				$blogImage = $link->real_escape_string($row['image']);
				$blogContent = $link->real_escape_string(nl2br($row['content']));
				$vidURL = $link->real_escape_string($row['url']);
				$keywords = $link->real_escape_string($row['keywords']);
				
				$blogContent = html_entity_decode($blogContent);
				$vidURL = html_entity_decode($vidURL);
				
				$blogContent = stripslashes($blogContent);
				$pubDate = date('F jS, Y', strtotime($pubDate));
				
			if(isset($_POST['editBlog'])) {
				$blogTitle = $link->real_escape_string(htmlentities($_POST['blogTitle']));
				$blogContent = $link->real_escape_string(htmlentities($_POST['content']));
				$pubDate = $link->real_escape_string($_POST['pubDate']);
				$category = $link->real_escape_string($_POST['category']);
				$vidURL = $link->real_escape_string($_POST['url']);
				$author = $link->real_escape_string($_POST['author']);
				$keywords = $link->real_escape_string($_POST['keywords']);
				
				$blogUpdate = $link->query("UPDATE blogs SET blog_title = '".$blogTitle."', content = '".$blogContent."', keywords = '".$keywords."' WHERE blog_id = '".$blogID."'");
				if($blogUpdate === FALSE) {
					printf("Query Failed: %s\n", $link->error);
				}
				
				else {
					echo "<span class='success'>Blog Successfully Updated!</span>";
				}
			}

			if($isVid === '1') {
				
					/* Blog Posting HTML */
					echo "<div class='blog_container'>";
						echo "<div class='post_container'>";
							echo "<div class='post_photo' style='margin-bottom: 15px;'>";
								echo "<img src='".$blogImage."'>";
							echo "</div>";
							echo "<form name='editBlogForm' action='".$_SERVER['PHP_SELF']."' method='post' />";
								echo "<div><label class='add_label'>Blog Title: </label><input type='text' class='add_form' name='blogTitle' value='".$blogTitle."' maxlength='55' /></div>";
								echo "<div><input type='hidden' name='pubDate' value='".$pubDate."' /></div>";
								echo "<div><label class='add_label'>Blog Author: </label><input type='text' name='author' value='".$blogAuthor."' maxlength='55' /></div>";
							
								echo "<div><label class='add_label'>Blog Category: </label>";
									echo "<select name='category'>";

										$catSQL = $link->query("SELECT * FROM categories ORDER BY cat_id");
								
										if($catSQL === FALSE) {
											echo "Could not retrieve categories.";
										} 
								
										while($catRow = mysqli_fetch_assoc($catSQL)) {
											$catID = $link->real_escape_string(intval($catRow['cat_id']));
											$catName = $link->real_escape_string($catRow['cat_name']);
									
											echo "<option value='".$catID."'>".$catName."</option>\n";
										}
				
									echo "</select>";
								echo "</div>";

								echo "<div><label class='add_label'>Video URL: </label><input type='text' name='url' class='add_form' value='".$vidURL."' /></div>";

								echo "<div><label class='add_label'>Video Description: </label><textarea class='add_txt' name='content'>".$blogContent."</textarea></div>";
								
								echo "<div><label class='add_label'>Keywords: </label><input type='text' name='keywords' class='add_form' value='".$keywords."' /></div>";
						
								echo "<input type='submit' name='editBlog' class='btn btn-primary' value='Edit Post' />";
								echo "<a href='delete_blog.php?id=".$blogID."'>Delete Blog</a>";
					echo "</form>";
				
			
			} else {
			
				/* Blog Posting HTML */
					echo "<div class='blog_container'>";
						echo "<div class='post_container'>";
							echo "<div class='post_photo' style='margin-bottom: 15px;'>";
								echo "<img src='uploads/blogs/photos/".$blogImage."' width='150px'>";
							echo "</div>";
							echo "<form name='editBlogForm' action='".$_SERVER['PHP_SELF']."' method='post' />";
								echo "<div><label class='add_label'>Blog Title: </label><input type='text' class='add_form' name='blogTitle' value='".$blogTitle."' maxlength='55' /></div>";
								echo "<div><input type='hidden' name='pubDate' value='".$pubDate."' /></div>";
								echo "<div><label class='add_label'>Blog Author: </label><input type='text' name='author' value='".$blogAuthor."' maxlength='55' /></div>";
							
								echo "<div><label class='add_label'>Blog Category: </label>";
									echo "<select name='category'>";

										$catSQL = $link->query("SELECT * FROM categories ORDER BY cat_id");
								
										if($catSQL === FALSE) {
											echo "Could not retrieve categories.";
										} 
								
										while($catRow = mysqli_fetch_assoc($catSQL)) {
											$catID = $link->real_escape_string(intval($catRow['cat_id']));
											$catName = $link->real_escape_string($catRow['cat_name']);
									
											echo "<option value='".$catID."'>".$catName."</option>\n";
										}
				
									echo "</select>";
								echo "</div>";

								echo "<div><label class='add_label'>Blog Content: </label><textarea class='add_txt' name='content'>".$blogContent."</textarea></div>";
								
								echo "<div><label class='add_label'>Keywords: </label><input type='text' name='keywords' class='add_form' value='".$keywords."' /></div>";
						
								echo "<input type='submit' name='editBlog' class='btn btn-primary' value='Edit Post' />";
								echo "<a href='deleteBlog.php?id=".$blogID."'>Delete Post</a>";
					echo "</form>";
					
			}	//close if($isVid === '1') 	
			
			/* Blog Comment Area HTML */
			echo "<div class='postComment'>";				
					
					$getComments = $link->query("SELECT * FROM b_comments WHERE blog_id = '".$blogID."' ORDER BY date DESC");
					
					if(!($getComments->num_rows > 0)) {
						
						echo "<div id='commentArea' style='display: none;'>";
						
						echo "</div>";
					} else {					
						echo "<div id='commentArea'>";	
							while($comRow = mysqli_fetch_assoc($getComments)) {
								$commentID = $link->real_escape_string($comRow['id']);
								$comAuthor = $link->real_escape_string($comRow['user_name']);
								$comDate = $link->real_escape_string($comRow['date']);
								$comContent = $link->real_escape_string($comRow['content']);
								$comPhoto = $link->real_escape_string($comRow['photo']);

								//$comDate = date('F jS, Y - g:ia', strtotime($comDate)); -- FOR NO "time-ago" function!
								$comDate = strtotime($comDate); //strtotime for time_ago function
								$comContent = stripslashes(html_entity_decode($comContent));
								
								echo "<div class='commentBlock'>";
									echo "<span class='comImg'><img src='".$comPhoto."' /></span>";
									echo $comAuthor. " - ";
									//echo $comDate. " - ";
									echo time_ago($comDate). " - ";
									echo $comContent;
									echo "<a href='deleteComment.php?id=".$commentID."'>Delete Comment</a>";
								echo "</div>";
							}
							
						
						echo "</div>";
					}
					
			echo "</div>";
			echo "</div>";		
			echo "</div>";
			echo "</div>";


} else {
	header("Location: index.php");
}//close if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'user_id')

include('includes/footer.php');

?>