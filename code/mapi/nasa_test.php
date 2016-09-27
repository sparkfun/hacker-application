<?php
//Test page for MApi and NasaApi - a 'HelloWorld' class instantiation/API call
ini_set('DISPLAY_ERRORS', 1);
error_reporting(E_ALL);

require_once('class.mapi.php');

$userDataConfig = array( 'queryParameters' => [ 'api_key' => 'RkZmeMuSHXMvKSyaDCXJuUZnNlPggonlc5TWH1yr' ] );

//Main MAPI class
$MApi = new MutableApi($db, []);

//NASA-specific class
$apiConfig = $MApi->getApiConfigByName('NASA');
$NasaApi = $MApi->getApiClassByName('NASA');// ?? new NasaApi($apiConfig, $userDataConfig); 

$data = [ 'method' => 'GET', 'apiMethod' => 'planetary/apod', 'data' => [ 'date' => '2016-08-25', 'hd' => 'TRUE', 'api_key' => $userDataConfig['queryParameters']['api_key'] ] ]; //First call's data payload

//echo "<br>data:"; var_dump($data);

$testResult = $NasaApi->makeApiCall($data);
//echo "Result: "; var_dump($testResult);
if ($testResult !== false && $testResult['success'] === true) {
	$result1 = json_decode($testResult['result'], true);
	print 'result1: '; print_r($result1);
	//echo '<div><img src="' . $result1['hd'] ?? $result1['
}

$data2 = [ 'method' => 'GET', 'apiMethod' => 'neo/rest/v1/neo/browse', 'data' => [ 'api_key' => $userDataConfig['queryParameters']['api_key'] ] ]; //Second call's data payload

$testResult2 = $NasaApi->makeApiCall($data2);
//echo "Result2: "; var_dump($testResult2);
if ($testResult2 !== false && $testResult2['success'] === true) {
	$result2 = json_decode($testResult2['result'], true);
	print "\r\n" . 'result2: '; print_r($result2);
}

echo 'MApi object:<br>'; echo '<pre>'; var_dump($MApi); echo '</pre>';
//echo "NasaApi Object:<br>"; echo '<pre>'; var_dump($NasaApi); echo '</pre>';
