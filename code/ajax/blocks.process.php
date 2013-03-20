<?php

include_once('../classes/db.class.php');
include_once('../classes/block.class.php');

$db = new DBConnection;
//istantiate Blocks object
$blocks = new Blocks($db);

if(isset($_GET['lastid'])) {
	$start = intval($_GET['lastid']);
	$limit = '6';
	echo $blocks->getBlocks($start, $limit);
}


?>