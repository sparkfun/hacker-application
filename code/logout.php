<?php

session_start();
session_unset();
session_destroy();

setcookie('username', sha1($username), time()-86400);
setcookie('session', $_REQUEST['PHPSESSID'], time()-86400);

header("Location: /index");

?>