<?php

class Wm_Isa_Filter_CustomerDba extends Wm_Isa_Filter {

    protected $_type = 'Zend_Form_Element_Text';

    protected $_label = 'DBA';

    protected $_possible_values = array();

    function filter(Zend_Db_Select $select)
    {
        $select->where('dba.dba_name LIKE (?)', '%' . $this->_value . '%');
    }
}