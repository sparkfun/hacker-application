<?php
/**
 * Defines the different classes and interfaces related to the Airplane
 *
 * FileName			Airplane.php
 * @author			Stephan Cavarra <scavarra@gmail.com>
 * @copyright		Copyright (c) 2015, Stephan Cavarra
 * 
 * Version History:
 * 		1/3/2015, Initial Version, Stephan Cavarra
 * 		1/4/2014, Updated T182TAirplaneModel class, Stephan Cavarra
 */


/**
 * Defines the Airplane class
 * 
 * @method			string getNnumber() returns the N-Number of the Airplane
 * @method			void setNnumber(string $nnumber) sets the N-Number of the Airplane
 * @method			string getModel() returns the Model of the Airplane
 * @method			void setModel(string $model) sets the Model of the Airplane
 * @method			CrewChief getCrewChief() retuns the CrewChief object of the Airplane
 * @method			void setCrewChief(CrewChief $crewChief) sets the CrewChief object of the Airplane
 * @method			printDescription() prints out information about the Airplane
 *
 * @version
 * 		1/3/2015, Initial Version, Stephan Cavarra
 */
class Airplane
{
	/**
	 * Set up our variables
	 * 	private string $nnumber N-Number of the Airplane
	 * 	private string $model Model of the Airplane
	 *	private CrewChief $crewChief CrewChief object of the Airplane 
	 */
	private $nnumber;
	private $model;
	private $crewChief;

	/**
	 * Method to return the N-Number of the Airplane
	 * @return 	string
	 */
	public function getNnumber()
	{
		return $this->nnumber;
	}

	/**
	 * Method to set the N-Number of the Airplane
	 * @param	string $nnumber
	 */
	 public function setNnumber($nnumber)
	{
		$this->nnumber = $nnumber;
	}

	/**
	 * Method to return the Model of the Airplane
	 * @return 	string
	 */
	public function getModel()
	{
		return $this->model;
	}
	
	/**
	 * Method to set the Model of the Airplane
	 * @param	string $model
	 */
	public function setModel($model)
	{
		$this->model = $model;
	}
	
	/**
	 * Method to return the CrewChief object of the Airplane
	 * @return 	CrewChief
	 */
	public function getCrewChief()
	{
		return $this->crewChief;
	}
	
	/**
	 * Method to set the CrewChief object of the Airplane
	 * @param	CrewChief $crewChief
	 */
	public function setCrewChief($crewChief)
	{
		$this->crewChief = $crewChief;
	}
	
	/**
	 * Method to print out information about the Airplane, such as N-Number, Crew Chief, Features and Cost
	 */
	public function printDescription()
	{
		echo("N-Number:  " . $this->getNnumber() . "\n");
		echo("  Crew Chief:  " . $this->getCrewChief() . "\n");
		foreach ($this->getModel()->getFeatures() as $option=>$description)
		{
			echo("    $option:  $description \n");
		}

		echo("Final cost:  " . $this->getModel()->getAirplaneCost() . "\n\n");
	}
}


/**
 * Defines AirplaneModel interface which will be used by other classes demonstrating a decorator pattern
 *
 * @method			string getAirplaneCost() returns the cost of the Airplane
 * @method			string getFeatures() returns the features of the Airplane
 *
 * @version
 * 		1/3/2015, Initial Version, Stephan Cavarra
 */
interface AirplaneModel
{
	function getAirplaneCost();
	function getFeatures();
}

/**
 * Defines the BaseAirplaneModel class which implements the AirplaneModel interface
 * This BaseAirplaneModel has the base cost of the airplane with defined feature set of:
 * 	Cost:			250000
 * 	Manufacturer:	Cessna
 * 	Type:			C182R
 * 	Navigation:		Standard
 * 	Autopilot:		None
 * 
 * @method			string getAirplaneCost() returns the cost of the Airplane
 * @method			array getFeatures() returns the features of the Airplane
 *
 * @version
 * 		1/3/2015, Initial Version, Stephan Cavarra
 */
class BaseAirplaneModel implements AirplaneModel
{
	/**
	 * Method to return the Cost of the Airplane
	 * @return 	string
	 */
	public function getAirplaneCost()
	{
		return 250000;
	}

