<?php
include ('Profile.php');

/**
 * Account summary:
 *
 * The account class holds the data specific to a person's account, such as orders and balance.
 *
 * @version 1.0
 * @author michael.bolles
 */

class Account extends Profile
{
    private $accountNumber;
    private $accessPermissions;

    //simple array of access permission levels available to switch between in lieu of an enum option
    private $enumeratorSupplement = array(0 => "admin", "internal", "customer");

    private $orderHistory;
    private $orderTracking;

    private $totalAccountBalance;

    function __construct($accountsArray, $permissions)
    {
        //This constructor is loaded with some arbitrary data for the purpose of displaying structure.
        //In a real world deployment, generic data would not act as a placeholder for these values at any time.
        
        $this->accountNumber = $this->CreateAccountNumber($accountsArray);
        $this->accessPermissions = $permissions;  //All new accounts are set at 'customer' permissions by default.

        //Incomplete: Order history is currently only a placeholder concept:
            /*
             * It will include information about previously completed orders accessed from the database and be added to anytime a new order is successfully submitted.
             * itemsOrdered list would be an array linking to any items on the shipment
             * shipped status would contain a boolean value indicating it was shipped and a tracking number if applicable
             * balance would be the total cost of the order, minus any payments made toward it.
             */
        $this->orderHistory = [ "orderNumber0" => array("itemsOrderedList", "shippedStatus", "balance"),
                                "orderNumber1" => array("itemsOrderedList", "shippedStatus", "balance"),
                                "orderNumber2" => array("itemsOrderedList", "shippedStatus", "balance")];

        $this->totalAccountBalance = 0.00;
    }

    //Demonstrating inheritance
    //The username and password are protected members of the Profile class which can be accessed as members of Account.
    public function SetUserName($newUserName)
    {
        //Ignoring username validation for this demo:
        $this->userName = $newUserName;
    }

    public function GetUserName()
    {
        return $this->userName;
    }

    public function SetPassword($newPassword)
    {
        if($this->ValidatePassword($newPassword))
        {
            $this->password = $newPassword;
        }
        else
            echo "Please enter a valid password.";
    }

    //Password is never accessed from outside the class, only checked against for login validation.
    public function CheckPassword($password)
    {
        if($this->password === $password)
            return true;
        else
            return false;
    }

    private function ValidatePassword($password)
    {
        //TODO: Add logic that verifies a password meets requirements (Number of upper and lower case letters, numbers, special characters, etc...).
        //Current logic is a generic placeholder designed to always return true.
        $valid = true;

        if($valid)
        {
            return true;
        }
        else
            return false;
    }

    private function CreateAccountNumber($existingNumbersArray)
    {
        //Find the next available number in a series of numbers loaded into the array and assigns it to the account
        if(count($existingNumberArray) >= 1)
        {
            //Assume the first and smallest account number possible is 1 for this logic to accurately assign the new acct number.
            $num = 1;
            
            foreach($existingNumbersArray as $existingNum)
            {
                if($num == $existingNum)
                {
                    $num++;
                }
                else
                {
                    $this->accountNumber = $num;
                    continue;
                }
            }
        }
        else
            $this->accountNumber = 1;
    }

    public function GetAccountNumber()
    {
        return $this->accountNumber;
    }

    public function CheckPermissions($accessRequest)
    {
        //Demonstrates a switch statement and returns true/false if the account holder has the correct hypothetical permissions:
        switch($accessPermissions)
        {
            case "admin":
                return true;
            break;

            case "internal":
                if($accessRequest == "admin")
                    return false;
                else
                    return true;
            break;

            case "customer":
                if($accessRequest == "admin" || $accessRequest == "internal")
                    return false;
                else
                    return true;
            break;
            default:
                echo "Permissions have not been set. Contact the helpdesk: support@Ijustcantrightnow.com";
        }
    }

    public function GetOrderHistory()
    {
        return $this->OrderHistory;
    }

    public function TrackOrder($orderNumber)
    {
        //Leaving this function empty until orderHistory is completed.
        //The orderNumber provided in the parameters will call an order's status and return the shipping details registered in the orderHistory table.
    }

    public function GetBalance()
    {
        return $this->totalAccountBalance;
    }
}

?>