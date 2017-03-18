<?php defined('SYSPATH') or die('No direct script access.');

/*
API Controller
All API Controllers extend this for validation & security.
*/

class Controller_Api extends Controller {
    private static $log_id;
    private static $start;

	/*
	::before
	Check if user is logged in - if not forward to login page

	General Authentication Procedure:
	Authentication:
		Key + Secret
			Key = The account username
			Secret = MD5 Hash Code
	All variables should be found in $_POST with their lowercase name.
	*/
    function before() {
        self::$start = microtime(TRUE);

		// JSON!
		header('Cache-Control: no-cache, must-revalidate');
		header('Expires: Mon, 26 Jul 1997 05:00:00 GMT'); // WTF?
		header('Content-type: application/json');

        // Allow internal calls for now.
        if ($this->request->controller == 'internalv1') {
            parent::before();
            return;
        }

		// Check if user authenticates...
		if (! isset($_POST['key'])) {
			$return_object = new StdClass;
			$return_object->status = '0';
			$return_object->error = 'Please provide an API key.';

			die(json_encode($return_object));
		}
		$key = $_POST['key'];

		// Process Single Use Token
		if (isset($_POST['token'])) {
			$token = $_POST['token'];
			$user = ORM::Factory('user')->where('username', 'LIKE', $key)->find();

			// User wasn't found...
			if (! $user->loaded()) {
				$return_object = new StdClass;
				$return_object->success = "0";
				$return_object->error = "That key was not found.  Are you using the correct username?";
				$return_object->warnings = "";

				die(json_encode($return_object));
			}

            // keep white label users out!
            if($user->is_whitelabel)
            {
                Helper_Wellcomemat::redirect($this, 'WHITELABEL_HOME');
            }

			$apitoken = ORM::Factory('apitoken')->where('value', '=', $token)->where('used', '=', '0')->find();

			// Token wasn't found.
			if (! $apitoken->loaded()) {
				$return_object = new StdClass;
				$return_object->success = '0';
				$return_object->error = 'That token was not found or is no longer valid.';
				$return_object->warnings = '';

				die(json_encode($return_object));
			}

			// Token wasn't for this user.
			if ($apitoken->apikey_id != $user->apikey->id) {
				$return_object = new StdClass;
				$return_object->success = '0';
				$return_object->error = 'That token was invalid for this account.';
				$return_object->warnings = '';

				die(json_encode($return_object));
			}

			// Mark token as used.
			$apitoken->used = 1;
			$apitoken->save();

		} else {

			// No key provided.
			if (! isset($_POST['secret'])) {
				$return_object = new StdClass;
				$return_object->success = '0';
				$return_object->error = 'Please provide an API secret.';
				$return_object->warnings = '';

				die(json_encode($return_object));
			}
			$secret = $_POST['secret'];

			// Load the user for $key
			$user = ORM::Factory('user')->where('username', 'LIKE', $key)->find();

			// User wasn't found...
			if (! $user->loaded()) {
				$return_object = new StdClass;
				$return_object->success = '0';
				$return_object->error = 'That key was not found.  Are you using the correct username?';
				$return_object->warnings = '';

				die(json_encode($return_object));
			}

			// Invalid $secret
			if (strtolower($user->apikey->secret) != strtolower($secret)) {
				$return_object = new StdClass;
				$return_object->success = '0';
				$return_object->error = 'That secret was incorrect.  Please verify that you are using the correct hash.';
				$return_object->warnings = '';

				die(json_encode($return_object));
			}
		}

        // Restrict access.
        if (! $user->access('api')) {
            $return_object = new StdClass;
            $return_object->success = '0';
            $return_object->error = 'Sorry, API access is not allowed for your account.';
            $return_object->warnings = '';

            die(json_encode($return_object));
        }

        // Log success request (as long as it's not internal).
        $url = $_SERVER['REQUEST_URI'];
        if (! preg_match('/^\/api\/internal\//', $url)) {
            $log = ORM::factory('apilog');
            $log->ip = Helper_Wellcomemat::client_ip();
            $log->key = $key;
            $log->url = $_SERVER['REQUEST_URI'];
            $log->save();

            self::$log_id = $log->id;
        }

        // Log FSBO.
        if (0 && $key == 'fsbovideo')
            file_put_contents('/tmp/api-fsbo', $log->url . "\n" . print_r($_REQUEST, TRUE) . "-----\n", FILE_APPEND);

		// The request verified correctly - send them through.
		parent::before();
	}

    public function add_timing() {
        if (self::$log_id && self::$start) {
            $log = ORM::factory('apilog', self::$log_id);

            if ($log->loaded()) {
                $log->elapsed = microtime(TRUE) - self::$start;
                $log->save();
            }
        }
    }
    public function error($msg) {
        $this->response(array('error' => $msg));
    }
    public function response($arr) {
        $obj = (object) $arr;

        die(json_encode($obj));
    }
    public function success() {
        $this->response(array('success' => 1));
    }
    public function update_log($detail, $callback = FALSE, $after = FALSE) {
        $log = ORM::factory('apilog', self::$log_id);

        if ($log->loaded()) {
            $log->detail = $detail;

            // Add callback.
            if ($callback)
                $log->callback = $callback;

            // Add post-processing functions.
            if ($after)
                $log->after = implode(';', $after);

            $log->save();
        }
    }
}

?>
