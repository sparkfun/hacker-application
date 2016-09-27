<?php
/* Extract, Transform, Load functions (for each API and method) for data fetched from an API */
//TODO Prepared statements for SQL queries
include('/sec/db_config.php');

$return = array('success' => 0, 'success_ids' => array(), 'fail' => 0, 'error' => 0, 'errors' => array());

$sqlFetchEtl = 'SELECT `id`, `api_id`, `url`, `url_long`, `method`, `parameters`, `response_text`, `status`, `status_text`, `created_at` FROM `mapi`.`api_call` WHERE `etl` = FALSE';

$sqlResult = $db->query($sqlFetchEtl);
if ($sqlResult !== FALSE) {
	if ($sqlResult->num_rows > 0) {
		echo "Processing " . $sqlResult->num_rows . " rows...";
		$etlRows = $sqlResult->fetch_all(MYSQLI_ASSOC);
		foreach ($etlRows as $k => $row) {
			$data = json_decode($row['response_text'], true);
			switch($row['api_id']) { //FIXME - BAD HARD-CODED api_ids
			case 1:
				nasaParse($data, $row, $dbNasa);
				break;
			default:
			}
		}
		$successCnt = count($return['success_ids']);
		echo " ... Done processing! Success: " . $return['success'] . ", SuccessIdsCnt: $successCnt Error: " . $return['error'] . ", Fail: ". $return['fail'] . " Errors count: " . count($return['errors']);
		
		if ($successCnt > 0 && is_array($return['success_ids']) && count($return['success_ids']) > 0) {
			$sqlUpdate = 'UPDATE `mapi`.`api_call` SET `etl` = true WHERE `id` IN ( ' . implode(',', $return['success_ids']) . ' )';
			$updateResult = $db->query($sqlUpdate);
			if ($db->affected_rows == $successCnt) {
				echo '... Successfully updated all success_id rows as etl=true!';
			}
		}
		if ($return['fail'] > 0 || $return['error'] > 0) {
			echo "ERRORS:\n";
			while(list($key, $errs) = each ($return['errors'])) {
				echo "\nError $key (No " . $errs['errno'] . '): ' . $errs['error'];
			}
			echo '--- ERRORS done';
		}
		
	} else {
		echo 'No ETL tasks to process.';
	}
} else {
	die('Bad query');
}

function nasaParse($data, $row, $dbh) {
	global $return;
	
	switch($row['method']) {
	case 'planetary/apod':
		echo "\n planetary/apod: " . $data['date'] . " ... ";
		//TODO Prepared statements for SQL queries
		$sqlInsert = 'INSERT INTO `nasa`.`apod` ( `copyright`, `date`, `explanation`, `hdurl`, `media_type`, `service_version`, `title`, `url` ) VALUES ( ' . (isset($data['copyright']) ? '\'' . addslashes($data['copyright']) . '\'' : 'NULL')  . ', \'' . $data['date'] . '\', \'' . addslashes($data['explanation']) . '\', ' . (isset($data['hdurl']) ? '\'' . $data['hdurl'] . '\'' : 'NULL') . ', \'' . $data['media_type'] . '\', \'' . $data['service_version'] . '\', \'' . addslashes($data['title']) . '\', \'' . addslashes($data['url']) . '\' )';
		$result = $dbh->query($sqlInsert);
		if ($result !== FALSE) {
			$return['success'] += 1;
			$return['success_ids'][] = $row['id'];
			
		} else {
			$return['fail'] += 1;
			$return['error'] += 1;
			$return['errors'][] = [ 'errno' => $dbh->mysql_errno, 'error' => $dbh->mysql_error ];
		}

	break;
	
	}
}

function swapiParse() {
	//TODO
}