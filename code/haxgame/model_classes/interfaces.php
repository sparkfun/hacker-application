<?php

interface Tangible {
	function getPos();
	function setPos( $newPos );
}

interface Mortal {
	function takeDamage( $amount );
	function expire();
}

interface Moving {
	function moveTo( $pos );
}

interface CanWalk {
	function walkTowards( $direction );
}

interface Pushable {
	function getPushed ( $pusher );
}

interface Playable {
	function takeInput( $worldClick );
}

interface Autonomous {
	function decideMove();
}

interface Deadly {
	function hurt( $target );
}

?>
