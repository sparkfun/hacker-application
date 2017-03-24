<?php

/**
 * Profile summary:
 *
 * This class holds the basic recursive information about an account holder.
 * Fields are protected to demonstrate encapsulation and inheritance of class member values/functions.
 *
 * @version 1.0
 * @author michael.bolles
 */

class Profile
{
    protected $firstName;
    protected $lastName;
    protected $userName;    //UN and PW, are defined in child class: Account
    protected $password;
    private $emailAddress;

    //Constructor
    function __construct()
    {
        $this->firstName = "John";
        $this->lastName = "Smith";
        $this->userName = "smithj1";
        $this->password = "qwerty";
        $this->emailAddress = "smithj1@webtv.com";
    }


    //Get & Set methods for accessing protected fields of the Profile class:

    public function SetFirstName($first)
    {
        if($first != null)
        {
            $this->firstName = $first;
        }
        else
            $this->NullRefErrorMessage("First Name");
    }

    public function SetLastName($last)
    {
        if($last != null)
        {
            $this->lastName = $last;
        }
        else
            $this->NullRefErrorMessage("Last Name");
    }

    public function GetFullName()
    {
        if($this->ValidateName())
        {
            return $this->firstName . " " . $this->lastName;
        }
        else
            return "Missing name information.";
    }

    public function SetEmailAddress($email)
    {
        if(ValidateEmail($email))
        {
            $this->emailAddress = $email;
        }
    }

    public function GetEmailAddress()
    {
        return $this->emailAddress;
    }


    //The following methods are for internal-class use as maintenance tools:

    protected function ValidateName()
    {
        if($this->firstName != null && $this->lastName != null)
            return true;
        else
            return false;
    }

    private function ValidateEmail($email)
    {
        //TODO: Add validation check by examining the string against proper email syntax.
        //Placeholder always return true:
        return true;
    }

    protected function NullRefErrorMessage($arg)
    {
        echo "Missing $arg.\n";
    }
}

?>
