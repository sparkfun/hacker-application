<?php
/* View individual pieces of an API's data */
include('/sec/db_config.php');


$api = !empty($_GET['api']) ? $_GET['api'] : 'nasa';
$method = !empty($_GET['method']) ? urldecode($_GET['method']) : 'planetary/apod';

echo "api : $api <br>method : $method";

$data = array();
$keyField = false;

switch($api) {
	case 'nasa':
			switch($method) {
			case 'planetary/apod':
				$sqlData = 'SELECT `id`, `copyright`, `date`, `explanation`, `hdurl`, `media_type`, `service_version`, `title`, `url` FROM `nasa`.`apod`';
				$sqlResult = $dbNasa->query($sqlData);
				if ($sqlResult !== FALSE) {
					$data = $sqlResult->fetch_all(MYSQLI_ASSOC);
				}
				$keyField = 'date';
				break;
			case 'neo':
				break;
			default:
				die("Unknown method for API $api: '$method'");
			}
			
			
			
		break;
	case 'swapi':
	
		break;
	default:
		die("Unknown API: '$api'");
}

$finalData = array();

if ($keyField !== FALSE && is_array($data) && count($data) > 0) {
	while(list($i, $row) = each($data)) {
		$finalData[$row[$keyField]] = $row;
	}
} else {
	$finalData = $data;
}

echo "<pre>"; print_r($finalData); echo "</pre>";