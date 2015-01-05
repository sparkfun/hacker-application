<?php
/**
 * Defines the different classes related to a EmailAddress
 *
 * FileName			EmailAddress.php
 * @author			Stephan Cavarra <scavarra@gmail.com>
 * @copyright		Copyright (c) 2015, Stephan Cavarra
 * 
 * Version History:
 * 		1/3/2015, Initial Version, Stephan Cavarra
 * 		1/4/2014, Added __toString, Stephan Cavarra
 */


/**
 * Import our required classes and utilities
 */
require_once('AddressUtils.php');


/**
 * Defines EmailAddress class
 * 
 * @method		string __toString() returns a readable versin of the EmailAddress object
 * @method		string getEmailAddress() returns the Email Address of the Email Address
 * @method		void setEmailAddress($address) sets the Email Address of the Email Address
 *
 * @version
 * 		1/3/2015, Initial Version, Stephan Cavarra
 */
class EmailAddress
{
	/**
	 * Set up our variables
	 *  private string $emailAddress Email Address
	 */
	private $emailAddress;
	
	public function __toString()
	{
		return $this->emailAddress;
	}
	
	/**
	 * Method to return the Email Address of the EmailAddress
	 * @return 	string
	 */
	public function getEmailAddress()
	{
		return $this->emailAddress;
	}

	/**
	 * Method to set the Email Address of the EmailAddress
	 * @param 	string $address
	 */
	public function setEmailAddress($address)
	{
		$this->emailAddress = $address;
	}
}

/**
 * Defines EmailAddressDisplayAdapter class which extends AddressDisplay
 * This will set the AddressType and AddressText
 *
 * @method		void __construct($emailAddr) constructs the class with a EmailAddress object
 *
 * Version History:
 * 		1/3/2015, Initial Version, Stephan Cavarra
 */

class EmailAddressDisplayAdapter extends AddressDisplay
{
	public function __construct($emailAddr)
	{
		$this->setAddressType("email");
		$this->setAddressText($emailAddr->getEmailAddress());
	}
}

?>

