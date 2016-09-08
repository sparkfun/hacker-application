<?php
class NasaApi extends MutableApi implements MapiInterface {
	
	private $apiName;
	
	private $database;
	private $cfg;
	private $userDataCfg;
	private $baseURL;
	
	public function __construct(array $cfg, array $userDataCfg) {
		$this->cfg = $cfg;
		$this->userDataCfg = $userDataCfg;
		$this->baseURL = $cfg['baseUrl'] ?? false;
	}
	
	public function makeApiCall(array $args) {
		$apiCallResult = null;
		$method = $args['method'] ?? 'GET';
		if (!isset($args['url'])) {
			$args['url'] = $this->baseURL;
		}
		$args['url'] .= $args['apiMethod'] ?? '';
		//echo "C:" . __CLASS__ . ", F:" . __FUNCTION__ . " args:"; var_dump($args);
		$apiCallResult = parent::makeRestfulGETApiCall($args); //NasaAPI only supports GET requests
		//echo '<br> ... GET apiCallResult:'; var_dump($apiCallResult);
		
		return $apiCallResult;
	}
	
	public function saveCallResults($result, array $args) {
		//TODO
	}
}