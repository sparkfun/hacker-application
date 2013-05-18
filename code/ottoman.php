<?php
/**
 * CouchDB manipulation library
 * Written in 2011 
 **/

if(!function_exists("curl_init")) {
    die("Curl extension is required for Ottoman to load.");
}

class ottoman {
    /**
     * Store's the field's value. Set during construction.
     **/
    protected $baseURL;
    /**
     * Store's the field's value. Set during construction.
     **/
    protected $dbName;
    /**
     * Query base string (has trailing slash) Set during construction.
     **/
    protected $qBase;
    /**
     * Curl object. Set during construction.
     **/
    protected $couchCurl;
    /**
     * Used for internal debugging.
     **/
    private $debugTime;
    
    /**
     * Returns the current number of seconds since the object was instanciated.
     **/
    private function execTime() {
        return microtime(true) - $this->debugTime;
    }
    
    /**
     * Return server data for a custom GET from a specific URL relative to base
     * 
     * @param string $url URL to encode and put relative to base
     * @param array $args additional args (assoc array) to pass
     * @return string JSON respose
     **/
    public function getCustom($url, $args = null) {
        $query = $this->qBase . urlencode($url);
    
        if(is_array($args) && count($args) > 0) {
            $query .= '?';
            foreach($args as $k => $v) {
                $query .= urlencode($k) . '=' . urlencode($v);
            }
        }

        curl_setopt($this->couchCurl, CURLOPT_URL, $query);
        curl_setopt($this->couchCurl, CURLOPT_CUSTOMREQUEST, 'GET');
        curl_setopt($this->couchCurl, CURLOPT_POSTFIELDS, null);
        return curl_exec($this->couchCurl);
    }

    /**
     * Builds correct base URL and Attempts to connect to the CouchDB.
     * 
     * @param string $base absolute base URL to CouchDB Server (e.x. 'http://localhost:5984/')
     * @param string $db name of database this Ottoman object represents (e.x. 'songs')
     * @return null
     * @throws Exception when unable to connect to CouchDB
     **/
    function __construct($base, $db) {
        $this->debugTime = microtime(true);
        
        $this->baseURL = $base;

        //add trailing slash if needed from URL
        if(substr($base, -1) != '/') {
            $this->baseURL .= '/';
        }

        //remove leading slash if needed from database name
        if($db[0] == '/') {
            $db = substr($db, 1);
        }
        $this->dbName = $db;
        
        //build our query base string
        $this->qBase = $this->baseURL . $this->dbName . '/';

        //init curl and set our options to see if couch is up
        $this->couchCurl = curl_init();

        curl_setopt($this->couchCurl, CURLOPT_URL, $this->qBase);
        curl_setopt($this->couchCurl, CURLOPT_RETURNTRANSFER, 1);
        
        //check couch
        $data = json_decode(curl_exec($this->couchCurl), true);
        
        if(isset($data['error']) || !isset($data['db_name'])) {
            throw new Exception('Ottoman was unable connect to CouchDB or the database doesn\'t exist!');
        }
    }

    function __destruct() {
        if($this->couchCurl) {
            curl_close($this->couchCurl);
        }
    }
    
    /**
     * Determines if the JSON passed to it represents a successful CouchDB Call
     * 
     * @param string $json data returned from the server
     * @return bool true if successful, false if indicates error
     **/    
    function couchSuccess($json) {
        if(is_array($json)) {
            $json = json_encode($json);
        }
        
        if(is_null($json)) {
            return false;
        }
        
        $test = json_decode($json, true);
        return !isset($test['error']);
    }

