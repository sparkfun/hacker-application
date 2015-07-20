<!doctype HTML><html><head><meta charset="utf-8" /><title> </title>
</head><body>
<?php
require 'view_classes/gui.php';
require 'controller_classes/DB.php';
require 'controller_classes/MCP.php';
require 'model_classes/interfaces.php';
require 'model_classes/entities.php';
require 'model_classes/Player.php';
require 'controller_classes/ViewBuilder.php';

function start() {
	$db = new DB( "https://emt.firebaseIO.com/haxgame/" );
	$db -> checkLogin(); // first try to identify the player
	if ( $db -> showGUI ) {
		$gui = new GUI( $db );
		$gui -> outputTiles();
	}
	else {
		$db -> fetchData();
		$mcp = new MCP( $db );
		$mcp -> updateWorld();
		$vb = new ViewBuilder( $db );
		$vb -> outputTiles(); // after everything has moved to its new position, composite to SVG and overwrite previous copies on server.
	}
}

$debugOn = false; // $debugOn = false; // disables instant bounce back to referrer to display echo'd text
if ( !$debugOn ) {
	echo '<script> if ( document.referrer.length > 1 )';
	echo '{ window.location = ( document.referrer ); }';
	echo 'else { history.go( -1 ) }; </script>';
}
echo '</body>';
start();
?>
</html>
