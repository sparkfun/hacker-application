<?

/* 
	I chose to use query functions instead of kata data wrappers to show I know sql. 
	The tables I am working with are more than 3 years old, please forgive the schema :p
*/

class Caseload extends AppModel 
{
	

	function getListFromWorkerKey($worker_key)
	{
		if ($worker_key > 0)
		{
			$worker_key = $this->escape($worker_key);
			$list = $this->query("SELECT `Key` as `key`, `First Name` as first_name, `Last Name` as last_name, `File Location` as file_location, `Mug Date` as mug_date, `DOB` as birth_date FROM `caseloads` JOIN `clients` WHERE `Worker` = '{$worker_key}' AND `Client` = `clients`.Key ORDER BY `Last Name`");
		}
		
		return $list;
	}
	
	function removeClient($worker_key, $client_key)
	{
		if ($worker_key > 0 && $client_key > 0)
		{
			$worker_key = $this->escape($worker_key);
			$client_key = $this->escape($client_key);
			
			$client = getModel('Client');
			$client = $client->fromKey($client_key); //confirming it exists
			
			$worker = getModel('Worker');
			$worker = $worker->fromKey($worker_key); //confirming it exists
			
			return $this->query("DELETE from `caseloads` WHERE `Worker` = '{$worker_key}' AND `Client` = '{$client_key}' LIMIT 1");
		}
	}
	
	function addClient($worker_key, $client_key)
	{
		if ($worker_key > 0 && $client_key > 0)
		{
			$worker_key = $this->escape($worker_key);
			$client_key = $this->escape($client_key);
			
			$client = getModel('Client');
			$client = $client->fromKey($client_key); //confirming existance and getting name
			
			$worker = getModel('Worker');
			$worker = $worker->fromKey($worker_key); //confirming it exists
			
			$values = array($worker_key, $client_key, $client['first_name'] . ' ' . $client['last_name']);
			return $this->query("INSERT INTO `caseloads` (`Worker`, `Client`, `Client Name`) VALUES ('" . implode("','", $values) . "')");
		}
	}

}