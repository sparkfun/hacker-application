<?php

class Wm_Isa_Filter_LegalEntity extends Wm_Isa_Filter {
    protected $_type = 'Zend_Form_Element_Text';

    protected $_label = 'Legal Entity';

    protected $_possible_values = array();

    function filter(Zend_Db_Select $select)
    {
        $select->having('legal_entities LIKE (?)', '%' . $this->_value . '%');
    }
}