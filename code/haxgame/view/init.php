<?php

$svg = "";
$markdown = "";

for ($y = 0; $y < 9; $y++) {
	for ($x = 0; $x < 16; $x++) {

		$name = $x . '_' . $y . '.svg';
		$markdown .= '[![](view/' .$name. ')](game.php?x=' .$x. '&y=' .$y. ')';
	}
	$markdown .= '  <br />';
}
echo $markdown;
?>
