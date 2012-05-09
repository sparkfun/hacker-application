<?php
$_PAGE['data_only'] = 1;
$_PAGE['quiet'] = 1;
$_PAGE['require_login'] = 1;
$_PAGE['path_to_root'] = "../";
$_PAGE['title'] = "";
include("../incs/header.php");
include("incs/common.inc");

$tbl1 = "`{$_CONFIG['db']['prefix']}users`";
$tbl2 = "`{$_CONFIG['module']['db']['prefix']}employees`";

$to = $_GET['to'];
$search = trim($to);
// real names and user names are in separate tables so a left join is necessary to poll necessary information.
$sql = "SELECT $tbl1.user_id, $tbl1.username, CONCAT($tbl2.f_name, ' ', $tbl2.l_name) AS name FROM $tbl1 LEFT JOIN ($tbl2) ON ($tbl2.id = $tbl1.user_id) WHERE $tbl1.active = '1' AND ( $tbl1.username LIKE \"%$search%\" OR $tbl2.f_name LIKE \"%$search%\" OR $tbl2.l_name LIKE \"%$search%\" )";
$q = db_query($sql);
$count = 0; // this will give each recipient found a unique id.
if(db_num_rows($q) == 0)
	echo "No Results";
while($fetch = db_fetch_array($q, 'ASSOC')){

	//styled names have the string matching $search marked in bold.
	$style_username =  str_ireplace($search, "<span style=\"font-weight:bold;\">$search</span>", $fetch['username']);
	$style_name =  str_ireplace($search, "<span style=\"font-weight:bold;\">$search</span>", $fetch['name']);

	// create the link behavior.
	echo "<a onclick='set_recipient(\"{$fetch['username']}\")' id='recip_$count' user_id='".strtolower($fetch['username'])."'";
	if($count == 0)	// start selection at first found item.
		echo "style='background-color:#CCDDFF'";
	echo ">&quot;<span style='text-transform:capitalize;'> {$style_name} </span>&quot; &lt;".strtolower($style_username)."&gt;</a>\n";
	$count++; // next recipient.
}


?>