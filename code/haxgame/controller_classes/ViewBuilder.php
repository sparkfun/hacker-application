<?php

class ViewBuilder { // updates view based on updated model
	private $db;
	protected $camPos;
	function __construct( $db ) {
		$this -> db = $db;
		$pChar = $db -> playerChars[ $db -> amPlayer ];
		$this -> camPos = $pChar -> camPos -> fine;
	}

	function outputTiles() {
		$tB = $this -> db -> tileBuffer;
		$tD = $this-> db -> tileDefs;
		$pcs = $this -> db -> playerChars;

		if (isset( $pcs[ 'player1' ] ) ) {
			$p1Pos = $pcs[ 'player1' ] -> getPos() -> fine;
			$p1Sprite = $pcs[ 'player1' ] -> sprite;
		}

		if (isset($pcs['player2'])) {
			$p2Pos = $pcs['player2'] -> getPos() -> fine;
			$p2Sprite = $pcs['player2'] -> sprite;
		}
		for ($y = 0; $y < 9; $y++) {
			for ($x = 0; $x < 16; $x++) {
				$mapXY = MCP::screenToWorldCoords($x,$y, $this -> camPos);

				$tileX = $mapXY['x'];
				$tileY = $mapXY['y'];

				$tileCode =	$tB[ $tileX ][ $tileY ];
				$bgName = $tD[ $tileCode ][ 'src' ][0];          // TODO: replace [0] with loop through src array
				$bgPath = 'assets/tiles/'.$bgName.'.svg';
				$bgStr = file_get_contents( $bgPath );
				$bgStr = '<svg' . explode( '<svg', $bgStr )[1];
				$fgName = '';
				$fgStr = '';

				if (isset ($p1Pos) ) {
					if ($tileX == $p1Pos['x'] && $tileY == $p1Pos['y']) {
						$fgStr = $p1Sprite ;
						echo ($fgStr);
					}
				}
				if (isset ($p2Pos) ) {
					if ($x == $p2Pos -> x && $y == $p2Pos -> y) {
						$fgStr = $p2Sprite ;
					}
				}

				$aE = $this -> db -> activeEntities;
				$caE = count($aE);
				for ( $e = 0; $e < $caE; $e++ ) {
					$entity = $aE[ $e ];
					$swc = MCP::screenToWorldCoords( $x, $y, $this -> camPos );
					foreach ( $entity as $it ) {
						//echo "(". $value['x'] . "," . $value['y'] . ")" ;
						//echo "{". $swc['x'] . "," . $swc[ 'y' ] . "}";
						if ( $value[ 'x' ] == $swc[ 'x' ] && $value[ 'y' ] == $swc[ 'y' ] ) {
							$name = explode( '_', $key )[0];
							$fgName = $GLOBALS[ 'entityDefs' ][ $name ][ 'sprite' ];
							break;
						}
					}
				}
				if ($fgName != '') {
					$fgPath = 'assets/sprites/'.$fgName.'.svg';
					$fgStr = file_get_contents( $fgPath );
					$fgStr = '<svg' . explode( '<svg', $fgStr )[1];
				}
				$svgStr = '<?xml version="1.0" encoding="UTF-8" standalone="no"?><svg width="32" height="32" x="0" y="0" xmlns="http://www.w3.org/2000/svg" version="1.1" xmlns:xlink="http://www.w3.org/1999/xlink">'. $bgStr . $fgStr . '</svg>';
				$nameStr = "view/" . (string)$x . "_" . (string)$y . ".svg";
				file_put_contents( $nameStr, $svgStr );
			}
		}
	}
}

?>
