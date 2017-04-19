<?php

//development only
error_reporting(E_ALL); // or E_STRICT
ini_set("display_errors",1);

//include all PHP classes
foreach (glob("classes/*.php") as $filename) {
	include_once($filename);
}



class User
{

	public $user_id;
	public $email;
	public $username;
	private $token;
	private $password;
	
	
	public function __construct(DBConnection $mysqli) {
		$this->mysqli = $mysqli->getLink();
	}

	
	
	/* New User Registration */
	public function userRegister() {
		
		$validate = new data_validation;
		
		//escape & sanitize the user input
		$_POST = $validate->sanitize($_POST);
		$f_name = $_POST['firstName'];
		$l_name = $_POST['lastName'];
		$email = $_POST['email'];
		$username = $_POST['username'];
		$password = $_POST['password'];
		$password2 = $_POST['password_confirm'];
		
		if($validate->validate_email($email) === FALSE) {
			echo "<span class='error'>Invalid email format.</span>";
			die();
		}
		
		if($validate->validate_username($username) === FALSE) {
			echo "<span class='error'>Username must be more than 3 characters and only contain letters and numbers. No spaces.</span>";
			die();
		}
		
		if($validate->validate_password($password) === FALSE) {
			echo "<span class='error'>Password must be at least 6 characters and must contain at least 1 letter.</span>";
			die();
		}
		
		if($validate->match_passwords($password, $password2) === FALSE) {
			echo "<span class='error'>Your passwords do not match.</span>";
			die();
		}
		
		//make sure there isn't duplicate emails
		$emailQuery = $this->mysqli->query("SELECT email FROM users WHERE email ='".$email."'");

		if (mysqli_num_rows($emailQuery) >= 1) {
			echo "An email is already registered with that address";
			return false;
		}

		//make sure there isn't duplicate usernames
		$userQuery = $this->mysqli->query("SELECT username FROM users WHERE username ='".$username."'");

		if ($userQuery->num_rows >= 1) {
			echo "That username is already registered.";
			return false;
		}

		// generate a random salt for converting passwords into MD5
		$salt = $this->mysqli->real_escape_string(bin2hex(mcrypt_create_iv(32, MCRYPT_DEV_URANDOM)));
		$saltedPW =  $password . $salt;
		$hashedPW = sha1($saltedPW);

		// our sql query
		$sql = $this->mysqli->query("INSERT INTO users (email, password, salt, username, session, register_date, last_login, complete_prof, user_type) VALUES ('$email', '$hashedPW', '$salt', '$username', '".$_REQUEST['PHPSESSID']."', Now(), Now(), '0', '3');");
		
		$seekerQuery = $this->mysqli->query("INSERT INTO seekers (id, first_name, last_name, username, email) VALUES ('".$this->mysqli->insert_id."', '$f_name', '$l_name', '$username', '$email');");
		if ($sql !== FALSE && $seekerQuery !== FALSE ) {

			$_SESSION['user_id'] = $this->mysqli->insert_id;
			$_SESSION['loggedin'] = TRUE;
			
			return true;
		} else {
			echo "<span class='error'>Error with user query. Please contact network administrator.</span>";
			die();
		}
		
	}
	
	
	
	
	/* User Login */
	public function Login() {
		
		//istantiate the data_validation object
		$validate = new data_validation;
		$token = $_SESSION['token'];
		
		//escape & sanitize the user input
		$_POST = $validate->sanitize($_POST);
		$email = $validate->sanitize($_POST['email']);
		$password = $validate->sanitize($_POST['password']);
		$formToken = $validate->sanitize($_POST['token']);
		
		//validate token
		if(!$formToken === $token) {
			echo "Invalid form token.";
			die();
		}
		
		if($validate->validate_email($email) === FALSE) {
			echo "Invalid email format.";
			die();
		}
		
		//get the salt from the database
		$saltQuery = $this->mysqli->query('SELECT salt FROM users WHERE email = "'.$email.'"');
		$salt = $saltQuery->fetch_assoc();
		//generate the salted password
		$saltedPW =  $password . $salt['salt'];
		//get the hashed password from the saltedPW
		$hashedPW = sha1($saltedPW);
		//check the user data against the database	
		$query = $this->mysqli->query("SELECT * FROM users WHERE email = '".$email."' AND password = '".$hashedPW."'");
		//if there is a match in the database
		if ($query->num_rows == 1) {
			$row = $query->fetch_array(MYSQLI_ASSOC);
			//escape the data from the database
			$row = $validate->sanitize($row);
			
			//get the users type
			$userType = $row['user_type'];
			//get the complete_profile boolean
			$completeProf = $row['complete_prof'];
			
			//switch case for userType sessions
			switch ($userType) {
				case '1':
					$_SESSION['admin'] = TRUE;
					break;
				case '2':
					$_SESSION['blogger'] = TRUE;
					break;
				case '3':
					$_SESSION['seeker'] = TRUE;
					break;
				case '4':
					$_SESSION['company'] = TRUE;
					break;
			}
			
			
			//set the logged-in sessions
			$_SESSION['user_id'] = $this->mysqli->real_escape_string(intval($row['user_id']));
			$_SESSION['loggedin'] = TRUE;
			
			//set the cookies (not neccessary at the moment)
			$userSQL = $this->mysqli->query("UPDATE users SET session = '".$_REQUEST['PHPSESSID']."', last_login = Now() WHERE user_id = '".$_SESSION['user_id']."'");
			
			//if profile is complete redirect to index page, else redirect to edit-profile page
			if($completeProf === '1') {
				$_SESSION['profile'] = TRUE;
				page_redirect('../index');
			} else {
				page_redirect('../edit-profile');
			}
		} else {
			page_redirect('../login?error=1');
		}
	} 

	
	
