<?php
session_start();
require 'includes/lib.php';

//check to see if logged in
if( !$_SESSION['admin'] )
{
	header( 'Location: index.php' );
};

$artist = new artist;
$result = $artist->find_where('id', '=', $_GET['id'], 1);
$artist = mysql_fetch_assoc($result);
?> 

<!DOCTYPE html>
<html>
	<head>
		<title>Edit Artist | Exigent Records</title>
		<?php require 'includes/head.html'; ?>
	</head>
	<header>
		<div class='menu'>
			<?php require 'includes/menu.php'; ?>
		</div>
		<div class='clear'></div>
	</header>

	<!-- confirm delete -->
	<div style="display:none">
		<div id="delete_fancybox">
			<h3>You want to delete this?</h3>
			<center><a href="submit.php?type=delete_artist&id=<?=$_GET['id']?>&name=<?=str_replace( ' ', '_', strtolower( $artist['name'] ) )?>"><button>Yes, delete it.</button></a></center>
		</div>
	</div>
	<body>
		<div id='wrapper'>
			<h2>Edit Artist | <a class="fancy_link" href="#delete_fancybox">Delete This Artist &rarr;</a></h2>
			<div class='item'>
				<form action='submit.php?type=update_artist&id=<?=$_GET['id']?>' method='post' enctype='multipart/form-data'>
					<h2><?=$artist['name']?></h2>
					<p><img src='images/<?=str_replace( ' ', '_', strtolower( $artist['name'] ) )?>.jpg' width='300' height='200'></p>
					<p>To replace the artist photo, simply place a new 300x200 JPG in the images folder and make sure it's named <?=str_replace( ' ', '_', strtolower( $artist['name'] ) )?>.jpg</p>
					<hr>
					<p>Music URL : <input type='text' size='45' name='music_url' placeholder='Artist Music URL' value='<?=$artist['music_url']?>' required></p>
					<p>Store URL : <input type='text' size='45' name='store_url' placeholder='Artist Store URL' value='<?=$artist['store_url']?>' required></p>
					<p>Website URL : <input type='text' size='45' name='artist_url' placeholder='Artist Website URL' value='<?=$artist['artist_url']?>' required></p>
					<p><textarea cols='100' rows='15' name='about' required><?=stripslashes($artist['about'])?></textarea></p>
					<input type='hidden' name='id' value='<?=$_GET['id']?>'>
					<p><input type='submit' value='SUBMIT'></p>
				</form>
			</div>
		</div>
	</body>
	<footer>
		<?php require 'includes/footer.php'; ?>
	</footer>
</html>