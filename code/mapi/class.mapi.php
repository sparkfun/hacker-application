<?php
declare(strict_types=1);
require_once('./sec/db_config.php');
require_once('interface.mapi.php');

class MutableApi { //REFAC::implements MapiInterface -> each class.$api_name.php file
	
	/*private $apiName;
	private $baseURL;*/
	private $cfg;
	private $allApis;
	private $db;
	
	function __construct($db, array $cfg) {
		/*
		*** db: (MysqlI object) Database for `mapi`
		*** cfg: (array) Configuration variables
		*/
		$this->db = $db;
		$this->cfg = $cfg;
		
		define('MAPI_DEFAULT_CURL_OPTIONS', [ //Default CURL options when none are passed
			CURLOPT_RETURNTRANSFER => TRUE,
			CURLOPT_SAFE_UPLOAD => TRUE
		]);
		
		define('API_CONFIG_PATH', './api/');
		define('CLASS_CONFIG_PATH', './api/');
		
		$allApis = self::getApiList($db); //Fetch all APIs from database
		if ($allApis !== false) {
			self::loadApiFiles($allApis); //Load all API config files
		} else {
			error_log('Could not find APIs');
		}
	}
	
	public function restfulApiCall(string $method, array $params): array {
		/* Send parameters to its correct $method call
		*** method: (string) the HTTP method to use
		*** params: (array) config parameters to be converted to final CURL options
		*/
		$ret = [];
		switch($method) {
		case 'GET':
			$ret = $this->makeRestfulGETApiCall($params);
			break;
		case 'POST':
			$ret = $this->makeRestfulPOSTApiCall($params);
			break;
		//case 'PUT': //TODO
		//case  'DELETE': //TODO
		default:
			error_log('Unhandled $method: \'' . $method . '\'');
		}
		return $ret;
	}
	
	private function makeRestfulGETApiCall(array $args) {
		/* Configure a GET call to be sent to $this->makeCURLCall
		*** args: (array) Arguments and CURL options
		*/
		$ret = false;
		$curl_options = $args['curl_options'] ?? MAPI_DEFAULT_CURL_OPTIONS;
		$curl_options[CURLOPT_HTTPGET] = true; //force GET
		
		$query = '';
		if (isset($args['data'])) {
			$query = http_build_query($args['data']);
		}
		
		$finalURL = '';
		if (isset($args['url'])) {
			$finalURL = $args['url'] . '?' . $query;
		} else {
			error_log('No URL found before GET call');
		}
		$curl_options[CURLOPT_URL] = $finalURL;
		//print '<br>sending curl... with options:'; var_dump($curl_options);
		$ret = $this->makeCURLCall($curl_options);
		
		return $ret;
	}
	
	private function makeRestfulPOSTApiCall(array $args) {
		/* Configure a POST call to be sent to $this->makeCURLCall
		*** args: (array) Arguments and CURL options
		*/
		if (!isset($args[CURLOPT_URL]))
			return false;
		$curl_options = $args['curl_options'] ?? MAPI_DEFAULT_CURL_OPTIONS;
		$curl_options[CURLOPT_POST] = true; //force POST
		$curl_options[CURLOPT_URL] = $args[CURLOPT_URL];
		
		$ret = $this->makeCURLCall($curl_options);
		return $ret;
	}
	