	/* Get Last Member ID */
	public function getLastUserId() {
		$sql = $this->mysqli->query("SELECT * FROM users ORDER BY user_id DESC LIMIT 1");
		
		$row = $sql->fetch_assoc();
		
		$num = $this->mysqli->real_escape_string(intval($row['user_id']));
		$num = $num + 1;
		return $num;
	}
	
	
	
	/* Get User Type */
	public function getUserType($id) {
		$validate = new data_validation;
		//query the users table for user_type
		$userTypeQuery = $this->mysqli->query("SELECT user_type FROM users WHERE user_id = '".$id."'");
		$userTypeRow = $userTypeQuery->fetch_assoc();
		$userType = $validate->sanitize(intval($userTypeRow['user_type']));
		
		return $userType;

	}
	
	
	
	
	/* Get User Information */
	public function getUserInfo($id) {
	
		//istantiate the data_validation object
		$validate = new data_validation;
		//get the user type by calling getUserType();
		$userType = $this->getUserType($id);
		
		//switch case for user type database queries
		if($userType === '4') {
			$sql = $this->mysqli->query("SELECT * FROM companies WHERE id = $id");
		} else {
			$sql = $this->mysqli->query("SELECT * FROM seekers WHERE id = $id");
		}
		//if sql query failed to get information from DB, display error
		if($sql === FALSE) {
			printf("Query Failed: %s\n", $this->mysqli->error);
		}
		//get the user info into an array
		$row = $sql->fetch_array(MYSQLI_ASSOC);
		//escape the user info array
		$row = $validate->sanitize($row);
		//return user info array
		return $row;

	}

	
	