    /**
     * Retrieves the JSON for a view that exists on the server.
     * 
     * @param string $category this is the _design/<category> part of the URL
     * @param string $name this it is the _design/<category>/_view/<name> part of the url
     * @return array|null AssocArray representation of view
     **/
    public function getView($category, $name) {
        $query = $this->qBase . '_design/' . urlencode($category);
        curl_setopt($this->couchCurl, CURLOPT_URL, $query);
        curl_setopt($this->couchCurl, CURLOPT_CUSTOMREQUEST, 'GET');
        curl_setopt($this->couchCurl, CURLOPT_POSTFIELDS, null);
        $json = json_decode(curl_exec($this->couchCurl), true);
        //$name doesn't need to be url encoded here, the JSON has the raw outpuit
        if(isset($json['views'][$name])) {
            return $json['views'][$name];
        }
        else {
            return null;
        }
    }

    /**
     * Retrieves the JSON for a view that exists on the server.
     * 
     * @param string $category this is the _design/<category> part of the URL
     * @return string JSON from the server for this design doc
     **/
    private function getDesignDoc($category) {        
        $query = $this->qBase . '_design/' . urlencode($category);
        curl_setopt($this->couchCurl, CURLOPT_URL, $query);
        curl_setopt($this->couchCurl, CURLOPT_CUSTOMREQUEST, 'GET');
        curl_setopt($this->couchCurl, CURLOPT_POSTFIELDS, null);
        return curl_exec($this->couchCurl);
    }

    /**
     * Attempts to creates a category/viewname on the server. If the category/view
     * already exists, this throws an exception
     * 
     * @param string $category this is the _design/<category> part of the URL
     * @param string $name this it is the _design/<category>/_view/<name> part of the url
     * @param string $json the view "source code" json that defines map/reduce
     * @return string JSON from server
     * @throws Exception if the view already exists
     * @todo optimize this so that getView isn't used anymore!
     **/
    public function createView($category, $name, $json) {
        if($this->couchSuccess($this->getView($category, $name))) {
            throw new Exception('Category/View already exists!'); 
        }
        else {
            //get the existing document
            $data = json_decode($this->getDesignDoc($category), true);
            
            //did it exist?
            if(!$this->couchSuccess($data)) {
                $data = array(
                    'language' => 'javascript'
                );
            }

            $data['views'][$name] = json_decode($json, true);
            //unset($data['_rev']);
            $json = json_encode($data);

            //PUT this document up
            $query = $this->qBase . '_design/' . urlencode($category);
            curl_setopt($this->couchCurl, CURLOPT_URL, $query);
            curl_setopt($this->couchCurl, CURLOPT_CUSTOMREQUEST, 'PUT');
            curl_setopt($this->couchCurl, CURLOPT_POSTFIELDS, $json);
            return curl_exec($this->couchCurl);
        }
    }

    /**
     * Delete a document from the database (by default, gets and deletes the
     * latest revision).
     * 
     * @param string $id id of the document to be deleted
     * @param string $rev optional revision to attempt delete
     * @return string json from the server
     **/    
    public function deleteDoc($id, $rev = null) {
        if(!$rev) {
            $rev = $this->getDocRevHEAD($id);
            //document wasn't found, bail
            if(!$rev) {
                return null;
            }
        }
        $query = $this->qBase . urlencode($id) . '?rev=' . $rev;
        curl_setopt($this->couchCurl, CURLOPT_URL, $query);
        curl_setopt($this->couchCurl, CURLOPT_CUSTOMREQUEST, 'DELETE');
        curl_setopt($this->couchCurl, CURLOPT_POSTFIELDS, null);
        return curl_exec($this->couchCurl);
    }

    /**
     * Lists all the views in a particular category
     * 
     * @param string $category this is the _design/<category> part of the URL
     * @return array|null array of views' code OR null if they don't exist
     **/
    public function listViews($category) {
        $data = json_decode($this->getDesignDoc($category), true);
        if(isset($data['views'])) {
            return $data['views'];
        }
        else {
            return null;
        }
    }

