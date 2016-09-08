<?php
declare(strict_types=1);
require_once('interface.mapi.php');

class MutableApi { //REFAC::implements MapiInterface -> each class.$api_name.php file
	
	private $apiName;
	private $baseURL;
	private $cfg;
	
	public function __construct($cfg) {
		$this->cfg = $cfg;
		define('MAPI_DEFAULT_CURL_OPTIONS', [
			'CURLOPT_RETURNTRANSFER' => TRUE,
			'CURLOPT_SAFE_UPLOAD' => TRUE
		]);
		
		
	}
	
	public function restfulApiCall($method, $params): array {
		//TODO - send to makeRestfulGETApiCall or makeRestfulPOSTApiCall
	}
	
	public function getApiName() { return $this->apiName; }
	public function getBaseURL() { return $this->baseURL; }
	
	public function makeRestfulGETApiCall(array $args) {
		$ret = [];
		
		$curl_options = $args['curl_options'] ?? MAPI_DEFAULT_CURL_OPTIONS;
		$curl_options['CURLOPT_HTTPGET'] = true; //force GET
		
		$query = '';
		if (isset($args['data'])) {
			$query = http_build_query($args['data']);
		}
		
		$finalURL = '';
		if (isset($args['url'])) {
			$finalURL = $args['url'] . '?' . $query;
		} else {
			$finalURL = self::baseURL;
			$finalURL .= '?' . $query;
		}
		$curl_options['CURLOPT_URL'] = $finalURL;
		
		$curl = curl_init($finalURL);
		$curl_options_set = curl_setopt_array($curl, $curl_options);
		if ($curl_options_set === FALSE) {
			error_log('Could not set CURL options');
			echo '<br>COULD NOT SET CURL OPTIONS<br>';
		}
		echo '<br>executing curl... with options:'; var_dump($curl_options);
		$result = curl_exec($curl);
		
		if (curl_errno($curl) !== 0) {
			echo "<br>CURL ERROR: #" . curl_errno($curl) . ": " . curl_error($curl);
		} else {
			echo '<br>CURL ok!<br>'; var_dump($curl); var_dump($result);
		}
		
		curl_close($curl);
		
		if ($result !== false) {
			$ret['success'] = true;
			$ret['result'] = $result;
		} else {
			$ret['success'] = false;
		}
		echo 'makeRestfulGETApiCall FINAL return:'; var_dump($ret);
		return $ret;
	}
	
	public function makeRestfulPOSTApiCall(array $args) {
		$curl = $args['curl'] ?? curl_init();
		$curl_options = $args['curl_options'] ?? [ 'CURLOPT_RETURNTRANSFER' => true, 'CURLOPT_POST' => true, 'CURLOPT_SAFE_UPLOAD' => true ];
		$curl_options['CURLOPT_POST'] = true; //force POST
		//TODO finish function makeRestfulPOSTApiCall
	}
	
	
	
	
	public function saveCallResults($result, array $args) {
		
	}
	
	
}