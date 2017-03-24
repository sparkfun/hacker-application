<!DOCTYPE html>
<html>
<body>
    <p>
        <?php echo 'Demonstration of PHP Fundamentals using a modular account management system'; ?>
    </p>
</body>


<?php
include ('AccountManager.php');

//This is an example location of where the user would get to interact with the data and any changes/requests they make would be run  
//through the AccountManager class for processing and verification.
$accountMgr = new AccountManager();


?>
</html>