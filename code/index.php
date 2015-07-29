<?php

class Colorado {
	var $droughts = true; //:(
	var $forestFires = true; //:(
	var $longerRidingSeason = true; //:)
	var $sparkfun = true; //:)
	var $dancing = true; //:)
}

/**
 * Dummy marijuana dispensary finder program
 * run in command line with php installed
 * type 'php index.php' and press enter to run
 **/

class DispensaryFinder EXTENDS Colorado {
	
	private $dispensaries = null;
	private $hasMedicalCard = null;
	private $is21orOlder = null;
	private $path = './';
	var $simulateLoading = null;

	function __construct($simulateLoading = true)
	{
		$this->simulateLoading = $simulateLoading;
		$msg = "Welcome to Colorado\n";
		$msg .= "'Come for the weed! Stay for the weed!'\n";
		$msg .= '_\|/_ _\|/_ _\|/_ _\|/_ _\|/_ _\|/_ _\|/_';
		$msg .= "\n\n";
		echo $msg;
		$this->prompt();
		$this->fetchDispensaries();
		$this->recommend();

	}

	/**
	 * Prompt user to ensure requirements are met
	 *
	 * @return void
	 **/


	private function prompt()
	{
		echo 'Do you have a medical card? [Y/N]:';
		//if y skip next 1
		if(!$this->hasMedicalCard = $this->answer()) {
			echo 'Are you 21 or older? [Y/N]:';		
			if(!$this->is21orOlder = $this->answer()) {
				echo 'Do you have a sibling or cousin who is 21 or older? [Y/N]:';
				if($this->answer()) {
					echo 'Please sit him/her in front of the screen then press enter to continue:';
					$this->answer();
				} else {
					die("It's black market for you buddy. Sorry :(\n");
				}
			}
		}
	}

	/**
	 * Simulate loading. Travel websites do this sh*t on purpose too "The more you know!"
	 *
	 * @param int $seconds how many seconds do you want to delay?
	 * @param string $loader loading text to repeat
	 * @return void
	 **/

	public static function simulateLoading($seconds = 3,$loader = '.') {
		for($i = 0;$i < $seconds;$i++)
		{
			sleep(1);
			echo $loader;
		}
	}

	/**
	 * Check prompt answer
	 * 
	 * @return bool
	 **/

	private function answer()
	{
		$handle = fopen ("php://stdin","r");
		$line = fgets($handle);
		fclose($handle);
		if(strtolower(trim($line)) != 'y'){
			return false;
		}
		return true;
	}

	/**
	 * Recommends dispensary from list
	 *
	 * @return void
	 **/

	private function recommend()
	{
		if(empty($this->dispensaries)){
			die("Uh OH! Out of dispensaries. Better head to Washington!\n");
		}

		$dispensary = array_pop($this->dispensaries);
		$msg = 'How about this dispensary? '."\n\n";
		$msg.= $dispensary->name."\n".$dispensary->address."\n\n[Y/N]:";
		echo $msg;
		if($this->answer()){
			$this->medPsa();
		} else {
			$this->recommend();
		}

	}

	/**
	 * A friendly PSA
	 *
	 * @return void
	 **/

	 public static function medPsa()
	 {
	 	$delay = 2;

		echo "Remember GANJA and wheels don't mix.\n";
		echo "Don't get high while being high and in drive!\n";
		self::simulateLoading($delay,'');
		echo "Or in gear!\n";
		self::simulateLoading($delay,'');
		echo "Or in neutral!\n";
		self::simulateLoading($delay,'');
		echo "Or in park!\n";
		self::simulateLoading($delay,'');
		echo "Oh just go home and watch Cartoon Network!\n";
		self::simulateLoading($delay,'');
		echo "NOW!\n";
		self::simulateLoading($delay,'');
		echo "SERIOUSLY!\n";
		self::simulateLoading($delay,'');
		echo "Message brought to you by the Colorado Marijuana Enforcement Division.\n";
		self::simulateLoading($delay,'');
		echo "Serious bidness YO!\n";
		self::simulateLoading($delay,'');
		echo "k thanx bai now\n";
		self::simulateLoading(1,'');
		//could of put this in an array but yeah next version
	 }

	/**
	 * Retrieves list of dispensaries from flat file
	 *
	 * @return void
	 **/

	private function fetchDispensaries()
	{
		echo 'Fetching Dispensaries...';
		if($this->simulateLoading) $this->simulateLoading();
		echo "\n";
		
		if($this->hasMedicalCard()){
			$file = 'medical.json';
		} else {
			$file = 'recreational.json';
		}

		$file = file_get_contents($this->path.$file);
		$this->dispensaries = json_decode($file);
	}

	/**
	 * Does the user have a medical card?
	 *
	 * @return bool
	 **/

	public function hasMedicalCard()
	{
		return $this->hasMedicalCard;
	}

	/**
	 * Is the user 21 or older?
	 * 
	 * @return bool
	 **/

	public function is21orOlder()
	{
		return $this->is21orOlder;
	}
}

new DispensaryFinder();