<?php
/**
 * Defines Utility classes and methods for displaying Addresses
 *
 * FileName			AddressUtils.php
 * @author			Stephan Cavarra <scavarra@gmail.com>
 * @copyright		Copyright (c) 2015, Stephan Cavarra
 * 
 * Version History:
 * 		1/3/2015, Initial Version, Stephan Cavarra
 * 		1/4/2014, Added CrewChiefAddressIterator, Stephan Cavarra
 */

/**
 * Defines AddressDisplay class
 * This class is used to display the different Addresses
 *
 * @method		void setAddressType($addressType) sets the Address Type (i.e. 'email', 'physical') of the Address
 * @method		string getAddressType() returns the Address Type (i.e. 'email', 'physical') of the  Address
 * @method		void setAddressText($addressText) sets the Address Text of the Address
 * @method		string getAddressText() returns the Address Text of the  Address
 *
 * @version
 * 		1/3/2015, Initial Version, Stephan Cavarra
 */
class AddressDisplay
{
	private $addressType;
	private $addressText;

	/**
	 * Method to set the Address Type (i.e. 'email', 'physical') of the Address
	 * @param 	string $address
	 */
	public function setAddressType($addressType)
	{
		$this->addressType = $addressType;
	}

	/**
	 * Method to return the Address Type (i.e. 'email', 'physical') of the Address
	 * @return 	string
	 */
	public function getAddressType()
	{
		return $this->addressType;
	}

	/**
	 * Method to set the Address Text of the Address
	 * @param 	string $address
	 */
	public function setAddressText($addressText)
	{
		$this->addressText = $addressText;
	}

	/**
	 * Method to return the Address Text of the Address
	 * @return 	string
	 */
	public function getAddressText()
	{
		return $this->addressText;
	}
}


/**
 * Defines AddressIterator interface which will be used by other classes to iterate through the class
 *
 * @method		boolean hasNext() returns if we have another item
 * @method		object next() returns the next object
 *
 * @version
 * 		1/3/2015, Initial Version, Stephan Cavarra
 */
interface AddressIterator
{
	public function hasNext();
	public function next();
}

/**
 * Defines EmailAddressIterator class which implements AddressIterator
 * This class is used to iterate the EmailAddresses
 *
 * @method		void __construct($emailAddresses) constructs the class based off an EmailAddress object
 * @method		boolean hasNext() returns if we have another item
 * @method		object next() returns the next EmailAddressDisplayAdapter object
 *
 * @version
 * 		1/3/2015, Initial Version, Stephan Cavarra
 */
class EmailAddressIterator implements AddressIterator
{
	private $emailAddresses;
	private $position;

	public function __construct($emailAddresses)
	{
		$this->emailAddresses = $emailAddresses;
		$this->position = 0;
	}

	/**
	 * Method to return if we have another EmailAddress
	 * @return 	boolean
	 */
	public function hasNext()
	{
		if ($this->position >= count($this->emailAddresses) ||
				$this->emailAddresses[$this->position] == null) {
					return false;
				} else {
					return true;
				}
	}

	/**
	 * Method to return the next EmailAddressDisplayAdapter
	 * @return 	EmailAddressDisplayAdapter
	 */
	public function next()
	{
		$item = $this->emailAddresses[$this->position];
		$this->position = $this->position + 1;
		return new EmailAddressDisplayAdapter($item);
	}
}

/**
 * Defines PhysicalAddressIterator class which implements AddressIterator
 * This class is used to iterate the PhysicalAddresses
 *
 * @method		void __construct($physicalAddresses) constructs the class based off a PhysicalAddress object
 * @method		boolean hasNext() returns if we have another item
 * @method		object next() returns the next object
 *
 * @version
 * 		1/3/2015, Initial Version, Stephan Cavarra
 */
class PhysicalAddressIterator implements AddressIterator
{
	private $physicalAddresses;
	private $position;

	public function __construct($physicalAddresses)
	{
		$this->physicalAddresses = $physicalAddresses;
		$this->position = 0;
	}

	/**
	 * Method to return if we have another PhysicalAddress
	 * @return 	boolean
	 */
	public function hasNext()
	{
		if ($this->position >= count($this->physicalAddresses) ||
				$this->physicalAddresses[$this->position] == null) {
					return false;
				} else {
					return true;
				}
	}

	/**
	 * Method to return the next PhysicalAddressDisplayAdapter
	 * @return 	PhysicalAddressDisplayAdapter
	 */
	public function next()
	{
		$item = $this->physicalAddresses[$this->position];
		$this->position = $this->position + 1;
		return new PhysicalAddressDisplayAdapter($item);
	}
}

/**
 * Defines CombinedAddressIterator class which implements AddressIterator
 * This class is used to iterate both the Email and Physical addresses of the Crew Cheifs
 *
 * @method		void __construct(array $emailAddresses, array $physicalAddresses) constructs the class based off an array of EmailAddress and an array of PhysicalAddress
 * @method		boolean hasNext() returns if we have another item
 * @method		object next() returns the next EmailAddressDisplayAdapter|PhysicalAddressDisplayAdapter object
 *
 * @version
 * 		1/4/2015, Initial Version, Stephan Cavarra
 */
class CombinedAddressIterator implements AddressIterator
{
	private $emailItr;
	private $physicalAddressItr;
	
	public function __construct($emailAddresses, $physicalAddresses)
	{
		$this->emailItr = new EmailAddressIterator($emailAddresses);
		$this->physicalAddressItr = new PhysicalAddressIterator($physicalAddresses);
	}
	
	/**
	 * Method to return if we have another Address
	 * @return 	boolean
	 */
	public function hasNext()
	{
		return ($this->emailItr->hasNext() || $this->physicalAddressItr->hasNext());
	}
	
	/**
	 * Method to return the next PhysicalAddressDisplayAdapter
	 * @return EmailAddressDisplayAdapter|PhysicalAddressDisplayAdapter
	 */
	public function next()
	{
		if ($this->emailItr->hasNext()) {
			return $this->emailItr->next();
		} else {
			return $this->physicalAddressItr->next();
		}
	}
	
}

?>
