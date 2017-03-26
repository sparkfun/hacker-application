<?php
/*-------------------------------------- 
This algorithm calculates the total a dog 
daycare business charges a customer.

The business offers half-day rates, overnight 
rates, and discounts for multiple dogs.

Inputs: Check-in date/time, Check-out date/time, # of dogs
Example: daycareCalculator("01/02/2017 07:45am","01/03/2017 11:45am",2)

Ezra Huscher
November 2016
-----------------------------------------*/
function daycareCalculator($checkedin, $checkedout, $numDogs) {
	$totalTime =  $checkedout - $checkedin;
	$exactDaysPassed = $totalTime/(60*60*24);
	$exactHoursPassed = $exactDaysPassed * 24;
	
	$start = date_create(date("Y-m-d",$checkedin));
	$end = date_create(date("Y-m-d",$checkedout));
	$interval = date_diff($start,$end);
	$days = $interval->format('%a');
	$wholeDaysPassed = $days - 1;

	$total=0;
	// Half day (four hours or less)
	if ($exactHoursPassed < 4.2) { $total = 7.5; $total += (($numDogs-1) * 5.50); }
	// Full day but not overnight
	else if (($exactHoursPassed >= 4.2 AND $exactHoursPassed < 12)) { $total = 15; $total += (($numDogs-1) * 11); }
	// Overnight
	else if ($exactHoursPassed >= 12) {
		// Count whole 24 hour periods
		$total = ($wholeDaysPassed) * 23;
		$total += (($numDogs-1) * ($wholeDaysPassed) * 17);
		// Now add drop-off day amount
		     if (date("H",$checkedin) < 18)  { $total += 23; $total += (($numDogs-1) * 17); } 
		else if (date("H",$checkedin) >= 18) { $total += 15.50; $total += (($numDogs-1) * 13.5); }
		// Now add pick-up day amount
		     if (date("H",$checkedout) < 8.2) $nocharge="yes"; 
		else if (date("H",$checkedout) < 11)  { $total += 7.5; $total += (($numDogs-1) * 5.5); } 
		else if (date("H",$checkedout) >= 11) { $total += 15; $total += (($numDogs-1) * 11); }
	}
	return $total;
}
?>
