<?php

include_once('../classes/db.class.php');
include_once("../classes/job.class.php");
include_once("../classes/string.class.php");
include_once("../classes/user.class.php");

//if there is a query in the URL
if(isset($_GET['lastid'])) {
	
	$db = new DBConnection;
	$jobs = new Job($db);
	$str = new str_format;
	$user = new User($db);
	
	$start = intval($_GET['lastid']);
	$limit = "10";
	
	/* Get Jobs - Standard View */			
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
} elseif(!(isset($_GET['lastid']))) {

	/* Place the ADD JOB AJAX CALL HERE */
	
}
?>
