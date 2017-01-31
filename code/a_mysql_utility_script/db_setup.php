#!/usr/bin/php

<?php

namespace utilities;

require_once './vendor/phpseclib0.3.8/Net/SSH2.php';
require_once './vendor/phpseclib0.3.8/Net/SCP.php';

require_once './CryptUtil.inc';
require_once './CmdLine.inc';

use utilities\CryptUtil as CryptUtil;
use utilities\CmdLine as CmdLine;

/**
 * Description of db_setup
 * 
 * A utility script for development environments to load mysql databases from a
 * remote database server. The script assumes that the proper network, ssh user,
 * and database setup has been done by an admin to allow access to the necessary
 * resources. This script uses the PHP SecLib project to make SSH and SCP 
 * connections to the remote ssh server.
 *
 * @author rhouckes
 */
class DbSetup extends CryptUtil {
    
    private $remote_host = 'mysql.domain.corp';
    private $remote_user = 'admin';
    private $remote_pass = null;
    
    private $local_host = '127.0.0.1';
    private $local_user = 'root';
    private $local_pass = null;
    
    private $db_user = null;
    private $db_pass = null;
    
    private $db_auth_file = 'db_auth.json';
    private $ssh;
    private $db_name;
    private $db_str;
    private $db_file;
    private $db_zip;
    private $db;
    private $use_zip = true;
    
    /**
     * Constructs a DbSetup object using database name.
     * 
     * @param string $db_name
     */
    public function __construct($db_name) {
        parent::__construct($this->db_auth_file);

        if (empty($db_name)) {
            die("Error - database must be defined");
        }
        $this->db_name = $db_name;
        $this->db_str = get_current_user();

        // The path should be in /tmp, otherwise the files will not be cleaned.
        // Note: we use the current user in the file name in case we are in a 
        // shared dev environment.
        $this->db_file = "/tmp/{$this->db_name}_{$this->db_str}.dump";
        $this->db_zip = $this->db_file.'.gz';
    }
    
    /**
     * This allows the user to override the remote ssh server.
     * 
     * @param string $db_serv
     */
    public function setServ($db_serv) {
        if (isset($db_serv)) {
            $this->remote_host = $db_serv;
        }
    }
    
    /**
     * This allows the user to override the remote ssh user.
     * 
     * @param string $db_user
     */
    public function setUser($db_user) {
        if (isset($db_user)) {
            $this->remote_user = $db_user;
        }
    }
    
    /**
     * This allows the user to override the remote ssh password.
     * 
     * @param string $db_pass
     */
    public function setPass($db_pass) {
        if (isset($db_pass)) {
            $this->remote_pass = $db_pass;
        }
    }
    
    /**
     * Disable the compression of mysql dump files.
     */
    public function disableZip() {
        $this->use_zip = false;
    }
    
    /**
     * Execute the steps to remotely retrieve and locally load a mysql database.
     */
    public function execute() {
        // clean any previous local files
        error_log('Cleaning previous local dump file');
        $this->clean_local();
        
        // ssh to the remote ssh server
        error_log('Connecting to remote host');
        $this->ssh_connect();
        
        // remove remote files
        error_log('Cleaning previous remote dump file');
        $this->clean_remote();
        
        // export the db
        error_log('Exporting remote database');
        $this->export_remote();
        
        // get the zip file
        error_log('Transferring database to local machine');
        $this->transfer_to_local();
        
        // remove remote files
        error_log('Cleaning remote dump file');
        $this->clean_remote();
        
        // connect to the local mysql server
        error_log('Connecting to local database');
        $this->db_connect();
        
        // create database
        error_log('Creating to local database');
        $this->db_create();
        
        // setup user and grant
        error_log('Setting local database grants');
        $this->db_grant();
        
        // close the connection
        $this->db->close();
        
        // import the dump on the local database
        error_log('Importing dump into local database');
        $this->import_local();
        
        // clean local files
        error_log('Cleaning local dump file');
        $this->clean_local();
    }
    
