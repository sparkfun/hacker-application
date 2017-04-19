<?php

error_reporting(E_ALL); // or E_STRICT
ini_set("display_errors",1);

//include all PHP classes
foreach (glob("classes/*.php") as $filename) {
	include_once($filename);
}

/* Controls All Blog Functions */
class Blog 
{
		
	public function __construct(DBConnection $mysqli) {
		$this->mysqli = $mysqli->getLink();
	}
	
	
	public function getLastBlogId() {
		$sql = $this->mysqli->query("SELECT * FROM blogs ORDER BY blog_id DESC LIMIT 1");
		
		$row = $sql->fetch_assoc();
		
		$num = $this->mysqli->real_escape_string(intval($row['blog_id']));
		$num = $num + 1;
		return $num;
	}
	
	
	/* Insert New Blog */
	public function insert_blog($user_id) {	
	
		$str = new str_format;
		$validate = new data_validation;
		$db = new DBConnection;
		$upload = new Upload($db);
		$tags = new Tag($db);
		
		$user_id = $_SESSION['user_id'];
		
		$_POST = $validate->sanitize($_POST); 
		
		$blogTitle = htmlentities($_POST['blogTitle']);
		$blogPhoto = $_FILES['photo'];
		$blogContent = nl2br(htmlentities($_POST['updateContent']));
		$category = $_POST['category'];
		$keywords = $_POST['blogKeys'];
		
		$imageFile = $upload->uploadImg('blogs', $user_id);
		
		if($imageFile === FALSE) {
			echo "<span class='error'>Could not upload blog photo</span>";
		} else {
		
			//insert tags into the database using insertTags();
			$tagQuery = $tags->insertTags($keywords);
					
			if($tagQuery === FALSE) {
				echo "Error with tags";
			}
			
			$blogSQL = $this->mysqli->query("INSERT INTO blogs (blog_title, pub_date, author, category, image, content, keywords, video) VALUES ('".$blogTitle."', Now(), '".$user_id."', '".$category."', '".$imageFile."', '".$blogContent."', '".$keywords."', '0')");
			
			$fullPostsQuery = $this->mysqli->query("INSERT INTO all_posts (blog, job, video, item_id) VALUES ('1', '0', '0', '".$this->mysqli->insert_id."')");
						
			if($fullPostsQuery === FALSE || $blogSQL === FALSE) {
				printf("Query Failed: %s\n", $this->mysqli->error);
				return false;
			} else {
				return true;
			}
	
		}
	}
	
	
	
	/* Insert Video Blog */
	public function insertVideoBlog($user_id) {
		
		//istantiate required objects
		$str = new str_format;
		$validate = new data_validation;
		$db = new DBConnection;
		$upload = new Upload($db);
		$tags = new Tag($db);
		$youtube = new YoutubeParser;
		
		//sanitize the $_POST auto global
		$_POST = $validate->sanitize($_POST);
		
		//put $_POST array into variables
		$videoTitle = htmlentities($_POST['title']);
		$videoCat = $_POST['category'];
		$videoURL = $_POST['url'];
		$keywords = $_POST['blogKeys'];
		
		$user_id = $_SESSION['user_id'];
		
		//parse the pasted URL
		$youtube->set('source', $videoURL);
		//get the video parameters
		$videoArray = $youtube->getall();
		//get the video details from the array
		$videoThumb = $videoArray['0']['thumb'];
		$videoEmbed = $videoArray['0']['embed'];
		
		$videoEmbed = htmlentities($videoEmbed);
		$videoDesc = nl2br(htmlentities($_POST['description']));	

		//insert tags into the database using insertTags();
		$tagQuery = $tags->insertTags($keywords);
					
		if($tagQuery === FALSE) {
			echo "Error with tags";
		}
		
		//insert video blog into database
		$blogSQL = $this->mysqli->query("INSERT INTO blogs (blog_title, pub_date, author, category, image, content, keywords, url, video) VALUES ('".$videoTitle."', Now(), '".$user_id."', '".$videoCat."', '".$videoThumb."', '".$videoDesc."', '".$keywords."', '".$videoEmbed."', '1')");
		
		if($blogSQL === FALSE) {
			echo $this->mysqli->error;
		} else {
			//insert blog into the all_posts table for the homepage feed
			$fullPostsQuery = $this->mysqli->query("INSERT INTO all_posts (blog, job, video, item_id) VALUES ('0', '0', '1', '".$this->mysqli->insert_id."')");
			
			if($fullPostsQuery === FALSE || $blogSQL === FALSE) {
				printf("Query Failed: %s\n", $this->mysqli->error);
			} else {
				return true;
			}
		}
	}
	
	
	
