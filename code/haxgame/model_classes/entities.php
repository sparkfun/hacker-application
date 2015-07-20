<?php

class Pos {
	public $fine;
	public $coarse;
	public function __construct( $xa,$ya, $xb,$yb ) {
		$this -> fine = array( 'x' => $xa, 'y' => $ya );
		$this -> coarse = array( 'x' => $xb, 'y' => $yb );
	}
}

class GameEntity implements Tangible {
	public $name; // : string
	protected $pos; // : Pos
	public $sprite; // : string name of currently displayed svg without path or extension.
	public function __construct( Pos $pos ) {
		$this -> setPos( $pos );
	}
	public function setPos( $newPos ) {
		$this -> pos = $newPos;
	}
	public function getPos() {
		$currentPos = $this -> pos;
		return $currentPos;
	}
}

class PlayerChar extends GameEntity implements Moving, Mortal, Playable {
	protected $pos;
	protected $died = false;
	protected $health = 300;
	public $falling = false;
	public $facing;
	public $camPos;
	public $owner; // : Player( $n : string, $inId : string, $inIP : string )
	public function __construct( Player $you ) { // the player character is initialized with a Player
		if ( $you -> hasGameSaved ) {
			$spawnPt = $you -> getSavedPos();
		}
		else if ( !$you -> isPlayingNow ) {
			$spawnPt = new Pos( 68, 87, 0, 0 ); // start at the default / zero position
		}
		parent::__construct( $spawnPt );
		$this -> camPos = new Pos( 59, 93, 0, 0 );
		$this -> moveTo( $spawnPt );
		$this -> owner = $you;
		$this -> sprite = 'player';
	}

	public function moveTo( $pos ) {
		$this -> pos = $pos;
	}

	public function takeDamage( $amount ) {
		$newHP = $this -> health;
		$newHP -= intval( $amount );
		if ( $newHP < 1 ) {
			$this -> health = 0;
			$this -> expire();
		} else {
			$this -> health = $newHP;
		}
	}

	public function takeInput ( $worldClick ) {
		$clickX = $worldClick['x'];
		$clickY = $worldClick['y'];
		$prevX = $this -> pos -> fine['x'];
		$prevY = $this -> pos -> fine['y'];

		if ($clickX > $prevX ) {
			$dir = ['x' => 1, 'y' => 0];
			$this -> facing = "right";
		} else if ($clickX < $prevX) {
			$dir = ['x' => -1, 'y' => 0];
			$this -> facing = "left";
		} else { // only recognize a directly up or down click
			if ($clickY < $prevY ) {
				$dir = ['x' => 0, 'y' => -1];
			} else if ($clickY > $prevY) {
				$dir = ['x' => 0, 'y' => 1];
			}
		}
		return $dir;
	}
	public function expire() {
		$this ['$died'] = true;
	}
}

class Enemy extends GameEntity implements Deadly, Mortal, Autonomous {
	protected $damageDealt;
	protected $died = false;
	public function __construct( $spawnPt ) {
		parent::__construct( $spawnPt );
	}

	public function decideMove() {

	}

	public function moveTo( $pos ) {

	}

	public function hurt( $target ) {

	}

	public function takeDamage( $amount ) {

	}

	public function expire() {
		$this['died'] = true;
	}
}




class Rock extends GameEntity implements Pushable, Deadly {
	protected $damageDealt = 100;
	public function __construct() {
		parent::__construct();
	}

	public function getPushed ( $pusher ) {


	}

	public function moveTo( $there ) {
		if ( $there['coarse'] ) {
			$tc = $there['coarse'];
			$tf = $there['fine'];
			$this -> pos = new Pos( $tf['x'], $tf['y'], $tc['x'], $tc['y']);
		}
		else {
			$this -> pos[ 'fine' ] = array( 'x' => $there['x'], 'y' => $there['y'] );
		}

	}
	public function hurt($target) {
		if ($target instanceOf Mortal) {
			$target -> takeDamage( $damageDealt );
		}
	}
}

?>
