<?php

$return = array( 'post' => array(), 'param' => array());

$urlRaw = isset($_POST['url']) ? $_POST['url'] : false;
$return['urlRaw'] = $urlRaw;

foreach ($_POST as $k => $v) {
	$return['post'][$k] = $v;
}

$paramString = '';
if (isset($_POST['paramString'])) {
	/*
	//$params = json_decode($_POST['params'], true);
	$params = $_POST['paramString'];
	foreach ($params as $param => $val) {
		$paramString .= $param . '=' . $val . '&';
		$return['param'][$param] = $val;
	}
	$paramString = substr($paramString, 0, -1);
	*/
	$paramString = $_POST['paramString'];
}

$return['paramString'] = $paramString;
/*
if ($urlRaw === false) {
	echo 'Error: No url found: ' . $urlRaw;
	echo json_encode($return);
	die();
}
*/

$url = urldecode($urlRaw);
/*if (strlen($paramString) > 0) {
	$url .= '?' . $paramString;
}*/

$data = file_get_contents($url);

$return['data'] = $data;
$return['data_json'] = json_encode($data);
$return['dataLen'] = strlen($data);


$return['url'] = $url;

echo json_encode($return);