<?php

class GUI {
	public $db;
	private $inputString;
	public $screen = "new_player";
	protected $guiTiles = array(
		'1' => "top-left-corner",
		'2' => "top-right-corner",
		'3' => "bottom-right-corner",
		'4' => "bottom-left-corner",
		'5' => "top-border",
		'6' => "right-border",
		'7' => "bottom-border",
		'8' => 'left-border',
		"-" => 'modal-bg',
		"_" => 'underscore',
		"(" => 'button-left',
		")" => 'button-right',
		"@" => 'name-label',
		"π" => 'pass-label',
		"<" => 'delete'
	);

	protected $screens = array(
		"new_player" => array(
		 	'tiles' => array( // I know
				'1555555555555552',
				'8--New Player--6',
				'8--------------6',
				'8@-.........(#)6',
				'8-----------(x)6',
				'8--QWERTYUIOP--6',
				'8--ASDFGHJKL---6',
				'8--ZXCVBNM-_-<-6',
				'4777777777777773'
			),
			'buttons' => array(   // this is not at all the correct way to design an input form gui
				'ok'=> array( '14|3', '13|3', '12|3'),   // but it works and I can finish it today.
				'cancel'=> array( '15|4', '14|4', '13|4' ),
				'Q'=>['3|5'],'W'=>['4|5'],'E'=>['5|5'],'R'=>['6|5'],'T'=>['7|5'],'Y'=>['8|5'],'U'=>['9|5'],'I'=>['10|5'],'O'=>['11|5'],'P'=>['12|5'],
				'A'=>['3|6'],'S'=>['4|6'],'D'=>['5|6'],'F'=>['6|6'],'G'=>['7|6'],'H'=>['8|6'],'J'=>['9|6'],'K'=>['10|6'],'L'=>['11|6'],
				'Z'=>['3|7'],'X'=>['4|7'],'C'=>['5|7'],'V'=>['6|7'],'B'=>['7|7'],'N'=>['8|7'],'M'=>['9|7'],'_'=>['11|7'],
				'delete'=>['13|7']
			),
		),
		"restore" => array(
		 	'tiles' => array(
				'1555555555555552',
				'8-[Restore...]-6',
				'8--------------6',
				'8@-.........(#)6',
				'8π-ºººººººº-(x)6',
				'8--QWERTYUIOP--6',
				'8--ASDFGHJKL---6',
				'8--ZXCVBNM-_-<-6',
				'4777777777777773'  // I know
			),
			'buttons' => array(   // this is not at all the correct way to design an input form gui
				'ok'=> array( '14|3', '13|3', '12|3' ),   // but it works and I can finish it today.
				'cancel'=> array( '15|4', '14|4', '13|4' ),
				'Q'=>['3|5'],'W'=>['4|5'],'E'=>['5|5'],'R'=>['6|5'],'T'=>['7|5'],'Y'=>['8|5'],'U'=>['9|5'],'I'=>['10|5'],'O'=>['11|5'],'P'=>['12|5'],
				'A'=>['3|6'],'S'=>['4|6'],'D'=>['5|6'],'F'=>['6|6'],'G'=>['7|6'],'H'=>['8|6'],'J'=>['9|6'],'K'=>['10|6'],'L'=>['11|6'],
				'Z'=>['3|7'],'X'=>['4|7'],'C'=>['5|7'],'V'=>['6|7'],'B'=>['7|7'],'N'=>['8|7'],'M'=>['9|7'],'_'=>['11|7'],
				'delete'=>['13|7']
			),
			'fields' => array(
				'name' => array( '4|3', '5|3', '6|4', '7|4', '8|4', '9|4', '10|4', '11|4' ),
				'pass' => array( '5|4', '5|5', '5|6', '5|7', '5|8', '5|9', '5|10', '5|11' )
			)
		),
	);