	/* Edit/Update Blogs */
	public function edit_blog($title, $photo, $cat, $content) {
	
	}

	
	
	/* Delete Blogs */
	public function delete_blog($blogID) {	
	
	}
	
	
	
	/* Get All Blogs */
	public function getBlogs($start, $limit) {
		
		$str = new str_format;
		$validate = new data_validation;
		
		$sql = "SELECT * FROM blogs WHERE blog_id < $start ORDER BY blog_id DESC LIMIT $limit";

		$result = $this->mysqli->query($sql);
		
		$blogArr = array();
		
		while($row = $result->fetch_array(MYSQLI_ASSOC)) {
						
			$row = $validate->sanitize($row);		
				
			$blogArr[] = $row;
			
		}

		return $blogArr ;

	} 
	
	
	
	/* Get Blogs By Category*/
	public function get_catBlogs($cat) {
	
		$str = new str_format;
		$validate = new data_validation;

		// get the blogs from the db 
		$sql = "SELECT * FROM blogs WHERE category = '".$cat."' ORDER BY blog_id DESC";
		$result = $this->mysqli->query($sql);
					
		$blogArr = array();
		
		while($row = $result->fetch_array(MYSQLI_ASSOC)) {
						
			$row = $validate->sanitize($row);		
				
			$blogArr[] = $row;
			
		}
		return $blogArr ;	
	}
	
	
	
	/* Get One Blog */
	public function get_oneBlog($blogID) {	
	
		$str = new str_format;
		$validate = new data_validation;
		
		//get blog posts
		$blogID = $this->mysqli->real_escape_string(intval($blogID));		
		$sql = $this->mysqli->query("SELECT * FROM blogs WHERE blog_id = '" .$blogID. "'");				
		$row = $sql->fetch_assoc();
		
		$row = $validate->sanitize($row);
			
		return $row;

	}
	
	

	/* Search Blogs */
	public function searchBlogs($q) {
	
		$str = new str_format;
		$db = new DBConnection;
		$validate = new data_validation;
	
		$q = $validate->sanitize($q);
				
		//instantiate search.class
		$searchStemmer = new PorterStemmer;
		$q = $searchStemmer->Stem($q);
				
		//run the search query
		$searchSQL = "SELECT * FROM blogs WHERE (blog_title LIKE '%".$q."%') OR (author LIKE '%".$q."%') OR (category LIKE '%".$q."%') OR (content LIKE '%".$q."%') OR (keywords LIKE '%".$q."%') ORDER BY blog_id";
		$result = $this->mysqli->query($searchSQL);
			
		if($result === FALSE) {
			echo "<span class='error'>Could not perform search. Please contact network administrator.</span>";
		} else {
			if(!$result->num_rows > 0) {
				return false;
			} else {

				$blogArr = array();
				
				while($row = $result->fetch_array(MYSQLI_ASSOC)) {
					$row = $validate->sanitize($row);
					$blogArr[] = $row;
				}
				return $blogArr;
			
			} //close if(!mysqli_num_rows($searchSQL) > 0)
		} //close if($searchSQL === FALSE)
	}
	
}

?>