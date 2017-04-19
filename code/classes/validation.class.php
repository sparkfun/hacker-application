<?php

/********************************************************* 
**************** Input Validation Class ******************
**************** Written by: Tyler Bailey ****************
**********************************************************/

include_once('db.class.php');

class data_validation 
{

	
	// escape data
	public function sanitize($input) {
		//istantiate database object
		$mysqli = new DBConnection;
		
		//if $input is an array
		if(is_array($input)) {
			//sanitize each value
			foreach($input as $key => $value) {	
				$input[$key] = $mysqli->real_escape_string($value); 
			}
		} else {
			if (get_magic_quotes_gpc()) {
				$input = stripslashes($input);
			}
			$input = $mysqli->real_escape_string($input);
		}
		return $input;
	}


	
	// validate numeric
	public function validate_numeric($data) {
		// Remove excess whitespace
		$data = trim($data);
		
		if ( ctype_digit($data) ) {
			return true;
		}
		else {
			//return 'Not numeric';
			return false;
		}
	}
	
	
	// validate strings
	public function validate_string($data) {
		// Remove excess whitespace
		$data = trim($data);
		
		if ( preg_match("/^[0-9A-Za-z \.\-\'\"]+$/", $data) ) {
			return true;
		}
		else {
			//return 'Not a valid string';
			return false;
		}
	}
	
	
	// validate email addresses
	public function validate_email($data) {
		// Remove excess whitespace
		$data = trim($data);
		
		if(filter_var($data, FILTER_VALIDATE_EMAIL)) {
			return true;
		} else {
			//return 'Invalid email';
			return false;
		}
	}
	
	//validate username
	public function validate_username($data) {
		$data = trim($data);
		
		if(!strlen($data) > 3 && preg_match("/^[0-9A-Za-z \.\-\'\"]+$/", $data)) {
			return false;
		} else {
			return true;
		}
	}
	
	
	public function validate_password($pass) {
		$pass = trim($pass);
		
		if(!strlen($pass) > 6 && preg_match("/^[0-9A-Za-z \.\-\'\"]+$/", $pass)) {
			return false;
		} else {
			return true;
		}
	}
	
	public function match_passwords($pass1, $pass2) {
		$pass1 = trim($pass1);
		$pass2 = trim($pass2);
		
		if($pass1 === $pass2) {
			return true;
		} else {
			return false;
		}
	}
	
	
	
	//validate urls
	public function validate_url($data) {
		$data = trim($data);
		
		if(filter_input($data, FILTER_VALIDATE_URL)) {
			return true;
		} else {
			return false;
		}
	}
	
	
	// validate phone numbers 
	public function validate_phone($data) {
		/* Matches the following
		123 456 7890
		123-456-7890
		123.456.7890
		(123)-456-7890
		*/
		
		// Remove excess whitespace
		$data = trim($data);
		
		if ( preg_match("/^[\(]?([0-9]{3})(\) \.-)?[0-9]{3}( \.-)?[0-9]{4}$/", $data)) {
			return true;
		}
		else {
			//return 'Invalid phone';
			return false;
		}
	}
	
	
	// validate zipcodes
	public function validate_zip($data) {
		/*
		Matches the following:
		12345
		12345-6789
		*/
		
		// Remove excess whitespace
		$data = trim($data);
		
		if ( preg_match("/^[0-9]{5}([0-9]{4})?$/", $data)) {
			return true;
		}
		else {
			//return 'Not a valid zip';
			return false;
		}
	}
	
	
	// validate credit cards
	public function validate_credit_card($type, $data) {
		// Remove excess whitespace
		$data = trim($data);
		
		switch ($type) {
			// American Express
			// Format starts with 34 or 37 and has 17 total digits
			case 1:
				if ( preg_match("/^([34|37]{2})([0-9]{13})$/", $data) ) {
					return true;
				}
			break;
			// Diners Club
			case 2:
				if ( preg_match("/^([30|36|38]{2})([0-9]{12})$/", $data) ) {
					return true;
				}
			break;			
			// Discover
			case 3:
				if ( preg_match("/^([6011]{4})([0-9]{12})$/", $data) ) {
					return true;
				}
			break;			
			// MasterCard
			case 4:
				if ( preg_match("/^([51|52|53|54|55]{2})([0-9]{14})$/", $data) ) {
					return true;
				}
			break;			
			// Visa
			case 5:
				if ( preg_match("/^([4]{1})([0-9]{12,15})$/", $data) ) {
					return true;
				}
			break;			
			default:
				return false;
		}
	}
}
?>