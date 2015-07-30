<!-- Header -->
<?php include_once "common/base.php"; ?>
<?php include_once "common/header.php"; ?>

<!-- BEGIN Index -->
<div id="main" class="main-section">

   <noscript>This site just doesn't work, period, without JavaScript</noscript>

<!-- If Logged In -->
<?php
if(isset($_SESSION['LoggedIn']) && isset($_SESSION['Username'])):
    echo "<ul id='list' class='list'>";

    include_once 'inc/class.lists.inc.php';
    $lists = new ColoredListsItems($db);
    list($LID, $URL, $order) = $lists->loadListItemsByUser();

    echo "</ul>";
?>

            <br />

            <form action="db-interaction/lists.php" id="add-new" method="post" class="add-new-form">
                <input type="text" id="new-list-item-text" name="new-list-item-text" />

                <input type="hidden" id="current-list" name="current-list" value="<?php echo $LID; ?>" />
                <input type="hidden" id="new-list-item-position" name="new-list-item-position" value="<?php echo   $order; ?>" />

                <input type="submit" id="add-new-submit" value="add" class="button" />
            </form>

            <div class="clear"></div>

            <div id="share-area">
                <p>Public list URL: <a target="_blank" href="http://stinedec.com/tasks/<?php echo $URL ?>.html">http://stinedec.com/tasks/<?php echo $URL ?>.html</a>
                &nbsp; <small>(Nobody but YOU will be able to edit this list)</small></p>
            </div>

<?php
elseif(isset($_GET['list'])):
    echo "<ul id='list'>";

    include_once 'inc/class.lists.inc.php';
    $lists = new ColoredListsItems($db);
    list($LID, $URL) = $lists->loadListItemsByListId();

    echo "</ul>";
else:
?>
  	<h2 class="sales--header">Welcome to Tasks!</h2>
  	<p class="sales--subheader">Create an account to start tasking.</p>

<?php endif; ?>

</div>

<script type='text/javascript' src='https://ajax.googleapis.com/ajax/libs/jquery/2.1.3/jquery.min.js'></script>
<script type="text/javascript" src="js/jquery-ui.min.js"></script>
<script type="text/javascript" src="js/jquery.jeditable.mini.js"></script>
<script type="text/javascript" src="js/lists.js"></script>
<script type="text/javascript">
	document.addEventListener("DOMContentLoaded", function(event) { 
	    initialize();
	});
</script>

<?php include_once "common/sidebar.php"; ?>

<?php include_once "common/footer.php"; ?>