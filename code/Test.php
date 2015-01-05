<?php
/**
 * Filename		Test.php
 * Author		Stephan Cavarra
 * Copyright	Copyright (c) 2015, Stephan Cavarra
 * 
 * Tests the Airplane, CrewChief, PhysicalAddress, EmailAddress and AddressUtils classes
 * 
 * This simple test scrip will first setup the needed Crew Chiefs with their information. This
 * includes setting up their names, email and physical addresses.
 * 
 * We will then define the Airplanes in the fleet.
 * 
 * Next we will assign the Crew Chiefs to the different airplanes and print out the fleet
 * information.
 * 
 * Version History:
 * 	1/3/2015, Initial Version, Stephan Cavarra
 *  1/4/2015, Added test for CrewChiefAddressIterator, Stephan Cavarra
 * 
 */

// Import our required classes and utilities
require('Airplane.php');
require('CrewChief.php');
require_once('AddressUtils.php');  // Utility class so only load once
require('EmailAddress.php');
require('PhysicalAddress.php');


// Define our Crew Chiefs and set their Email Addresses
$crewChief1 = new CrewChief();
$crewChief1->setName('Jim Jenkins');
$email1 = new EmailAddress();
$email1->setEmailAddress("jimj@example.com");
$crewChief1->setEmailAddress($email1);

$crewChief2 = new CrewChief();
$crewChief2->setName('Mel Callen');
$email2 = new EmailAddress();
$email2->setEmailAddress("melc@example.com");
$crewChief2->setEmailAddress($email2);

$crewChief3 = new CrewChief();
$crewChief3->setName('Stephan Cavarra');
$email3 = new EmailAddress();
$email3->setEmailAddress("stephanc@example.com");
$crewChief3->setEmailAddress($email3);


// Set up and array of email addresses to test the EmailAddressIterator
$emailAddresses = array(
		$email1,
		$email2,
		$email3
);

echo("Email Addresses:  \n");

$itr = new EmailAddressIterator($emailAddresses);

while ($itr->hasNext())
{
	$item = $itr->next();
	echo("  " . $item->getAddressText() . "\n");
}

echo("\n");


// Define our Physical Addresses and assign to Crew Chiefs
$pa1 = new PhysicalAddress("123 Any St.", "Boulder", "CO", "80303");
$pa2 = new PhysicalAddress("123 Any Blvd.", "Milliken", "CO", "80543");
$pa3 = new PhysicalAddress("123 Any Ave.", "Fort Collins", "CO", "80524");

$crewChief1->setPhysicalAddress($pa1);
$crewChief2->setPhysicalAddress($pa2);
$crewChief3->setPhysicalAddress($pa3);


// Set up and array of physical addresses to test the Iterator
$physicalAddresses = array( $pa1, $pa2, $pa3 );

// Test the compound iterator!
echo("Iterating through both types of addresses (Email and Physical):\n");

$itr3 = new CombinedAddressIterator($emailAddresses, $physicalAddresses);

while ($itr3->hasNext())
{
	$item = $itr3->next();
	echo("  " . $item->getAddressText() . "\n");
}

echo("\n");


// Create New Airplane as a base C182
$airplane = new Airplane();
$model = new BaseAirplaneModel();
$airplane->setModel($model);
$airplane->setNnumber('N9669X');
$airplane->setCrewChief($crewChief1);
$airplane->printDescription();  // Print Airplane info

// Add another Airplane as a C182S
$airplane = new Airplane();
$model = new BaseAirplaneModel();
$model = new C182SAirplaneModel($model);  // Extend Base Airplane with C182S items
$airplane->setModel($model);
$airplane->setNnumber('N9559X');
$airplane->setCrewChief($crewChief2);
$airplane->printDescription();

//Add another Airplane as a C182T
$airplane = new Airplane();
$model = new BaseAirplaneModel();
$model = new C182SAirplaneModel($model); // Extend Base Airplane with C182S items
$model = new T182TAirplaneModel($model); // Extend Base Airplane and C182S with T182T items
$airplane->setModel($model);
$airplane->setNnumber('N652CP');
$airplane->setCrewChief($crewChief3);
$airplane->printDescription();

?>