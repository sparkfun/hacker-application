<?php
// Todo: Move to a more secure location
$_ENV["GEOCODE_KEY"] = "AIzaSyBzaYojdccFaRHkouxZK8cYOijBMcYsi1E";
$_ENV["FORECAST_KEY"] = "963c2a286c46883b606d0962897eeef7";

require_once "db.php";
require_once "model.php";

class Controller {

    private $LocationsDb;
    public $conditions;

    function __construct() {
        $this->LocationsDb = new LocationsDb();
    }

    public function showLocations() {
        $result = $this->LocationsDb->findAll();
        $rows = pg_num_rows($result);
        
        return $result;
    }

    public function addLocation($data) {
        $data = str_replace (" ", "+", $data);
        $service_url = "https://maps.googleapis.com/maps/api/geocode/json?address=" . $data . "&key=" . $_ENV["GEOCODE_KEY"];
        $curl = curl_init($service_url);
        curl_setopt($curl, CURLOPT_RETURNTRANSFER, true);
        $curl_response = curl_exec($curl);
        if ($curl_response === false) {
            $info = curl_getinfo($curl);
            curl_close($curl);
            die("error occured during curl exec. Additioanl info: " . var_export($info));
        }
        curl_close($curl);
        $decoded = json_decode($curl_response);
        if (isset($decoded->response->status) && $decoded->response->status == "ERROR") {
            die("error occured: " . $decoded->response->errormessage);
        }
        $results = $decoded->results[0];
        $newLoaction = new Location($results->address_components[0]->long_name, $results->address_components[2]->short_name, $results->geometry->location->lat, $results->geometry->location->lng);
        return $this->setLocationConditions($results->address_components[0]->long_name, $results->address_components[2]->short_name, $results->geometry->location->lat, $results->geometry->location->lng);
    }

    public function setLocationConditions($city, $state, $lat, $lng) {
        $service_url = "https://api.darksky.net/forecast/" . $_ENV["FORECAST_KEY"] . "/" . $lat . "," . $lng;
        $curl = curl_init($service_url);
        curl_setopt($curl, CURLOPT_RETURNTRANSFER, true);
        $curl_response = curl_exec($curl);
        if ($curl_response === false) {
            $info = curl_getinfo($curl);
            curl_close($curl);
            die("error occured during curl exec. Additioanl info: " . var_export($info));
        }
        curl_close($curl);
        $decoded = json_decode($curl_response);
        if (isset($decoded->response->status) && $decoded->response->status == "ERROR") {
            die("error occured: " . $decoded->response->errormessage);
        }
        session_start();
        $_SESSION["conditions"] = array("conditions" => $decoded->currently, "city" => $city, "state" => $state);
        header("Location: index.php");
        exit;
    }

    public function findById($id) {
        $result = $this->LocationsDb->findById($id);
        while($data = pg_fetch_object($result)) {
            $lat = $data->lat; 
            $lng = $data->lng;
            $city = $data->city;
            $state = $data->state;
            $id = $data->id;
            return $this->setLocationConditions($city, $state, $lat, $lng);
        }

    } 

    public function deleteLocation($id) {
        $result = $this->LocationsDb->delete($id);
        return header("Location: index.php");
    }  
}
?>