<?php
class NasaApi extends MutableApi implements MapiInterface {
	
	private $apiName;
	private $baseURL;
	
	private $database;
	private $cfg;
	private $userDataCfg;
	
	function __construct(array $cfg, array $userDataCfg) {
		$this->cfg = $cfg;
		$this->userDataCfg = $userDataCfg;
		$this->baseURL = $cfg['baseUrl'] ?? false;
	}
	
	public function makeApiCall(array $args) {
		$apiCallResult = false;
		if (!isset($args['url'])) {
			$args['url'] = $this->baseURL;
		}
		$args['url'] .= $args['apiMethod'] ?? '';
		//$apiCallResult = parent::makeRestfulGETApiCall($args);
		$apiCallResult = parent::restfulApiCall('GET', $args); //NasaAPI only supports GET requests
		//echo '<br> ... GET apiCallResult:'; var_dump($apiCallResult);
		
		return $apiCallResult;
	}
	
	public function saveCallResults($result, array $args) {
		//TODO
	}
	
	public function getApiName() { return $this->apiName; }
	public function getBaseURL() { return $this->baseURL; }
}