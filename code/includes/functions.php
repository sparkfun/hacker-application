<?php

/*
 * hs.functions.php
 * Copyright 2012. Tyler Bailey. All Rights Reserved
 * built specifically for HireStarts, LLC.
 *
 */ 
 

/********************************************************* 
**************** Security Enhancing Functions **************
**********************************************************/

// create a secure token
function token() {
	$token = sha1(uniqid(rand(), true));
	return $token;
}


/********************************************************* 
**************** Miscellaneous Functions **************
**********************************************************/


//page redirect
function page_redirect($location) {
   echo '<META HTTP-EQUIV="Refresh" Content="0; URL='.$location.'">';
   exit; 
}