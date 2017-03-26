<?php
require 'controller.php';

$controller = new Controller();

if (isset($_POST['address'])) {
    return $controller->addLocation($_POST['address']);
}

if (isset($_GET['remove'], $_GET['item'])) {
    return $controller->deleteLocation($_GET['item']);
}

if (isset($_GET['findById'])) {
    return $controller->findById($_GET['findById']);
}

?>