	public function __construct( $db ) {
		$this -> db = $db;
		$inputCoords = $_GET['x'] . '|' . $_GET['y'];
		$scr = $this -> screens[ $this -> screen ];
		foreach( $scr['buttons'] as $name => $tiles) {
			foreach( $tiles as $tile ) {
				if ( $inputCoords == $tile ) {
					$this -> pressedButton( $name );
				}
			}
		}
	}
	public function outputTiles() {
		$scr = $this -> screens[ $this -> screen ];
		for ($y = 0; $y < 9; $y++) {
			$xStr = $scr['tiles'][$y];
			for ($x = 0; $x < 16; $x++) {
				$tileChar = $xStr[$x];
				if (isset ($this -> guiTiles[ $tileChar ])) {
					$bgName = $this -> guiTiles[ $tileChar ];
					$fgStr = '';
				} else {
					switch ($tileChar) {
						case '#' : $fgName = '&#10003;'; break; // unicode check
						case 'x' : $fgName = '&#8856;'; break; // unicode cancel
						case '.' : // text area
						$iS = $this -> inputString;
						$fgName = substr($iS, $x-3, 1); break;
						default: $fgName = $tileChar;
					}
					$bgName = 'empty';
					$xyStr = $x . '|' . $y;
					foreach ($scr['buttons'] as $buttonName => $buttonCoords ) {
						if ($buttonName == 'ok' || $buttonName == 'cancel') {
							foreach ($buttonCoords as $xy) {
								if ($xyStr == $xy) {
									$bgName = "button-middle";
								}
							}
						}
					}
					$fgStr = '<g><text x="10" y = "22" font-family="Consolas, Menlo, monospace" font-weight="bold" font-size="22">'. $fgName. '</text></g>';
				}
				$bgPath = 'assets/tiles/gui/'.$bgName.'.svg';
				$bgStr = file_get_contents( $bgPath );
				$bgStr = '<svg' . explode( '<svg', $bgStr )[1];
				$svgStr = '<?xml version="1.0" encoding="UTF-8" standalone="no"?><svg width="32" height="32" x="0" y="0" xmlns="http://www.w3.org/2000/svg" version="1.1" xmlns:xlink="http://www.w3.org/1999/xlink">'. $bgStr . $fgStr . '</svg>';
				$nameStr = "view/" . (string)$x . "_" . (string)$y . ".svg";
				file_put_contents( $nameStr, $svgStr );
			}
		}
	}

	protected function pressedButton( $whichButton ) {
		switch ( $whichButton ) {
			case "ok":
				switch ( $this -> screen ) {
					case 'new_player':
						$this -> db -> registerPlayer( $this -> inputString );
					break;
				}
				break;
			case "cancel":
				echo "[X]";
				break;
			case "delete":
				$url = $this-> db -> dbURL . "gui.json";
				$old = $this-> db -> curlGet( $url );
				$oldObj = json_decode( $old );
				$oldString = $oldObj -> inputString; // get current input string
				$sl = strlen($oldString) - 1;
				$newString = substr( $oldString, 0, $sl ); // shorten it by 1
				if (strlen ( $newString ) < 1) {
					$newString = " "; // because firebase doesn't like empty strings
				}
				$data = array('inputString' => $newString);
				$dataStr = json_encode($data);
				$this-> db -> curlPut( $url, $dataStr ); // put back the shorter string
				$this -> inputString = $newString;
				break;
			default : // any other button is a keyboard key
				$url = $this-> db -> dbURL . "gui.json";
				$old = $this-> db -> curlGet( $url );
				$oldObj = json_decode( $old );
				$newString = $oldObj -> inputString;
				if (strlen ($newString) < 8) { // limit length of input
					$newString .= $whichButton; // append pressed key to saved input string
				}
				$data = array('inputString' => $newString);
				$dataStr = json_encode($data);
				$this-> db -> curlPut( $url, $dataStr ); // return string to firebase
				$this -> inputString = $newString;
				break;
		}
	}
}
?>
