<?php

include('ottoman.php');

ini_set('max_execution_time', 5);

//we use a special class for testing stuff that includes a database emptying tool
class ottomanTests extends ottoman {
    function __construct($base, $db) {
        parent::__construct($base, $db);
        
        //set an absurdly low timeout
        curl_setopt($this->couchCurl, CURLOPT_TIMEOUT_MS, 200);
    }
    
    public function emptyDB() {
        //attempt delete
        curl_setopt($this->couchCurl, CURLOPT_URL, $this->qBase);
        curl_setopt($this->couchCurl, CURLOPT_CUSTOMREQUEST, 'DELETE');
        curl_setopt($this->couchCurl, CURLOPT_RETURNTRANSFER, 1);
        $data = json_decode(curl_exec($this->couchCurl), true);
        if(isset($data['error'])) {
            throw new Exception('Ottoman was unable to delete database \'' . $this->dbName . '\'!');
        }

        curl_setopt($this->couchCurl, CURLOPT_URL, $this->qBase);
        curl_setopt($this->couchCurl, CURLOPT_CUSTOMREQUEST, 'PUT');
        curl_setopt($this->couchCurl, CURLOPT_RETURNTRANSFER, 1);
        $data = json_decode(curl_exec($this->couchCurl), true);
        if(isset($data['error'])) {
            throw new Exception('Ottoman was unable to create database \'' . $this->dbName . '\'!');
        }
    }
}

function assertNotFound($json) {
    $test = json_decode($json, true);
    return isset($test['error']) && $test['error'] == 'not_found';
}

function assertSuccess($json) {
    $test = json_decode($json, true);
    return !isset($test['error']);
}

//these throw exceptions if there is an issue
$otto = new ottomanTests('http://localhost:5984/', 'ottomantests');
$otto->emptyDB();

