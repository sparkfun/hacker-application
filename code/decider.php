<!--
	Author: Mitchell Block
	Date last modified: March 26th, 2017
	Purpose: takes input from web decision maker, gives each option an equal number of random tickets,
			 then picks one of the options, lottery style
-->
<html>
<body>
<?php
	$options = array();
	if ($_SERVER["REQUEST_METHOD"] == "POST") {
		$options = sanitize($_POST["options"]);
	}

	$multiplier = rand(5,10);
	$maxValue = $multiplier * count($options);
	$randomizedOptions = array();
	foreach ($options as $indivOption){
		for ($i = 0; $i < $multiplier; $i++){ // assign "tickets" to each option
			$randomizedOptions[$indivOption][] = rand(1,$maxValue);
		}
	}

	$decidingValue = rand(1,$maxValue);
	$finalDecision = "";
	foreach ($randomizedOptions as $key => $values){
		if (in_array($decidingValue, $values)){
			$finalDecision = $key;
		}
	}
	//TO-DO: Make sure random value is one of the given values for the options
	//		 Find different way to present result
	//		 Return to original page

	echo "<script type='text/javascript' language='Javascript'>alert ('".$finalDecision."')</script>";



	function sanitize($data){
		if (is_array($data)){
			$data = array_map("trim", $data);
			$data = array_map("stripcslashes", $data);
			$data = array_map("htmlspecialchars", $data);
		}
		else{
			$data = trim($data);
			$data = stripcslashes($data);
			$data = htmlspecialchars($data);
		}
		return $data;
	}
?>
</body>
</html>