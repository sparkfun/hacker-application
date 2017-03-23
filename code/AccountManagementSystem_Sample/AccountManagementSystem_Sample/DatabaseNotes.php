
<?php
//Demo script for setting up a connection to a database, retrieving and manipulating data contained within a table.

//Connecting to a database:
//Required parameters to create a connection to a particular database:
$dbServer = "localhost";
$dbUserName = "username";
$dbPassword = "password";
$dbName = "MyDatabase";

$connection = new mysqli($dbServer, $dbUserName, $dbPassword, $dbName);

//Database connectivity exception handler:
if($connection->connect_errno)
{
    exit("Database connection failed: " . $connection->connect_error);
}

//Creating a table from php script:
$newTable = "CREATE TABLE myTable (id INT(6) UNSIGNED AUTO_INCREMENT PRIMARY KEY,
                                    valueString VARCHAR(50) NOT NULL,
                                    anotherString VARCHAR(100) NOT NULL,
                                    reg_date TIMESTAMP)";

//Table creation exception handler:
if ($connection->query($newTable) === true)
{
    echo "Created new table successfully.";
}
else
{
    echo "Error creating table: " . $connection->error;
}

//Samples for sending command queries to the database;
$deleteQuery = "DELETE FROM $tableName WHERE id = 4";
$updateQuery = "UPDATE $tableName SET valueName = 'new value' WHERE id = 2";
$insertQuery = "INSERT INTO $tableName ($value, $anotherValue, $etc) VALUES (123, 'string value', 'a value for each of the table's fields')";

$connection->query($queryName);

//Retrieving information from a table:
$selectQuery = "SELECT value1, value2, value3 FROM $tableName ORDER BY value1";
$resultObject = $connection->query($selectQuery);

if($resultObject->num_rows > 0)
{
    while($singleRowFromQuery = $resultObject->fetch_assoc())   //fetch associative array from query object
    {
        //print_r($singleRowFromQuery);
        echo "Value: " .  $singleRowFromQuery['key'].PHP_EOL;
    }
}

//close the object when you're done with it.
$resultObject->close();

//close the connection to a database:
$connection->close();
?>