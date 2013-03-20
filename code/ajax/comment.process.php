<?php

	session_start();

	include_once('../classes/comment.class.php');
	include_once('../classes/db.class.php');
	include_once('../classes/user.class.php');
	
	$user_id = $_SESSION['user_id'];
	
	$db = new DBConnection;
	$comments = new Comment($db);
	$user = new User($db);
	
	$blogID = intval($_POST['blogID']);
	
	$addCom = $comments->addComment($user_id);

	echo json_encode(array('status'=>1,'html'=>$addCom));
	
	

?>