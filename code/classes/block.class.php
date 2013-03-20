<?php

error_reporting(E_ALL); // or E_STRICT
ini_set("display_errors",1);


include_once('validation.class.php');
include_once('upload.class.php');
include_once('string.class.php');

/* Controls the homepage masonry layout */
class Blocks
{

	public $isBlog;
	public $isVid;
	public $isJob;
	public $blogID;
	public $blogTitle;
	public $blogDate;
	public $blogPhoto;
	public $blogAuthor;
	public $blogContent;
	public $jobID;
	public $jobTitle;
	public $jobCompany;
	public $jobIndustry;
	public $jobLocation;
	public $jobPhoto;
	public $jobContent;
	
	
	public function __construct(DBConnection $mysqli) {
		$this->mysqli = $mysqli->getLink();
	}
	
	public function getRecentPost() {
		$sql = $this->mysqli->query("SELECT * FROM all_posts ORDER BY post_id DESC LIMIT 1");
		$post = $sql->fetch_assoc();
		
		if($sql === FALSE) {
			printf("Query Failed: %s\n", $this->mysqli->error);
		}
		
		$id = intval($post['post_id']);
		//add one to ensure the most recent post is included
		$id = $id + 1;
		return $id;
	}
	
	
	/* Get the homepage blocks */
	public function getBlocks($start, $limit) {
	
		$str = new str_format;
	
		$allPosts = $this->mysqli->query("/*qc=on*/SELECT * FROM all_posts WHERE post_id < $start ORDER BY post_id DESC LIMIT $limit");
		//$allPosts = $this->mysqli->query("/*qc=on*/SELECT * FROM all_posts ORDER BY post_id DESC LIMIT $start,$limit");
		
		if($allPosts === FALSE) {
			printf("Query Failed: %s\n", $this->mysqli->error);
			die();
		}
		
		while($allRows = mysqli_fetch_assoc($allPosts)) {
			$postID = $this->mysqli->real_escape_string(intval($allRows['post_id']));
			$isBlog = $this->mysqli->real_escape_string(intval($allRows['blog']));
			$isJob = $this->mysqli->real_escape_string(intval($allRows['job']));
			$isVid = $this->mysqli->real_escape_string(intval($allRows['video']));
			$itemID = $this->mysqli->real_escape_string(intval($allRows['item_id']));
				
			if($isBlog === '1') {
				$query = "SELECT * FROM blogs WHERE blog_id = '".$itemID."'";
				$result = $this->mysqli->query($query);
				if($result === FALSE) {
					printf("Query Failed: %s\n", $this->mysqli->error);
				}
				while($blogRow = mysqli_fetch_assoc($result)) {
					$blogID = $this->mysqli->real_escape_string(intval($blogRow['blog_id']));
					$blogTitle = $this->mysqli->real_escape_string(html_entity_decode($blogRow['blog_title']));
					$blogDate = $blogRow['pub_date'];
					$blogPhoto = $this->mysqli->real_escape_string($blogRow['image']);
					$blogAuthor = $this->mysqli->real_escape_string($blogRow['author']);
					$blogContent = $this->mysqli->real_escape_string(nl2br($blogRow['content']));	
						
					//clean up the text
					$blogTitle = stripslashes($blogTitle);
					
					//format the blog content
					$blogContent = nl2br(str_replace('\\r\\n', "\r\n", $blogContent));
					$blogContent = html_entity_decode(stripslashes($str->truncate($blogContent, 300)));
					
					$blogURL = $str->Seo($blogTitle);
					
					echo "<div class='masonryBlock' id='".$postID."'>";
					echo "<a href='post/".$blogID."/".$blogURL."'>";
					echo "<div class='imgholder'><img src='".$blogPhoto."'></div>";
					echo "<span class='masonryTitle'>".$blogTitle."</span>";
					echo "<p>".$blogContent."</p>";
					echo "</a>";
					echo "</div>";
						
				}
			}
				
			if($isVid === '1') {
				$query = "SELECT * FROM blogs WHERE blog_id = '".$itemID."'";
				$result = $this->mysqli->query($query);
				if($result === FALSE) {
					printf("Query Failed: %s\n", $this->mysqli->error);
				}
				while($blogRow = mysqli_fetch_assoc($result)) {
					$blogID = $this->mysqli->real_escape_string(intval($blogRow['blog_id']));
					$blogTitle = $this->mysqli->real_escape_string(html_entity_decode($blogRow['blog_title']));
					$blogDate = $blogRow['pub_date'];
					$blogPhoto = $this->mysqli->real_escape_string($blogRow['image']);
					$blogAuthor = $this->mysqli->real_escape_string($blogRow['author']);
					$blogContent = $this->mysqli->real_escape_string(nl2br($blogRow['content']));
						
					//clean up the text
					$blogTitle = stripslashes($blogTitle);
					//format the blog content
					$blogContent = nl2br(str_replace('\\r\\n', "\r\n", $blogContent));
					$blogContent = html_entity_decode(stripslashes($str->truncate($blogContent, 300)));
					
					$blogURL = $str->Seo($blogTitle);
					
			
					echo "<div class='masonryBlock' id='".$postID."'>";
					echo "<a href='post/".$blogID."/".$blogURL."'>";
					echo "<div class='imgholder'><img src='".$blogPhoto."'></div>";
					echo "<span class='masonryTitle'>".$blogTitle."</span>";
					echo "<p>".$blogContent."</p>";
					echo "</a>";
					echo "</div>";
						
				}
			}
				
			if($isJob === '1') {
				$query = "SELECT * FROM jobs WHERE job_id = '".$itemID."'";
				$result = $this->mysqli->query($query);
				if($result === FALSE) {
					printf("Query Failed: %s\n", $this->mysqli->error);
				}
				while($jobRow = $result->fetch_assoc()) {
					$jobID = $this->mysqli->real_escape_string(intval($jobRow['job_id']));
					$jobTitle = $this->mysqli->real_escape_string($jobRow['job_title']);
					$jobCompany = $this->mysqli->real_escape_string($jobRow['company']);
					$jobIndustry = $this->mysqli->real_escape_string($jobRow['industry']);
					$jobLocation = $this->mysqli->real_escape_string($jobRow['location']);
					$jobPhoto = $this->mysqli->real_escape_string($jobRow['photo']);
										//format the blog content
					$jobContent = nl2br(str_replace('\\r\\n', "\r\n", $jobRow['content']));
					$jobContent = html_entity_decode(stripslashes($str->truncate($jobContent, 300)));
						
					//clean up the text
					$jobTitle = stripslashes($jobTitle);
					
					$jobURL = $str->Seo($jobTitle);
						
					echo "<div class='masonryBlock' id='".$postID."'>";
					echo "<a href='job/".$jobID."/".$jobURL."'>";
					echo "<div class='imgholder'><img src='".$jobPhoto."'></div>";
					echo "<span class='masonryTitle'>".$jobTitle."</span>";
					echo "<strong style='margin-top: -13px;><a href=''>".$jobCompany."</a></strong>";
					echo "<p>".$jobContent."</p>";
					echo "</a>";
					echo "</div>";
				}
			} 
		} 
	}
	
}

?>