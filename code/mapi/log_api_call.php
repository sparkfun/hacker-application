<?php

ini_set('error_log', './debug.log');
ini_set('log_errors', "1");

require_once('class.mapi.php');
$MApi = new MutableApi($db);

$data = [];
$return = ['success' => false];

$baseUrls = array( 'https://api.nasa.gov' => [ 'name' => 'nasa', 'api_id' => 1 ], 'https://swapi.co/api/' => [ 'name' => 'swapi', 'api_id' => 2 ] );

/*$debug = array('post' => []);
foreach ($_POST as $k => $v) {
	$debug[$k] = $v;
}*/

//Check for data
if (isset($_POST['data'])) {
	$dataRaw = $_POST['data'];
	$data = json_decode($dataRaw, true);
	error_log('PostData: ' . $dataRaw);
} else {
	error_log('NO DATA FOUND');
}

include_once('./sec/db_config.php');

//Determine API
$apiId = 0;
foreach ($baseUrls as $baseUrl => $cfg) {
	if (strpos($data['url'], $baseUrl) === 0 || strpos($data['url_long'], $baseUrl) === 0) {
		$apiId = $cfg['api_id'];
		break;
	}
}

//Insert data
$sqlInsert = 'INSERT INTO `mapi`.`api_call` (`api_id`, `url`, `url_long`, `method`, `parameters`, `response_text`, `status`, `status_text` ) VALUES (?, ?, ?, ?, ?, ?, ?, ?)';

$stmt = $db->prepare($sqlInsert);

$queryResult = false;
if ($stmt === false) {
	error_log('Mysqli Stmt error on query ' . $sqlInsert);
} else {
	//Bind and process
	$stmt->bind_param('isssssss', $apiId, $data['url'], $data['url_long'], $data['method'], $data['parameters'], $data['response_text'], $data['status'], $data['status_text']);

	$queryResult = $stmt->execute();
	
	if ($queryResult === true) {
		$id = $db->insert_id;
		$return = [ 'success' => true, 'id' => $id, 'data' => $data ];
	} else {
		$return['mysqli_errno'] = $db->errno;
		$return['mysqli_error'] = $db->error;
	}

	$stmt->close();
}

$db->close();

$returnJson = json_encode($return);
error_log('Return: ' . $returnJson);
echo $returnJson;