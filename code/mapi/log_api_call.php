<?php

ini_set('error_log', './debug.log');
ini_set('log_errors', "1");
$debug = array('post' => []);
$data = [];

$baseUrls = array( 'https://api.nasa.gov' => [ 'name' => 'nasa', 'api_id' => 1 ], 'https://swapi.co/api/' => [ 'name' => 'swapi', 'api_id' => 2 ] );

foreach ($_POST as $k => $v) {
	$debug[$k] = $v;
}
if (isset($_POST['data'])) {
	$dataRaw = $_POST['data'];
	$data = json_decode($dataRaw, true);
	//$data = $_POST['data'];
	//$dbgData = json_encode($data);
	//error_log('Data: ' . $dbgData);
	error_log('Data: ' . $dataRaw);
} else {
	error_log('NO DATA FOUND');
}
include('./sec/db_config.php');

$apiId = false;
foreach ($baseUrls as $baseUrl => $cfg) {
	if (strpos($data['url'], $baseUrl) === 0 || strpos($data['url_long'], $baseUrl) === 0) {
		$apiId = $cfg['api_id'];
		break;
	}
}

$sqlInsert = 'INSERT INTO `mapi`.`api_call` (`api_id`, `url`, `url_long`, `method`, `parameters`, `response_text`, `status`, `status_text` ) VALUES (' . (is_numeric($apiId) ? $apiId : 0) . ', \'' . $data['url'] . '\', \'' . $data['url_long'] . '\', \'' . $data['method'] . '\', \'' . addslashes($data['parameters']) . '\', \'' . addslashes($data['response_text']) . '\', ' . $data['status'] . ', \'' . $data['status_text'] . '\' )';

$queryResult = $db->query($sqlInsert);

$return = ['success' => false, 'debug' => $debug];
if ($queryResult === true) {
	$id = $db->insert_id;
	$return = [ 'success' => true, 'id' => $id, 'data' => $data ];
} else {
	$return['mysqli_errno'] = $db->errno;
	$return['mysqli_error'] = $db->error;
}

$returnJson = json_encode($return);
error_log('Return: ' . $returnJson);
echo $returnJson;