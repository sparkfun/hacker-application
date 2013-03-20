<?php

include_once('validation.class.php');
include_once('job.class.php');

class Industry extends Job
{

	public function __construct(DBConnection $mysqli) {
		$this->mysqli = $mysqli->getLink();
	}
	
	/* Company Industries By Name */
	public function getIndustry($ind) {
		$validate = new data_validation;
		
		$industryQuery = $this->mysqli->query("SELECT * FROM industries WHERE name = '".$ind."'");
		$indRow = $industryQuery->fetch_assoc();
		$indName = $validate->sanitize($indRow['name']);
		
		return $indName;
		$industryQuery->free;
		$this->mysqli->close;
	}
	
	/* Company Industries By ID */
	public function getIndustryByID($ind) {
		$validate = new data_validation;
		
		$industryQuery = $this->mysqli->query("SELECT * FROM industries WHERE ind_id = '".$ind."'");
		$indRow = $industryQuery->fetch_assoc();
		$indRow = $validate->sanitize($indRow);
		
		return $indRow;
	}
	
	public function getIndustries() {
		$validate = new data_validation;
		//get the categories
		$indSQL = $this->mysqli->query("/*qc=on*/SELECT * FROM industries ORDER BY name ASC");
		
		$indArr = array();	
		while($indRow = $indSQL->fetch_array(MYSQLI_ASSOC)) {		
			
			$row = $validate->sanitize($indRow);
			
			$indArr[] = $row;
		
		} //close while($catRow = mysqli_fetch_assoc($catSQL))
		return $indArr;
		
		$indSQL->free;
		$this->mysqli->close;
	}

}


?>