    /**
     * Execute a sub-process capturing the stdout and stderr in one output 
     * stream and then returning the unix return code and the output stream as
     * a string.
     * 
     * @param string $cmd
     * @return array
     */
    private function cmd_exec($cmd) {
        $output = null;
        $pipes = array();
        $spec = array(
            0 => array("pipe", "r"),  // stdin is a pipe that the child will read from
            1 => array("pipe", "w"),  // stdout is a pipe that the child will write to
            2 => array("pipe", "w") // stderr is a file to write to
        );
        $proc = proc_open($cmd, $spec, $pipes);
        
        if (is_resource($proc)) {
            $output .= stream_get_contents($pipes[1]);
            $output .= stream_get_contents($pipes[2]);
        }
        
        fclose($pipes[0]);
        fclose($pipes[1]);
        fclose($pipes[2]);

        proc_terminate($proc);
        $return_var = proc_close($proc);
        return array($return_var, trim($output));
    }
    
    /**
     * Check the return code and if not 0 print the output in the error log.
     * 
     * @param type $output
     * @param type $return_var
     */
    private function checkOutput($output, $return_var) {
        if ($return_var > 0) {
            error_log('Error cmd: '.var_export($output,true));
        }
    }
    
    /**
     * Connect to the remote ssh server and set the ssh instance variable.
     */
    private function ssh_connect() {
        $this->ssh = new Net_SSH2($this->remote_host);
        if (!$this->ssh->login($this->remote_user, $this->get_pass($this->remote_host, $this->remote_user))) {
            die("Error - ssh login failure [{$this->remote_host}]".PHP_EOL);
        }
    }
    
    /**
     * Clean remote database files created in temporary space by this script.
     */
    private function clean_remote() {
        // NOTE: The check for /tmp is to ensure that we don't remove something
        // unintended from the remote system.
        if (!empty($this->db_zip) && dirname($this->db_zip) == '/tmp') {
            $this->ssh->exec("rm $this->db_zip");
        }
        if (!empty($this->db_file) && dirname($this->db_file) == '/tmp') {
            $this->ssh->exec("rm $this->db_file");
        }
    }
    
    /**
     * Export the remote database to a dump file and if enabled compress the 
     * dump file.
     */
    private function export_remote() {
        // export remote db
        echo $this->ssh->exec("mysqldump $this->db_name -r $this->db_file");
        if ($this->use_zip) {
            // zip the dump file
            error_log('Compressing remote dump file');
            echo $this->ssh->exec("gzip $this->db_file");
        }
    }
    
    /**
     * Transfer the remote export file to the local machine.
     */
    private function transfer_to_local() {
        // transfer the zip via scp
        $scp = new Net_SCP($this->ssh);
        
        if ($this->use_zip) {
            if ($scp->get($this->db_zip, $this->db_zip) === false) {
                die("Error - scp transfer failure [{$this->db_zip}]".PHP_EOL);
            }
        } else {
            if ($scp->get($this->db_file, $this->db_file) === false) {
                die("Error - scp transfer failure [{$this->db_file}]".PHP_EOL);
            }
        }
    }
    
    /**
     * Remove null bytes from the end of a file if present.
     * 
     * @param string $file
     */
    private function truncateNullByte($file) {
        $pos = -1;
        $fh = fopen($file, 'r+');
        fseek($fh, $pos, SEEK_END);
        $c = fgetc($fh);
        if ($c == "\x0") {
            $stat = fstat($fh);
            ftruncate($fh, $stat['size']-1);
        }
        fclose($fh);
    }
    
    /**
     * Import a mysql dump file into the local mysql instance. If the file is 
     * compressed the unzip the file first.
     */
    private function import_local() {
        if ($this->use_zip) {
            $this->truncateNullByte($this->db_zip);
            // unzip the local dump file
            error_log('Decompressing local dump file');
            list($return_var, $output) = $this->cmd_exec("gunzip $this->db_zip");
            if (substr_count($output, 'trailing garbage ignored') === 0) {
                $this->checkOutput($output, $return_var);
            }
        } else {
            $this->truncateNullByte($this->db_file);
        }
        
        exec("mysql -u{$this->db_local_user} $this->db_name < $this->db_file", $output, $return_var);
        $this->checkOutput($output, $return_var);
    }
    
    /**
     * Remove the local copy of the database dump file.
     */
    private function clean_local() {
        if (file_exists($this->db_file)) {
            unlink($this->db_file);
        }
        if (file_exists($this->db_zip)) {
            unlink($this->db_zip);
        }
    }
    
