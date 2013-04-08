<html>
<title>Beer Calc Test</title>

	<?php
	include 'Beer.php';
	
	echo "Beer Class Test <br>";
	echo "<hr>";
	echo "a Lager<br>";
	$lager = new StandardAmericanLager( "ASMZlgr", "0.5", "0.4" );
	$lager->getName();
	$lager->getStyle();
	$lager->getABV();
	$lager->getIBU();
	echo "<hr>";
	
	echo "an IPA<br>";
	$ipa = new ImperialIPA( "ASMZipa", "0.75", "0.6" );
	$ipa->getName();
	$ipa->getStyle();
	$ipa->getABV();
	$ipa->getIBU();
	echo "<hr>";
	
	echo "a Stout<br>";
	$stout = new RussianImperialStout( "ASMZstout", "0.8", "0.9" );
	$stout->getName();
	$stout->getStyle();
	$stout->getABV();
	$stout->getIBU();
?>
</html>
