<?php

/* An example class I wrote for a login library i could re-use */

/* TODO: Unfortunately I should probably move the logic in the getters and setters into UserUtil */
class User {
	private $id;
	private $username;
	private $email;
	private $created;
	
	private $nonce;
	private $salt;
	private $digest;
	
	private $logged;
	
	/* Setter and accesor for Id */
	public function getId() {
		return $this->id;
	}

	public function setId($id) {
		$this->id = $id;
	}
	
	/* Setter and accesor for username */
	public function getUsername() {
		return $this->username;
	}

	public function setUsername($username) {
		$this->username = $username;
	}

	/* Setter and accesor for email */
	public function getEmail() {
		return $this->email;
	}

	public function setEmail($email) {
		if($this->validEmail($email)) {
			$this->email = $email;
		} else {	
			throw new InvalidUserEmailException("Email address is invalid ({$email})");
		}
	}	
	
	public function getNonce() {
		if (!$this->nonce) {
			$this->nonce = $this->rand_sha();
		}	
		
		return $this->nonce;
	}
	
	public function getSalt() {
		if (!$this->salt) {
			$this->salt = $this->rand_sha();		
		}
		
		return $this->salt;
	}

	public function setSalt($salt) {
		return $this->salt = $salt;
	}
	
	/* TODO: Do this with bcrypt or whatever */
	public function setPassword($password) {
		$this->digest = sha1($this->getSalt() . $password);
		return $this->digest;
	}
	
	public function getDigest() {
		return $this->digest;
	}

	public function createRandomUser() {
		if (!$this->username) {
			$this->username = ucwords($this->rand_string(5));
		}
		
		if (!$this->email) {
			$this->email = $this->rand_string(5) . '@' . $this->rand_suffix();
		}
	}
	
	private function validEmail($email) {
	   $isValid = true;
	   $atIndex = strrpos($email, "@");

	   if (is_bool($atIndex) && !$atIndex) {
	      $isValid = false;
	   } else {
	      $domain = substr($email, $atIndex+1);
	      $local = substr($email, 0, $atIndex);
	      $localLen = strlen($local);
	      $domainLen = strlen($domain);

	      if ($localLen < 1 || $localLen > 64) {
	         $isValid = false; // local part length exceeded
	      } else if ($domainLen < 1 || $domainLen > 255){
	         $isValid = false; // domain part length exceeded
	      } else if ($local[0] == '.' || $local[$localLen-1] == '.') {
	         $isValid = false; // local part starts or ends with '.'
	      } else if (preg_match('/\\.\\./', $local)) {
	         $isValid = false; // local part has two consecutive dots
	      } else if (!preg_match('/^[A-Za-z0-9\\-\\.]+$/', $domain)) {
	         $isValid = false; // character not valid in domain part
	      } else if (preg_match('/\\.\\./', $domain)) {
	         $isValid = false;  // domain part has two consecutive dots
	      } else if (!preg_match('/^(\\\\.|[A-Za-z0-9!#%&`_=\\/$\'*+?^{}|~.-])+$/',
	                 str_replace("\\\\","",$local)))  {
	         // character not valid in local part unless 
	         // local part is quoted
	         if (!preg_match('/^"(\\\\"|[^"])+"$/',
	             str_replace("\\\\","",$local))) {
	            $isValid = false;
	         }
	      }
	      /*if ($isValid && !(checkdnsrr($domain,"MX") || checkdnsrr($domain,"A"))) {
	         // domain not found in DNS
	         $isValid = false;
	      }*/
	   }
	   return $isValid;
	}
	
	public function __toString() {
		return sprintf('user %s (id %d)', $username, $id);
	}
	
	function rand_sha() {
		return bin2hex(openssl_random_pseudo_bytes(20));
	}

	function rand_string($length) {
		$str = '';
	    $charset ='abcdefghijklmnopqrstuvwxyz';
	    while ($length--)
	        $str .= $charset[mt_rand(0, strlen($charset)-1)];
	    return $str;
	}

	function rand_suffix() {
		$str = '';
	    $domains = array('gmail', 'yahoo', 'hotmail', 'aim', 'live');
	    $suffixes = array('com', 'net', 'me', 'co.uk', 'gov', 'edu');
	    return $domains[mt_rand(0, sizeof($domains)-1)] . '.' . $suffixes[mt_rand(0, sizeof($suffixes)-1)];
	}
}

class InvalidUserEmailException extends Exception {} 

