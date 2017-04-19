<?php 

session_start();
//check if there is a cookie on users pc
if(isset($_COOKIE['username']) && isset($_COOKIE['logged'])) {
	
	$cookieUser = $_COOKIE['username'];
	$cookieSalt = $_COOKIE['logged'];
	
	include_once('includes/config.php');
	
	//match the cookie information with the database information
	$cookieSQL = $link->query("SELECT user_id, username, salt FROM users WHERE username = '".$cookieUser."' AND salt = '" .$cookieSalt. "'");
	
	if(mysqli_num_rows($cookieSQL) === 1) {
	
		$cookieRow = mysqli_fetch_assoc($cookieSQL);
		
		$user_id = intval($cookieRow['user_id']);
		$_SESSION['loggedin'] === TRUE;
		$_SESSION['user_id'] === $user_id;
	} else {
		echo "Invalid cookie information. Please login again.";
	}
	
}

//if user is logged in
if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id' && isset($_SESSION['admin'])) {
	
	//set user_id
	$user_id = $_SESSION['user_id'];
	
	include_once('includes/user.header.php');
		
?>

<div class="container" style="margin-top: 100px; min-height: 1200px;">
	
	<?php include('includes/admin.nav.php'); ?>
	
	<table border="0" class="user_table" width="100%">
		<?php
				
			/*=== prepare the user pagination ===*/
				
			// find out how many rows are in the table 
			$countSQL = "SELECT COUNT(*) FROM companies";
			$result = $link->query($countSQL);
			$r = mysqli_fetch_row($result);
			$numrows = $r[0];
				
			// number of rows to show per page
			$rowsperpage = 10;			
				
			// find out total pages
			$totalpages = ceil($numrows / $rowsperpage);

			// get the current page or set a default
			if (isset($_GET['page']) && is_numeric($_GET['page'])) {
				// cast var as int
				$currentpage = (int) $_GET['page'];
			} else {
				// default page num
				$currentpage = 1;
			} // end if

			// if current page is greater than total pages...
			if ($currentpage > $totalpages) {
				// set current page to last page
				$currentpage = $totalpages;
			} // end if
				
			// if current page is less than first page...
			if ($currentpage < 1) {
				// set current page to first page
				$currentpage = 1;
			} // end if

			// the offset of the list, based on current page 
			$offset = ($currentpage - 1) * $rowsperpage;
		
			//get the user information from database
			$userSQL = $link->query("SELECT * FROM companies ORDER BY id");	
			
			while($row = mysqli_fetch_assoc($userSQL)) {
				
				$userID = $link->real_escape_string(intval($row['id']));
				$companyName = $link->real_escape_string($row['company_name']);
				$companyIndustry = $link->real_escape_string($row['industry']);
				$companyLogo = $link->real_escape_string($row['logo_path']);
				$userName = $link->real_escape_string($row['username']);
				$userEmail = $link->real_escape_string($row['email']);
				
				
				echo "<tr><td class='userIMG'><img src='uploads/companies/logos/".$companyLogo."' width='100px'></td>";
				echo "<td class='user_info'><span class='title'>Name: </span>".$companyName."<br /><span class='title'>Industry: </span>".$companyIndustry."<br /><span class='title'>Username: </span>".$userName."<br /><span class='title'>Email: </span>".$userEmail."</td>";
				echo "<td class='user_actions'><a href='edit_user.php?user_id=".$userID."'>Edit Company</a><br /><br /><a href='delete_user.php?id=".$userID."'>Delete Company</a></td></tr>";
				
			}
		?>
	</table>
	<div class="pagination_container" style="margin-top: 15px;">
			<span class="pagination">
				<?php
				
				/*===  build the pagination links ===*/
				
				// range of num links to show
				$range = 5;

				// if not on page 1, don't show back links
				if ($currentpage > 1) {
					echo " <a href='{$_SERVER['PHP_SELF']}?page=1'><<</a> ";
					// get previous page num
					$prevpage = $currentpage - 1;
					echo " <a href='{$_SERVER['PHP_SELF']}?page=$prevpage'><</a> ";
				}

				// loop to show links to range of pages around current page
				for ($x = ($currentpage - $range); $x < (($currentpage + $range) + 1); $x++) {
					// if it's a valid page number...
					if (($x > 0) && ($x <= $totalpages)) {
						// if we're on current page...
						if ($x == $currentpage) {
							echo " [<b>$x</b>] ";
						// if not current page...
						} else {
							echo " <a href='{$_SERVER['PHP_SELF']}?page=$x'>$x</a> ";
						} 
					}
				} 
								 
				// if not on last page, show forward and last page links        
				if ($currentpage != $totalpages) {
					// get next page
					$nextpage = $currentpage + 1;
					echo " <a href='{$_SERVER['PHP_SELF']}?page=$nextpage'>></a> ";
					echo " <a href='{$_SERVER['PHP_SELF']}?page=$totalpages'>>></a> ";
				} 
				
				/*=== end build pagination links ===*/
				?>
			</span>
		</div>
	
</div><!-- close container -->

<?php

} else {
	header("Location: index.php");
}//close if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'user_id')

include('includes/footer.php');

?>