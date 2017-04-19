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
	
	//istantiate required objects
	$db = new DBConnection;
	$jobs = new Job($db);
	$str = new str_format;
	$user = new User($db);
	$industry = new Industry($db);
	
	//get the current user data
	$curData = $user->getUserInfo($user_id);
	
	//if the company session is not set
	if(!(isset($_SESSION['company']))) {
		//set the users first and last name and get their photo
		$firstName = $curData['first_name'];
		$lastName = $curData['last_name'];
		$poster = $firstName. " " .$lastName;
		$photo = $curData['photo_path'];
	
	} else {
		//set the company name and get their logo
		$poster = $curData['company_name'];
		$photo = $curData['logo_path'];
	}
		
	$token = token();
	
	$start = $jobs->getLastJobId();
	$limit = '10';

?>


	<div class='sidebar'>
		<form class='side-search form-search' name='blog-search' method='GET' action='<?php echo $_SERVER['PHP_SELF']; ?>'>
			<input type='text' name='q' class='job_search search-query' placeholder='Search by title, author, or keyword' />
			<input type='hidden' name='token' value='<?php echo $_SESSION['token']; ?>' />
			<input type='submit' value='Search' class='job_search_btn btn'>
		</form>
	

			<?php	
				if(isset($_SESSION['company']) || isset($_SESSION['admin'])) {
					echo "<div class='sideButton'><a data-toggle='modal' data-target='#addJob' href='#'><button class='btn btn-primary'>Post Job</button></a></div>";
				}
			?>
		<ul class='sideList'>
			<?php
				$industries = $industry->getIndustries();
				foreach($industries as $ind => $val) {
					$url = $str->Seo($val['name']);
					echo "<li><a href='/opportunities?ind=" .$val['ind_id']. "&t=".$url."'>" .$val['name']. "</a></li>";
				}
			?>
			
		</ul>	
	</div>
	
	
	<div class='post-container'>
		<?php
			/* Get Jobs - Standard View */
			if(!(isset($_GET['q'])) && !isset($_GET['ind'])) {
				
				//get the job data calling the getJobs() function
				$jobData = $jobs->getJobs($start, $limit);
				
				//foreach job returned from the getJobs() function
				foreach($jobData as $job => $val) {
				
					/* Format The Job Data For Display */
					$jobURL = $str->Seo($val['job_title']);
					$content = html_entity_decode(nl2br(stripslashes($str->truncate($val['content'], 375))));
					//get the poster information
					$userData = $user->getUserInfo($val['company_id']);
					//create the users profile link
					$profile = "/company/".$userData['username'];
		?>

		<div class='post'>
			<span class='post-photo'>
				<img src='<?php echo $val['photo']; ?>'>
		</span>
			<span class='post-title'><a href="/job/<?php echo $val['job_id']."/".$jobURL; ?>"><?php echo $val['job_title']; ?></a></span><br />
			<span class='post-author'>By <a href='<?php echo $profile; ?>'><?php echo $val['company']; ?></a></span>
			<span class='post-date'><img src='images/icon_date.gif' class='icon'><?php echo $val['date']; ?></span>
				<p class='post-content'>
					<p>
						<?php echo $content; ?>
						<span class='read_more'><a href='/job/<?php echo $val['job_id']."/".$jobURL; ?>'> Read More</a></span>
					</p>
				</p>
		</div>

		<?php		
				}//close foreach(standard jobs)
				
			/* If isset(search query) */
			} elseif(isset($_GET['q'])) {
				//get the search query from URL
				$q = $_GET['q'];
				//get the job data using searchJobs() passing the search query
				$jobData = $jobs->searchJobs($q);
				
				if($jobData === FALSE) {
					echo "<span class='error'>Could not match any jobs with the provided criteria. Please try again.</span>";
					die();
				}
				//foreach job returned from searchJobs($q)
				foreach($jobData as $job => $val) {
				
					/* Format The Job Data For Display */
					$jobURL = $str->Seo($val['job_title']);
					$content = html_entity_decode(nl2br(stripslashes($str->truncate($val['content'], 375))));
					//get the poster information
					$userData = $user->getUserInfo($val['company_id']);
					//create the users profile link
					$profile = "/company/".$userData['username'];

		?>
		
		<div class='post'>
			<span class='post-photo'>
				<img src='<?php echo $val['photo']; ?>'>
			</span>
			<span class='post-title'><a href="/job/<?php echo $val['job_id']."/".$jobURL; ?>"><?php echo $val['job_title']; ?></a></span><br />
			<span class='post-author'>By <a href='<?php echo $profile; ?>'><?php echo $val['company']; ?></a></span>
			<span class='post-date'><img src='images/icon_date.gif' class='icon'><?php echo $val['date']; ?></span>
				<p class='post-content'>
					<p>
						<?php echo $content; ?>
						<span class='read_more'><a href='/job/<?php echo $val['job_id']."/".$jobURL; ?>'> Read More</a></span>
					</p>
				</p>
		</div>
		
		<?php
				}//close foreach(jobData search)
				
			/* If isset(industry) */	
			} else {
				
				//get the requested industry
				$jobInd = intval($_GET['ind']);
				
				//get the job data using getJobsInd passing the industry ID
				$jobData = $jobs->getJobsInd($jobInd);
				//foreach job returned from getJobsInd($jobInd)
				foreach($jobData as $job => $val) {
				
					/* Format The Job Data For Display */
					$jobURL = $str->Seo($val['job_title']);
					$content = html_entity_decode(nl2br(stripslashes($str->truncate($val['content'], 375))));
					//get the poster information
					$userData = $user->getUserInfo($val['company_id']);
					//create the users profile link
					$profile = "/company/".$userData['username'];
				
		?>
		
		<div class='post'>
			<span class='post-photo'>
				<img src='<?php echo $val['photo']; ?>'>
			</span>
			<span class='post-title'><a href="/job/<?php echo $val['job_id']."/".$jobURL; ?>"><?php echo $val['job_title']; ?></a></span><br />
			<span class='post-author'>By <a href='<?php echo $profile; ?>'><?php echo $val['company']; ?></a></span>
			<span class='post-date'><img src='images/icon_date.gif' class='icon'><?php echo $val['date']; ?></span>
				<p class='post-content'>
					<p>
						<?php echo $content; ?>
						<span class='read_more'><a href='/job/<?php echo $val['job_id']."/".$jobURL; ?>'> Read More</a></span>
					</p>
				</p>
		</div>
		
		<script type="text/javascript">
		//job infinite scroll
				var loading = false;
				$(window).scroll(function(){
			 
					if($(window).scrollTop() == $(document).height() - $(window).height()){
						loading = true;
						$('#ajaxLoader').show();
						$.ajax({
							url: "/ajax/job.process.php?lastid=" + $(".post:last").attr("id"),
							success: function(html){
								if(html){
									$(".post-container").append(html);
									$('div#ajaxLoader').hide();
								}else{
									$('div#ajaxLoader').html('<center>No more jobs to show.</center>');
								}
								loading = false;
							}
						});
					}
				});
		</script>
		<div id="ajaxLoader"></div>
		
		<?php
			
				}//close foreach(job industry)
			}
			/* End Job Listings Completely */
			
			
			//if a job is added
			if(isset($_POST['addJob'])) {
				//insert the job using the insertJob() function of the Job object
				$addJob = $job->insertJob();
				//if insertJob() returns false send an error
				if($addJob === FALSE) {
					echo "<span class='error'>There was an error adding your job.</span>";
				}
			}
		
	?>		
		<div id="ajaxLoader"></div>	
		
		<!-- Add Job Modal -->
		<div id='addJob' class='modal-add-job hide fade'>
			<div class='modal-header'>
				<a class='close' data-dismiss='modal'>×</a>
				<h3>Post Job</h3>
			</div>
				
			<div class='modal-add-job-body'>
				<div class='modal-form'>
				<?php
					//user can only post a job if they are a company or an administrator
					if(isset($_SESSION['company']) || isset($_SESSION['admin'])) { 
						
				?>

					<form id="addJobForm" name='addJobForm' action='<?php echo $_SERVER['PHP_SELF']; ?>' method='POST'  enctype='multipart/form-data' onSubmit='ajaxSubmit(this);'>
						<div class='edit-profile-field'>
							<label>Job Title:</label>
							<input type='text' class='job-input' name='jobTitle' maxlength='55' placeholder='i.e. Front-end Engineer' required />
						</div>
						
						<div class='edit-profile-field'>
							<label>Upload New Photo:</label>
							<input type='checkbox' name='photoCheck' id='newPhoto' />
							<span class='tip' style='margin-left: 10px;'>If you do not upload a photo your profile photo will be used.</span>
						
						
							<input style='margin: 15px 0 15px 30px;' type='file' id='photoInput' class='job-input' name='photo' />
						</div>
						
						<div class='edit-profile-field' style='margin: 15px 0;'>
							<label>Department:</label>
							<select class="job-input" name='industry' value='Select a Department'>
								<?php
									//get the job industries for option select
									$industries = $industry->getIndustries();
									foreach($industries as $ind => $indData) {
										echo "<option value='".$indData['ind_id']."'>".$indData['name']."</option>";
									}
								?>
				
							</select>
						</div>
						
						<div class='edit-profile-field'>
							<label>Location:</label>
							<input type='text' class='job-input' name='jobLocation' maxlength='75' placeholder='i.e. Kalamazoo, MI' required />
						</div>
						
						<div class='edit-profile-field'>
							<label>Contact Email:</label>
							<input type='text' class='job-input' name='contactEmail' maxlength='75' placeholder='Optional' pattern='([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,6})' onchange='
  this.setCustomValidity(this.validity.patternMismatch ? \"Invalid email format.\" : \"\");'/>
						</div>
						
						<div class='edit-profile-field'>
							<label>Description:</label>
							<textarea class='add_txt' name='jobContent' placeholder='Job Description' required></textarea>
						</div>
						
						<div class='edit-profile-field'>
							<label>Requirements:</label>
							<input type='text' class='job-input' name='jobReq' maxlength='75' placeholder='Separate skills with a comma' required />
							<span class='tip'>Separate skills with a comma</span>
						</div>
						
						<div class='edit-profile-field'>
							<label>Preferred:</label>
							<input type='text' class='job-input' name='jobPref' maxlength='75' placeholder='Separate skills with a comma' required />
							<span class='tip'>Separate skills with a comma</span>
						</div>
						
						<div class='edit-profile-field'>
							<label>Keywords/Tags:</label>
							<input type='text' id="keywords" class='job-input' name='tags' maxlength='140' placeholder='Separate tags with a comma' required />
						</div>
						
						<input type='hidden' name='poster' value='<?php echo $poster; ?>' />
						<input type='hidden' name='photo' value='<?php echo $photo; ?>' />
						<input type='hidden' name='posterID' value='<?php echo $user_id; ?>' />
				
						<div class='modal-footer'>
							<input type='submit' id="addJob" name='addJob' class='btn btn-success' value='Post Job'>
							<a href='#' class='btn' data-dismiss='modal'>Cancel</a>
						</div>
					</form>
					<?php					 
						} else {						
							echo "<span class='error'>Only companies may post jobs.</span>";
						}//close if(isset($_SESSION['company'])) for form modal
					?>
				</div>
			</div>
		</div>

		
<?php
		
	echo "</div>";	
echo "</div>";

			
	//user is not logged in
	} else {		
		header("Location: index");
	}//close if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'user_id')

include('includes/footer.php');

?>