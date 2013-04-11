<?

/* 
	I chose to use query functions instead of kata data wrappers to show I know sql. 
	The tables I am working with are more than 3 years old, please forgive the schema :p 
*/

class Client extends AppModel 
{
	
	const ERROR_NO_CLIENT = 'Invalid client';	
	const ERROR_NO_RESULTS = 'No results';
	
	function fromKey($key)
	{
		if (!empty($key))
		{
			$key = $this->escape($key);
			$rows = $this->query("SELECT `Key` as `key`, `First Name` as first_name, `Last Name` as last_name, `File Location` as file_location, `Mug Date` as mug_date, `DOB` as birth_date FROM `clients` WHERE `key` = '{$key}' LIMIT 1");

			if (!empty($rows))
			{
				$client = $rows[0];
				$client['birth_date'] = date('M d, Y', strtotime($client['birth_date']));
				return $client;
			}
		}
		
		throw new ErrorException(self::ERROR_NO_CLIENT);
	}
	
	function searchByName($name)
	{
		$names = explode(" ", $name); //BOOOOOM
		if (count($names) == 2)
		{
			$names[0] = $this->escape($names[0]);
			$names[1] = $this->escape($names[1]);
			$rows = $this->query("SELECT `Key` as `key`, `First Name` as first_name, `Last Name` as last_name, `File Location` as file_location, `Mug Date` as mug_date, `DOB` as birth_date FROM `clients` WHERE `First Name` = '{$names[0]}' AND `Last Name` = '{$names[1]}' LIMIT 1");
			if (!empty($rows))
			{
				$client = $rows[0];
				$client['birth_date'] = date('M d, Y', strtotime($client['birth_date']));
				return $client;
			}			
		}
		throw new ErrorException(self::ERROR_NO_RESULTS);
	}

}