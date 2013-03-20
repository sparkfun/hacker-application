<?php

error_reporting(E_ALL); // or E_STRICT
ini_set("display_errors",1);

//include all PHP classes
foreach (glob("classes/*.php") as $filename) {
	include_once($filename);
}


/* Controls all Job Functions */
class Job 
{
		
	public function __construct(DBConnection $mysqli) {
		$this->mysqli = $mysqli->getLink();
	}
	
	/* Insert New Job */
	public function insertJob() {
		
		//istantiate required objects
		$db = new DBConnection;
		$upload = new Upload($db);
		$validate = new data_validation;
		$tag = new Tag($db);
		
		//escape everything the user has entered
		$_POST = $validate->sanitize($_POST);
		
		//put user entered information into variables
		$jobTitle = $_POST['jobTitle'];
		$jobLocation = $_POST['jobLocation'];
		$contactEmail = $_POST['contactEmail'];
		$jobIndustry = $_POST['industry'];
		$jobCompany = $_POST['poster'];
		$jobPhoto = $_POST['photo'];
		$jobCompID = intval($_POST['posterID']);
		$jobContent = $_POST['jobContent'];
		$tagStr = $_POST['tags'];
		
		//validate the information is correct
		if($validate->validate_email($contactEmail)) {
			$ret = "Invalid Contact Email!";
			return $ret;
		}
		//if they don't upload a new photo
		if(!(isset($_FILES['newPhoto']))) {
			//the upload URL remains the same as their current logo
			$uploadURL = $jobPhoto;
		} else {
			//if a photo is uploaded, use the Upload object and call the uploadImg function, passing 'jobs' as the $where variable
			$uploadURL = $upload->uploadImg('jobs');
		}
		//if there was an error uploading the photo, let us know
		if($uploadURL === FALSE) {
			$ret = "<span class='error'>Could not upload image</span>";
			return $ret;
		}
		
		//call the Tag object and insert the job keywords as 'Tags' in the database
		$tagQuery = $tags->insertTags($tagStr);
		
		//insert the job into the jobs table
		$addQuery = $this->mysqli->query("INSERT INTO jobs(job_title, company, company_id, industry, date, location, contact_email, photo, content) VALUES ('".$jobTitle."', '".$jobCompany."', '".$jobCompID."', '".$jobIndustry."', Now(), '".$jobLocation."', '".$contactEmail."', '".$uploadURL."', '".$jobContent."')");
		
		//if an error occured during database update display it
		if($addQuery === FALSE) {
			$ret = $this->mysqli->error;
			return $ret;
		} else {
			//if addQuery is successful, get the last inserted ID into the database
			$last_id = $this->mysqli->last_id;
			//add the job to the all_posts table
			$fullPostsQuery = $this->mysqli->query("INSERT INTO all_posts (blog, job, video, item_id) VALUES ('0', '1', '0', '".$last_id."')");
			//if fullPostsQuery fails send an error
			if($fullPostsQuery === FALSE) {
				$ret = $this->mysqli->error;
				return $ret;
			} else {								
				//if everything is a success, redirect the user with a success query
				page_redirect('../opportunities?post=success');
			}
		}		
		
	}
	
	
	
	/* Delete Job */
	public function deleteJob($id) {
	
	}
	
	
	
	/* Edit Jobs */
	public function editJob($id) {
	
	}
	
	
	public function getLastJobId() {
		$sql = $this->mysqli->query("SELECT * FROM jobs ORDER BY job_id DESC LIMIT 1");
		
		$row = $sql->fetch_assoc();
		
		$num = $this->mysqli->real_escape_string($row['job_id']);
		return $num;
	}
	
	
	
	/* Get All Jobs - No Requirements */
	public function getJobs($start, $limit) {
		
		//istantiate required objects
		$str = new str_format;
		$validate = new data_validation;
		//select the jobs from the database
		$sql = "SELECT * FROM jobs WHERE job_id <= $start ORDER BY job_id DESC LIMIT $limit";
		$result = $this->mysqli->query($sql);
		
		//create the job array
		$jobArr = array();
		
		while($row = $result->fetch_array(MYSQLI_ASSOC)) {
			//validate the results returned from $sql
			$row = $validate->sanitize($row);		
			//place the results in the $jobArr array	
			$jobArr[] = $row;		
		}	
		//return the array
		return $jobArr ;		
	} 
	
	
	
	/* Get Jobs By Company */
	public function getCompanyJobs($id) {
		
		//istantiate required objects
		$str = new str_format;
		$validate = new data_validation;
		//select the jobs from the database
		$sql = "SELECT * FROM jobs WHERE company_id = $id";
		$result = $this->mysqli->query($sql);
		
		//create the job array
		$jobArr = array();
		
		while($row = $result->fetch_array(MYSQLI_ASSOC)) {
			//validate the results returned from $sql
			$row = $validate->sanitize($row);		
			//place the results in the $jobArr array	
			$jobArr[] = $row;		
		}	
		//return the array
		return $jobArr ;		
	} 

	
	
	/* Search Jobs */
	public function searchJobs($q) {
	
		$str = new str_format;
		$validate = new data_validation;
	
		$q = $validate->sanitize($q);
				
		//instantiate search.class
		$searchStemmer = new PorterStemmer;
		$q = $searchStemmer->Stem($q);
				
		//run the search query
		$searchSQL = "SELECT * FROM jobs WHERE (job_title LIKE '%".$q."%') OR (company LIKE '%".$q."%') OR (industry LIKE '%".$q."%') OR (location LIKE '%".$q."%') OR (content LIKE '%".$q."%') ORDER BY job_id";
		$result = $this->mysqli->query($searchSQL);
			
		if($result === FALSE) {
			echo "<span class='error'>Could not perform search. Please contact network administrator.</span>";
		} else {
			if(!$result->num_rows > 0) {
				return false;
			} else {

				$jobArr = array();
				
				while($row = $result->fetch_array(MYSQLI_ASSOC)) {
					$row = $validate->sanitize($row);
					$jobArr[] = $row;
				}
				
				return $jobArr;
			
			} //close if(!mysqli_num_rows($searchSQL) > 0)
		} //close if($searchSQL === FALSE)
	}
	
	
	
	
	/* Get Jobs By Industry */
	public function getJobsInd($ind) {
		
		$str = new str_format;
		$validate = new data_validation;
		
		$sql = "SELECT * FROM jobs WHERE industry = '".$ind."' ORDER BY job_id DESC";

		$result = $this->mysqli->query($sql);
		
		$jobArr = array();
		
		while($row = $result->fetch_array(MYSQLI_ASSOC)) {
						
			$row = $validate->sanitize($row);		
				
			$jobArr[] = $row;
			
		}//close while($row = $this->mysqli->fetch_assoc($sql))

		return $jobArr ;	
		
		$result->free;
		$this->mysqli->close;
	} 
	
	
	
	/* Get One Job Listing - For Single Job Page */
	public function getJobListing($id) {
		$db = new DBConnection;
		$str = new str_format;
		$validate = new data_validation;
		
		$sql = $this->mysqli->query("SELECT * FROM jobs WHERE job_id = '".$id."'");
		
		$row = $sql->fetch_array(MYSQLI_ASSOC);
		$row = $validate->sanitize($row);		

		return $row ;
		
		$sql->free;
		$this->mysqli->close;
	}
	
}

?>