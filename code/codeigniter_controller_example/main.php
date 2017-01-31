<?php if ( ! defined('BASEPATH')) exit('No direct script access allowed');

require_once 'MobileDetect/MobileDetect_1.1.php';
require_once 'common_libraries/commonController.inc';
require_once 'common_libraries/basepage.inc';
require_once 'common_libraries/baseSectionConfig.inc';
require_once 'common_libraries/pageLoader.inc';
require_once 'common_libraries/eteamLead.inc';
require_once 'storeLocator/storeLocator_1.2.inc';

class Main extends commonController {
    
    private $lead;
    private $is_mobile = false;
    private $post_to_client = true;
    
    public function __construct() {
        // Sets up the customer by code in the parent class.
        parent::__construct('AW');
        
        // Sets up a new lead object.
        $this->lead = new eteamLead();
        $this->lead->setProperty($this->imm_property);
        $this->lead->setPublisher($this->imm_publisher);
        
        // Checks if the current reqquest is from a mobile device.
        $detect = new Mobile_Detect();
        $this->is_mobile = $detect->isMobile();
        
        // The next several parameters were requested by QA for testing purposes.
        // Check the GET params for post_to_client.
        if (isset($_GET['post_to_client'])) {
            if ($_GET['post_to_client'] == 'true' || $_GET['post_to_client'] == '') {
                $_SESSION['post_to_client'] = true;
            }
            if ($_GET['post_to_client'] == 'false') {
                $_SESSION['post_to_client'] = false;
            }
        }
        
        if (isset($_SESSION['post_to_client']) && $_SESSION['post_to_client']) {
            $this->post_to_client = true;
        } else {
            $this->post_to_client = false;
        }
        
        // Use the dev sandbox to write leads.
        if (isset($_GET['use_sandbox'])) {
            if ($_GET['use_sandbox'] == 'true' || $_GET['use_sandbox'] == '') {
                $_SESSION['use_sandbox'] = true;
            }
            if ($_GET['use_sandbox'] == 'false') {
                $_SESSION['use_sandbox'] = false;
            }
        }

        if (isset($_GET['test_ip'])) {
            $_SESSION['test_ip'] = $_GET['test_ip'];
        }
        
        if (isset($_GET['ed_debug'])) {
            if ($_GET['ed_debug'] == 'true' || $_GET['ed_debug'] == '') {
                $_SESSION['ed_debug'] = true;
            }
            if ($_GET['ed_debug'] == 'false') {
                $_SESSION['ed_debug'] = false;
            }
        }
        
        if (!isset($_SESSION['ed_debug'])) {
            $_SESSION['ed_debug'] = false;
        }
    }

    /**
     * Initializes the page.
     */
    public function index() {
        // Load the page object and give it access to the smarty template object.
        $objPageLoader = new pageLoader();
        $objPageConfig = $objPageLoader->run($this->smarty);
        $objPageConfig->setUp();
        
        // Set the smarty source variable based on the publisher.
        //$arr_search_publishers = $this->obj_translator_service->translate('arr_search_publishers');
        $arr_search_publishers = $this->lead->getSearchPubs();
        if (in_array($this->imm_publisher, $arr_search_publishers)) {
            $this->smarty->assign('source', 'IMM-SEARCH');
        } else {
            $this->smarty->assign('source', 'IMM-DISPLAY');
        }
        
        // load the pixel here.
        $page_name = $objPageLoader->getPageName();
        //error_log('page name: '.$page_name);
        if ($page_name === 'map') {
            $sku = !empty($_SESSION['mpuid']) ? $_SESSION['mpuid'] : 'eteam';
            $arr_params = ['id' => $this->imm_property, 'sku' => $sku];
            $this->_setConversionFile('PixelAppend.php', 'CUSTOMER');
            $pixel = $this->_getConversionPixel($arr_params);
            // get mp pixel
            $mp_pixel = $this->_getMediaplexPixel();
        } else {
            $pixel = $this->_getLandingPixel();
            $mp_pixel = null;
        }        
        
        // assign variables to smarty so they are accesible in the template.
        $this->smarty->assign('pixel', $pixel);
        $this->smarty->assign('mp_pixel', $mp_pixel);

        $this->smarty->assign('is_mobile', $this->is_mobile);
        $this->smarty->assign('objPageConfig', $objPageConfig);
        $this->smarty->assign('post_to_client', $this->post_to_client);
        
        // display the page.
        $this->smarty->display($objPageConfig->structureDirectory().'/'.$objPageConfig->structureTpl());
    }
    
