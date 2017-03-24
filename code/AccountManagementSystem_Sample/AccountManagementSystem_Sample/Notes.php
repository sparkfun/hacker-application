//March 2017
//The following are notes taken by Michael Bolles while reviewing 
//PHP Fundamentals course from www.pluralsight.com
//They serve no functional value to this program, other than being a syntax resource.

<?php   //HTML insert: <?php ...

//Reserved words:
//$_GET, $_POST, $_COOKIE, $_SERVER, $_FILES, $_ENV, $this

//Integers: No unsigned.
$regInt = -1234;
$octNum = 01234;
$hexNum = 0xABC123;
$binaryNum = 0b1000;

//Floating point numbers:
$float = 1.234;
$scientificFloat = 0.1234E4;

//Boolean values:
$bool = true;
$hasValue = 1234;
echo (bool)$hasValue;   //typecasting

//Constants: (always global)
define('NEW_CONST', "Hello World");
echo NEW_CONST;

//Value test functions (check that it is typed correctly)
is_int($value);
is_float($value);
is_string($value);
is_bool($value);
defined('CONST_NAME');  //check to see if a const is defined

//Creating a function: set a default parameter value for function overriding
function Add($num1, $num2 = 2)
{
	echo (num1 + num2);
}

//Calling a function:
Add(1,2);
Add(1); //Will add with default value for $num2

//Call a function by name
$addFunction = "Add";
$addFunction(1,2); //Will execute Add function as if it were called directly.

//Strings:
$s1 = 'string1';
$s2 = "string2";
$s3 = "$s1 and $s2";
$sConcat = $s1 . " " . $s2;
$s4 = 'St. Patrick\'s Day'; //backslash in front of single quote keeps it from deliminating the string.
$second = 2;
$s5 = "{$second}nd place";  //brackets ensure that the variable is separated if there aren't spaces between var call and chars

//Outputing strings and other text:
echo "Hello World.";

//String manip
strtolower($value);
strtoupper($value);
strlen($value);
strpos($value, "value"); //case sensitive
str_replace("value", "replacement value", $stringSource);
substr($value, $startPos);  //substr($value, $startPos, $length);   //Negative numbers affect the end of the string
$strArray = str_split($value);  //adds string to an array   //str_split($value, $length);   //Converts into an array with elements of $length characters long.

//Concatenating strings with a period:
echo 'Hello' . 'world';

//Double quoted strings are evaluated:
$w = 'world';
echo "Hello $w";

//Arrays:
$myArray = array("this", "that", "theOther");
$anotherArray = ["this", "that", "theOther"];
$yetAnotherArray = [1, 1.1, "string", true];
echo $myArray[2];   //output: that

//Associative Arrays: (key, value)
$dictionaryArray = array ("a" => "AAA", "b" => "BBB", "c" => "CCC");
echo $dictionaryArray["b"]; //output: BBB
echo array_key_exists($key, $arrayName);    //boolean check
echo in_array($value, $arrayName);  //Optional parameter = $dataType

//Add to arrays:
array_push($arrayName, $value); //adding an indexed value to an array (doesn't work on associative arrays)
$arrayName[] = $value;  //adds an element to the array
$arrayName[$key] = $value;  //Add to an associative array.

//Remove from array:
array_pop($arrayName);  //remove last element
$value = array_pop($arrayNAme);     //sets last element to a variable

//Sorting an array:
sort($arrayName);   //default for string is alphabetical

//Count the number of elements in an array:
count($arrayName);

//Multidimensional array:
$arrayName = ["key1" => array("a", "b", "c"),
              "key2" => array("d", "e", "f")];

//For Each loop:
foreach($arrayName as $value)
{
    echo $value . "\n";
}

//Finding Key and value in an associative array in a foreach loop:
foreach($arrayName as $key => $value)
{
    echo "$value ($key)\n";
}

//clearing out the contents of a variable
unset($variable);   //can remove an element of an array: unset($arrayName[index]);

