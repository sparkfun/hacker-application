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

<div class="container" style="margin: 80px auto 15px auto; width: 80%;">
<p>
<h3>HireStarts Developers</h3>
</p>
<p>
As a developer you can create applications and add them to HireStarts, taking advantage of the existing user base. You host the application, which means that you are in full control of your server specifications.
</p>
<p>
<b>Setting up an App:</b><br />
To interact with HireStarts your application can issue calls to our API and request or post information.
When you add an application to our site we will give you an APP ID.
</p>
<p>
<b>Requesting a Token:</b><br />
Whenever you plan on using our API you must first request a token. In order to request a token you need a unique key that we send to you when a user visits your APP from an iframe on our site. We pass this along as <b>$_GET['key']</b>.
This is an example of how you can request a token:
</p>
<pre>
http://hirestarts.com/token.php?key=$_GET['key']
</pre>
<p>
<b>If successful, you will get a JSON response like:</b>
</p>

<pre>
Object
(
    [token] => LS0tLS1CRUdJTiBQVUJMSUMgS0VZLS0tLS0KTUc4d0RRWUpLb1pJaHZjTkFRRUJCUUFEWGdBd1d3SlVBdHFZdmVWOXFEdDd6NFhXTXYzS3VZM2JyWXpUKzR0VgpBbERrN1dQWjhqRVpoVzBNWjE1Z3lHdGNlNm5ueFRNenp4SXpHM29BRVIzc0JVRCtYdStHb21JeVV4UE1RN1NtCkVPdFg0ZTNwekp6R081cUxBZ01CQUFFPQotLS0tLUVORCBQVUJMSUMgS0VZLS0tLS0K
)
</pre>

<p>
<b>Sending a Request:</b><br />

Now that you have a valid token you can make requests to our server. With each request you must pass the token we created for you.
An example call to our API server would look like:
<pre>http://hirestarts.com/api.php?token=#{TOKEN}&method=#{METHOD_NAME}</pre>
</p>
<p>
<b>Understanding an API Response:</b><br />
For methods that could return more than one item the response will contain an indicator of the total items available as well as how many pages there are. We return by default 10 items at most and in order to get the next 10 items you would have to pass the param "page=2".
To the left you will find a list of the modules that implement API methods. Click on the module and you will see a list of the methods that your application can use. For shortness and formatting purposes we do not include the full request in there but only the most relevant parts.
</p>
</div>


<?php

include("includes/footer.php");

?>