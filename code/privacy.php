<?php

session_start();
//check if there is a cookie on users pc
if(isset($_COOKIE['username']) && isset($_COOKIE['logged'])) {
	
	$cookieUser = $_COOKIE['username'];
	$cookieSalt = $_COOKIE['logged'];
	
	include('includes/config.php');
	
	//match the cookie information with the database information
	$cookieSQL = $link->query("SELECT user_id, username, salt FROM users WHERE username = '".$cookieUser."' AND salt = '" .$cookieSalt. "'");
	
	if(mysqli_num_rows($cookieSQL) === 1) {
	
		$cookieRow = mysqli_fetch_assoc($cookieSQL);
		
		$user_id = intval($cookieRow['user_id']);
		$_SESSION['loggedin'] === TRUE;
		$_SESSION['user_id'] === $user_id;
	} else {
		echo "Invalid cookie information. Please login again.";
	}
}

//if user is logged in
if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id') {

	//set user_id
	$user_id = $_SESSION['user_id'];
	
	include('includes/user.header.php');	

} else {
	include('includes/anon.header.php');
} //close if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id')
?>

<div class="home_container" style="margin: 80px auto 15px auto; width: 80%;">
<p>
<h3>HireStarts Privacy Policy</h3>
</p>
<p>
HireStarts is committed to safeguarding your privacy online. Please read the following policy to understand how your information will be treated as you make use of our web site. This policy may change from time to time so please check back periodically. Information Collection and Use: 
</p>

<p>
Our site's registration form requires users to give us personal information in the form of a valid e-mail address and birthdate to authenticate users. Contact information from the registration forms are used to get in touch with users when necessary. Your IP address and User Agent are automatically logged throughout this site. This information is only available to staff and used for administration, monitoring, and diagnostic purposes. 
</p>

<p>
<b>Cookies:</b> <br />



Our site uses cookies to save your login information so you don't need to enter it every time you come to the site. We also use a cookie to store information about the account, if any, that you were last logged into for tracking and monitoring purposes. You may be able to configure your browser to accept or reject all or some cookies, or notify you when a cookie is set. Each browser is different, so check the "Help" menu of your browser to learn how to change your cookie preferences. However, you must have cookies from HireStarts enabled in order to use most functions on the site. Please note that third party advertisers may use cookies. See the appropriate section in this privacy policy for more information. 
</p>

<p>
<b>Third Party Advertisers: </b><br />



Ads appearing on this Web site may be delivered to users on HireStarts or one of our advertising partners. Our advertising partners may set cookies. These cookies allow the ad server to recognize your computer each time they send you an online advertisement. In this way, ad servers may compile information about where you, or others who are using your computer, saw their advertisements and determine which ads are clicked on. This information allows an ad network to deliver targeted advertisements that they believe will be of most interest to you. This privacy policy covers the use of cookies by HireStarts and does not cover the use of cookies by any advertisers. 
</p>

<p>
<b>External Links: </b><br />



This site contains links to other sites. HireStarts is not responsible for the privacy policies on other sites. When linking to another site a user should read their privacy policy stated on that site. Our privacy policy only governs information collected on HireStarts. 
</p>

<p>
<b>Public Areas: </b><br />



Please be aware that whenever you voluntarily post information to public areas, that information can be accessed by the public and can be be used by those people to send you unsolicited communications. 
</p>

<p>
<b>Security: </b><br />



User accounts are secured by user-created passwords. We take precautions to insure that account information is kept private. We use reasonable measures to protect user information that is stored within our database, and we restrict access to user information to staff who need to access to perform their job functions. Please note that we cannot guarantee the security of user account information. Unauthorized entry or use, hardware or software failure, and other factors may compromise the security of user information at any time. 
</p>

<p>
<b>Children's Privacy: </b><br />



Children should always ask a parent for permission before sending personal information to HireStarts, or anyone online for that matter. This site is not intended for anyone under the age of 18 with the exception of those who are 14 years of age or older and have obtained permission from their parent or legal guardian. 
</p>

<p>
<b>Correcting, Updating, or Removing Information: </b><br />



HireStarts users may modify or remove any of their personal information based on available options at any time by logging into their account and accessing the appropriate sections in "My Account". 
</p>

<p>
When your account is deleted for any reason, including you deleting your own account, all or parts of information associated with your account may be retained and not permanently deleted for an undetermined amount of time for history purposes. 
</p>

<p>
<b>Sharing of Information and Discloser: </b><br /> 



HireStarts does not rent, sell, or share personal information about you with other people without your permission except under the following conditions: 
</p>

<p>
We respond to subpoenas, court orders, or legal process, or to establish or exercise our legal rights or defend against legal claims. We believe it is necessary to share information in order to investigate, prevent, or take action regarding illegal activities, suspected fraud, situations involving potential threats to the physical safety of any person, violations of HireStarts's Terms of Service, or as otherwise required by law. 
</p>

<p>
<b>Contacting Us: </b><br />


If you have any questions about this privacy policy, the practices of this site, or your dealings with this Web site, please contact us at: support@HireStarts.com
</p>
</div>


<?php

include("includes/footer.php");

?>