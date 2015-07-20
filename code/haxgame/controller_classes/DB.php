<?php

class DB { // Gets the info about the player and world from the database and stores it in arrays for MCP to read.
	public $dbURL;
	public $sideLength = 128;
	public $tileBuffer; // : string[];

	public $tileDefs = array(
			[ src => ['blue_sky'], flags => ['gothru'] ],
			[ src => ['grass_flat'], flags => ['gothru', 'standon'] ],
			[ src => ['solid_cliff'], flags => ['destructible'] ],
			[ src => ['hot_lava'], flags => ['gothru', 'deadly'] ]
		);

	public $entityDefs = array(
			[ src => ['blue_sky'], flags => ['gothru'] ],
			[ src => ['grass_flat'], flags => ['gothru', 'standon'] ],
			[ src => ['solid_cliff'], flags => ['destructible'] ],
			[ src => ['hot_lava'], flags => ['gothru', 'deadly'] ]
		);

	public $activeEntities; // : GameEntity[]
	public $playerChars;
	public $players;
	public $showGUI = false;
	public $canPlay = false;
	public $amPlayer;

	public function __construct( $dbUrl ) {	      //              //              //
		$this -> dbURL = $dbUrl;
	}                 //              //               //   __construct    //
	public function checkLogin() { // who am i??
		if (isset( $_COOKIE[ 'loginName' ] )) { // is there a login name in the cookie?
			echo "found saved login!"; //
		}
		else {                             // if not, just play as guest
		 echo " No login or saved game found. ";
		 if (isset( $_COOKIE[ 'anonUser' ] )) { // no 'else,' because it will be set next time.
			 echo ' Anonymous user cookie found. ';
			 $anonCookie = $_COOKIE[ 'anonUser' ];
			 $idCookie = $_COOKIE[ 'reqID' ]; // this is the unique id returned by Firebase last time

			 $getURL0 = $this -> dbURL . 'guests.json';
			 $gotJSON = $this -> curlGet( $getURL0 );
			 $gotGuestHits = json_decode( $gotJSON, true );

			 $recentGuestHits = [];

			 foreach ($gotGuestHits as $uid => $guest) {
				 $deltaT =  time() - explode( "|", $guest )[3];
				 if ($deltaT < 300) { // discard records older than 5 minutes
					 $recentGuestHits[$uid] = $guest;
				 }
			 }
			 ksort( $recentGuestHits );
			 $rg = json_encode( $recentGuestHits );
			 $this -> curlPut( $getURL0, $rg );

			 $ak = array_keys( $recentGuestHits );
			 $mostRecent = array_pop( $ak ); // this is to allow two guests to play
			 $twoHitsAgo = array_pop( $ak ); // alternating moves as p1 and p2

			 $getURL1 = $this -> dbURL . 'players.json';
			 $gotJSON1 = $this -> curlGet( $getURL );
			 $this -> players = json_decode( $gotJSON1, true );
			 $recentPlayers = [];
			 if (!empty($this -> players)) {
				 foreach ($this -> players as $key => $value) {
				 	$deltaT = time() - $value['lastSeen'];
				 	if ($deltaT < 60) {
					 	$recentPlayers[$key] = $value;
				 	}
				}
			 }
			 if (empty( $recentPlayers )) {
				 // Nobody has touched the game in at least the last 60 seconds. Safe to start a new game as guest.
				 $this -> initGuest( 0, 1); //  create 'GUEST^0', create 'player1' and put the former in control of the latter
			 }
		 }
		 $expiry = time() + 30; // set a temporary cookie. (This part of the function )
		 $val = $this -> makeCookie();
		 setcookie( 'anonUser', $val, $expiry );
		 $url = $this -> dbURL . 'guests.json';
		 $data = json_encode( $val );
		 $result = $this -> curlPost( $url, $data );
		 setcookie( 'reqID', $result[ 'name' ], $expiry );
	 }
}


protected function findIP() {
	if ( !empty($_SERVER[ 'HTTP_X_REAL_IP' ]) ) {
		$ip = $_SERVER[ 'HTTP_X_REAL_IP'];
	} else if ( !empty( $_SERVER[ 'HTTP_CLIENT_IP' ]) ) {
		$ip = $_SERVER[ 'HTTP_CLIENT_IP'];
	} else if ( !empty( $_SERVER[ 'HTTP_X_FORWARDED_FOR' ]) ) {
		$ip = $_SERVER[ 'HTTP_X_FORWARDED_FOR' ];
	} else { $ip = $_SERVER[ 'REMOTE_ADDR' ];
	}
	return $ip;
}

protected function makeCookie() {
	$ip = $this -> findIP();
	$ref = $_SERVER[ 'HTTP_REFERER' ];
	$ua = $_SERVER[ 'HTTP_USER_AGENT' ];
	return $ip.'|'.$ref."|".$ua . "|". time();
}

protected function initGuest( $whichGuest, $whichPchar ) {

	$guestNumber = $whichGuest == 1 ? 'GUEST^1' : 'GUEST^0';
	$playerNumber = $whichPchar == 2 ? 'player2' : 'player1';
	$ip = $this -> findIP();
	$newGuestPlayer = '{"lastSeen": '.time().', "ip": "'.$ip.'", "amPlayer" : "'.$playerNumber.'"}';
	$url = $this -> dbURL . 'players/' . $guestNumber . '.json';
	$this -> curlPut( $url, $newGuestPlayer );
	$newPlayer = new Player( $guestNumber );
	$newPlayer -> ip = $ip;
	$newPlayer -> lastSeen = time();
	$newPC = new PlayerChar( $newPlayer );
	$this -> players[ $guestNumber ] = $newPlayer;

	$this -> playerChars[ $playerNumber ] = $newPC;
	$this -> amPlayer = $playerNumber;
	$newPlayer -> playerChar = $newPC;
}

public function fetchData() {

		$url1 = $this -> dbURL . 'worldmap.json';
		$response1 = $this -> curlGet( $url1 );
		$r1 = json_decode( $response1, true );
		$tB = $r1[ 'chunks' ][ '0_0' ];  // reverse x and y coords

		$this -> sideLength = count( $tB );   // better than having to draw the (unicode) map at 90º
		$flipped = [];
		$side = $this-> sideLength;
		for ($g = 0; $g < $side; $g++) {
			$col = [];
			for ($h = 0; $h < $side; $h++) {
				array_push ($col, $tB[ $h ][ $g ]);
			}
			array_push ($flipped, $col);
		}
		$tB = $flipped;
		$this -> tileBuffer = $tB;
	}

