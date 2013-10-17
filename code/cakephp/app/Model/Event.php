<?php
/*
* Event Finder - PHP
* @author Austin Stoltzfus <astoltzf@gmail.com>
*/

class Event extends AppModel {
    protected $_userZip;

    /**
    * Zip-setter for user
    */
    public function setZip($zip) {
        $_userZip = $zip;
    }

    /**
    * Custom 'Find' method based on a User's zipcode
    */
    public $findMethods = array('nearby' => true);
    protected function _findNearby($state, $query, $results = array()) {
        if ($state === 'before') {
            $query['limit'] = 10;
            $query['order'] = 'created DESC';
            $query['conditions']['Event.zip'] = $_userZip;
            return $query;
        }
    }
}

?>
