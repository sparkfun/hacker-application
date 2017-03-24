<?php
include('Account.php');

/**
 * AccountManager summary:
 *
 * This class represents the "controller" in the MVC architecture. It will manage the user requests, manipulate the data presented in an account
 * and update the database with the new information, then return the changes to the user so they may proceed.
 * 
 * @version 1.0
 * @author michael.bolles
 */

class AccountManager
{
    public $database = new Database();    
    public $account = new Account($this->database->GetAccountNumbers());
     
    function LoadAccountInfo()
    {    
        $account->SetFirstName("Mike");
    }    

    public function GetProfile()
    {
        $fullName = $this->account->GetFullName();
        $userName = $this->account->GetUserName();
        $emailAddress = $this->account->GetEmailAddress();
        $accountNumber = $this->account->GetAccountNumber();
    }

    public function UpdateDatabase($newInfo)
    {
        //Database table calls to add new info to the correct bit...
    }

    public function RecordPlacedOrder($items)
    {
        //TODO: build a database query for adding a new order to the orderHistory table. 
        //This function will be set up to store the details of a completed order in this account holder's orderHistory table
        //and update the account balance whether they owe or have credit.
        
        //Example: $insertQuery = "INSERT INTO $tableName ($value, $anotherValue, $etc) VALUES (123, 'string value', 'a value for each of the table's fields')";
            //Concatenate the query string with a foreach loop with each iteration in the $items array.
        return $insertQuery;
    }
}

?>