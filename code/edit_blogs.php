<?php 

session_start();

//if user is logged in
if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id' && isset($_SESSION['admin'])) {

	//is the users profile complete?
	if(!(isset($_SESSION['profile']))) {
		Header('Location: edit-profile.php');
	}
	
	//set user_id
	$user_id = $_SESSION['user_id'];
	
	include('includes/user.header.php');
	
	$token = token();
		

echo "<div style='margin-top: 100px;'>";
include('includes/admin.nav.php');
echo "</div>";		
		
/* Blog Header HTML */
echo "<div class='job_header' style='padding-top: 0px'>";
	echo "<div class='well'>";
		echo "<span class='company_header'>Sort by: <a href='edit_blogs.php?sort=pub_date'>Date</a> <a href='edit_blogs.php?sort=blog_title'>Title</a>"; echo "<a href='edit_blogs.php?sort=author'>Author</a> <a href='edit_blogs.php?sort=category'>Category</a> <a href='edit_blogs.php?show=video'>Video</a></span>";
		echo "<form class='job_header_search form-search' name='job-search' method='GET' action='".$_SERVER['PHP_SELF']."'>";
			echo "<input type='text' name='q' class='job_search search-query' placeholder='Search by title, author, or keyword' />";
			echo "<input type='hidden' name='token' value='".$token."' />";
			echo "<input type='submit' value='Search' class='job_search_btn btn'>";
		echo "</form>";
	echo "</div>";
echo "</div>";
/* End Blog Header HTML */



