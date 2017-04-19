<?php

error_reporting(E_ALL); // or E_STRICT
ini_set("display_errors",1);

//include all PHP classes
foreach (glob("classes/*.php") as $filename) {
	include_once($filename);
}

/* Controls All Comment Functions */
class Comment extends Blog
{

	private $data = array();

	public function __construct(DBConnection $mysqli) {
		$this->mysqli = $mysqli->getLink();
	}
	
	
	//	This method outputs the XHTML markup of the comment
	public function comMarkup($blog_id) {
		
		$str = new str_format;
		$validate = new data_validation;
		$db = new DBConnection;
		$user = new User($db);

		$sql = $this->mysqli->query("SELECT * FROM b_comments WHERE blog_id = '".$blog_id."' ORDER BY date DESC");
		
		while($d = $sql->fetch_assoc()) {
		
			$d = $validate->sanitize($d);
		
			// Converting the time to a UNIX timestamp:
			$d['date'] = date('F jS, Y - g:ia', strtotime($d['date']));
			
			$userID = $d['user_id'];
			
			//get the commenting user information
			$comUserType = $user->getUserType($userID);
			$comUserData = $user->getUserInfo($userID);
			
			$comContent = html_entity_decode(stripslashes($d['content']));
		
			if($comUserType === '4') {
				$photo_path = $comUserData['logo_path'];
				if(strlen($photo_path) < 5) {
					$photo_path = "/images/company_default.png";
				}
				$userName = $comUserData['company_name'];
				$profile = "/company/".$comUserData['username'];
			} else {
				$photo_path = $comUserData['photo_path'];
				if(strlen($photo_path) < 5) {
					$photo_path = "/images/default_image.gif";
				}
				$f_name = $comUserData['first_name'];
				$l_name = $comUserData['last_name'];
				$userName = $f_name. " " .$l_name;
				$profile = "/user/".$comUserData['username'];
			}	
			
			echo "
			
				<div class='comment-block'>
					<span class='com-img'><img src='".$photo_path."' /></span>
					<h3 style='display: inline;'><a href='".$profile."'>".$userName."</a></h3>
					<div class='com-date'>".$d['date']."</div>
					<p>".$comContent."</p>
				</div>
			";
		}
	}
	
	
	/* Get Number of Comments For Blog */
	public function getNumComments($id) {
		//get the number of comments on this blog
		$commentSQL = $this->mysqli->query("SELECT * FROM b_comments WHERE blog_id = '".$id."'");
		echo $commentSQL->num_rows. " comment(s)";
	}
	
	
	
	/* Add New Comment */
	public function addComment($user_id) {
	
		$validate = new data_validation;
		
		$_POST = $validate->sanitize($_POST);
		
		$newCom = htmlspecialchars($_POST['blogComment']);
		$blog_id = intval($_POST['blogID']);
		$photoSubmit = $_POST['comPhoto'];
				
		$newComQuery = $this->mysqli->query("INSERT INTO b_comments (blog_id, user_id, date, content, photo) VALUES ('".$blog_id."', '".$user_id."', Now(), '".$newCom."', '".$photoSubmit."')");
			
		if($newComQuery === false) {
			echo '{"status":0,"errors":'.json_encode($this->mysqli->error).'}';

		}else{

			 echo $this->comMarkup($blog_id);

		}			
	}
	
	
}

?>