	/**
	 * Method to return the Features of the Airplane
	 * @return 	array (string => string)
	 */
	public function getFeatures()
	{
		return array(
				'manufacturer' => 'Cessna',
				'type' => 'C182R',
				'navigation' => 'standard',
				'engine' => 'Continental O-470-U',
				'autopilot' => 'none'
		);
	}
}

/**
 * Defines the C182SAirplaneModel class which implements the AirplaneModel interface
 * This C182SAirplaneModel EXTENDS an AirplaneModel class with the following attributes:
 * 	Cost:			BaseModel + 100000
 * 	Type:			C182S
 *  Engine:			Lycoming IO-540-AB1A5
 * 	Navigation:		Apollo GX-55
 * 
 * @method			void __constructor(AirplaneModel $model) constructs the class with a base AirplaneModel
 * @method			string getAirplaneCost() returns the cost of the Airplane
 * @method			array getFeatures() returns the features of the Airplane
 *
 * @version
 * 		1/3/2015, Initial Version, Stephan Cavarra
 */
class C182SAirplaneModel implements AirplaneModel
{
	/**
	 * Set up our variables
	 * 	private string $model Model of the Airplane
	 */
	private $model;

	/**
	 * Constructor Method constructs constructs the class with a base AirplaneModel
	 * @param	AirplaneModel $model
	 */
	public function __construct($model)
	{
		$this->model = $model;
	}

	/**
	 * Method to return the Cost of the Airplane
	 * In this case it is adding 50000 to the AirplaneModel
	 * @return 	string
	 */
	public function getAirplaneCost()
	{
		$cost = $this->model->getAirplaneCost();
		$cost += 100000; /* The sport model is more expensive! */
		return $cost;
	}

	/**
	 * Method to return the Features of the Airplane
	 * In this case it is EXTENDING the features of the AirplaneModel with additions features
	 * @return 	array (string => string)
	 */
	public function getFeatures()
	{
		$features = $this->model->getFeatures();

		$features['type'] = 'C182S';
		$features['engine'] = 'Lycoming IO-540-AB1A5 230 BHP';
		$features['navigation'] = 'Apollo GX-55';
		$features['autopilot'] = 'STec System Fifty Five X';

		return $features;
	}
}

/**
 * Defines the T182TAirplaneModel class which implements the AirplaneModel interface
 * This T182TAirplaneModel EXTENDS an AirplaneModel class with the following attributes:
 * 	Cost:			BaseModel + 200000
 * 	Type:			T182T
 * 	Autopilot:		Bendix/King KAP140
 * 	Engine:			Lycoming TIO-540-AK1A Turbo 235 BHP
 * 	Navigation:		Garmin G1000 (NAV III)
 * 	Safety:			AmSafe Airbags
 *
 * @method			void __constructor(AirplaneModel $model) constructs the class with a base AirplaneModel
 * @method			string getAirplaneCost() returns the cost of the Airplane
 * @method			array getFeatures() returns the features of the Airplane
 *
 * @version
 * 		1/3/2015, Initial Version, Stephan Cavarra
 * 		1/3/2015, Updated Cost and Features, Stephan Cavarra
 */
class T182TAirplaneModel implements AirplaneModel
{
	/**
	 * Set up our variables
	 * 	private string $model Model of the Airplane
	 */
	private $model;

	/**
	 * Constructor Method constructs the class with a base AirplaneModel
	 * @param	AirplaneModel $model
	 */
	public function __construct($model)
	{
		$this->model = $model;
	}

	/**
	 * Method to return the Cost of the Airplane
	 * In this case it is adding 200000 to the AirplaneModel
	 * @return 	string
	 */
	public function getAirplaneCost()
	{
		$cost = $this->model->getAirplaneCost();
		$cost += 200000;
		return $cost;
	}

	/**
	 * Method to return the Features of the Airplane
	 * In this case it is EXTENDING the features of the AirplaneModel with additions features
	 * @return 	array (string => string)
	 */
	public function getFeatures()
	{
		$features = $this->model->getFeatures();

		$features['type'] = 'T182T';
		$features['autopilot'] = 'Bendix/King KAP140';
		$features['engine'] = 'Lycoming TIO-540-AK1A Turbo 235 BHP';
		$features['navigation'] = 'Garmin G1000 (NAV III)';
		$features['safety'] = 'AmSafe Airbags';

		return $features;
	}
}

?>