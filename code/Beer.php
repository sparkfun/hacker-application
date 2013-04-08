<?php
abstract class Beer
{

	private $name;

	private $strength;

	private $bitterness;

	protected $MIN_ABV = 0.0;

	protected $MAX_ABV = 100.0;

	protected $MIN_IBU = 0;

	protected $MAX_IBU = 100;

	public function Beer()
	{
		$this->__construct();
	}

	public function __construct( $name, $strength, $bitterness )
	{
		$this->name = $name;
		$this->strength = $strength;
		$this->bitterness = $bitterness;
	}

	// a user-defined name for this beer
	public function getName()
	{
		echo "Name: ";
		echo $this->name;
		echo "<br>";
	}

	// based on the subclass/style of beer
	abstract public function getStyle();

	// strength should be 0.0 - 1.0
	public function getABV()
	{
		echo "ABV: ";
		echo ($this->MIN_ABV + ($this->MAX_ABV - $this->MIN_ABV) * $this->strength);
		echo "<br>";
	}

	// bitterness should be 0.0 - 1.0
	public function getIBU()
	{
		echo "IBU: ";
		echo ($this->MIN_IBU + ($this->MAX_IBU - $this->MIN_IBU) * $this->bitterness);
		echo "<br>";
	}

}

class StandardAmericanLager extends Beer
{
	protected $MIN_ABV = 4.2;

	protected $MAX_ABV = 5.3;

	protected $MIN_IBU = 8;

	protected $MAX_IBU = 15;

	public function StandardAmericanLager()
	{
		$this->__construct();
	}

	public function __construct( $name, $strength, $bitterness )
	{
		parent::__construct( $name, $strength, $bitterness );
	}

	public function getStyle()
	{
		echo "Style: Standard American Lager<br>";
	}
}

class ImperialIPA extends Beer
{
	protected $MIN_ABV = 7.5;

	protected $MAX_ABV = 10.0;

	protected $MIN_IBU = 60;

	protected $MAX_IBU = 120;

	public function __construct( $name, $strength, $bitterness )
	{
		parent::__construct( $name, $strength, $bitterness );
	}

	public function getStyle()
	{
		echo "Style: Imperial IPA<br>";
	}
}

class RussianImperialStout extends Beer
{
	protected $MIN_ABV = 8.0;

	protected $MAX_ABV = 12.0;

	protected $MIN_IBU = 50;

	protected $MAX_IBU = 90;

	public function __construct( $name, $strength, $bitterness )
	{
		parent::__construct( $name, $strength, $bitterness );
	}

	public function getStyle()
	{
		echo "Style: Russian Imperial Stout<br>";
	}
}

?>
