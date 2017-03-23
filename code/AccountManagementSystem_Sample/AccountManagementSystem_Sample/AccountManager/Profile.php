
<?php

//This class holds the basic recursive information about an account holder.
//Fields are protected to demonstrate encapsulation and inheritance of class member values/functions.

class Profile
{
    protected $firstName;
    protected $lastName;
    protected $userName;    //Defined in child class: Account
    protected $password;

    //Constructor
    function __construct()
    {
        $this->firstName = "";
        $this->lastName = "";
        $this->userName = "";
        $this->password = "";
    }


    //Get & Set methods for accessing protected fields of the Profile class:

    protected function SetFirstName($first)
    {
        if($first != null)
        {
            $this->firstName = $first;
        }
        else
            $this->NullRefErrorMessage("First Name");
    }

    protected function SetLastName($last)
    {
        if($last != null)
        {
            $this->lastName = $last;
        }
        else
            $this->NullRefErrorMessage("Last Name");
    }

    protected function GetFullName()
    {
        if($this->ValidateName())
        {
            return $this->firstName . " " . $this->lastName;
        }
        else
            return "Missing name information.";
    }


    //The following methods are for internal use as maintenance tools:

    protected function ValidateName()
    {
        if($this->firstName != null && $this->lastName != null)
            return true;
        else
            return false;
    }

    protected function NullRefErrorMessage($arg)
    {
        echo "Missing $arg.\n";
    }
}

?>
