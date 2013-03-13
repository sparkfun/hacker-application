<?php

require_once("database.class.php");

class Products {

	# all protected vars
	protected static $table_name = "products"; 										
	protected static $db_fields = array('id', 'productName', 'productDescription', 'productPrice', 'productImage', 
										'filename', 'productColor', 'category', 'inStock', 'size', 'other1', 'other2', '
										designer', 'designerBio', 'specialOrder', 'sale', 'asSeen', 'bridal', 'hauteFlash', 
										'preOrder', 'status');
	
	# all public vars
	public $id , $productName, $productDescription, $productPrice, $productImage, $filename, $productColor,
		   $category, $inStock, $size, $other1, $other2, $designer, $designerBio, $specialOrder, $sale, $asSeen, 
		   $bridal, $hauteFlash, $preOrder, $status;

	# get all records
	public static function find_all() {
		return self::find_by_sql("SELECT * FROM " . self::$table_name . " WHERE status = 1");
	}

	# shows 5 products for the front page based on color
	public static function front($color) {
		return self::find_by_sql("SELECT * FROM " . self::$table_name . " WHERE status = 1 AND asSeen = 'no' AND specialOrder = 'no' AND preOrder != 'yes' AND productColor= '$color' ORDER BY RAND() LIMIT 5");
	}

	# view the products in the store, based on category (if no category, it shows all)
	public static function store($category) {
		# going to make this dynamic
		$sort = " ORDER BY productPrice + 0 ASC";
		
		if ( $category == 'view_all') {
			return self::find_by_sql("SELECT * FROM " . self::$table_name . " WHERE status = 1 AND asSeen = 'no' AND specialOrder = 'no' AND preOrder != 'yes' $sort");	
		} else {
			return self::find_by_sql("SELECT * FROM " . self::$table_name . " WHERE status = 1 AND asSeen = 'no' AND specialOrder = 'no' AND preOrder != 'yes' AND category = '$category' $sort ");
		}
	}

	# sql for view page, shows only 1 product on the view page
	public static function view($pid) {
		return self::find_by_sql("SELECT * FROM " . self::$table_name . " WHERE id = '$pid' LIMIT 1");
	}

	# locate via ID
	public static function find_by_id($id=0) {
		global $database;
		$result_array = self::find_by_sql("SELECT * FROM " . self::$table_name . " WHERE id ={$id} LIMIT 1");
		return !empty($result_array) ? array_shift($result_array) : false;
	}
	
	# locate via sql statement
	public static function find_by_sql($sql="") {
		global $database;
		$result_set = $database->query($sql);
		$object_array = array();
		while ($row = $database->fetch_array($result_set)){
			$object_array[] = self::instantiate($row);
		}
		return $object_array;
	}
	
	# instantiate 
	private static function instantiate($record) {
		$object = new self;
		foreach ($record as $attribute=>$value) {
			if($object->has_attribute($attribute)){
				$object->$attribute = $value;
				}
		}
		return $object;
	}
	
	# has attributes
	private function has_attribute($attribute) {
		$object_vars = $this->attributes();
		return array_key_exists($attribute, $object_vars);
	}
	
	# get attributes
	protected function attributes() {
		// return an array of attribute keys and their values
		$attributes = array();
		foreach(self::$db_fields as $field) {
			if(property_exists($this, $field)) {
				$attributes[$field] = $this->$field;
			}
		}
		return get_object_vars($this);
	}	
	
	# sanitize 
	protected function sanitized_attributes() {
		global $database;
		$clean_attributes = array();
		// sanitize the values before submitting
		// Note: does not alter the actual value of each attribute
		foreach ($this->attributes() as $key => $value) {
			$clean_attributes[$key] = $database->escape_value($value);
		}
		return $clean_attributes;
	}
	
	# save instead of create or update
	public function save () {
		// a new row won't have an id
		return isset($this->id) ? $this->update() : $this->create();
	}
	
	public function update() {
		global $database;
		$attributes = $this->sanitized_attributes();
		$attribute_pairs = array();
		foreach ($attributes as $key => $value) {
			$attribute_pairs[] = "{$key}='{$value}'";
		}
		$sql = "UPDATE " . self::$table_name . " SET ";
		$sql .= join(", ", $attribute_pairs);
		$sql .= " WHERE id=" . $database->escape_value($this->id);
		$database->query($sql);
		return ($database->afftected_rows() == 1) ? true : false;
	}

	public function delete() {
		global $database;
		$sql = "DELETE FROM " . self::$table_name . " ";
		$sql .= "WHERE id=" . $database->escape_value($this->id);
		$sql .= " LIMIT 1";
		$database->query($sql);
		return ($database->affected_rows() == 1) ? true : false;
	}
}


?>
