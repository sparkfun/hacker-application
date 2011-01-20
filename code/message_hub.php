<?php
/************************************************************************************
* message_hub.php is a simple internal messaging system that uses MySQL to store    *
* messages.  message_recipients.php is called from an AJAX request in the compose   *
* mode.  Four modes are possible: compose, view, del, or inbox as default.          *
************************************************************************************/
if(isset($_GET['mode'])){
  if($_GET['mode'] == 'view') $_PAGE['title'] = "ACS Messaging System -> View Message";
  elseif($_GET['mode'] == 'compose') $_PAGE['title'] = "ACS Messaging System -> Compose Message";
  else $_PAGE['title'] = 'ACS Messaging System -> Inbox';
}
else $_PAGE['title'] = 'ACS Messaging System -> Inbox';
$_PAGE['require_login'] = 1;
$_PAGE['path_to_root'] = "../";

include("../incs/header.php");
include("incs/common.inc");
?>
<script src='incs/functions/dbQuery.js'></script>
<script type='text/javascript'>
	document.getElementById('msg_div').style.display = 'none'; // hide the msg_div that appears on all pages.
<?php
// Javascript code for the compose mode
if($mode=='compose')
echo "
	var recipNum = 0;
	/************************************************************************************
	 * This function is used when the user is choosing message recipients.  when        *
	 * certain keys are pressed different actions happen.                               *
	 ************************************************************************************/
	function whichKey(event)
	{
		if(event.keyCode == '38'){ 		//if the down arrow is pressed
			if(recipNum > 0){ 			//can't select a negative recipNum
				document.getElementById('recip_'+recipNum).style.backgroundColor='#DDEEFF';
				recipNum--;
				document.getElementById('recip_'+recipNum).style.backgroundColor='#CCDDFF';
				return;
			}
		}
		else if(event.keyCode == '40'){		//if the up arrow is pressed
			if(document.getElementById('recip_'+(recipNum+1)) != null){ //prevents selecting non existent recipient
				document.getElementById('recip_'+recipNum).style.backgroundColor='#DDEEFF';
				recipNum++;
				document.getElementById('recip_'+recipNum).style.backgroundColor='#CCDDFF';
				return;
			}
		}
		// if return, comma, spacebar, or right arrow is pressed: execute block
		else if((event.keyCode == '13') || (event.keyCode == '9') || (event.keyCode == '188') || (event.keyCode == '32')){
			to_obj = document.forms.msg_form.to;  //get the object for the to field
			to_obj.value = to_obj.value.substr(0,document.forms.msg_form.to.value.length - 1); //subtract the last character of the value
			if(document.getElementById('user_div').style.display == ''){ //if the user div is visible
				set_recipient(document.getElementById('recip_'+recipNum).attributes.user_id.value);
				return;
			}else {
				to_obj.value = to_obj.value.replace(/[\\s\\n]/,''); // clean up unwanted characters
			}
		}
		else //if any other key is pressed initiate a search for matching users.
			get_users(document.forms.msg_form.to.value);
	}

	/*******************************************************************************
	 * This function is called when the user is typing a name in the recipient box *
	 * the output will be an AJAX document displayed in a div called user_div      *
	 *******************************************************************************/
	function get_users(value)
	{
		search = value.substring(value.lastIndexOf(',') + 1); //get string following the last comma in the box
		if(search.length > 0){

			send_request('message_recipients.php?to='+search, 'user_div',null,'subject', null,null,0,0); // AJAX call; output into user_div
			document.getElementById('user_div').style.display = ''; 		// show user div
		}else
			document.getElementById('user_div').style.display = 'none';  	// hide user div

		recipNum = 0;
	}
	/*********************************************
	* This is called when 'user_div' loses focus *
	*********************************************/
	function hide_user_div()
	{
		setTimeout(\"document.getElementById('user_div').style.display = 'none';document.forms.msg_form.subject.focus();\",250);
	}

	/************************************************
	 * A quick message validation to help prevent   *
	 * accidental sending of blank messages         *
	 ************************************************/
	function validate_message(mData)
	{
		with(mData){
			try{
				if(to.value == '')
					throw new Error('The message has no recipient.');
				if((subject.value == '') && (body.value == ''))
					throw new Error('Message has no Body or Subject');
				if(subject.value=='')
					subject.value = '(no subject)';

				action = 'message_hub.php';
				submit();

			} catch(e){
				alert(e);
			}
		}
	}

	function set_recipient(value)
	{
		to_obj = document.forms.msg_form.to;  						// the 'to' field
		to_str = to_obj.value										// the value of the to field
		base = to_str.substr(0,to_obj.value.lastIndexOf(',')); 		// data preceeding the most recent recipient
		if(base != '') base += ', ';								// append ', ' if there is already data in the 'to' field
		to_obj.value = base + value + ',';							// combine the base with the value argument
		document.getElementById('user_div').style.display='none';	// hide the user_div
	}";
// Javascript for the inbox mode
if($mode='inbox')
echo "
	/*****************************************
	* This function provides coloration when *
	* the user selects or deselects a row.   *
	*****************************************/
	function toggle_row(id){
		if(document.getElementById('row_'+id).className.search('selected') == -1)
			document.getElementById('row_'+id).className += ' selected';
		else
			document.getElementById('row_'+id).className = document.getElementById(id).className;
	}";
?>
</script>

<style type='text/css'>
	div[class=menu]{
		margin:2px 20px;
	}
	div[class=menu] a{
		cursor:pointer;
	}
	#inbox_div{
		width:98%;
		font-size:80%;
	}
	#inbox_div table{
		border:1px solid black;
	}
	#inbox_div .pre_body{
		white-space:nowrap;
		overflow:hidden;
	}
	#inbox_div tr{
		cursor:pointer;
		background-color:white;
		font-weight:bold;
		white-space:nowrap;
	}
	#inbox_div tr.read {
		background-color:#DEF;
		font-weight:normal;
	}
	#inbox_div tr.replied{
		background-color:#DFD;
		font-weight:normal;

	}
	#inbox_div tr.selected{
		background-color:#FFC !important;
	}
	#inbox_div td{
		white-space:nowrap;
		padding:0px 4px;
	}
	#inbox_div .msg_body{
		font-weight:normal;
		color:#999;
	}

	#view_div{
		width:98%;
		margin-top:2em;
	}
	#view_div table{
		background-color:#DEF;
		border:2px solid #AAA;
		border-collapse:collapse;
		margin-top:0px;
	}
	#view_div td{
		border:2px solid #AAA;
	}
	#view_div td:first-child, #compose_div td:first-child{
		font-weight:bold;
		vertical-align:top;
		white-space:nowrap;
		text-align:right;
	}
	#compose_div{
		width:75%;
		position:relative;
		z-index:0;
	}
	#compose_div table{
		background-color:#DEF;
	}
	#compose_div input, #compose_div textarea{
		width:99%;
	}
	#compose_div textarea[name=body]{
		height:20em;
	}
	#compose_div textarea[name=to]{
		height:1.4em;
		margin:0px;
	}
	#user_div{
		font-size:85%;
		padding:2px 5px;
		color:darkblue;
		position:relative;
		z-index:2;
		background-color:#DEF;
		cursor:pointer;
		border:1px solid black !important;
	}
	#user_div a{
		display:block;
		text-decoration:none;
	}

