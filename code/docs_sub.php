<?php
	$_PAGE['quiet'] = 1;
	$_PAGE['data_only'] = 0;
	$_PAGE['require_login'] = 0;
	$_PAGE['path_to_root'] = "../";
	$_PAGE['title'] = "Add New Document";
	include("../incs/header.php");
	include("incs/common.inc");

//build user_id to username table 
$sql= "SELECT `user_id`,`username` FROM `{$_CONFIG['db']['prefix']}users`";
$q = db_query($sql);
while($user = db_fetch_array($q, 'ASSOC'))
	$user_table[$user['user_id']] = $user['username'];

echo "<link rel='stylesheet' type='text/css' href='../themes/default/standard/acs.css' />";
if(empty($_GET)||isset($_GET['mode']))
	echo "<style type=\"text/css\">
		body{background-color:#DDD}
	</style>";
echo "<style type='text/css'>
	body{margin:10px;}
	</style>
	<script type=\"text/javascript\">
		function closeWindow()
		{
			opener.location.reload();
			window.close();
		}
	</script>";
//This code block runs if the user clicked a delete icon.
if(isset($_GET['d'])){
	if(isset($_GET['mode'])) $mode = $_GET['mode'];
	
	/*********************************************************************************************************
	 * If the user wants to delete a file, the id is sent via 'd', if there is more than one version
	 * of the file, then the user is prompted to click a button determining what type of deletion to perform.
	 *********************************************************************************************************/
		if($mode == 'select_type'){
			echo "<script type='text/javascript'>
				function delete_single(){
					window.location.search = 'd={$_GET['d']}&mode=version&type=single';
				}
				function delete_all(){
					window.location.search = 'd={$_GET['d']}&mode=version&type=all';			
				}
				function cancel(){
					window.close();
				}
				</script>";
			echo "<span style='font-weight:bold;font-style:italic'>Select an option:</span><br />
				<input type='button' value='Delete This Version Only' onClick='delete_single()'><input type='button' value='Delete All Versions' onClick='delete_all()' /><input type='button' value='Cancel' onClick='cancel()' />";
			die();
		}
			try{
				$sql = "SELECT * FROM {$_CONFIG['module']['db']['prefix']}docs WHERE `id` = '{$_GET['d']}'";
				if(!$doc_ref = db_query($sql))
					throw new Exception('SQL error.  Document not found. <br />\n' . $sql); 
				$doc = db_fetch_array($doc_ref, 'ASSOC');
				if( !(($_USER['user_id']==$doc['owner']) || $_USER['priv']) )
					throw new Exception('You do not have authorization to delete this file.');
		// **** If the user has selected an option (or the file had no versions) then delete file from db **** \\
				if($mode == 'version'){
				// **** With a single delete of a versioned file, the next newest file is renamed to match the original **** \\
					if($_GET['type'] == 'single'){	
					// delete newest version
						$del_sql = "DELETE FROM {$_CONFIG['module']['db']['prefix']}docs WHERE `id` = '{$_GET['d']}' LIMIT 1";
						if(!db_query($del_sql))
							throw new Exception('SQL error. Could not delete file. <br />\n' . $sql);
						system('del '. $doc['path']);
					// get older versions
						$select = "SELECT `path` FROM {$_CONFIG['module']['db']['prefix']}docs WHERE `original` = '{$doc['original']}' ORDER BY `timestamp` DESC;";
						$doc_vers = db_query($select);
						$next_vers = db_fetch_array($doc_vers, 'NUM');
					//rename next newest version in db
						$update_sql = "UPDATE {$_CONFIG['module']['db']['prefix']}docs SET `path` = '{$doc['path']}' WHERE `path` = '{$next_vers[0]}' LIMIT 1;";
						if(!db_query($update_sql))
							throw new Exception("Error renaming database entry.<br />" . db_error()."<br />[$update_sql]");
						else{
					// rename file on file system
							rename($next_vers[0],"docs/{$doc['original']}");
						}
						
						echo "<script type=\"text/javascript\">setTimeout('closeWindow()',2500);</script>
							<span class='success'>SUCCESS!</span> Version deleted.";
						die();
					}
				// **** if user deletes all then delete all files with the same original column. **** \\
					if($_GET['type'] == 'all'){
						$del_sql = "DELETE FROM {$_CONFIG['module']['db']['prefix']}docs WHERE `original` = '{$doc['original']}'";
					}
				// **** if just one version exists, delete it **** \\
				}else
					$del_sql = "DELETE FROM {$_CONFIG['module']['db']['prefix']}docs WHERE `id` = '{$_GET['d']}' LIMIT 1";

				if(!db_query($del_sql))
					throw new Exception('SQL error. Could not delete file. <br />\n' . $sql);
				system('del '. $doc['path']);
				echo "<script type=\"text/javascript\">setTimeout('closeWindow()',2500);</script>
					<span class='success'>SUCCESS!</span> File(s) deleted.";
				die();
			} catch(Exception $e){
				echo("<span class=\"error\">FAIL!</span> " . $e->getMessage());
// Debugging information
				/*echo "<pre>";
				print_r($doc);
				print_r($_USER);
				echo "</pre>";
				die();*/
			}
}
//This block runs if delete isnt set and a file id has been set in the url.
elseif(isset($_GET['file'])){
	$sql = "SELECT * FROM {$_CONFIG['module']['db']['prefix']}docs WHERE `original` = '{$_GET['file']}'";
	$docs = db_query($sql);
	while($doc = db_fetch_array($docs)){
		if(basename($doc['path']) != $doc['original']){
			echo "{$doc['name']} <span style='text-decoration:italic; color:#999;'>" . date($_USER['format_date'],$doc['timestamp']) . "</span>&nbsp;&nbsp;<a title='{$doc_name}' href=\"{$doc['path']}\"><img src=\"{$_THEME['icon']['download']}\" border=0/></a>&nbsp;&nbsp;";
			if(($user_id == $doc['owner']) || $_USER['priv'])
				echo "<a style='cursor:pointer' title='Delete {$doc['name']}' onclick=\"deleteDoc({$doc['id']},'{$doc['name']}')\"><img src='{$_THEME['icon']['trash']}' /></a>";
			echo "<span style='text-decoration:italic; color:#999;'>Owner:{$user_table[$doc['owner']]}</span><br />";
		}
	}
	die();
}


if(!empty($_FILES['local_path']))
try
{
// This switch statment utilizes some of the built in error checking for the $_FILES array
	switch ($_FILES["local_path"]["error"]) {
	   case UPLOAD_ERR_OK:
	       break;
	   case UPLOAD_ERR_INI_SIZE:
	       throw new Exception("The uploaded file exceeds the upload_max_filesize directive (".ini_get("upload_max_filesize").") in php.ini.");
	       break;
	   case UPLOAD_ERR_FORM_SIZE:
	       throw new Exception("The uploaded file exceeds the MAX_FILE_SIZE directive that was specified in the HTML form.");
	       break;
	   case UPLOAD_ERR_PARTIAL:
	       throw new Exception("The uploaded file was only partially uploaded.");
	       break;
	   case UPLOAD_ERR_NO_FILE:
	       throw new Exception("No file name was entered.");
	       break;
	   case UPLOAD_ERR_NO_TMP_DIR:
	       throw new Exception("Missing a temporary folder.");
	       break;
	   case UPLOAD_ERR_CANT_WRITE:
	       throw new Exception("Failed to write file to disk");
	       break;
	   default:
	       throw new Exception("Unknown File Error");
	}//end switch
	
// if any field is blank, throw error, exit try statement. \\
	if(!(($_FILES['local_path']['name'] != '')&&($_POST['name'] != '')&&($_POST['doc_type'] != '')))
		throw new Exception("Incomplete File information");
	
// check to see if file or file description exists, if yes throw error and exit try statement \\
	$target_path = "docs/" . basename($_FILES['local_path']['name']);
	$new_md5 = md5_file($_FILES['local_path']['tmp_name']);
//	echo $new_md5;
	$sql = "SELECT * FROM {$_CONFIG['module']['db']['prefix']}docs";
	$docs = db_query($sql);
// cycle through each row obtained from db_query \\
	while($doc = db_fetch_array($docs, 'ASSOC'))
	{
		foreach($doc as $key => $value)
		{
			switch($key)
			{
				/*case 'name':
					if($value == $_POST['name'])
						throw new Exception("Duplicate file description found, please enter a different description");
					break;*/
				case 'path':
					if($target_path == $value){
						$dot_pos = strrpos($value,'.');
						$new_name = substr($value,0,$dot_pos).'_'.$doc['timestamp'].substr($value, $dot_pos);
						
						// if file is already there, throw error, exit try
						if($doc['md5'] == $new_md5)
							throw new Exception("This version of the file already exists as {$doc['name']} ({$doc['path']})");
							
						// tell user value exists, then update file and database.
						echo "<b>Notice:</b> $value already exits, renaming old file as $new_name...<br />\n";
						$sql = "UPDATE {$_CONFIG['module']['db']['prefix']}docs SET `path` = '$new_name' WHERE `path` = '{$doc['path']}'";
						if(!db_query($sql))
							throw new Exception("An SQL error occurred.<br />\n Query string: $sql<br><br>\n" . db_error() );
						if(!rename($value, $new_name))
							throw new Exception("Error renaming file from $value to $new_name");
					}
					break;
				case 'md5':
					if($value == $new_md5)
						throw new Exception("This file already exists as {$doc['name']} ({$doc['path']})");
				default:
					break;
			}
		}
	}
	
// INSERT into Database, if db_query fails throw error, exit try statement.
	$require_login = isset($_POST['require_login']) ? '1' : '0';
	$sql = "INSERT INTO {$_CONFIG['module']['db']['prefix']}docs (`doc_type`,`name`,`original`,`path`,`timestamp`,`md5`,`owner`,`require_login`) VALUES('{$_POST['doc_type']}','{$_POST['name']}','".basename($_FILES['local_path']['name'])."','$target_path','" .correct_gmt_time() . "','$new_md5','{$_USER['user_id']}','$require_login')";
	if(!db_query($sql)) 
		throw new Exception("An SQL error occurred.<br />\n Query string: $sql<br><br>\n" . db_error() );
	
// Move uploaded file, if move fails throw error, exit try statement.
	if(!move_uploaded_file($_FILES['local_path']['tmp_name'], $target_path)) 
		throw new Exception("There was an error uploading the file, please try again!");

// And allow users to read the file...
	echo shell_exec("chmod a+r $target_path -R");

	
// If file upload succeeds print success message and end document.
	echo "<script type='text/javascript'>setTimeout('closeWindow()',2500)</script><span class='success'>Success!</span><br /> The file ".  basename($_FILES['local_path']['name']) . " has been uploaded.</html>";
	exit(); 		
	
}catch(Exception $e){
	echo "<span style=\"font-size:20px;color:#E00;\">FAIL!</span> " . $e->getMessage();
/*	echo "<pre>";
	var_dump($_POST);
	var_dump($_FILES);
	var_dump($doc,$new_md5,$new_name);
	echo"</pre>";*/

}	
/**************************************************************************************
 * The following code is only run if there is no data in the $_FILES array or if there
 * has been an Exception.
 **************************************************************************************/
$maxsize = ini_get('upload_max_filesize');
if (!is_numeric($maxsize)) {
	if (strpos($maxsize, 'G') !== false) $maxsize = intval($maxsize)*1024*1024*1024;
	elseif (strpos($maxsize, 'M') !== false) $maxsize = intval($maxsize)*1024*1024;
	elseif (strpos($maxsize, 'K') !== false) $maxsize = intval($maxsize)*1024;
	else $maxsize = intval($maxsize);
}
?>

<form enctype="multipart/form-data" name="newDocForm" id="newDocForm" method="POST" action="docs_sub.php">
	<input type="hidden" name='MAX_FILE_SIZE' value='<?php echo $maxsize ?>' />
	<table border="0" cellspacing="0" cellpadding="5" style="border:1px solid black;margin-top:20px;">
		<tr><td align="right">Document Type:&nbsp;&nbsp;</td><td><input type="text" size="30" name="doc_type" value="<?php echo $_POST['doc_type']; ?>"/></td></tr>
		<tr><td align="right">Short Description:&nbsp;&nbsp;</td><td><input type="text" size="30" name="name" value="<?php echo $_POST['name']; ?>"/></td></tr>
		<tr><td align="right">File to Upload:&nbsp;&nbsp;</td><td><input type="file" size="50" name="local_path" value="<?php echo $_POST['path']; ?>"/></td></tr>
		<tr><td colspan="2" align="right"><input type="checkbox" name="require_login" <?php if(isset($_POST['require_login'])) echo"checked"; ?> />Login required to view file?</td></tr>
		<tr><td align="right" colspan="2"><input type="button" value="Cancel" onClick="window.close();"/>&nbsp;&nbsp;<input type="submit" value="Upload"/></td></tr>
	</table>
</form>
</html>

<?php

/*
echo "<pre>";
var_dump($_POST);
var_dump($_FILES);
echo"</pre>";
*/

?>