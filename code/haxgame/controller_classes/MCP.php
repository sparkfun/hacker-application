<?php

class MCP { // controller / business functions. Takes input from view and updates state of model
	private $db;


	function __construct( $inDB ) { // doing this instead of setting a global
		$this -> db = $inDB;
	}

	function updateWorld() {
		$this -> movePlayer();
		$this -> moveMobs();
	}

	function movePlayer() {
		$tB = $this -> db -> tileBuffer;
		$pCs = $this -> db -> playerChars;
		if (empty($pCs)) {
			return false;
		}
		$whichOne = $this -> db -> amPlayer;
		$you = $pCs[ $whichOne ];
		$currentPos = $you -> getPos() -> fine;
		$worldClick = $this -> screenToWorldCoords( intval( $_GET['x'] ), intval( $_GET['y'] ), $you -> camPos -> fine);
		$heading = $you -> takeInput( $worldClick );
		$currentTile = $this -> db -> tileDefs[ $tB[ $currentPos -> x ][ $currentPos -> y ] ];
		if ( !$currentTile['flags']['standon'] ) {
			$you -> falling = true;
			$heading = ['x' => 0, 'y' => 1];
		}
		$targetTile = $this -> db -> tileDefs[ $tB[ $currentPos -> x + $heading['x'] ][ $currentPos -> y + $heading['y'] ] ];
		if (!$targetTile['flags']['gothru']) {
			// blocked
		} else {
			$newPos = new Pos( ($currentPos -> x + $heading['x']), ($currentPos -> y  + $heading['y']), ($you -> pos -> coarse -> x ), ($you -> pos -> coarse -> y ) );
			$you -> moveTo( $newPos );
		}
}


		function moveMobs() {

		}
		static function screenToWorldCoords( $screenX, $screenY, $camPos) {
			$worldX = intval( $camPos['x']) + intval( $screenX );
			$worldY = intval( $camPos['y'] ) + intval( $screenY );
			$coords = array ('x' => $worldX, 'y' => $worldY);
			return $coords;
		}

		static function worldToScreenCoords( $worldX, $worldY, $camPos) {
			$screenX = intval( $worldX ) - $camPos -> x;
			$screenY = intval( $worldY ) - $camPos -> y;
			$coords = array ('x' => $screenX, 'y' => $screenY);
			return $coords;
		}

	}
?>