?>
<style>
span { font: monospace; }
.green { color: #008000; }
.red { color: #800000; }
</style>
<?php

//empty database and verify
echo 'Attempting to list all design documents, expecting none...';
if(assertNotFound($otto->getCustom('_design'))) {
    echo '<span class="green">PASS</span><br/>';
}
else {
    echo '<span class="red">FAIL</span>';
    echo '<br/><pre>' . print_r($test, true) . '</pre><br/>';
}

//create a view
echo 'Attempting to create a view in category "tests"...';
$json = '{"map": "function(doc) { if(doc.type == \'cookie\') { emit(null, doc); }}"}';
$test = $otto->createView('tests', 'showAllCookies', $json);
if(assertSuccess($test)) {
    echo '<span class="green">PASS</span><br/>';
}
else {
    echo '<span class="red">FAIL</span>';
    echo '<br/><pre>' . print_r($test, true) . '</pre><br/>';
}

//try to recreate it, this should throw an exception!
try {
    echo 'Attempting to recreate a view in category "tests"...';
    $test = $otto->createView('tests', 'showAllCookies', $json);
    //this shouldn't get called as the above should throws an exception
    echo '<span class="red">FAIL</span>';
}
catch(Exception $e) {
    echo '<span class="green">PASS</span><br/>';
}

//create another view
echo 'Attempting to create another view in category "tests"...';
$json = '{"map": "function(doc) { if(doc.type == \'donut\') { emit(null, doc); }}"}';
$test = $otto->createView('tests', 'showAllDonuts', $json);
if(assertSuccess($test)) {
    echo '<span class="green">PASS</span><br/>';
}
else {
    echo '<span class="red">FAIL</span>';
    echo '<br/><pre>' . print_r($test, true) . '</pre><br/>';
}

//list all views, there should be two!
echo 'Attempting to list all views in category "tests", expecting 2...';
$test = $otto->listViews('tests');
if(count($test) == 2) {
    echo '<span class="green">PASS</span><br/>';
}
else {
    echo '<span class="red">FAIL</span>';
    echo '<br/><pre>' . print_r($test, true) . '</pre><br/>';
}

//sanity check to make sure we are still testing as expected!
echo 'Run one of our previously created views, expecting 0 results...';
$test = json_decode($otto->runView('tests', 'showAllCookies'), true);
if(isset($test['rows']) && count($test['rows']) == 0) {
    echo '<span class="green">PASS</span><br/>';
}
else {
    echo '<span class="red">FAIL</span>';
    echo '<br/><pre>' . print_r($test, true) . '</pre><br/>';
}

//lets insert 6 donuts
$i = 5;
while($i-- >= 0) {
    echo 'Creating donuts...';
    $donutJSON = json_encode(array('type' => 'donut', 'hole' => 'yes', 'jellyfilled' => rand(0, 1)));
    $res = $otto->putDoc(uniqid(), $donutJSON);
    if(assertSuccess($res)) {
        echo '<span class="green">PASS</span> (_rev=' . $otto->getDocVitals($res, '_rev') . ')<br/>';
    }
    else {
        echo '<span class="red">FAIL</span>';
        echo '<br/><pre>' . print_r(json_decode($res, true), true) . '</pre><br/>';
    }
}

//and a 3 cookies
$i = 2;
while($i-- >= 0) {
    echo 'Creating cookies...';
    $donutJSON = json_encode(array('type' => 'cookie', 'doughy' => 'yes', 'frosted' => rand(0, 1)));
    $res = $otto->putDoc(uniqid(), $donutJSON);
    if(assertSuccess($res)) {
        echo '<span class="green">PASS</span> (_rev=' . $otto->getDocVitals($res, '_rev') . ')<br/>';
    }
    else {
        echo '<span class="red">FAIL</span>';
        echo '<br/><pre>' . print_r(json_decode($res, true), true) . '</pre><br/>';
    }
}

//make sure our views are returning properly!
echo 'Run our showCookies view, expecting 3 results...';
$test = json_decode($otto->runView('tests', 'showAllCookies'), true);
if(isset($test['rows']) && count($test['rows']) == 3) {
    echo '<span class="green">PASS</span><br/>';
}
else {
    echo '<span class="red">FAIL</span>';
    echo '<br/><pre>' . print_r($test, true) . '</pre><br/>';
}

echo 'Run our showDonuts view, expecting 6 results...';
$test = json_decode($otto->runView('tests', 'showAllDonuts'), true);
if(isset($test['rows']) && count($test['rows']) == 6) {
    echo '<span class="green">PASS</span><br/>';
}
else {
    echo '<span class="red">FAIL</span>';
    echo '<br/><pre>' . print_r($test, true) . '</pre><br/>';
}

//lets make one of the cookies no longer doughy, grab the first cookie we can
echo 'Run our showCookies view with limit argument, expecting 1 result...';
$test = json_decode($otto->runView('tests', 'showAllCookies', array('limit' => 1)), true);
if(isset($test['rows']) && count($test['rows']) == 1) {
    echo '<span class="green">PASS</span><br/>';
}
else {
    echo '<span class="red">FAIL</span>';
    echo '<br/><pre>' . print_r($test, true) . '</pre><br/>';
}
$test = $test['rows'][0]['value'];
$test['doughy'] = 'no';
$testid = $test['_id'];
$testrev = $test['_rev'];
unset($test['_id']);
$test = json_encode($test);

echo 'Replace that cookie with one that isn\'t doughy...';
$test = $otto->replaceDoc($testid, $test);
if(assertSuccess($test)) {
    echo '<span class="green">PASS</span><br/>';
}
else {
    echo '<span class="red">FAIL</span>';
    echo '<br/><pre>' . print_r($test, true) . '</pre><br/>';
}
$test = json_decode($test, true);
$testrev2 = $test['rev'];

echo 'Get that cookie back...';
$test = $otto->getDoc($testid);
$data = json_decode($test, true);
if(assertSuccess($test) && $data['doughy'] = 'no') {
    echo '<span class="green">PASS</span><br/>';
}
else {
    echo '<span class="red">FAIL</span>';
    echo '<br/><pre>' . print_r($data, true) . '</pre><br/>';
}

echo 'Grab the older cookie and see...';
$test = $otto->getDoc($testid, $testrev);
$data = json_decode($test, true);
if(assertSuccess($test) && $data['doughy'] = 'yes') {
    echo '<span class="green">PASS</span><br/>';
}
else {
    echo '<span class="red">FAIL</span>';
    echo '<br/><pre>' . print_r($data, true) . '</pre><br/>';
}

echo 'Delete the cookie...';
$test = $otto->deleteDoc($testid);
if(assertSuccess($test)) {
    echo '<span class="green">PASS</span> (_id=' . $testid . ')<br/>';
}
else {
    echo '<span class="red">FAIL</span>';
    echo '<br/><pre>' . print_r($test, true) . '</pre><br/>';
}

echo 'Did it actually delete?...';
$test = $otto->getDoc($testid);
$data = json_decode($test, true);
if(!assertSuccess($test)) {
    echo '<span class="green">PASS</span><br/>';
}
else {
    echo '<span class="red">FAIL</span>';
    echo '<br/><pre>' . print_r($data, true) . '</pre><br/>';
}

//attempt to add an arbitrary unit to an existing document and change an existing field
echo 'Now, lets grab a donut...';
$test = json_decode($otto->runView('tests', 'showAllDonuts', array('limit' => 1)), true);
if(isset($test['rows']) && count($test['rows']) == 1) {
    echo '<span class="green">PASS</span><br/>';
}
else {
    echo '<span class="red">FAIL</span>';
    echo '<br/><pre>' . print_r($test, true) . '</pre><br/>';
}
$test = $test['rows'][0]['value'];
$testid = $test['_id'];

//did it work?
echo 'Add sprinkles and overload the jellyfilled goodness...';
$test = $otto->updateDoc($test['_id'], array('sprinkles' => 'yes', 'jellyfilled' => 'overload'));
if(assertSuccess($test)) {
    echo '<span class="green">PASS</span><br/>';
}
else {
    echo '<span class="red">FAIL</span>';
    echo '<br/><pre>' . print_r($data, true) . '</pre><br/>';
}

//are we sure?
echo 'Check to make sure the donut actually got merged...';
$data = json_decode($otto->getDoc($testid), true);
if($data['jellyfilled'] == 'overload' && isset($data['hole']) && isset($data['sprinkles'])) {
    echo '<span class="green">PASS</span><br/>';
}
else {
    echo '<span class="red">FAIL</span>';
    echo '<br/><pre>' . print_r($data, true) . '</pre><br/>';
}




?>