//Creating a class (Same as C++ without the ';' at the end):
class Person
{
    private $firstName = "John";
    public $lastName;
    public $birthYear = 1980;
    const MAX_AGE = 99;

    public static $personData = 321;

    //Constructor:
    function __construct($newBirthYear = 1983)
    {
        //Set default values...
        $this->lastName = "Smith";
        $this->birthYear = $newBirthYear;
    }

    public function GetFirstName()
    {
        return $this->firstName;
    }

    public function SetFirstName($newName)
    {
        $this->firstName = $newName;
    }
}

//Inheritance:
class Author extends Person
{
    public $penName = "Charles Dickens";
    public static $authorData = 123;

    public function GetPenName()
    {
        return $this->penName;
    }

    public static function GetData()
    {
        return self::$authorData + parent::$personData;
    }
}

//Create instance of the class: (added a constructor that takes a parameter for birth year)
$myObject = new Person(1883);
$myAuthor = new Author();

//Accessing static content from a class:
Author::GetData();

//Object operators:
echo $myObject->firstName;
$myObject->firstName = "Johnny";
echo $myObject::MAX_AGE;    //Consts require the scope operator
echo Person::MAX_AGE;       //can be called from class definition

//Object Method call:
$myObject->SetFirstName("Jonathan");

//Inclusion statements:
include "directory/name.php";
include_once "directory/name.php";
require "directory/name.php";   //terminates program if unavailable. Also has require_once optionally

//Comparison operators:
$number == $anotherNumber;
$number === $anotherNumber; //Checks value and type.
//>, <, >=, <=, <=> &&, ||, !

//Miscellanious operators
//+, -, *, /, ++, --. +=, -=, %, 

//switch statements:
switch($value)
{
    case 0:
        "Do something...";
        break;
    case 1:
        "Do something else...";
        break;
    default:
        "Do something by default";
        break;
}

//Ternary operator
$result = ($ifThis <=> $toThis) ? $doThis : $otherwiseDoThis;

//Null coalesce operator:
$result = $value ?? "default";  //checks that the $value has been set before assigning to result, and if not, fills in with default

//While loop:
while($value == true)
{
    //do something...
    if($done)
    { 
        $value = false;
    }
}

//alternate begin and end syntax:
if($condition) :    //(if, while, for) replace {} with : and endif/endfor/endwhile
    //if true...
else :
    //else tasks...
endif;


//NGinX demo:
class Registration
{
    public $FirstName = "";
    public $LastName = "";
    public $DateOfBirth = "";
    public $EmailAddress = "";
    public $IPAddress = "";
    public $ValidationErrors;
    //Encapsulate validation logic, checking that all the entry fields of the class have functional entries.
    public function IsValid()
    {
        $this->ValidationErrors = array();
        if($this->IsNullOrEmpty($this->FirstName))
        {
            array_push($this->ValidationErrors, 'First name is required');
        }
        if($this->IsNullOrEmpty($this->LastName))
        {
            array_push($this->ValidationErrors, 'Last name is required');
        }
        if($this->IsNullOrEmpty($this->EmailAddress))
        {
            array_push($this->ValidationErrors, 'Email address is required');
        }
        if($this->IsNullOrEmpty($this->DateOfBirth))
        {
            array_push($this->ValidationErrors, 'Date of Birth is required');
        }
        else
        {
            $date = DateTime::createFromFormat('d/m/y', $this->DateOfBirth);
            if(DateTime::getLastErrors()['warning_count'] > 0)
            {
                array_push($this->ValidationErrors, 'Date of Birth must be in dd/mm/yyy format');
            }
            else
            {
                $this->DateOfBirth = $date;
            }
        }
        return count($this->ValidationErrors) == 0;
    }
    private function IsNullOrEmpty($string)
    {
        return (!isset($string) || trim($string) === '');
    }
}

//PHP end statement:
?>