    /**
     * Connect to the local mysql server and set the db instance variable.
     */
    private function db_connect() {
        // Get the local server auth information from the CryptUtil object.
        $this->local_pass = $this->get_pass($this->local_host, $this->local_user);
        $this->db = new mysqli($this->local_host, $this->local_user, $this->local_pass);
        if ($this->db->connect_error > 0) {
            die('Connect Error (' . $this->db->connect_errno . ') '. $this->db->connect_error.PHP_EOL);
        }
        $result = $this->db->query("SET NAMES 'utf8'");
        if ($result === false) {
            die("Error - mysql character set failure");
        }
        $this->db_free($result);
    }
    
    /**
     * Create a database in the local mysql server.
     */
    private function db_create() {
        $query_create = "CREATE DATABASE IF NOT EXISTS {$this->db_name} CHARACTER SET utf8 COLLATE utf8_general_ci";
        $result = $this->db->query($query_create);
        if ($result === false) {
            die("Error - database create failure [{$this->db_name}]".PHP_EOL);
        }
        $this->db_free($result);
    }
    
    /**
     * Setup the local database grants.
     */
    private function db_grant() {        
        // Get the database auth information from the CryptUtil object.
        $this->db_pass = $this->get_pass($this->db_name, $this->db_user);
        
        // run the grant query
        $query_grant = "GRANT ALL ON `{$this->db_name}`.* to `{$this->db_user}`@`localhost` identified by '{$this->db_pass}';";
        //error_log('grant query: '.$query_grant);
        $result = $this->db->query($query_grant);
        if ($result === false) {
            die("Error - database grant failure [{$this->db_name}]".PHP_EOL);
        }
        $this->db_free($result);

        // flush the mysql privileges
        $result = $this->db->query('FLUSH PRIVILEGES');
        $this->db_free($result);
    }
    
    /**
     * Explicitely free the mysql result resource.
     * 
     * @param mysqli_result $result
     */
    private function db_free($result) {
        // free the mysql result statement
        if ($result instanceof mysqli_result) {
            $result->free();
        }
    }
}

// A database name is required at command line. The remote server, user and 
// password can be overidden, but note these are ssh credentials not the database 
// credentials. All database credentials should be stored in the auth file by 
// database name. Use the create_pass.php script to create both server and 
// database credentials. The local database is assumed to be '127.0.0.1', 'root'
// and no password, these credentials are already stored in the auth file.
$cmdline = new CmdLine();
$cmdline->addOptionByArr(['opt_name' => '-d', 'opt_required' => true, 'opt_val_required' => true, 'opt_text' => 'database to retrieve and import locally']);
$cmdline->addOptionByArr(['opt_name' => '-s', 'opt_required' => false, 'opt_val_required' => true, 'opt_text' => 'remote database server to retrieve from']);
$cmdline->addOptionByArr(['opt_name' => '-u', 'opt_required' => false, 'opt_val_required' => true, 'opt_text' => 'remote database server username']);
$cmdline->addOptionByArr(['opt_name' => '-p', 'opt_required' => false, 'opt_val_required' => false, 'opt_text' => 'ask for remote server password']);
$cmdline->addOptionByArr(['opt_name' => '-z', 'opt_required' => false, 'opt_val_required' => false, 'opt_text' => 'turn off zip compression']);
$arr_options = $cmdline->parse();

$db_name = $cmdline->getParsedOption('-d');
$remote_serv = $cmdline->getParsedOption('-s');
$remote_user = $cmdline->getParsedOption('-u');
$use_zip = !$cmdline->wasOptionParsed('-z');

$remote_pass = null;
if ($cmdline->wasOptionParsed('-p')) {
    // ask for the remote password, if the password option was given.
    print 'enter password: ';
    system('stty -echo');
    $remote_pass = trim(fgets(STDIN));
    system('stty echo');
    echo PHP_EOL;
}

$setup = new DbSetup($db_name);
if (!empty($remote_serv)) {
    $setup->setServ($remote_serv);
}
if (!empty($remote_user)) {
    $setup->setUser($remote_user);
}
if (!empty($remote_pass)) {
    $setup->setPass($remote_pass);
}
if (!$use_zip) {
    $setup->disableZip();
}
$setup->execute();