</style>

<?php
//if a mode is set: set variable $mode; otherwise mode is inbox
if(isset($_GET['mode'])) $mode = $_GET['mode'];
else $mode = 'inbox';

//if replying to a message, post info and mode compose will be set, in which case we
//don't want to send the message, we want to auto fill out compose fields.
if(isset($_POST['to']) && ($_GET['mode']!='compose'))
	try{
		$recipients = explode(',',$_POST['to']);
		foreach($recipients AS $recipient){
			$recipient = trim($recipient);
			if($recipient == '')
				continue;
			$sql = "SELECT `user_id` FROM {$_CONFIG['db']['prefix']}users WHERE `username` = \"$recipient\"";
			$q = db_query($sql);
			if(!list($recip_id) = db_fetch_array($q,'NUM')){
				echo("User $recipient does not exist.<br />");
				continue;
			}

			$sql = "INSERT INTO {$_CONFIG['module']['db']['prefix']}messages (`from`,`to`,`subject`,`body`,`timestamp`) VALUES ('{$_USER['user_id']}','$recip_id','{$_POST['subject']}','{$_POST['body']}','".correct_gmt_time()."')";
			if(!db_query($sql))
				throw new Exception('Error inserting message into database<br>'.db_error() );

			if($_POST['msg_id']!=''){
				$sql = "UPDATE {$_CONFIG['module']['db']['prefix']}messages SET `status` = 'replied' WHERE `id` = {$_POST['msg_id']} LIMIT 1";
				if(!db_query($sql))
					throw new Exception('Error changing message status <br>' . $sql . "<br>" . db_error());
			}
		}
		//on a successful send, send confirmation and refresh the page.
		echo "<span class='success'>SUCCESS!</span> Message sent.
		<script type='text/javascript'>setTimeout(\"window.location.search=''\",2000)</script>";
		//print_r($_POST);
	} catch(Exception $e){
		echo "<span class='error'>FAIL! </span>" . $e->getMessage();
		$mode = 'compose';
		//print_r($_POST);

	}

//This block creates a lookup table for usernames and full names versus user ids
$tbl1 = "`{$_CONFIG['db']['prefix']}users`"; // SITE_users
$tbl2 = "`{$_CONFIG['module']['db']['prefix']}employees`"; // ACS_employees

$sql = "SELECT $tbl1.user_id, $tbl1.username, CONCAT($tbl2.f_name, ' ', $tbl2.l_name) AS name FROM $tbl1 LEFT JOIN ($tbl2) ON ($tbl2.id = $tbl1.user_id)";
$q = db_query($sql);
while($user = db_fetch_array($q, 'ASSOC')){
	$user_table[$user['user_id']]['username'] = $user['username'];
	$user_table[$user['user_id']]['name'] = $user['name'];
}
// end lookup table block

if($mode == 'del'){
	foreach($_POST as $msg_id => $junk){  //in delete mode _POST content will be message ids, which have no value.
		$sql = "DELETE FROM {$_CONFIG['module']['db']['prefix']}messages WHERE `id` = '$msg_id' LIMIT 1";
		db_query($sql);
	}
	echo "<script type='text/javascript'>setTimeout(\"window.location.search=''\",100)</script>"; // refresh window on completion.
}

/** In this mode, the inbox is built from the message database for a particular user
  * The color scheme is as follows: white and bold text if unread, blue BG if read,
  * yellow if selected.  Message status is stored as a class applied to the checkbox
  * for a row, this is polled if a row is unselected.
  **/
if($mode == 'inbox'){
	try{
		echo "<div id='inbox_div'>
		<div class='menu'><a href='message_hub.php'>[inbox]</a>&nbsp;&nbsp;&nbsp;<a href='message_hub.php?mode=compose'>[compose]</a>&nbsp;&nbsp;&nbsp;<a onclick='javascript:document.forms.inbox_form.submit()' style='cursor:pointer;' href='#'>[delete selected]</a></div>
		<form name='inbox_form' method='POST' action='message_hub.php?mode=del'>
		<table border='0' cellspacing='1' width='98%'>
			<tr class=\"read\"><td>&nbsp;</td><td>From:</td><td>Subject - Message Preview</td><td>Date/Time</td></tr>";

		$sql = "SELECT * FROM {$_CONFIG['module']['db']['prefix']}messages WHERE `to` = '{$_USER['user_id']}' ORDER BY `timestamp` DESC";
		$q = db_query($sql);
		while($msg = db_fetch_array($q, 'ASSOC')){  // create a row for each row in query
			echo "<tr id='row_{$msg['id']}' ";
			if($msg['status']=='read') echo "class='read'";
			elseif($msg['status']=='replied') echo "class='replied'";
			echo "><td><input type='checkbox' id='{$msg['id']}' name='{$msg['id']}'";
			if($msg['status']=='read') echo "class='read'";
			elseif($msg['status']=='replied') echo "class='replied'";
			echo " onclick='toggle_row({$msg['id']})'></td><td onclick=\"window.location.search='mode=view&id={$msg['id']}'\" class='from'>{$user_table[$msg['from']]['name']}";
			echo "</td>
				<td onclick=\"window.location.search='mode=view&id={$msg['id']}'\" class='pre_body' width='100%' on><div style='width:100%;white-space:nowrap;overflow:hidden;'>{$msg['subject']}<span class='msg_body'> - ";
				if(strstr($_SERVER['HTTP_USER_AGENT'], MSIE)) // IE compatibility
					echo str_shorten($msg['body'],100);
				else echo $msg['body'];
				echo "</span></div>
				</td>
				<td onclick=\"window.location.search='mode=view&id={$msg['id']}'\">".date($_USER['format_dt'],gmt_to_local_time($msg['timestamp']))."
			</tr>";
		}
		echo "</table></form></div>";
	} catch(Exception $e){
		echo "Error: " . $e->getMessage();
	}
}elseif($mode == 'view'){
	$sql = "SELECT * FROM {$_CONFIG['module']['db']['prefix']}messages WHERE `id` = '{$_GET['id']}' LIMIT 1";
	$q = db_query($sql);
	$msg = db_fetch_array($q, 'ASSOC');

	if($msg['status'] == 'unread'){
		$sql = "UPDATE {$_CONFIG['module']['db']['prefix']}messages SET `status` = 'read' WHERE `id` = '{$_GET['id']}' LIMIT 1";
		db_query($sql);
	}

	echo "<div id='view_div'>

	<div class='menu'><a href='message_hub.php'>[inbox]</a>&nbsp;&nbsp;&nbsp;<a href='message_hub.php?mode=compose'>[compose]</a>&nbsp;&nbsp;&nbsp;<a onclick='javascript:document.forms.msg_form.submit()'  style='cursor:pointer;' href='#'>[reply]</a>&nbsp;&nbsp;&nbsp;<a onclick='javascript:document.forms.delete.submit()' style='cursor:pointer;' href='#'>[delete]</a></div>
	<table border='1' cellspacing='1'>
		<tr><td>From:</td><td width='100%'>{$user_table[$msg['from']]['name']}</td></tr>
		<tr><td>Sent On:</td><td>".date($_USER['format_dt'],gmt_to_local_time($msg['timestamp']))."</td></tr>
		<tr><td>Subject:</td><td>{$msg['subject']}</td></tr>
		<tr><td>Message Body:</td><td style=''><textarea style='width:99%' rows='15' readonly>{$msg['body']}</textarea></td></tr>
		<tr><td colspan='2' align='right'>
			<form name='msg_form' action='message_hub.php?mode=compose' method='POST' style='display:inline;'>
				<input type='hidden' name='msg_id' value='{$msg['id']}'/>
				<input type='submit' value='reply'/>
			</form>
			<form style='display:inline' name='delete' action='message_hub.php?mode=del' method='POST'>
				<input type='hidden' value='{$msg['id']}' name='{$msg['id']}'/>
				<input type='submit' value='delete'/>
			</form></td></tr>
	</table></div>";

}elseif($mode == 'compose'){

	echo floating_div('user_div', 0); //creates a dynamic div element with id 'user_div'

	if(isset($_POST['msg_id'])){ // if msg_id is set then this is a reply and we will auto fill out fields.
		$sql = "SELECT * FROM {$_CONFIG['module']['db']['prefix']}messages WHERE `id` = '{$_POST['msg_id']}'";
		$q = db_query($sql);
		$msg = db_fetch_array($q, 'ASSOC');

		$body = "\n\n\nOn ".date($_USER['format_dt'],gmt_to_local_time($msg['timestamp']))." {$user_table[$msg['from']]} said: \n\n{$msg['body']}";
		$subject = 'RE: ' . $msg['subject'];
		$to = $user_table[$msg['from']]['username'];
	}

	echo "<div id='compose_div'><form name='msg_form' action='message_hub.php' method='POST'>
	<input type='hidden' name='msg_id' value='{$msg['id']}' />

	<div class='menu'><a href='message_hub.php'>[inbox]</a></div>
	<table border='1' cellspacing='1' width='100%'>
		<tr>
			<td>From:</td>
			<td width='100%'>
				<input type='text' size='50' value='&quot;{$_USER['module']['f_name']} {$_USER['module']['l_name']}&quot; &lt;{$_USER['email']}&gt;' readonly/>
			</td>
		</tr>
		<tr>
			<td>To:</td>
			<td>
				<textarea id='to' rows='1' cols='50' name='to' onkeyup='whichKey(event)' onfocus='get_users(this.value)' onblur='hide_user_div()'>$to</textarea>
			</td>
		</tr>
		<tr>
			<td>Subject:</td>
			<td><input type='text' size='50' id='subject' name='subject' value='$subject'/></td>
		</tr>
		<tr>
			<td>Message Body:</td>
			<td><textarea name='body' cols='50' rows='15'>$body</textarea></td>
		</tr>
		<tr><td colspan='2'><input type='button' value='Send' onclick='validate_message(this.form)'></td></tr>
	</table></form></div>";

}

include("../incs/footer.php");