	/* Get User Profile */
	public function getProfile($username) {
	
		//istantiate data_validation object
		$validate = new data_validation;
		
		$profileSQL = $this->mysqli->query("SELECT * FROM users WHERE username = '$username'");
		
		//if profileSQL fails, send error message
		if($profileSQL === FALSE) {
			printf("Query Failed: %s\n", $this->mysqli->error);
		}
		
		//get user profile information into array
		$row = $profileSQL->fetch_array(MYSQLI_ASSOC);	
		//get the user profile ID
		$user_id = intval($row['user_id']);
		
		//get the users profile information
		$userData = $this->getUserInfo($user_id);	
		return $userData;	

	}
	
	
	
	
	/* Update Photo */
	public function updatePhoto($id) {
		
		//istantiate the required objects (database object is required for upload object)
		$db = new DBConnection;
		$upload = new Upload($db);
		
		if(!(isset($_SESSION['company']))) {
			//if user is not a company, call the uploadImg() function passing 'users' as the $where variable
			$uploadPhoto = $upload->uploadImg('users', $id);
		} else {
			//if user is a company, call the uploadImg() function passing 'companies' as the $where variable
			$uploadPhoto = $upload->uploadImg('companies', $id);
		}
		
		//if uploadPhoto returns false
		if($uploadPhoto === FALSE) {
			return false;
		} else {
			return true;
		}
		
	}
	
	
	
	
	/* Update User Profile */
	public function updateProfile($id) {
		
		//istantiate required objects
		$db = new DBConnection;
		$validate = new data_validation;
		$tags = new Tag($db);
		
		//escape the user input
		$_POST = $validate->sanitize($_POST);
		
		//if the user is not a company
		if(!(isset($_SESSION['company']))) {
			$errorCount = 0;
			$userSchool = $_POST['school'];
			$userMajor = $_POST['major'];
			$graduationDate = $_POST['gradDate'];
			$userEmployer = $_POST['employer'];
			$userPosition = $_POST['position'];
			$userBackground = $_POST['background'];
			$userSkills = $_POST['skills'];
			$interests = $_POST['interests'];	
			
			$userInterests = $_POST['interests'];
			
			$twitterHandle = $_POST['twitter'];
			$blogURL = $_POST['blog'];
			
			if(filter_var($blogURL, FILTER_VALIDATE_URL)) {
				return true;
			} else {
				echo "<span class='error'>Invalid blog url</span>";
			}
			
			$gradDate = date("Y-m-d", strtotime($graduationDate));
			
			//combine the skills and interests tags into one array for database insertion
			$tagStr = $userSkills. ", " .$userInterests;
			//insert tags into the database using insertTags();
			$tagQuery = $tags->insertTags($tagStr);
					
			if($tagQuery === FALSE) {
				echo "Error with tags";
			}
			//update users profile
			$sql = $this->mysqli->query("UPDATE seekers SET school = '".$userSchool."', major = '".$userMajor."', graduation_date = '".$gradDate."', employer = '".$userEmployer."', position = '".$userPosition."',  twitter = '".$twitterHandle."', blog = '".$blogURL."', background = '".$userBackground."', skills = '".$userSkills."', interests = '".$userInterests."' WHERE id = '".$id."';");
					
		} else {
			
			/* Get Company Variables And Put Update Query Here */
			
		}	
		
		if($sql === FALSE) {
			echo "Profile update failed. Please try again. ";
		} else {
			//update the complete_prof field in the users table
			$profileQuery = $this->mysqli->query("UPDATE users SET complete_prof = '1' WHERE user_id = '".$id."'");
			//set the profile session 
			$_SESSION['profile'] = TRUE;

			page_redirect('edit-profile?complete=1');
		}
	}
	
	
	
	
	
	/* Change Password */
	public function changePass($id) {
		$validate = new data_validation;
		
		$oldPass = $validate->sanitize($this->mysqli->real_escape_string($_POST['currentPass']));
		$newPass = $validate->sanitize($this->mysqli->real_escape_string($_POST['newPass']));
		$confirmPass = $validate->sanitize($this->mysqli->real_escape_string($_POST['confirmPass']));
		
		$passCheck = $this->mysqli->query("SELECT password FROM users WHERE user_id = '".$id."'");
		$passRow = $passCheck->fetch_assoc();
		$currentPass = $validate->sanitize($passRow['password']);
		
		if(!$currentPass === $oldPass) {
			$ret = "<span class='error'>Invalid current password</span>";
		} elseif(!$newPass === $confirmPass) {
			$ret = "<span class='error'>Passwords do not match</span>";
		} else {
			// generate a random salt for converting passwords into MD5
			$salt = $this->mysqli->real_escape_string(bin2hex(mcrypt_create_iv(32, MCRYPT_DEV_URANDOM)));
			$saltedPW =  $newPass . $salt;
			$hashedPW = sha1($saltedPW);
			$updateQuery = $this->mysqli->query("UPDATE users SET password = '".$hashedPW."', salt = '".$salt."' WHERE user_id = '".$id."'");
			$ret = TRUE;
		}
		return $ret;
	}
	
	
	
	
	
