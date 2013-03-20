<?php 

session_start();

//if user is logged in
if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id') {

	//is the users profile complete?
	if(!(isset($_SESSION['profile']))) {
		Header('Location: edit-profile');
	}
	
	//set user_id
	$user_id = $_SESSION['user_id'];
	
	include('includes/user.header.php');
	
	$db = new DBConnection;
	$jobs = new Job($db);
	$str = new str_format;
	$user = new User($db);
	
	if(isset($_SESSION['company'])) {
		
		//get company information from database and cache it
		$sql = $link->query('/*qc=on*/SELECT * FROM companies WHERE id = "'.$user_id.'"');
		$row = mysqli_fetch_assoc($sql);
		
		$companyName = $link->real_escape_string($row['company_name']);
		$username = $link->real_escape_string($row['username']);
		
		$logo = $link->real_escape_string($row['logo_path']);

		$industry = $link->real_escape_string($row['industry']);
	}

			//get job posts
			$jobID = $_GET['id'];
			
			$jobData = $jobs->getJobListing($jobID);
			$jobTitle = stripslashes($jobData['job_title']);
			$jobURL = $str->Seo($jobTitle);
			
			$compData = $user->getUserInfo($jobData['company_id']);
			$profile = "/company/".$compData['username'];
			
			$postDate = date('F jS, Y', strtotime($jobData['date']));
			$jobContent = html_entity_decode(nl2br(stripslashes($jobData['content'])));

	
?>
	
	
	<div class='sidebar'>
		<a data-toggle='modal' data-target='#applyMod' href='#'><button class='btn btn-primary' style='width: 150px; margin: 30px auto;'>Apply</button></a>
		
		
	</div>
	
	<div class='post-container'>
			<span class='post-photo'>
				<img src='<?php echo $jobData['photo']; ?>'>
		</span>
			<span class='post-title'><?php echo "<a href='/job/".$jobData['job_id']."/".$jobURL."'>".$jobTitle."</a>";?></span><br />
			<span class='post-author'>By <a href='<?php echo $profile; ?>'><?php echo $jobData['company']; ?></a></span>
			<span class='post-date'><img src='/images/icon_date.gif' class='icon'><?php echo $postDate; ?></span>
				<p class='post-content'>
					<p>
						<?php echo $jobContent; ?>
					</p>
				</p>
		</div>

	
	<!-- Application Modal -->
		<div id="applyMod" class="modal-add-job hide fade">
			<div class="modal-header">
				<a class="close" data-dismiss="modal">×</a>
				<h3>Apply</h3>
			</div>
				
			<div class="modal-body">
				<div class="modal-form">
					<form name="applyJobForm" action="<?php echo $_SERVER['PHP_SELF']; ?>" method="POST">
						
						<label style="font-weight: bold;">Upload Resume: </label>
						<input type="checkbox" value="upload" id="uploadResume" />
						
						<label style="font-weight: bold;">Send Profile: </label>
						<input type="checkbox" value="profile" id="sendProfile" />
						
						<div style="margin-top: 10px;">
							<label class="add_label">Notes:</label>
							<textarea class="add_txt" name="jobContent" placeholder="Additional Notes" required></textarea>
						</div>
				
						<div class="modal-footer">
							<input type="submit" name="jobApp" class="btn btn-success" value="Submit Application">
							<a href="#" class="btn" data-dismiss="modal">Cancel</a>
						</div>
					</form>
				</div>
			</div>
		</div>

</div>



<?php

} else {
	header("Location: index");
}//close if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'user_id')

include('includes/footer.php');

?>