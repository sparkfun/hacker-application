<?php
abstract class Wm_Isa_Filter {
    protected $_value;
    protected $_type;
    protected $_label;
    protected $_possible_values;

    /**
     * @var array
     */
    protected $attributes = [];

    public function __construct($value = null) {
        if (is_array($value)) {
            $this->_value = array_map('trim', $value);
        } else {
            $this->_value = trim($value);
        }
    }

    abstract function filter(Zend_Db_Select $select);

    /**
     * @return mixed
     */
    public function getLabel()
    {
        return $this->_label;
    }

    /**
     * @return mixed
     */
    public function getPossibleValues()
    {
        return $this->_possible_values;
    }

    /**
     * @return mixed
     */
    public function getType()
    {
        return $this->_type;
    }

    /**
     * @return string
     */
    public function getValue()
    {
        return $this->_value;
    }

    /**
     * @return mixed
     */
    public function getAttributes()
    {
        return $this->attributes;
    }
}
