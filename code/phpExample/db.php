<?php
class Db {
    private $psql;

    function __construct() {
        $this->psql = pg_connect("host=localhost port=5432 dbname=phpweatherapp");
    }

    public function getDbConnection() {
        return $this->psql;
    }
}

class LocationsDb extends Db {

    function __construct() {
        parent::__construct();
    }

    public function create($city, $state, $lat, $lng) {
        $escapedCity = pg_escape_string($city);
        $escapedState = pg_escape_string($state);
        $escapedLat = pg_escape_string($lat);
        $escapedLng = pg_escape_string($lng);
        $query = "INSERT INTO LOCATIONS(city, state, lat, lng) VALUES('" . $escapedCity . "', '" . $escapedState . "', '" . $escapedLat . "', '" . $escapedLng . "') RETURNING id";
        $result = pg_query($this->getDbConnection(), $query);

        return $result;
    }

    public function findById($id) {
        $query = "SELECT * FROM LOCATIONS WHERE ID = '" . $id . "'";
        $result = pg_query($this->getDbConnection(), $query);

        return $result;
    }

    public function findAll() {
        $query = "SELECT * FROM LOCATIONS";
        $result = pg_query($this->getDbConnection(), $query);

        return $result;
    }

    public function delete($id) {
        if(!empty($id)) {
            $result = pg_delete($this->getDbConnection(), 'locations', array('id'=>$id));
            return $results;
        }
    }
}
?>