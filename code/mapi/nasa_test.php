<?php
ini_set('DISPLAY_ERRORS', 1);
error_reporting(E_ALL);

require_once('class.mapi.php');
include_once('api/api_config.nasa.php');
require_once('api/class.api.nasa.php');

$userDataConfig = array( 'queryParameters' => [ 'api_key' => 'RkZmeMuSHXMvKSyaDCXJuUZnNlPggonlc5TWH1yr' ] );

$MApi = new MutableApi($apiConfig);
$NasaApi = new NasaApi($apiConfig, $userDataConfig);

$data = [ 'method' => 'GET', 'apiMethod' => 'planetary/apod', 'data' => [ 'date' => '2016-08-25', 'hd' => 'TRUE', 'api_key' => $userDataConfig['queryParameters']['api_key'] ] ];

//echo "<br>data:"; var_dump($data);

$testResult = $NasaApi->makeApiCall($data);
echo "Result: "; var_dump($testResult);

//echo 'MApi object:<br>'; echo '<pre>'; var_dump($MApi); echo '</pre>';
//echo "NasaApi Object:<br>"; echo '<pre>'; var_dump($NasaApi); echo '</pre>';
//echo "Default curl options:"; var_dump(MAPI_DEFAULT_CURL_OPTIONS);