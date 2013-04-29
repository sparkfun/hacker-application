<?php

class artist
{
	function find_where($column, $operator, $operand, $limit)
	{
		require 'includes/config.php';
		$con = mysql_connect($hostname,$username,$password);
		mysql_select_db($database, $con);
		$result = mysql_query("SELECT * FROM artists WHERE $column $operator $operand ORDER BY name ASC LIMIT $limit");
		mysql_close($con);
		return $result;
	}

	function find($limit)
	{
		require 'includes/config.php';
		$con = mysql_connect($hostname,$username,$password);
		mysql_select_db($database, $con);
		$result = mysql_query("SELECT * FROM artists ORDER BY name ASC LIMIT $limit");
		mysql_close($con);
		return $result;
	}

	function insert($name, $about, $music_url, $store_url, $artist_url, $image)
	{
		mysql_real_escape_string($name);
		mysql_real_escape_string($about);
		mysql_real_escape_string($music_url);
		mysql_real_escape_string($store_url);
		mysql_real_escape_string($artist_url);
		require 'includes/config.php';
		$con = mysql_connect($hostname,$username,$password);
		mysql_select_db($database, $con);
		mysql_query("INSERT INTO artists (name, about, music_url, store_url, artist_url) VALUES ('" . str_replace( "'", '', $name ) . "', '" . str_replace( $nicedit_find, $nicedit_replace, $about ) . "', '$music_url', '$store_url', '$artist_url')");
		mysql_close($con);

		if ( $image['type'] == 'image/jpeg' )
		{
			//use artist name for image filename
			move_uploaded_file($_FILES["image"]["tmp_name"], "images/" . str_replace( "'", '', str_replace( ' ', '_', strtolower( $name ) ) ) . '.jpg' );
		}
	}

	function update($about, $music_url, $store_url, $artist_url, $id)
	{
		mysql_real_escape_string($about);
		mysql_real_escape_string($music_url);
		mysql_real_escape_string($store_url);
		mysql_real_escape_string($artist_url);
		mysql_real_escape_string($id);
		require 'includes/config.php';
		$con = mysql_connect($hostname,$username,$password);
		mysql_select_db($database, $con);
		mysql_query("UPDATE artists SET about='" . str_replace( $nicedit_find, $nicedit_replace, $about ) . "', music_url='$music_url', store_url='$store_url', artist_url='$artist_url' WHERE id = $id");
		mysql_close($con);
	}

	function delete($id, $name)
	{
		require 'includes/config.php';
		$con = mysql_connect($hostname,$username,$password);
		mysql_select_db($database, $con);
		mysql_query("DELETE FROM artists WHERE id = $id LIMIT 1");
		mysql_close($con);
		unlink('images/' . str_replace( "'", '', str_replace( ' ', '_', strtolower( $name ) ) ) . '.jpg');
	}
}

?>