    /**
     * Runs a particular view and returns the JSON
     * 
     * @param string $category this is the _design/<category> part of the URL
     * @param string $name this it is the _design/<category>/_view/<name> part of the url
     * @param array $args assoc array of arguments to be passed to the server
     * @return string JSON returned by the server
     **/
    public function runView($category, $name, $args = null) {
        $data = json_decode($this->getDesignDoc($category), true);
        $query = $this->qBase . '_design/' . urlencode($category) . '/_view/' . $name;
    
        if(is_array($args) && count($args) > 0) {
            $query .= '?';
            foreach($args as $k => $v) {
                $query .= urlencode($k) . '=' . urlencode($v);
            }
        }

        curl_setopt($this->couchCurl, CURLOPT_URL, $query);
        curl_setopt($this->couchCurl, CURLOPT_CUSTOMREQUEST, 'GET');
        curl_setopt($this->couchCurl, CURLOPT_POSTFIELDS, null);
        return curl_exec($this->couchCurl);
    }
    
    /**
     * Create a document in the database. By default this will create a blank
     * document and return the server response as expected.
     * 
     * @param string $id _id of the document we're trying to create, this is urlencoded and required
     * @param string $json json data to store in the document, defaults to a blank document
     * @return string JSON returned by the server containing the revision ID or errors
     **/
    public function putDoc($id, $json = '{}') {
        $query = $this->qBase . urlencode($id);
        curl_setopt($this->couchCurl, CURLOPT_URL, $query);
        curl_setopt($this->couchCurl, CURLOPT_CUSTOMREQUEST, 'PUT');
        curl_setopt($this->couchCurl, CURLOPT_POSTFIELDS, $json);
        return curl_exec($this->couchCurl);
    }

    /**
     * Replace an existing document in CouchDB with a new one. If you do not provide
     * any json, the library will automatically empty your document for you (not delete it)
     * This call assumes you have placed the revision ID in the document to allow a proper
     * update.
     * 
     * @param string $id _id of the document we're trying to replace, this is urlencoded automatically and required
     * @param string $json json data to store in the document, defaults to a blank document
     * @return string JSON returned by the server containing the revision ID or errors
     **/
    public function replaceDoc($id, $json = null) {
        if($json) {
            //we need to replace the existing document with whats there
            $query = $this->qBase . urlencode($id);
            curl_setopt($this->couchCurl, CURLOPT_URL, $query);
            curl_setopt($this->couchCurl, CURLOPT_CUSTOMREQUEST, 'PUT');
            curl_setopt($this->couchCurl, CURLOPT_POSTFIELDS, $json);
            return curl_exec($this->couchCurl);
        }
        else {
            //we need to get the revision, then we can do it
            $rev = $this->getDocRevHEAD($id);
            //create a blank docment with the latest revision
            $json = json_encode(array('_rev'=>$rev));

            $query = $this->qBase . urlencode($id);
            curl_setopt($this->couchCurl, CURLOPT_URL, $query);
            curl_setopt($this->couchCurl, CURLOPT_CUSTOMREQUEST, 'PUT');
            curl_setopt($this->couchCurl, CURLOPT_POSTFIELDS, $json);
            return curl_exec($this->couchCurl);            
        }
    }
    
    /**
     * Update doc takes the latest revision of a doc (if it's not deleted) and merges
     * the given $data array into the document, overwriting anything in the document.
     * It then loads htis back into the database with a replaceDoc
     * 
     * @param string $id _id of the document we're trying to update
     * @param string $data associative array of data we want to merge into the doc
     * @return string JSON returned by the server containing the revision ID or errors
     **/
    public function updateDoc($id, $data) {
        $rev = $this->getDocRevHEAD($id);

        if(!$rev) {
            //this document either doesn't exist or got deleted so we can just
            // create a new one
            
            unset($data['_id'], $data['_rev']);
            return $this->putDoc($id, json_encode($data));
        }
        else {
            //this document already exists, so now the fun begins
            $json = $this->getDoc($id, $rev);
            if($this->couchSuccess($json)) {
                $curDoc = json_decode($json, true);
                $curDoc = array_merge($curDoc, $data);

                //just in case, again - this can probably be optimized
                unset($curDoc['_id']);

                return $this->replaceDoc($id, json_encode($curDoc));
            }
        }
        
        return null;
    }
    
