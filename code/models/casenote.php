<?

/* 
	I chose to use query functions instead of kata data wrappers to show I know sql. 
	The tables I am working with are more than 3 years old, please forgive the schema :p 
*/

class Casenote extends AppModel 
{
	
	const ERROR_NO_NOTE = 'Invalid note';
	const ERROR_NOTE_ADD = 'Unable to add note. Please try again.';
	
	
	public $type_to_verb = array('Phone'=>'Called','Email'=>'Emailed','Fax'=>'Faxxed','In Person'=>'Met with','Text'=>'Texted');
	
	//client didn't want any paging
	function getListFromKey($client_key)
	{
		if ($client_key > 0)
		{
			$client_key = $this->escape($client_key);
			$rows = $this->query("SELECT `casenotes`.`Key` as `key`, `Client` as client_key, `casenotes`.`Date` as date, `Name` as name, `Contactee` as contactee, `Contactor` as contactor, `Subject` as subject, `Comment` as comment, `Type` as type FROM `casenotes` JOIN `workers` ON (`Creator` = `workers`.`Key`) WHERE `Client` = '{$client_key}' ORDER BY `casenotes`.`Date` DESC, `casenotes`.`Key` DESC");
			
			//gotta loop to do this ... :(
			foreach($rows as &$row)
			{
				$row['type_verb'] = $this->type_to_verb[$row['type']];
			}
			
			return $rows;
		}
		
		return array();
	}
	
	function fromKey($note_key)
	{
		if ($note_key > 0)
		{
			$note_key = $this->escape($note_key);
			$rows = $this->query("SELECT `casenotes`.`Key` as `key`, `Client` as client_key, `casenotes`.`Date` as date, `Name` as name, `Contactee` as contactee, `Contactor` as contactor, `Subject` as subject, `Comment` as comment, `Type` as type FROM `casenotes` JOIN `workers` ON (`Creator` = `workers`.`Key`) WHERE `casenotes`.`Key` = '{$note_key}' LIMIT 1");
			
			$note = $rows[0];
			
			$note['type_verb'] = $this->type_to_verb[$note['type']];
			
			return $note;
		}	
		
		throw new ErrorException(self::ERROR_NO_NOTE);
	}
	
	function insert($client_key, $worker_key, $note)
	{
		$client = $this->fromKey($client_key); //throws an exception if invalid client
		if (!empty($note))
		{
			$values = array($client_key,  $worker_key, $note['date'], $note['contactee'], $note['contactor'], $note['subject'], $note['comment'], $note['type']);
			$values = $this->escapeValues($values);
			
			$key = $this->query('INSERT INTO `casenotes` (`Client`, `Creator`, `Date`, `Contactee`, `Contactor`, `Subject`, `Comment`, `Type`) VALUES (\'' . implode("','", $values) . '\')');
			
			$note['key'] = $key;
			$note['type_verb'] = $this->type_to_verb[$note['type']];
			
			return $note;
		}
		
		throw new ErrorException(self::ERROR_NOTE_ADD);
	}
	
	function edit($note_key, $note)
	{
		if (!empty($note))
		{
			$values = array();
			foreach($note as $k=>$v)
			{
				if ($k == 'key' || $k == 'client_key') continue;
				$values[] = $k . "='" . $this->escape($v) . "'";
			}
		
			return $this->query('UPDATE `casenotes` SET ' . implode(',', $values) . " WHERE `Key` = '{$note_key}' LIMIT 1");
		}
		
		return false;//badhappenings
	}

}