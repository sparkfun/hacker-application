<?php
session_start();
require 'includes/lib.php';
$artist = new artist;
$result = $artist->find(9999);
?> 

<!DOCTYPE html>
<html>
	<head>
		<title>Artists | Exigent Records</title>
		<?php require 'includes/head.html'; ?>
		<script>
			$(function() {
				$('.toggle').click(function() {
					$(this).parent().siblings('#img').slideToggle(300);
					$(this).parent().siblings('#about').toggle(300);
				});
			});
		</script>
	</head>
	<header>
		<div class='menu'>
			<?php require 'includes/menu.php'; ?>
		</div>
		<div class='clear'></div>
	</header>
	<body>
		<div id='wrapper'>
			<h2>Exigent Artists<?php if($_SESSION['admin']){ echo ' | <a href="add_artist.php">Add an Artist &rarr;</a>';}?></h2>

			<?php while($row = mysql_fetch_array($result)) { ?>

			<div class='artist'>
				<p id='img'><img src='images/<?=str_replace( ' ', '_', strtolower( $row['name'] ) )?>.jpg' width='300' height='200' style='box-shadow: 5px 5px 15px #000000;'></p>
				<h3><?=$row['name']?><?php if($_SESSION['admin']){ echo ' | <a href="edit_artist.php?id=' . $row['id'] . '">Edit</a>';}?></h3>
				<p class='post' id='about' style='display: none;'><?=stripslashes($row['about'])?></p>
				<p>
					<span class='toggle'>About</span> | 
					<a href="shows.php?artist=<?=$row['id']?>">Shows</a> | 
					<a href="<?=$row['music_url']?>">Music</a> | 
					<a href="<?=$row['store_url']?>">Store</a> | 
					<a href="<?=$row['artist_url']?>">Website</a>
				</p>
			</div>

			<?php } ?>

			<div class='clear'></div>
		</div>
	</body>
	<footer>
		<?php require 'includes/footer.php'; ?>
	</footer>
</html>