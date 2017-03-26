<?php
class Location {
    public $city;
    public $state;
    public $lat;
    public $lng;

    function __construct($city, $state, $lat, $lng) {
        $this->city = $city;
        $this->state = $state;
        $this->lat = $lat;
        $this->lng - $lng;
        require_once 'db.php';
        $instance = new LocationsDb();
        $response = $instance->create($city, $state, $lat, $lng);
    }

}

class Conditions {
    public $conditions;

    function __construct($conditions) {
        $this->conditons = $conditions;
    }
}
?>