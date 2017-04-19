<?php

error_reporting(E_ALL); // or E_STRICT
ini_set("display_errors",1);

//include all PHP classes
foreach (glob("classes/*.php") as $filename) {
	include_once($filename);
}

/* Network Post Tags */
class Tag
{
		
	public function __construct(DBConnection $mysqli) {
		$this->mysqli = $mysqli->getLink();
	}
	
	/* Get All Tags */
	public function getTags() {
		$validate = new data_validation;
		$sql = $this->mysqli->query("SELECT * FROM tags");
	
		$tagArr = array();
		
		while($row = $sql->fetch_array(MYSQLI_ASSOC)) {
						
			$row = $validate->sanitize($row);		
				
			$tagArr[] = $row;
			
		}//close while($row = $this->mysqli->fetch_assoc($sql))

		return $tagArr ;
	}
	
	/* Get A Limited Number Of Tags */
	public function getTagLimit($limit) {
		$validate = new data_validation;
		$sql = $this->mysqli->query("SELECT * FROM tags ORDER BY tag_id LIMIT $limit");
	
		$tagArr = array();
		
		while($row = $sql->fetch_array(MYSQLI_ASSOC)) {
						
			$row = $validate->sanitize($row);		
				
			$tagArr[] = $row;
			
		}//close while($row = $this->mysqli->fetch_assoc($sql))

		return $tagArr ;
	}
	
	/* Insert New Tags */
	public function insertTags($tagStr) {
		
		$tags = explode(',', $tagStr);
		$tags = array_unique($tags);
	
		foreach($tags as $tag) {
			$tagQuery = $this->mysqli->query("INSERT IGNORE INTO tags (tag) VALUES('".$tag."')");
		}
		if($tagQuery === FALSE) {
			printf($this->mysqli->error);
			return false;
		} else {
			return true;
		}
	}
}
?>