    /**
     * Grab a document from the database
     * 
     * @param string $id _id of the document we're trying to grab
     * @param string $rev optional argument to try and get a specific revision
     * @return string JSON returned by the server
     **/
    public function getDoc($id, $rev = null) {
        $query = $this->qBase . urlencode($id);
        if($rev) {
            $query .= '?rev=' . $rev;
        }

        curl_setopt($this->couchCurl, CURLOPT_URL, $query);
        curl_setopt($this->couchCurl, CURLOPT_CUSTOMREQUEST, 'GET');
        curl_setopt($this->couchCurl, CURLOPT_POSTFIELDS, null);
        return curl_exec($this->couchCurl);
    }
    
    /**
     * Helper function that returns the _rev/_id component of a document's json. This grabs the first
     * id it finds (so be careful passing it output from a view) and looks for the first rev it can find
     * (again, careful with views)
     * 
     * @param string $json the JSON string representing a document (or the add result)
     * @param string $opt null means an array with both _rev, _id. '_id' means just _id, _rev likewise
     * @return array|string associated array('_rev' => ? & '_id' => ?) or just hte _rev or _id as req'd
     **/    
    public function getDocVitals($json, $opt = null) {
        $data = json_decode($json, true);
        if(!$data) {
            return null;
        }

        $id = '';
        $rev = '';

        if(isset($data['_rev'])) {
            $rev = $data['_rev'];
        }
        elseif(isset($data['rev'])) {
            $rev = $data['rev'];
        }
        elseif(isset($data['value']['_rev'])) {
            $rev = $data['value']['_rev'];
        }
     
        if(isset($data['_id'])) {
            $id = $data['_id'];
        }
        else {
            $id = $data['id'];
        }
        
        if($opt == '_id') {
            return $id;
        }
        elseif($opt == '_rev') {
            return $rev;
        }
        elseif($opt == null) {
            return array('_id' => $id, '_rev' => $rev);
        }
       
        throw new Exception('Invalid $opt value. Valid options are _rev, _id or null for both.');
    }

    /**
     * Helper function that uses the HEAD custom HTTP request in order to find the document'
     * revision. This is faster than pulling the entire document
     * 
     * @param string $id id of the document to snag
     * @return string $rev latest revision of document, null if not found
     **/      
    public function getDocRevHEAD($id) {
        $query = $this->qBase . urlencode($id);
        curl_setopt($this->couchCurl, CURLOPT_URL, $query);
        curl_setopt($this->couchCurl, CURLOPT_CUSTOMREQUEST, 'HEAD');
        curl_setopt($this->couchCurl, CURLOPT_HEADER, 1);
        curl_setopt($this->couchCurl, CURLOPT_NOBODY, 1);
        $data = $this->http_parse_headers(curl_exec($this->couchCurl));
        curl_setopt($this->couchCurl, CURLOPT_HEADER, 0);
        curl_setopt($this->couchCurl, CURLOPT_NOBODY, 0);
        
        if(isset($data['Etag'])) {
            //remove the quotes from the Etag header
            return substr($data['Etag'], 1, -1);            
        }
        else {
            return null;
        }
    }
    
    /**
     * Helper function that parses http headers.
     * 
     * @author http://www.bhootnath.in/blog/2010/10/parse-http-headers-in-php/
     * @param string $header http headers
     * @return array associative array of parsed headers
     **/
    private function http_parse_headers($header) {
        $retVal = array();
        $fields = explode("\r\n", preg_replace('/\x0D\x0A[\x09\x20]+/', ' ', $header));
        foreach($fields as $field) {
            if(preg_match('/([^:]+): (.+)/m', $field, $match)) {
                $match[1] = preg_replace('/(?<=^|[\x09\x20\x2D])./e', 'strtoupper("\0")', strtolower(trim($match[1])));
                if(isset($retVal[$match[1]])) {
                    $retVal[$match[1]] = array($retVal[$match[1]], $match[2]);
                }
                else {
                    $retVal[$match[1]] = trim($match[2]);
                }
            }
        }
        return $retVal;
    }
    
}
