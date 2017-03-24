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
    public $account;
    
    private $dataTransmissionStatus;

    //Collected data from a form submission would be passed to this function to initialize a new account.
    public function CreateNewAccount($newUserName, $newPassword, $newFirstName, $newLastName, $newEmail, $permissions = "customer")
    {
        $this->account = new Account($this->database->GetAccountNumbers(), $permissions);

        $this->account->SetFirstName($newFirstName);
        $this->account->SetLastName($newLastName);
        $this->account->SetUserName($newUserName);
        $this->account->SetPassword($newPassword);
        $this->account->SetEmailAddress($newEmail);

        if($this->database->dbIsConnected)
        {
            $insert = "INSERT INTO accountTable (accountNumber, firstName, lastName, username, password, permissions) 
                       VALUES (" . $this->account->GetAccountNumber . ", " . $this->account->GetFirstName() . ", " .
                                $this->account->GetLastName() . ", " . $this->account->GetUserName() . ", $newPassword, $permissions)";

            $this->database->query($insert);
        }
        else
            $dataTransmissionSuccess = false;   //Will need a try/catch for any database queries which will determine what to do with fails.
    }

    public function LoadAccount()
    {   
        //Find existing account in table and retrieve the information to load into active Account iteration.
        //Requires username and password authentication.
        //Leaving this function alone for now. Will build last.
    }    

    //This function is designed to show a delete function. In a real-world deploy I believe that accounts should be deactivated, but not deleted.
    public function DeleteAccount($deletedAccount)
    {
        if($this->database->dbIsConnected)
        {
            $delete = "DELETE FROM accountTable WHERE accountNumber = $deletedAccount";

            $this->database->query($delete);
        }
        else
            $dataTransmissionSuccess = false;
    }

    //Call this function with the three string parameters in order to update the database's table dynamically.
    public function UpdateDatabase($account, $newDataType, $newData)
    {
        if($this->database->dbIsConnected)
        {
            $update = "UPDATE accountTable SET $newDataType = $newData WHERE accountNumber = $account";
            
            $this->database->query($update);
        }
        else
            $dataTransmissionSuccess = false;
    }

    //These next two functions were early thoughts. Please ignore as irrelevant.

    //public function GetProfileInfo()
    //{
    //    $fullName = $this->account->GetFullName();
    //    $userName = $this->account->GetUserName();
    //    $emailAddress = $this->account->GetEmailAddress();
    //    $accountNumber = $this->account->GetAccountNumber();
    //}

    //public function PlaceOrder($items)
    //{
    //    //TODO: build a database query for adding a new order to the orderHistory table. 
    //    //This function will be set up to store the details of a completed order in this account holder's orderHistory table
    //    //and update the account balance whether they owe or have credit.
        
    //    //Example: $insertQuery = "INSERT INTO $tableName ($value, $anotherValue, $etc) VALUES (123, 'string value', 'a value for each of the table's fields')";
    //        //Concatenate the query string with a foreach loop with each iteration in the $items array.
    //    return $insertQuery;
    //}
}

?>