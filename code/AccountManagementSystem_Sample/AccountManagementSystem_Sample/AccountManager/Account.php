<?php
include ('Profile.php');

class Account extends Profile
{
    private $accountNumber;
    private $accessPermissions;
    private $enumeratorSupplement = array("admin", "standard user", "customer");    //simple array of access permission levels available to switch between
    
    private $orderHistory;
    //private $wishlist;    //disregarding to reduce the size of the project for now.
    //orderTracking;

    private $totalAccountBalance;

    function __construct()
    {
        //load defaults:
        $this->accountNumber = -01234;
        $this->accessPermissions = $this->enumeratorSupplement[2];  //All new accounts are set at 'customer' permissions by default.

        //Oder history will include information about previously completed orders accessed from the database and be added to anytime a new order is successfully submitted.
            //itemsOrdered list would be an array linking to any and all items on the shipment
            //shipped status would contain a boolean value indicating it was shipped and a tracking number if applicable
            //balance would be the total cost of the order, minus any payments made toward it.
        $this->orderHistory = [ "orderNumber0" => array("itemsOrderedList", "shippedStatus", "balance"),
                                "orderNumber1" => array("itemsOrderedList", "shippedStatus", "balance"),
                                "orderNumber2" => array("itemsOrderedList", "shippedStatus", "balance")];

    }

    //Demonstrating inheritance
    //The username and password are protected members of the Profile class which can be accessed as members of Account.
    public function SetUserName($newUserName)
    {
        //Ignoring username completeness validation for this demo:
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

    //Password is never accessed from outside the class, only checked against for login validation
    public function CheckPassword($password)
    {
        if($this->password === $password)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private function ValidatePassword($password)
    {
        //TODO: Add logic that verifies a password meets requirements.
        //Current logic is a generic placeholder designed to always return true
        $valid = true;
        if($valid)
        {
            return true;
        }
        else
            return false;
    }
}

?>