	private function makeCURLCall(array $options) {
		/* Initiate a CURL call and return results
		*** options: (array) CURL options for this call
		*/
		if (!isset($options[CURLOPT_URL]))
			return false;
		$ret = [];
		$curl = curl_init();
		foreach ($options as $opt => $val) { //Set options
			$r = curl_setopt($curl, $opt, $val);
			if (!$r) {
				error_log("--- bad curl opt/val: '$opt' / '$val'");
			}
		}
		
		//echo '<br>executing curl... with options:'; var_dump($options);
		$result = curl_exec($curl);
		
		if ($result === false && curl_errno($curl) !== 0) {
			$err = curl_error($curl);
			$errno = curl_errno($curl);
			error_log("CURL ERROR: #$errno : $err");
		}
		
		curl_close($curl);
		
		if ($result !== false) {
			$ret['success'] = true;
			$ret['result'] = $result;
		} else {
			$ret['success'] = false;
			$ret['result'] = false;
		}
		
		return $ret;
	}
	
	
	protected function saveCallResults($result, array $args) {
		//TODO
	}
	
	
	private function getApiList($db) {
		/* Fetch all APIs from the `mapi`.`api` table
		*** db: (MysqlI object) for `mapi` database
		*/
		$data = false;
		$rslt = $db->query('SELECT `id`, `name`, `notes`, `base_url`, `api_cfg`, `class_cfg`, `class_name` FROM `mapi`.`api`');
		if ($rslt !== false) {
			$data = $rslt->fetch_all(MYSQLI_ASSOC);
			$rslt->free_result();
		} else {
			error_log('Could not fetch APIs from getApiList()!');
		}
		return $data;
	}
	
	private function loadApiFiles($apis) {
		/* Load required `api_cfg` and `class_cfg` files
		*** apis: (array) All APIs to instantiate/configure
		*/
		$loadedApis = [];
		unset($apiConfig);
		foreach ($apis as $i => $apiData) {
			$loadedApis[$apiData['id']] = [];
			//Include api_config files
			if (is_string($apiData['api_cfg']) && strlen($apiData['api_cfg']) > 0) {
				if (file_exists(API_CONFIG_PATH . $apiData['api_cfg'])) {
					include_once(API_CONFIG_PATH . $apiData['api_cfg']);
					if (isset($apiConfig)) {
						$loadedApis[$apiData['id']] = [ 'cfg' => $apiConfig ];
					} else {
						error_log('Loaded api_cfg file but $apiConfig variable not found');
					}
					//print '--- loaded api_cfg:' . $apiData['api_cfg'];
				} else {
					error_log('Cannot find api_cfg at: ' . API_CONFIG_PATH . $apiData['api_cfg']);
				}
				
			}
			//Require and instantiate classes, if necessary
			if (is_string($apiData['class_cfg']) && strlen($apiData['class_cfg']) > 0) {
				if (file_exists(CLASS_CONFIG_PATH . $apiData['class_cfg'])) {
					require_once(CLASS_CONFIG_PATH . $apiData['class_cfg']);
					if (is_string($apiData['class_name']) && strlen($apiData['class_name']) > 0) {
						$className = $apiData['class_name'];
						if (class_exists($className)) {
							$class = new $className($apiConfig, []); //TODO Pass proper $userDataCfg variables
							$loadedApis[$apiData['id']]['class'] = $class;
						} else {
							error_log('Cannot instantiate (!class_exists) on class: ' . $className);
						}
					}
					//print '--- loaded class_cfg:' . $apiData['class_cfg'];
				} else {
					error_log('Cannot find class_cfg file at: ' . CLASS_CONFIG_PATH . $apiData['class_cfg']);
				}
			}
			unset($apiConfig);
		}
		$this->allApis = $loadedApis;
	}
	
	public function getApiConfigByName($name) {
		/* Return an apiConfig found by ['cfg']['name'] match
		*** name: (string) Name of the API to find
		*/
		if (count($this->allApis) === 0)
			return false;
		$apiCfg = false;
		foreach($this->allApis as $i => $api) {
			if (isset($api['cfg']) && !empty($api['cfg']['name']) && $api['cfg']['name'] === $name) {
				$apiCfg = $api['cfg'];
				break;
			}
		}
		return $apiCfg;
	}
	
	public function getApiClassByName($name) {
		/* Return an instantiated API class by ['cfg']['name'] match
		*** name: (string) Name of the API to find
		*/
		if (count($this->allApis) === 0)
			return false;
		$apiClass = false;
		foreach($this->allApis as $i => $api) {
			if (isset($api['cfg']) && !empty($api['cfg']['name']) && $api['cfg']['name'] === $name) {
				if (isset($api['class'])) {
					$apiClass = $api['class'];
					break;
				}
			}
		}
		return $apiClass;
	}
	
}