echo "<div id='dynamic_container'>";
	/* Blog Categories */
	echo "<div class='blog_categories'>";
	
		echo "<ul>";
		
			//if user is an administrator, show the "Post Blog" button
			if(isset($_SESSION['admin'])) {
				echo "<li><a href='new_blog'><button class='btn btn-primary'>Post Blog</button></a></li>";
			}
			//get the categories
			$catSQL = $link->query("/*qc=on*/SELECT * FROM categories ORDER BY cat_name ASC");
			
			while($catRow = mysqli_fetch_assoc($catSQL)) {		
				$catID = $link->real_escape_string($catRow['cat_id']);
				$catName = $link->real_escape_string($catRow['cat_name']);
					
				echo "<li><a href='edit_blogs?cat=" .$catID. "'>" .$catName. "</a></li>";
		
			} //close while($catRow = mysqli_fetch_assoc($catSQL))
		
	echo "</ul>";	
	echo "</div>";	
	/* End Blog Categories */
	echo "<div class='blog_container'>";
		
		
		
		
			/* IF USER SEARCHES FOR A BLOG */
			if(isset($_GET['q'])) {
				$searchQuery = $link->real_escape_string($_GET['q']);
				$searchToken = $link->real_escape_string($_GET['token']);
				
				if(!$searchToken === $token) {
					echo "Invalid token";
					die();
				}
				//instantiate search.class.php
				$searchStemmer = new PorterStemmer;
				$searchQuery = $searchStemmer->Stem($searchQuery);
				
				//split the words into an array
				$searchArray = split(" ", $searchQuery);
				
				//foreach value in array
				foreach($searchArray as $key=>$val) {
					//run the search query
					$searchSQL = $link->query("SELECT * FROM blogs WHERE (blog_title LIKE '%$val%') OR (author LIKE '%$val%') OR (category LIKE '%$val%') OR (content LIKE '%$val%') OR (keywords LIKE '%$val%')");
				}
			
				if($searchSQL === FALSE) {
					echo "Could not perform search. Please contact network administrator.";
				} else {
					if(!$searchSQL->num_rows > 0) {
						echo "Could not match any blogs with the provided criteria. Please try again.";
					} else {
						//assign blog info to variables	
						while($row = mysqli_fetch_assoc($searchSQL)) {
						
							$isVid = $link->real_escape_string(intval($row['video']));
							$blogID = $link->real_escape_string(intval($row['blog_id']));
							$blogTitle = $link->real_escape_string($row['blog_title']);
							$pubDate = $link->real_escape_string($row['pub_date']);
							$blogCat = $link->real_escape_string($row['category']);
							$blogAuthor = $link->real_escape_string($row['author']);
							$blogImage = $link->real_escape_string($row['image']);
							$blogContent = $link->real_escape_string(truncate($row['content'], 300));
							
							if(!($isVid === '1')) {
								$blogImage = "uploads/blogs/photos/".$blogImage;
							}
							
							$pubDate = date('F jS, Y', strtotime($pubDate));
							$blogContent = html_entity_decode($blogContent);
						
							/* Blog Post HTML */
							echo "<div class='blog_post'>";
								echo "<span class='blog_photo'>";
									echo "<img src='".$blogImage."'>";
								echo "</span>";
								echo "<span class='blog_title'><a href='edit_edit_post?id=".$blogID."'>".$blogTitle."</a></span> - <span class='blog_date'>".$pubDate."</span>";
								echo "<div class='commentContainer'>";
									echo "<span class='comment'><a href='edit_post?id=".$blogID."#commentArea'>";
														
										$commentSQL = $link->query("SELECT * FROM b_comments WHERE blog_id = '".$blogID."'");
										
										echo $commentSQL->num_rows. " comment(s)";
									
									echo "</a></span>";
								echo "</div>";
								echo "<p class='blog_content'>";
									echo "<p>";
										echo $blogContent; 
										echo "<span class='read_more'><a href='edit_post?id=".$blogID."'> Read More</a></span>";
									echo "</p>";
								echo "</p>";
							echo "</div>";
			
						}//close while($row = $link->fetch_assoc($sql))		
					} //close if(!mysqli_num_rows($searchSQL) > 0)
				} //close if($searchSQL === FALSE)
				
				
				
				
				
			/* NO SEARCH AND NO CATEGORY (STANDARD VIEW) */
			} else {					
				if(!(isset($_GET['cat']))) {
					
					/*=== prepare the blog pagination ===*/
					// find out how many rows are in the table 
					$countSQL = "SELECT COUNT(*) FROM blogs";
					$result = $link->query($countSQL);
					$r = mysqli_fetch_row($result);
					$numrows = $r[0];
					
					// number of rows to show per page
					$rowsperpage = 5;			
					
					// find out total pages
					$totalpages = ceil($numrows / $rowsperpage);

					// get the current page or set a default
					if (isset($_GET['page']) && is_numeric($_GET['page'])) {
						// cast var as int
						$currentpage = (int) $_GET['page'];
					} else {
						// default page num
						$currentpage = 1;
					} // end if

					// if current page is greater than total pages...
					if ($currentpage > $totalpages) {
						// set current page to last page
						$currentpage = $totalpages;
					} // end if
					
					// if current page is less than first page...
					if ($currentpage < 1) {
						// set current page to first page
						$currentpage = 1;
					} // end if

					// the offset of the list, based on current page 
					$offset = ($currentpage - 1) * $rowsperpage;

					// get the blogs from the db 
					
					if(isset($_GET['sort'])) {
						$sortBy = $_GET['sort'];
						$sql = "SELECT * FROM blogs ORDER BY $sortBy LIMIT $offset, $rowsperpage";
					} else {
						$sql = "SELECT * FROM blogs ORDER BY blog_id DESC LIMIT $offset, $rowsperpage";
						
					}
					
					$result = $link->query($sql);
					//assign blog info to variables	
					while($row = mysqli_fetch_assoc($result)) {
					
						$isVid = $link->real_escape_string(intval($row['video']));
						$blogID = $link->real_escape_string(intval($row['blog_id']));
						$blogTitle = $link->real_escape_string($row['blog_title']);
						$pubDate = $link->real_escape_string($row['pub_date']);
						$blogCat = $link->real_escape_string($row['category']);
						$blogAuthor = $link->real_escape_string($row['author']);
						$blogImage = $link->real_escape_string($row['image']);
						$blogContent = $link->real_escape_string(truncate($row['content'], 300));
						
						if(!($isVid === '1')) {
							$blogImage = "uploads/blogs/photos/".$blogImage;
						}
						
						$pubDate = date('F jS, Y', strtotime($pubDate));
						$blogContent = html_entity_decode($blogContent);
						
			
						/* Blog Post HTML */
						echo "<div class='blog_post'>";
							echo "<span class='blog_photo'>";
								echo "<img src='".$blogImage."'>";
							echo "</span>";
							echo "<span class='blog_title'><a href='edit_post?id=".$blogID."'>".$blogTitle."</a></span> - <span class='blog_date'>".$pubDate."</span>";
							echo "<div class='commentContainer'>";
								echo "<span class='comment'><a href='edit_post?id=".$blogID."#commentArea'>";
														
									$commentSQL = $link->query("SELECT * FROM b_comments WHERE blog_id = '".$blogID."'");										
									echo $commentSQL->num_rows. " comment(s)";
									
								echo "</a></span>";
							echo "</div>";
							echo "<p class='blog_content'>";
								echo "<p>";
									echo $blogContent; 
									echo "<span class='read_more'><a href='edit_post?id=".$blogID."'> Read More</a></span>";
								echo "</p>";
							echo "</p>";
						echo "</div>";
		
					}//close while($row = $link->fetch_assoc($sql))
		
		
					/*===  build the pagination links ===*/	
					echo "<div class='pagination_container'>";
					echo "<span class='pagination'>";
					
					
					// range of num links to show
					$range = 5;

					// if not on page 1, don't show back links
					if ($currentpage > 1) {
						echo " <a href='{$_SERVER['PHP_SELF']}?page=1'><<</a> ";
						// get previous page num
						$prevpage = $currentpage - 1;
						echo " <a href='{$_SERVER['PHP_SELF']}?page=$prevpage'><</a> ";
					}

					// loop to show links to range of pages around current page
					for ($x = ($currentpage - $range); $x < (($currentpage + $range) + 1); $x++) {
						// if it's a valid page number...
						if (($x > 0) && ($x <= $totalpages)) {
							// if we're on current page...
							if ($x == $currentpage) {
								echo " [<b>$x</b>] ";
							// if not current page...
							} else {
								echo " <a href='{$_SERVER['PHP_SELF']}?page=$x'>$x</a> ";
							} 
						}
					} 
									 
					// if not on last page, show forward and last page links        
					if ($currentpage != $totalpages) {
						// get next page
						$nextpage = $currentpage + 1;
						echo " <a href='{$_SERVER['PHP_SELF']}?page=$nextpage'>></a> ";
						echo " <a href='{$_SERVER['PHP_SELF']}?page=$totalpages'>>></a> ";
					} 
					
					

				echo "</span>";
			echo "</div>";
			/*=== end build pagination links ===*/
			
			
				
				/* IF SPECIFIC CATEGORY IS SELECTED */
				} else {
				
					$blogCat = intval($_GET['cat']);
					
					/*=== prepare the blog pagination ===*/
					
					// find out how many rows are in the table 
					$countSQL = "SELECT COUNT(*) FROM blogs WHERE category = $blogCat";
					$result = $link->query($countSQL);
					$r = mysqli_fetch_row($result);
					$numrows = $r[0];
					
					// number of rows to show per page
					$rowsperpage = 5;			
					
					// find out total pages
					$totalpages = ceil($numrows / $rowsperpage);

					// get the current page or set a default
					if (isset($_GET['page']) && is_numeric($_GET['page'])) {
						// cast var as int
						$currentpage = (int) $_GET['page'];
					} else {
						// default page num
						$currentpage = 1;
					} // end if

					// if current page is greater than total pages...
					if ($currentpage > $totalpages) {
						// set current page to last page
						$currentpage = $totalpages;
					} // end if
					
					// if current page is less than first page...
					if ($currentpage < 1) {
						// set current page to first page
						$currentpage = 1;
					} // end if

					// the offset of the list, based on current page 
					$offset = ($currentpage - 1) * $rowsperpage;

					// get the blogs from the db 
					$sql = "SELECT * FROM blogs WHERE category = $blogCat ORDER BY blog_id DESC LIMIT $offset, $rowsperpage";
					$result = $link->query($sql);
					
					//assign blog info to variables	
					while($row = mysqli_fetch_assoc($result)) {
					
						$isVid = $link->real_escape_string(intval($row['video']));
						$blogID = $link->real_escape_string(intval($row['blog_id']));
						$blogTitle = $link->real_escape_string($row['blog_title']);
						$pubDate = $link->real_escape_string($row['pub_date']);
						$blogCat = $link->real_escape_string($row['category']);
						$blogAuthor = $link->real_escape_string($row['author']);
						$blogImage = $link->real_escape_string($row['image']);
						$blogContent = $link->real_escape_string(truncate($row['content'], 300));
						
						if(!($isVid === '1')) {
							$blogImage = "uploads/blogs/photos/".$blogImage;
						}
						
						
						/* Blog Post HTML */
						echo "<div class='blog_post'>";
							echo "<span class='blog_photo'>";
								echo "<img src='".$blogImage."'>";
							echo "</span>";
							echo "<span class='blog_title'><a href='edit_post?id=".$blogID."'>".$blogTitle."</a></span> - <span class='blog_date'>".$pubDate."</span>";
							echo "<div class='commentContainer'>";
								echo "<span class='comment'><a href='edit_post?id=".$blogID."#commentArea'>";
														
									$commentSQL = $link->query("SELECT id FROM b_comments WHERE blog_id = '".$blogID."'");										
									echo $commentSQL->num_rows. " comment(s)";
									
								echo "</a></span>";
							echo "</div>";
							echo "<p class='blog_content'>";
								echo "<p>";
									echo $blogContent; 
									echo "<span class='read_more'><a href='edit_post?id=".$blogID."'> Read More</a></span>";
								echo "</p>";
							echo "</p>";
						echo "</div>";
		
					}//close while($row = $link->fetch_assoc($sql))
			
			
			
			/*===  build the pagination links ===*/
			echo "<div class='pagination_container'>";
				echo "<span class='pagination'>";

					
					// range of num links to show
					$range = 5;

					// if not on page 1, don't show back links
					if ($currentpage > 1) {
						echo " <a href='{$_SERVER['PHP_SELF']}?page=1'><<</a> ";
						// get previous page num
						$prevpage = $currentpage - 1;
						echo " <a href='{$_SERVER['PHP_SELF']}?page=$prevpage'><</a> ";
					}

					// loop to show links to range of pages around current page
					for ($x = ($currentpage - $range); $x < (($currentpage + $range) + 1); $x++) {
						// if it's a valid page number...
						if (($x > 0) && ($x <= $totalpages)) {
							// if we're on current page...
							if ($x == $currentpage) {
								echo " [<b>$x</b>] ";
							// if not current page...
							} else {
								echo " <a href='{$_SERVER['PHP_SELF']}?page=$x'>$x</a> ";
							} 
						}
					} 
									 
					// if not on last page, show forward and last page links        
					if ($currentpage != $totalpages) {
						// get next page
						$nextpage = $currentpage + 1;
						echo " <a href='{$_SERVER['PHP_SELF']}?page=$nextpage'>></a> ";
						echo " <a href='{$_SERVER['PHP_SELF']}?page=$totalpages'>>></a> ";
					} 
					
					echo "</span>";
					echo "</div>";
					/*=== end build pagination links ===*/
	
				}//close if(!(isset($_GET['cat'])))
				
			}//close if(isset($_GET['q'])) 
		

	echo "</div>";
	echo "</div>";
	
	//user IS company
	} else {		
		header("Location: index");
	}//close if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'user_id')

include('includes/footer.php');

?>