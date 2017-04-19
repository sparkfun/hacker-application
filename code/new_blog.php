<?php 

session_start();

//if user is logged in
if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id' && isset($_SESSION['admin']) || isset($_SESSION['blogger'])) {
	
	//set user_id
	$user_id = intval($_SESSION['user_id']);
	
	include('includes/user.header.php');
	
	$db = new DBConnection;
	$blog = new Blog($db);
	$user = new User($db);
	$category = new Category($db);

	$userData = $user->getUserInfo($user_id);	
	//create piece name together
	$firstName = $userData['first_name'];
	$lastName = $userData['last_name'];
	$author = $firstName. " " .$lastName;
	
	include_once('includes/user.header.php');
		
?>
	
	<?php include('includes/admin.nav.php'); ?>
	<div class='edit-profile-form'>
		<form name="addBlog" action="<?php echo $_SERVER['PHP_SELF']; ?>" method="POST" enctype="multipart/form-data">
			<div class='edit-profile-field'>
				<label class="add_label">Blog Title: </label>
				<input type="text" class="add_form" name="blogTitle" maxlength="50" placeholder="Blog Title" />
			</div>
			
			<div class='edit-profile-field'>
				<label class="add_label">Blog Category: </label>
				<select name="category">
					<?php
					
						//get all the categories from the database and display them
						$catArray = $category->get_allCats();
						foreach($catArray as $key => $val) {
							echo "<option value='" .$key. "'>" .$val. "</option>";
						}
					
					?>
				</select>
			</div>
			
			<div class='edit-profile-field'>
				<label class="add_label">Upload Photo: </label>
				<input type="file" class="add_form" name="photo" id="image_file" onchange="fileSelected();" /><br />
			</div>
			
			<div class='edit-profile-field'>
				<label class="add_label">Blog Content: </label>
				<textarea class="add_txt" class="updateContent" id="format" name="updateContent" placeholder="Write blog post here..."></textarea>
			</div>
			
			<div class='edit-profile-field'>
				<label class="add_label">Keywords: </label>
				<input type="text" id="keywords" class="add_form" name="blogKeys" maxlength="160" placeholder="Separate keywords by commas" /><span class='tip'>Separate Keywords By A Comma</span><br />
			</div>
			
			<div class='edit-profile-field'>
				<input type="submit" name="submitUpdate" class="btn btn-primary" value="Add Blog" onclick="startUploading()" />
			</div>
		</form>
	</div>

<?php

	//if the form is submitted
	if(isset($_POST['submitUpdate'])) {
		
		$addBlog = $blog->insert_blog($user_id);
		
		if($addBlog === FALSE) {
			echo "<span class='error'>Blog posting failed.</span>";
		} else {
			echo "<span class='success'>Blog successfully posted!</span>";
		}
			
	}


} else {
	header("Location: index.php");
}//close if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'user_id')

include('includes/footer.php');

?>