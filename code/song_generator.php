<?
	#brad mclaughlin
?>

<head>
	<title>Random song generator</title>
</head>
<style>
	body {
		font: 11pt/1.5 sans-serif;
		color: #333;
	}
	.line {
		width: 300px;
		padding: 9px;
		background: #ddd;
		clear: both;
		border-bottom: 1px solid #ccc;
		font-size: 125%;
	}
	.line span {
		width: 100px;
		float: left;
		font-weight: bold;
	}
</style>

<h1>Which key would you like your song to be in?</h1>
<h2>Select the key from the drop down list to begin</h2>
<form method="get" action="">
	<select name="key">
		<option value="a">A</option>
		<option value="b">B</option>
		<option value="c">C</option>
	<select>
	<button type="submit" value="submit">Make me a song!</button>
	<a href="song_generator.php">Reset</a>
</form>

<?php

	if ( isset($_REQUEST['key']) ) {
			$key = $_REQUEST['key'];
	
			# set arrays for keys (major only)
			$a = array('a ','b ','c# ','d ','e ','f# ','g# ');
			$b = array('b ','c# ','d# ','e ','f# ','g# ','a# ');
			$c = array('c ','d ','e ','f ','g ','a ','b ');
			
			# set length for chorus and verse
			$length = 8;
			$i 		= 1;
			
			
			while ( $i <= $length ) {
				#match the random digitals to corresponding notes in array
				$verse	.= strtoupper(${$key}[rand(1,8)]);
				$chorus	.= strtoupper(${$key}[rand(1,8)]);
				$i++;
			}
	
			echo "
				<div class='line'><span>VERSE:</span> $verse</div>
				<div class='line'><span>CHORUS:</span> $chorus</div>
				<div class='line'><span>VERSE:</span> $verse</div>
				";
	}				

?>