	public function curlGet ($url) { // download json from database
		//echo "**GET**";
		$ch = curl_init();
		$cOptions = array(
		CURLOPT_URL => $url,
		CURLOPT_HTTPGET => true,
		CURLOPT_ENCODING => "gzip",
		CURLOPT_RETURNTRANSFER => true
		); curl_setopt_array($ch, $cOptions);
		 $output = curl_exec( $ch );
		 curl_close( $ch );
		 return $output;
	}

	public function curlPost ($url, $data_json) {
		//echo "**POST**";
	//	echo $url;
		$ch = curl_init();
		$cOptions = array(
		CURLOPT_URL => $url,
		CURLOPT_RETURNTRANSFER => true,
		CURLOPT_CUSTOMREQUEST => "POST",
		CURLOPT_POSTFIELDS => $data_json
		); curl_setopt_array($ch, $cOptions);
		$ce = curl_exec( $ch );
		curl_close( $ch );
		return json_decode($ce,true);
	}

	public function curlPut ($url, $data_json) {
	//	echo "**PUT**";
//		echo $url;
		$ch = curl_init();
		$cOptions = array(
		CURLOPT_URL => $url,
		CURLOPT_RETURNTRANSFER => true,
		CURLOPT_CUSTOMREQUEST => "PUT",
		CURLOPT_POSTFIELDS => $data_json
		); curl_setopt_array( $ch, $cOptions );
		$ce = curl_exec( $ch );
		curl_close( $ch );
		return json_decode( $ce, true );
	}

	public function curlPatch ($url, $data_json) {
		//echo "**PATCH**";
		$ch = curl_init();
		$cOptions = array(
		CURLOPT_URL => $url,
		CURLOPT_RETURNTRANSFER => true,
		CURLOPT_CUSTOMREQUEST => "PATCH",
		CURLOPT_POSTFIELDS => $data_json
		); curl_setopt_array($ch, $cOptions);
		curl_exec( $ch );
		curl_close( $ch );
	}
} ?>
