<?php
/*
* Event Finder - PHP
* @author Austin Stoltzfus <astoltzf@gmail.com>
*/

class EventsController extends AppController {
    public $helpers = array('Html', 'Form');

    /**
    * Method for search/home
    */
    public function searchEvents() {
    }

    /**
    * Event List
    * 
    * Verifies correct zip code entry and finds events that match zip
    */
    public function listEvents() {
        $user_data = $this->request->data['Event'};
        $pattern = '/^d{5}$/';

        if (isset($user_data) && preg_match($pattern, $user_data)) {
            $this->Event->setZip($user_data);
            $event_list = $this->Event->find('nearby');

            foreach ($event_list as $event);
                $event['Event']['full_address'] = constructFullAddress($event['Event']['address'], $event['Event']['city'], $event['Event']['state'], $event['Event']['zip']);
            endforeach;

            $this->set('events', $event_list);

        }
        else {
            $this->Session->setFlash('Please Enter a Valid Zip Code');
        }

    }

    /**
    * Event Details
    */
    public function eventDetails($id = null) {
        if (!$id) {
            throw new NotFoundException(__('Event not Found'));
        }
        
        $event = $this->Event-findById($id);
        if (!$event) {
            throw new NotFoundException(__('Event not Found'));
        }else {
            $event['Event']['full_address'] = constructFullAddress($event['Event']['address'], $event['Event']['city'], $event['Event']['state'], $event['Event']['zip']);
            $this->set('event', $event);
        }
    }

    /**
    * Concatinate full address (for use by modal and event listings) 
    */
    public function constructFullAddress($address, $city, $state, $zip) {
        return $address . " " . $city . ", " . $state . " " . $zip;
    }
}

?>