    /**
     * Tries to create a new lead from ajax post.
     */
    public function signup() {
        // read the lead data from the ajax request.
        $arr_data = $this->lead->readJsonData();
        
        // verify the lead data        
        list($result, $error_result) = $this->lead->verify($arr_data);
        
        if ($result) {
            // if the lead is ok, create the lead
            $this->lead->create();
            
            // set the client result
            $result = [
                'success' => true,
                'data' => ''
            ];

            // save variables that will be used later to the session
            $arr_lead_data = $this->lead->getLeadData();
            $_SESSION['email'] = $arr_lead_data['email'];
            $_SESSION['zip'] = $arr_lead_data['zip'];
            $_SESSION['i_session_id'] = isset($arr_data['i_session_id']) ? $arr_data['i_session_id'] : '';
            $_SESSION['tracking_master_id'] = $arr_lead_data['tracking_master_id'];
        } else {
            // if the lead is bad, set the client result
            $result = [
                'success' => false,
                'data' => '',
                'errors' => $error_result['errors']
            ];
        }

        // return json result
        echo json_encode($result);
        exit();
    }
    
    /**
     * Get the store location data using zip code and returns it as a json 
     * object. This method is normally called by map page via ajax.
     */
    public function getLocations() {
        $zip = null;
        $result = [
            'success' => false,
            'stores' => [],
            'errors' => []
        ];
        
        // get the zip_code from the uri.
        if (!empty($this->uri->uri_string)) {
            $zip = basename($this->uri->uri_string);
        }

        // Find the stores for a given zip code within a defined radius.
        if (!empty($zip) && $this->lead->validateZipCode($zip)) {
            $locator = new storeLocator(eteamLead::ETEAM_DB, eteamLead::ETEAM_STORE_TABLE, 'lat', 'lon');
            $arr_locations = $locator->findLocationsByZip($zip, eteamLead::ETEAM_ZIP_RADIUS, '*', eteamLead::ETEAM_STORE_COUNT);
            
            // Format phone numbers
            foreach ($arr_locations as $key => $location) {
                $location['raw_phone'] = $location['phone'];
                $location['phone'] = substr_replace($location['phone'], '(', 0, 0);
                $location['phone'] = substr_replace($location['phone'], ') ', 4, 0);
                $location['phone'] = substr_replace($location['phone'], '-', 9, 0);
                $arr_locations[$key] = $location;
            }
            
            $result = [
                'success' => !empty($arr_locations),
                'stores' => $arr_locations
            ];
        }
        
        if (!$result['success']) {
            $result['errors']['message'] = 'There are no stores within '.eteamLead::ETEAM_ZIP_RADIUS.' miles of the zip code you entered.  Please try another.';
        }
        
        echo json_encode($result);
        exit();
    }
    
    /**
     * Returns information about the zip (i.e. lat and lon) and stores the 
     * results in the session. 
     */
    public function getZipInfo() {
        $zip = null;
        $result = array(
            'success' => false,
            'errors' => ['field' => 'zip-input']
        );
        
        if (!empty($this->uri->uri_string)) {
            $zip = basename($this->uri->uri_string);
        }

        $key = 'zip-'.$zip;
        if (isset($_SESSION[$key])) {
            $result['success'] = true;
            $result['info'] = $_SESSION[$key];
        } elseif ($this->lead->validateZipCode($zip)) {
            $arr_zip_info = storeLocator::getZipInfo($zip);
            if (!empty($arr_zip_info)) {
                $_SESSION[$key] = $arr_zip_info;
                $result['success'] = true;
                $result['info'] = $arr_zip_info;
            } else {
                $result['errors']['message'] = 'No matches for zipcode: ' . $zip;
            }
        } else {
            $result['errors']['message'] = 'Invalid zip code';
        }

        echo json_encode($result);
        exit();
    }
}

/* End of file welcome.php */
/* Location: ./application/controllers/welcome.php */