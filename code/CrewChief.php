<?php
/**
 * Defines the different classes and interfaces related to a CrewChief
 *
 * FileName			CrewChief.php
 * @author			Stephan Cavarra <scavarra@gmail.com>
 * @copyright		Copyright (c) 2015, Stephan Cavarra
 * 
 * Version History:
 * 		1/3/2015, Initial Version, Stephan Cavarra
 */


/**
 * Define the CrewChief class
 * 
 * @method			string __toString() returns a readable versin of the CrewChief object
 * @method			string getName() returns the Name of the Crew Chief
 * @method			void setName(string $name) sets the Name of the Crew Chief
 * @method			EmailAddress getEmailAddress() returns EmailAddress object of the Crew Chief
 * @method			void setEmailAddress(EmailAddress $eaddress) sets the EmailAddress object of the Crew Chief
 * @method			PhysicalAddress getPhysicalAddress() retuns the PhysicalAddress object of the Crew Chief
 * @method			void setPhysicalAddress(PhysicalAddress $physicalAddress) sets the PhysicalAddress object of the Crew Chief
 *
 * @version
 * 		1/3/2015, Initial Version, Stephan Cavarra
 */
class CrewChief
{
	/**
	 * Set up our variables
	 *  private string $name Name of the Crew Chief
	 *  private EmailAddress $emailAddress EmailAddress object of the Crew Cheif
	 *  private PhysicalAddress $physicalAddress PhysicalAddress object of the Crew Cheif
	 */
	private $name;
	private $emailAddress;
	private $physicalAddress;

	public function __toString()
	{
		return ($this->name . ", " . $this->emailAddress . ", " . $this->physicalAddress);
	}
	
	/**
	 * Method to return the Name of the Crew Chief
	 * @return 	string
	 */
	public function getName()
	{
		return $this->name;
	}

	/**
	 * Method to set the Name of the Crew Chief
	 * @param	string $nnumber
	 */
	public function setName($name)
	{
		$this->name = $name;
	}

	/**
	 * Method to return the EmailAddress object of the Crew Chief
	 * @return 	EmailAddress
	 */
	public function getEmailAddress()
	{
		return $this->emailAddress;
	}

	/**
	 * Method to set the EmailAddress object of the Crew Chief
	 * @param	EmailAddress $eaddress
	 */
	public function setEmailAddress($eaddress)
	{
		$this->emailAddress = $eaddress;
	}

	/**
	 * Method to return the PhysicalAddress object of the Crew Chief
	 * @return 	PhysicalAddress
	 */
	public function getPhysicalAddress()
	{
		return $this->physicalAddress;
	}

	/**
	 * Method to set the PhysicalAddress object of the Crew Chief
	 * @param	PhysicalAddress $paddress
	 */
	public function setPhysicalAddress($paddress)
	{
		$this->physicalAddress = $paddress;
	}

}

?>
