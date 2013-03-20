<?php

//include all PHP classes
foreach (glob("classes/*.php") as $filename) {
	include_once($filename);
}

/* Controls All Blog Categories */
class Category extends Blog
{

	private $mysqli;
	public $catName;
	public $catArray;

	
	public function __construct(DBConnection $mysqli) {
		$this->mysqli = $mysqli->getLink();
	}
	
	
	
	/* Insert New Category */
	public function insert_cat($catName) {
		$catName = $this->mysqli->real_escape_string($catName);
		$validate = new data_validation;
		
		if(!($validate->validate_string($catName))) {
			echo "Invalid characters found in category name";
			die();
		}
		
		$query = $this->mysqli->query("INSERT INTO categories(name) VALUES ('".$catName."')");
		
		if($query === false) {
			printf("Error: %s\n", $this->mysqli->error);
			die();
		} else {
			return true;
		}
	}
	
	
	
	/* Delete Categories */
	public function delete_cat($catID) {
		
	}
	
	
	
	/* Get All Categories From Database */
	public function get_allCats() {
		$validate = new data_validation;
		
		$result = $this->mysqli->query("/*qc=on*/SELECT * FROM categories ORDER BY cat_name ASC");
		
		if($result === false) {
			printf("Error: %s\n", $this->mysqli->error);
			die();
		} else {
		
			$catArray = array();      
			while ($row = $result->fetch_array(MYSQLI_ASSOC)) {
				$catArray = $validate->sanitize($catArray);
				$catArray["$row[cat_id]"] = $row['cat_name'];
			}
			return $catArray;
		}
	}
	
}

?>