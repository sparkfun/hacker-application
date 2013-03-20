<?php

session_start();

//if user is logged in
if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id') {

	//set user_id
	$user_id = $_SESSION['user_id'];
	
	include('includes/user.header.php');	

} else {
	include('includes/anon.header.php');
} //close if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id')
?>

<div class="container" style="margin: 80px auto 15px auto; width: 80%;">
<p>
<h3>About HireStarts</h3>
</p>
<p>
HireStarts is a social network designed by students, for students. We are an online community for students to showcase themselves to companies and access jobs posted specifically for our students. HireStarts seeks to modernize the hiring process for OUR generation. Our goal is for students to be able to find internships and careers, while still focusing on school. 
</p>
<p>
Our team knows the issues with the current hiring process, because we were going through the same struggle. We have built a resource both students and companies have taken advantage of, bringing the brightest candidates to the top companies with just a click of the mouse. HireStarts is showcasing students with their unique skills and abilities to help create the strongest workforce available. 
</p>
<p>
Started in October of 2011 we have spent countless hours perfecting our network. Our number one priority is to make sure everything we do is tailored to students. We are not developing our network from the executive floor; instead we are on your campus, in your classrooms and at your career fairs to make sure we can bring the necessary tools to your hands. School should be the biggest priority to a student, and we strive to make sure you are able to find a job without spending an unnecessary amount of time doing so. 
</p>
<p>
Saving money on recruiting, career fairs, sorting resumes and contacting candidates saves time and money, helping companies focus on more important goals. Never worry about hiring under qualified or undesirable employees ever again. We want our companies to be able to meet our qualified candidates before contacting them.
</p>
</div>


<?php

include("includes/footer.php");

?>