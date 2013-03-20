<?php

error_reporting(E_ALL); // or E_STRICT
ini_set("display_errors",1);

//handles all file uploads 
class Upload
{

	public $imageHash;
	public $where;

	public function __construct(DBConnection $mysqli) {
		$this->mysqli = $mysqli->getLink();
	}
	
	/* Upload an Image */	
	public function uploadImg($where, $user_id) {
		
		$allowedExts = array("jpg", "jpeg", "gif", "png");
		$extension = end(explode(".", $_FILES["photo"]["name"]));
		if ((($_FILES["photo"]["type"] == "image/gif")
		|| ($_FILES["photo"]["type"] == "image/jpeg")
		|| ($_FILES["photo"]["type"] == "image/png")
		|| ($_FILES["photo"]["type"] == "image/pjpeg"))
		&& ($_FILES["photo"]["size"] < 1048000)
		&& in_array($extension, $allowedExts)) {
		
			if ($_FILES["photo"]["error"] > 0) {
				echo "Return Code: " . $_FILES["photo"]["error"] . "<br>";
				$return = false;
			} else {
				
				$imageHash = sha1($_FILES['photo']['tmp_name']).".".$extension;
				
				if (file_exists("uploads/".$where."/photos/" . $_FILES["photo"]["name"])) {
					
					echo $_FILES["photo"]["name"] . " already exists. ";
					$return = false;
						
				} else {
					
					if(move_uploaded_file($_FILES["photo"]["tmp_name"],
					"uploads/".$where."/photos/" . $imageHash)) {
					
						//save image path to database
						switch ($where) {
							case 'blogs':
								$uploadURL = "/uploads/blogs/photos/".$imageHash;
								return $uploadURL;
								break;
							case 'companies':
								$uploadURL = "/uploads/companies/photos/".$imageHash;
								$photoSQL = $this->mysqli->query("UPDATE companies SET logo_path = '".$uploadURL."' WHERE id = '".$user_id."'");
								return $uploadURL;
								break;
							case 'users':
								$uploadURL = "/uploads/users/photos/".$imageHash;
								$photoSQL = $this->mysqli->query("UPDATE seekers SET photo_path = '".$uploadURL."' WHERE id = '".$user_id."'");
								return $uploadURL;
								break;
							case 'jobs':
								$uploadURL = '/uploads/jobs/photos/'.$imageHash;
								return $uploadURL;
								break;
						}
						
					} else {
						return false;
					}

				}
					
			}
		} else {
			$return = false;
		}
		return $return;
		$photoSQL->free;
		$this->mysqli->close;
	}
	
	
	/* Upload a Document */
	public function uploadDoc($where) {
		// Not Currently Done
	}
	
}



?>