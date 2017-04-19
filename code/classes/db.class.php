<?php

/********************************************************* 
****************  Database Configuration  ****************
**************** Written by: Tyler Bailey ****************
**********************************************************/

class DBConnection  
{       
	protected $mysqli;
	private  $db_host='localhost';
	private  $db_name='main';
	private  $db_username="root";
	private  $db_password='';

	public function __construct() {
        $this->mysqli = new mysqli($this->db_host,$this->db_username,
        $this->db_password, $this->db_name) or die($this->mysqli->error);

        return $this->mysqli;
    }
	
	public function getLink() {
		return $this->mysqli;
	}

	public function query($query) {
		return $this->mysqli->query($query);
     }

	public function real_escape_string($str) {
		return $this->mysqli->real_escape_string($str);
	}

	function __destruct(){
		$this->mysqli->close();
    }
}

?>