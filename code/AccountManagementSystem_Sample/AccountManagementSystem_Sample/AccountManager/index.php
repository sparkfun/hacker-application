<!DOCTYPE html>
<html>
<body>
    <p>
        <?php echo 'Demonstration of PHP Fundamentals using a modular account management system'; ?>
    </p>
</body>


<?php
include ('AccountManager.php');

$accountMgr = new AccountManager();


?>
</html>