<?php

error_reporting(E_ALL); // or E_STRICT
ini_set("display_errors",1);

//include all PHP classes
foreach (glob("classes/*.php") as $filename) {
	include_once($filename);
}

/* Controls All Administrator Functions */
class Admin
{
		
	public function __construct(DBConnection $mysqli) {
		$this->mysqli = $mysqli->getLink();
	}
	
	//add administrator messages to database
	public function postAdminMessage() {
		
		$validate = new data_validation;
		
		$_POST = $validate->sanitize($_POST);
		
		$message = htmlentities($_POST['message']);
		$poster = $_POST['poster'];
					
		$addQuery = $this->mysqli->query("INSERT INTO a_messages (poster, date, message) VALUES ('".$poster."', Now(), '".$message."')");
					
		if($addQuery === FALSE) {
			echo "Query Failed: " .$this->mysqli->error;
		}
	}
	
	//get administrator messages from database
	public function getAdminMessages() {
	
		$validate = new data_validation;
		
		//get the messages from the db
		$messageSQL = $this->mysqli->query("SELECT * FROM a_messages ORDER BY date DESC LIMIT 3");
				
		if($messageSQL === FALSE) {
			echo "Query Failed: " .$this->mysqli->error;
		}
		
		$messageArr = array();	
		while($row = $messageSQL->fetch_array(MYSQLI_ASSOC)) {
			$row = $validate->sanitize($row);
			$messageArr[] = $row;
		}
		return $messageArr;
	}
	
}

?>