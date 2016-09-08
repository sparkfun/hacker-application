<?php

$apiList = [ 'nasa', 'swapi', 'etymonline' ];

$return = false;
$api = false;
if (isset($_POST['api'])) {
	$api = $_POST['api'];
} elseif (isset($_GET['api'])) {
	$api = $_GET['api'];
} else {
	$api = 'nasa';
}

if ($api !== FALSE && $api !== '_all') { //Fetch single API
	include('api_config.' . $api . '.php');
	$return = json_encode($apiConfig);
} elseif ($api === '_all') { //Fetch all APIs in $apiList
	$all = array();
	while(list($i, $apiName) = each($apiList)) {
		include('api_config.' . $apiName . '.php');
		$all[$apiName] = $apiConfig;
	}
	$return = json_encode($all);
}
//var_dump($return);
echo $return;