	/* Get All Users Base Information From 'users' Table -- (user_id, complete_prof, user_type) */
	public function getAllUserBase($start, $limit) {
		
		$str = new str_format;
		$validate = new data_validation;
		
		$allUserQuery = "SELECT * FROM users WHERE user_id <= $start ORDER BY user_id DESC LIMIT $limit";
		$allResults = $this->mysqli->query($allUserQuery);
		
		$allArr = array();
		
		while($allRow = $allResults->fetch_array(MYSQLI_ASSOC)) {
			$allRow = $validate->sanitize($allRow);		
			$allArr[] = $allRow;			
		}
		return $allArr;
	}
	
	
	
	
	/* Get All Users Information From Their Specific Table (seekers or companies) */
	public function getAllUsersInfo($start, $limit) {
	
		$validate = new data_validation;
		
		//get all the users base information from the users table
		$allArr = $this->getAllUserBase($start, $limit);
		$userArr = array();

		foreach($allArr as $val) {
			if($val['complete_prof'] === '1') {
				$userInfo = $this->getUserInfo($val['user_id']);
				$userArr[] = $userInfo;
			}
		}
		return $userArr;
		
	}

	
	/* Search For Users */
	public function searchUsers($q) {
	
		$str = new str_format;
		$db = new DBConnection;
		$validate = new data_validation;
	
		$q = $validate->sanitize($q);

		$userArr = array();			
		

		/* UNION ALL 
			
			- The first 5 fields in query are renamed to satisfy both the companies and seekers
			- 'photo_path' field is used for both seekers and companies
			- 6 total index's shared
		
		*/
		
		$searchSQL = "SELECT id, first_name as name1, last_name as name2, username as user_industry, school as school_slogan, major as major_location, photo_path, background, interests, skills 
						FROM seekers 
							WHERE (first_name LIKE '%".$q."%') 
							OR (last_name LIKE '%".$q."%') 
							OR (username LIKE '%".$q."%') 
							OR (school LIKE '%".$q."%') 
							OR (major LIKE '%".$q."%') 
							OR (background LIKE '%".$q."%') 
							OR (interests LIKE '%".$q."%') 
							OR (skills LIKE '%".$q."%') 
					UNION ALL 
						SELECT id, company_name, username, industry, slogan, location, logo_path, NULL, NULL, NULL 
							FROM companies 
								WHERE (company_name LIKE '%".$q."%') 
								OR (username LIKE '%".$q."%') 
								OR (industry LIKE '%".$q."%') 
								OR (slogan LIKE '%".$q."%') 
								OR (location LIKE '%".$q."%')";
						
		$result = $this->mysqli->query($searchSQL);
			
		if($result === FALSE) {
			printf("Query Failed: %s\n", $this->mysqli->error);
		} else {
			if(!$result->num_rows > 0) {
				echo "<span class='error'>Could not find any users with the provided criteria. Please try again.</span>";
			} else {
				
				while($row = $result->fetch_array(MYSQLI_ASSOC)) {
					$row = $validate->sanitize($row);
					$userArr[] = $row;
				}
			
			} //close if(!mysqli_num_rows($searchSQL) > 0)
		} //close if($searchSQL === FALSE)
		
		return $userArr;
	}
	

	//get total number of users
	public function getNumUsers() {
		$userQuery = $this->mysqli->query("SELECT * FROM users");
									
		if($userQuery === FALSE) {
			echo "Query failed: " .$this->mysqli->error;
		} else {
			echo $userQuery->num_rows;
		}
	}
	
	
	//get number of registered job seekers
	public function getNumSeekers() {
		$seekerQuery = $this->mysqli->query("SELECT * FROM seekers");
									
		if($seekerQuery === FALSE) {
			echo "Query failed: " .$this->mysqli->error;
		} else {
			echo $seekerQuery->num_rows;
		}
	}
	
	//get number of registered companies
	public function getNumCompanies() {
		$companyQuery = $this->mysqli->query("SELECT * FROM companies");
									
		if($companyQuery === FALSE) {
			echo "Query failed: " .$this->mysqli->error;
		} else {
			echo $companyQuery->num_rows;
		}
	}
	
}

?>