#!/usr/bin/php -e
<?php

//print_r( $argv );
//echo $argc, "\n" ;

function printHelp() {
	echo 'Usage:', "\n";
	echo '	', basename( __FILE__ ), ' www.somedomain.com', "\n", "\n";
	echo 'multiple domains can be listed for multiple outputs:', "\n";
	echo '	', basename( __FILE__ ), ' www.somedomain.com www.otherdomain.com subdomain.yetanother.edu', "\n", "\n";
	echo 'parts are reversed for alphabetizing directories or files', "\n";
}

if ( $argc > 1 ) {
	$script = array_shift( $argv );
	foreach( $argv as $domain ) {
		$pieces = explode( '.', $domain );
		$reverse = array_reverse( $pieces );
		echo implode( '.', $reverse ), "\n";
	}
}
else {
	printHelp();
}
