<?php
$_PAGE['quiet'] = 0;
$_PAGE['require_login'] = 0;
$_PAGE['path_to_root'] = "../";
$_PAGE['title'] = "Documents";
$_PAGE['module']['development']=0;
include("../incs/header.php");
include("incs/common.inc");

$active = $_USER['active'];
$user_id = $_USER['user_id'];

$sql = "SELECT `report_th_bgcolor` FROM {$_CONFIG['module']['db']['prefix']}employees WHERE `id` = '{$_USER['user_id']}'";
$q = db_query($sql);
$th_bgcolor = db_fetch_array($q, 'NUM');

echo "<style type='text/css'>
	#versions_div{	white-space: nowrap !IMPORTANT; }
	#docs_table{ border:1px solid #DDD; margin-left:20px; font-size:80%;} 
	#docs_table td, #docs_table th{ border:1px solid #DDD; padding:2px; border-collapse:collapse; }
	#docs_table th{ background-color:{$th_bgcolor[0]} }
	#docs_table li{ margin-left:25px; }
</style>";

echo floating_div('versions_div',1);
echo "<script type=\"text/javascript\">
function getVersions(file,li_id)
	{
		send_request(encodeURI('docs_sub.php?file='+file), 'versions_div', null, li_id, null, null, null, 30);
	}";
if ($active)
	echo "
	function popUp(url)
	{
		id = new Date().getTime();
		window.open(url, id, 'toolbar=0,scrollbars=0,location=0,statusbar=0,menubar=0,resizable=0,width=575,height=265');
	}
	function deleteDoc(id, name)
	{
		if(confirm('OK to delete ' + name + '?')){
			getMySQLData('docs_sub.php?d='+id+'&type=single','feedback_msg');
			setTimeout('window.location.reload()',2500);
		}
	}";
echo "</script>";

// **** Create instructions for ease of use **** \\
echo floating_div('instructions',1);
echo "Please read the <a class='link' id='instructions_link' onClick=\"send_request('gen_instructions.php?url='+window.location.pathname,'instructions','Instructions',this.id,null,null,null,null)\">instructions</a> before using.<br /><br />";

echo "<div id=\"feedback_msg\"></div>";

//This block creates a lookup table for username versus user_id
$sql= "SELECT `user_id`,`username` FROM `{$_CONFIG['db']['prefix']}users`";
$q = db_query($sql);
while($user = db_fetch_array($q, 'ASSOC'))
	$user_table[$user['user_id']] = $user['username'];

$doc_type = null;
$sql = "SELECT * FROM {$_CONFIG['module']['db']['prefix']}docs ORDER BY 'doc_type','name'";
$docs = db_query($sql);
$sql = "SELECT `original`,COUNT(original) AS count FROM {$_CONFIG['module']['db']['prefix']}docs GROUP BY 'original'";
$list = db_query($sql);
while($item = db_fetch_array($list))
	$doc_count[$item['original']] = $item['count'];
	
	
//****************BUILD DOC LIST***************//
echo "<h3>Available Documents:</h3>";
echo "<table border='0' id='docs_table' cellspacing='0'>\n";
while($doc = db_fetch_array($docs, 'ASSOC'))
{
	$doc_name = basename($doc['path']);
	if($doc['doc_type'] != $doc_type){
/*		if($doc_type!=null)
			echo "</ul>";*/
		echo "<tr><th colspan='6'>{$doc['doc_type']}</th></tr>\n";
		$doc_type = $doc['doc_type'];
	}
	
	/*********************************************************************************************************
	 * Docs display only if no login required (!$doc['require_login']) OR the user is logged in ($active), 
	 * AND that the doc isn't a version (strstr($doc['path'],$doc['timestamp'])) [the timestamp will be 
	 * part of the path in a version]. 
	 *********************************************************************************************************/
	if((!$doc['require_login'] || $active) && !strstr($doc['path'],$doc['timestamp'])){
		echo "<tr id='doc_{$doc['id']}' name='{$doc_name}'><td><li>{$doc['name']}</li></td><td><span style='text-decoration:italic; color:#999;'>" . date($_USER['format_date'],$doc['timestamp']) . "</span></td><td>";
		if($doc_count[$doc_name] > 1)
			echo "<span class='menu' style='vertical-align:middle;'><a style='cursor:pointer' title='View previous versions of {$doc['name']}' onclick=\"getVersions('{$doc_name}','doc_{$doc['id']}')\">[versions]</a></span>";
		else echo "&nbsp;";
		echo "</td><td><a title='{$doc_name}' href=\"{$doc['path']}\"><img src=\"{$_THEME['icon']['download']}\" border=0/></a></td><td>";
		if(($user_id == $doc['owner']) || $_USER['priv']){
			echo "<a style='cursor:pointer' title='Delete {$doc_name}' ";
			if($doc_count[$doc_name] > 1)
				echo "onclick=\"popUp('docs_sub.php?d={$doc['id']}&mode=select_type')\"";
			else
				echo "onclick=\"deleteDoc({$doc['id']},'{$doc_name}')\"";
			echo "><img src='{$_THEME['icon']['trash']}' /></a>";
		}
		else echo "&nbsp;";
		echo "</td><td><span style='text-decoration:italic; color:#999;'>Owner: {$user_table[$doc['owner']]}</span></td></tr>\n";
	}
}
echo "</table>\n";
//****************END DOC LIST*****************//
if($active)
	echo "<br /><input type=\"button\" value=\"New Document...\" id=\"newDocButton\" onclick=\"popUp('docs_sub.php')\"/>";

include("../incs/footer.php");
?>