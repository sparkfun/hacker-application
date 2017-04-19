<?php
/*
 * DTAdmin
 *
 * About:
 * Generic DineTouch administration task class.
 *
 */


/* Import Amazon AWS SDK for PHP functionality */
require_once('AWSSDKforPHP/sdk.class.php');
class DTUser
{
	public $first_name;
	public $last_name;
	public $class;
}

/* DTAdmin class */
class DTAdmin
{
	/* Instance variables */
	private $Memcached;
	public $S3;
	public $SDB;
	public $user;
	public $dbc;

	/* Constructor */
	public function __construct()
	{
		/* Set up Memcached */
		$this->Memcached = new Memcached();
		$this->Memcached->addServer("localhost", 11211);

		/* Set up Amazon S3 */
		$this->S3 = new AmazonS3();

		/* Set up Amazon SimpleDB */
		$this->SDB = new AmazonSDB();

		$this->dbc = pg_connect("host=dtdatabase
									port=5432
									dbname=[removed]
									user=[removed]
									password=[removed]");

		/* Get the current logged in user information (if any) */
		if ($this->logged_in()) {

			$user_id = $_COOKIE['u'];

			$query = sprintf("SELECT first_name, last_name, class
									FROM admin_users
									WHERE id = %d",
									$user_id);

			$result = pg_query($this->dbc, $query);
			if (!$result) {
				// Error: Unable to get user info from database.
				die("Error: Unable to get user information from database.");
			}

			$row = pg_fetch_assoc($result);

			$this->user->first_name = $row['first_name'];
			$this->user->last_name = $row['last_name'];
			$this->user->class = (int) $row['class'];
		}
	}

	public function cache_set($key, $value)
	{
		return $this->Memcached->set($key, $value);
	}

	public function cache_get($key)
	{
		return $this->Memcached->get($key);
	}

	public function cache_delete($key)
	{
		return $this->Memcached->delete($key);
	}


	/* Check to see if a user is logged into the system */
	public function logged_in()
	{
		/* XXX: should probably save return value of this function
		 *		as it may be called multiple times during one page
		 *		load
		 */

		if (!isset($_COOKIE['u']) || !isset($_COOKIE['s'])) {
			return false;
		}

		$user_id = $_COOKIE['u'];
		$session = $_COOKIE['s'];

		/* Check cache for session */
		$c_user_id = $this->cache_get("session:$session");
		
		if (!$c_user_id) {
			// Session not found in cache.
			// TODO: get values from database. But for now, this is a failure.
			return false;
		}

		if ($c_user_id != $user_id) {
			// Error: User ids don't match. Unauthorized.
			return false;
		}

		/* Found and verified. Update cache expiration and return success. */
		$this->Memcached->replace("session:$session", $user_id, 3600);

		return true;
	}


	/* log_in
	 *
	 * Description:
	 *   Log a user onto the system
	 *
	 * Arguments:
	 *   + username - The user's username (string)
	 *   + password - The user's plaintext password (string)
	 *
	 * Return values:
	 *   Returns TRUE on success and FALSE on failure
	 */
	public function log_in($username, $password)
	{
		/* Verify password for user */
		$query = sprintf("SELECT id FROM admin_users
								WHERE username = '%s'
									AND password = '%s'
									AND enabled = TRUE
									AND deleted = FALSE",
								pg_escape_string($username),
								$this->hash_password($password));

		$result = pg_query($this->dbc, $query);
		if (!$result) {
			// Error: Database query failed
			die("Error: Database query failed");
		}

		if (pg_num_rows($result) > 1) {
			// Error: This is impossible
			die("Error: This is impossible, but for the sake of completeness...");
		}

		if (pg_num_rows($result) == 0) {
			// Error: Bad username/password combo
			return false;
		}
		
		list ($user_id) = pg_fetch_row($result);
		$session = $this->gen_session();

		/* Add the session to the DB */
		// TODO: do this

		/* Add the session to the cache */
		$this->Memcached->add("session:$session", $user_id, 3600);

		/* Set cookies */
		setcookie('u', $user_id, 0, '/', '.admin.dinetouch.com');
		setcookie('s', $session, 0, '/', '.admin.dinetouch.com');

		return true;
	}


	/* Log a user off the system */
	public function log_out()
	{
		$session = $_COOKIE['s'];

		// Delete session cache
		$this->cache_delete("session:$session");

		// Delete cookies
		setcookie('u', "", time() - 3600, '/', '.admin.dinetouch.com');
		setcookie('s', "", time() - 3600, '/', '.admin.dinetouch.com');
	}

	public function get_nextval($sequence)
	{
		// Get value
		$response = $this->SDB->get_attributes(
			'Sequences',
			$sequence,
			'Value',
			array(
				'ConsistentRead' => 'true'
			)
		);

		if ($response->isOK(200) && isset($response->body->GetAttributesResult->Attribute->Value)) {

			$value = $response->body->GetAttributesResult->Attribute->Value->to_string() + 1;

			// Update database value
			$response = $this->SDB->put_attributes(
				'Sequences',
				$sequence,
				array(
					'Value' => $value
				),
				true
			);

			if ($response->isOK(200)) {
				return $value;
			}
		}

		// Something went wrong
		return;
	}

	/* get_id()
	 * 
	 * Return a unique 64-bit integer identification number
	 */
	public function get_id()
    {
		// Get the time component (time to millisecond).
		// Time is relative to the DTEpoch (August 1st, 2012 00:00:00 GMT).
		$t = intval(microtime(true) * 1000) - 1343779200000;

		// Get the instance component.
		// TODO: Make this value unique for each running instance and set automatically.
		//       Must be in [1, 1023].
		$mach_id = 3;

		// Get the key component.
		$key = $this->Memcached->increment('get_id_key');
		if ($key === false) {
			// This key doesn't exist. Add it.
			$this->Memcached->add('get_id_key', 0);
			$key = $this->Memcached->increment('get_id_key');
			if ($key === false) {
				// XXX: Something else must be wrong.. Record it and fail.
				//      This should never happen!
				error_log("Unable to get_id key for machine id $mach_id!");
				POSUpdateService::error(DTError::DTErrorServiceFailure);
			}
		}

		// Note: this only works on 64-bit machines
		$t = ($t & 0x1FFFFFFFFFF) << 23;
		$mach_id = ($mach_id & 0x3FF) << 13;
		$key = ($key & 0x1FFF);

		// Return the unique 64-bit id
		return ($t | $mach_id | $key);
	}

	/* Generate an alphanumeric access code */
	public function gen_access_code()
    {
        $keys = array_merge(range(0, 9), range('a', 'z'), range('A', 'Z'));

        $code = "";
        for ($i = 0; $i < 32; $i++) {
            $code .= $keys[array_rand($keys)];
        }

        return $code;
    }

	/* Hash a password */
	public function hash_password($password)
	{
		return sha1($password . "Batman1316203037");
	}

	/* Generate a session id for a user */
	public function gen_session()
	{
		/* XXX: Consider using UUID or at least SHA1 */
		return md5(microtime() . mt_rand());
	}

}
?>
