<?php 

session_start();

//if user is logged in
if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id' && isset($_SESSION['admin'])) {
	
	//set user_id
	$user_id = $_SESSION['user_id'];
	
	include('includes/user.header.php');
		
?>

<div class="container" style="margin-top: 100px;">
	
	<?php 
	
		include('includes/admin.nav.php'); 
	
			//if the form is submitted
		if(isset($_POST['addCategory'])) {
			$catName = $link->real_escape_string($_POST['catName']);
		
			if(empty($catName)) {
				echo "You forgot to add the category name...";
			} else {
			
				$catSQL = $link->query("INSERT INTO categories (cat_name) VALUES ('$catName')");
			
				if($catSQL === FALSE) {
					echo "<h3>Add category failed. Please try again.</h3>";
				} else {
					echo "<h3>Category added!</h3>";
				}		
			}
		}
	?>
	
	
	<form name="addCategory" action="<?php echo $_SERVER['PHP_SELF']; ?>" method="POST">
		<div>
			<label class="add_label">Add Category: </label>
			<input type="text" class="add_form" name="catName" maxlength="50" placeholder="Category Name" />
		</div>
		
		<div>
			<input type="submit" name="addCategory" class="btn btn-primary" value="Add Category" />
		</div>
	</form>
	
	<h3 style="margin-top: 20px;">Current Categories</h3>
	<table border="0">
		<?php
			
			$currentSQL = $link->query("SELECT * FROM categories ORDER BY cat_id");
			
			if($currentSQL === FALSE) {
				echo "Could not retrieve current categories. Please contact your network administrator (Ty) to fix this error.";
			}
			
			while($currentRow = mysqli_fetch_assoc($currentSQL)) {
				$catID = $link->real_escape_string(intval($currentRow['cat_id']));
				$cat_name = $link->real_escape_string($currentRow['cat_name']);
				
				echo "<tr><td class='user_info' style='width: 100px; text-align: right;'>".$cat_name."</td> <td class='user_actions' style='width: 150px; text-align: right;'><a href='delete_cat.php?cat_id=".$catID."'>Delete Category</a></td></tr>";
			}
		
		?>
	</table>
	
</div>

<?php

} else {
	header("Location: index.php");
}//close if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'user_id')

include('includes/footer.php');

?>