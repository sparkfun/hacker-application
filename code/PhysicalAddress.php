<?php
/**
 * Defines the different classes related to a PhysicalAddress
 * 
 * FileName			PhysicalAddress.php
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
require_once('AddressUtils.php');  // Utility class so only load once


/**
 * Defines PhysicalAddress class
 *
 * @method		void __construct(string $streetAddress, string $city, string $state, string $postalCode) constructs the class with data
 * @method		string __toString() returns a readable versin of the PhysicalAddress object
 * @method		string getStreetAddress() returns the Street Address of the Physical Address
 * @method		string getCity() returns the City of the Physical Address
 * @method		string getState() returns the State of the Physical Address
 * @method		string getPostalCode() returns the PostalCode of the Physical Address
 * @method		void setStreetAddress(string $streetAddress) sets the Street Address of the Physical Address
 * @method		void setCity(string $city) sets the City of the Physical Address
 * @method		void setState(string $state) sets the State of the Physical Address
 * @method		void setPostalCode(string $postalCode) sets the Postal Code of the Physical Address
 *
 * @version
 * 		1/3/2015, Initial Version, Stephan Cavarra
 */
class PhysicalAddress
{
	/**
	 * Set up our variables
	 *  private string $streetAddress Street Address
	 *  private string $city City
	 *  private string $state State
	 *  private string $postalCode Postal Code
	 */
	private $streetAddress = '';
	private $city = '';
	private $state = '';
	private $postalCode = '';

	public function __construct($streetAddress, $city, $state, $postalCode)
	{
		$this->streetAddress = $streetAddress;
		$this->city = $city;
		$this->state = $state;
		$this->postalCode = $postalCode;
	}
	
	public function __toString()
	{
		return ($this->streetAddress . ", " .
			$this->city . ", " .
			$this->state . ", " .
			$this->postalCode);
	}

	/**
	 * Method to return the Street Address of the PhysicalAddress
	 * @return 	string
	 */
	public function getStreetAddress()
	{
		return $this->streetAddress;
	}

	/**
	 * Method to return the City of the Physical Address
	 * @return 	string
	 */
	public function getCity()
	{
		return $this->city;
	}

	/**
	 * Method to return the State of the Physical Address
	 * @return 	string
	 */
	public function getState()
	{
		return $this->state;
	}
	
	/**
	 * Method to return the Postal Code of the Physical Address
	 * @return 	string
	 */
	public function getPostalCode()
	{
		return $this->postalCode;
	}

	/**
	 * Method to set the Street Address of the Physical Address
	 * @param	string $streetAddress
	 */
	public function setStreetAddress($streetAddress)
	{
		$this->streetAddress = $streetAddress;
	}

	/**
	 * Method to set the City of the Physical Address
	 * @param	string $city
	 */
	public function setCity($city)
	{
		$this->city = $city;
	}

	/**
	 * Method to set the State of the Physical Address
	 * @param	string $state
	 */
	public function setState($state)
	{
		$this->state = $state;
	}
	
	/**
	 * Method to set the Postal Code of the Physical Address
	 * @param	string $postalCode
	 */
	public function setPostalCode($postalCode)
	{
		$this->postalCode = $postalCode;
	}
}


/**
 * Defines PhysicalAddressDisplayAdapter class which extends AddressDisplay
 * This will set the AddressType and AddressText
 *
 * @method		void __construct($physicalAddress) constructs the class with a PhysicalAddress object
 *
 * @version
 * 		1/3/2015, Initial Version, Stephan Cavarra
 */
class PhysicalAddressDisplayAdapter extends AddressDisplay
{
	public function __construct($physicalAddress)
	{
		$this->setAddressType("physical");
		$this->setAddressText($physicalAddress->getStreetAddress() . " " . $physicalAddress->getCity() . ", " .
				$physicalAddress->getState() . "  " . $physicalAddress->getPostalCode());
	}
}

?>