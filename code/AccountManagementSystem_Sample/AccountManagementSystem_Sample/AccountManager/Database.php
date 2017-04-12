<?php

/**
 * Database summary:
 *
 * This class is a small representation of the various capabilities of database creation and maintenance.
 *
 * @version 1.0
 * @author michael.bolles
 */

class Database
{
    //Connecting to a database:
    //Required parameters to create a connection to a particular database:
    private $dbServer = "localhost";
    private $dbUserName = "username";
    private $dbPassword = "password";
    private $dbName = "MyDatabase";
    
    
    public $databaseConn;
    public $dbIsConnected;
    public $accountTable;

    function __construct()
    {
        $this->databaseConn = new mysqli($this->dbServer, $this->dbUserName, $this->dbPassword, $this->dbName);
        $this->accountTable = $this->CreateNewTable();
    }

    function __destruct()
    {
        $this->CloseConnection();
    }
    //Database connectivity exception handler:
    public function CheckConnection()
    {
        if($this->databaseConn->connect_errno)
        {
            $this->dbIsConnected = false;
            exit("Database connection failed: " . $this->databaseConn->connect_error);
        }
        else
            $this->dbIsConnected = true;
    }

    public function CloseConnection()
    {
        if(dbIsConnected == true)
        {
            $this->dbIsConnected = false;
            $this->databaseConn->Close();
        }
    }

    //Designed to generate the table for use with account information only.
    public function CreateNewTable()
    {
        //Create simple a table from php script for account and profile storage:
        $table = "CREATE TABLE accountTable (id INT(6) UNSIGNED AUTO_INCREMENT PRIMARY KEY,
                                             accountNumber INT(12),
                                             firstName VARCHAR(50) NOT NULL,
                                             lastName VARCHAR(50) NOT NULL,
                                             username VARCHAR(50) NOT NULL,
                                             password VARCHAR(100) NOT NULL,
                                             permissions VARCHAR(8))";
        
        //Table creation exception handler:
        if ($this->databaseConn->query($table) === true)
        {
            echo "Created new table successfully.";
        }
        else
        {
            echo "Error creating table: " . $this->databaseConn->error;
        }

        return $table;
    }

    public function GetAccountNumbers()
    {
        if($this->dbIsConnected)
        {
            $selectQuery = "SELECT accountNumber FROM accountTable ORDER BY accountNumber";
            $accounts = $this->databaseConn->query($selectQuery);
            
            if($accounts->num_rows > 0)
            {
                return $accounts->fetch_assoc();
            }
        }
    }
}

?>
