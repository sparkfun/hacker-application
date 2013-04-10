<?

/* 
	I chose to use query functions instead of kata data wrappers to show that I know sql. 
	The tables I am working with are more than 3 years old, please forgive the schema :p 
*/

class Worker extends AppModel 
{
	
	const ERROR_NO_USER = 'Invalid username or password';
	const ERROR_PASSWORD_EXPIRED = 'Password has expired';
	
	function fromKey($key)
	{
		if ($key > 0)
		{
			$key = $this->escape($key);
			$row = $this->query("SELECT `Key` as `key`, `User` as nickname, `Password` as password_hash, `Salt` as salt, `Access` as access, `Name` as name, `Email` as email, `Date` as date, `PasswordExpires` as password_expires FROM `workers` WHERE `Key` = '{$key}' LIMIT 1");

			if (!empty($row))
			{
				return $row[0];
			}
		}
		
		throw new ErrorException(self::ERROR_NO_USER);
	}
	
	function fromNick($nick)
	{
		if (!empty($nick))
		{
			$nick = $this->escape($nick);
			$row = $this->query("SELECT `Key` as `key`, `User` as nickname, `Password` as password_hash, `Salt` as salt, `Access` as access, `Name` as name, `Email` as email, `Date` as date, `PasswordExpires` as password_expires FROM `workers` WHERE `User` = '{$nick}' LIMIT 1");

			if (!empty($row))
			{
				return $row[0];
			}
		}
		
		throw new ErrorException(self::ERROR_NO_USER);
	}
	
	function fromNickAndPass($nick, $password)
	{
		$nick = $this->escape($nick);
		$row = $this->fromNick($nick);
		
		if (!empty($row))
		{				
			if (strtotime($row['password_expires']) < time())
			{
				throw new ErrorException(self::ERROR_PASSWORD_EXPIRED);
			}
			
			$hash = hash('sha256', $password.$row['salt']);
			
			if ($hash == $row['password_hash'])
			{
				return $row;
			}
		}
			
		throw new ErrorException(self::ERROR_NO_USER);
	}

}