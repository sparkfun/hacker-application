<?php

class Player { // the player as in the person at the keyboard
	public $screenName; // string
	public $ip;  // string
	public $lastSeen; // string
	public $isPlayingNow; // bool
	public $hasGameSaved; // bool
	private $saveGame; // array

	public function __construct( $name ) {
		$this -> screenName = $name;
		$this -> isPlayingNow = false;
		$this -> hasGameSaved = false;
	}

	private function restoreSaveGame( array $saveData ) {
		$this -> saveGame = $saveData;
		$this -> hasGameSaved = true;
	}

	public function getSavedPos() {
		return $this -> saveGame['pos'];
	}

}
?>
