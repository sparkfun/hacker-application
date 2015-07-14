<?php

class Wm_Isa_Filter_Factory extends Wm_Factory {
    protected $_filterLibrary = array(
        'customer_name'                       => 'Wm_Isa_Filter_CustomerName',
        'legal_entity'                        => 'Wm_Isa_Filter_LegalEntity',
        'is_active'                           => 'Wm_Isa_Filter_Active',
        'is_archived'                         => 'Wm_Isa_Filter_Archived',
        //'customer_dba'                        => 'Wm_Isa_Filter_CustomerDba',
        'customer_signatory'                  => 'Wm_Isa_Filter_CustomerSignatory',
        'wm_signatory'                        => 'Wm_Isa_Filter_WmSignatory',
        'auto_renewal'                        => 'Wm_Isa_Filter_AutoRenewal',
        'one_time_event'                      => 'Wm_Isa_Filter_OneTimeEvent',
        'national_account'                    => 'Wm_Isa_Filter_NationalAccount',
        'effective_date'                      => 'Wm_Isa_Filter_EffectiveDate',
        'expiration_date'                     => 'Wm_Isa_Filter_ExpirationDate',
        'national_account_number'             => 'Wm_Isa_Filter_NationalAccountNumber',
        'national_account_type'               => 'Wm_Isa_Filter_NationalAccountType',
        'surcharge_exclusions'                => 'Wm_Isa_Filter_SurchargeExclusions',
        'wm_facility_exclusions'              => 'Wm_Isa_Filter_WmFacilityExclusions',
        'third_party_facility_exclusions'     => 'Wm_Isa_Filter_ThirdPartyFacilityExclusions',
        'language'                            => 'Wm_Isa_Filter_Language',
        'has_wm_facility_exclusions'          => 'Wm_Isa_Filter_HasWmFacilityExclusions',
        'has_third_party_facility_exclusions' => 'Wm_Isa_Filter_HasThirdPartyFacilityExclusions',
        'customer_name_or_dba'                => 'Wm_Isa_Filter_CustomerNameOrDba',
        'is_refused'                          => 'Wm_Isa_Filter_Refused',
        'status'                              => 'Wm_Isa_Filter_Status',
        'type'                                => 'Wm_Isa_Filter_Type',
    );

    /**
     * @var array filter_name => filter_value
     */
    protected $_defaultFilters = array(
        'is_archived' => 0
    );

    /**
     * @var array filter_name => filter_value
     */
    protected $_mandatoryFilters = array(
        'is_active' => 1
    );

    /**
     * $filterset expects an array of filterName => filterValue
     *
     * @param array $filterset
     * @param null  $db
     */
    public function __construct($filterset = array(), $db = null)
    {
        parent::__construct($db);

        if (is_array($filterset)) {
            $this->_defaultFilters = array_merge($this->_defaultFilters, $filterset);
        }
    }

    /**
     * @param $filterName
     * @param null $filterValue
     * @return mixed
     * @throws Exception
     */
    public function getFilter($filterName, $filterValue = null) {
        if (!isset($this->_filterLibrary[$filterName])) {
            throw new Exception(sprintf("Trying to get filter that does not exist. Given %s", $filterName));
        }

        if (!class_exists($this->_filterLibrary[$filterName])) {
            throw new Exception(sprintf("ISA Filter does not exist. Given %s", $this->_filterLibrary[$filterName]));
        }

        if (is_null($filterValue)) {
            if (isset($this->_defaultFilters[$filterName])) {
                $filterValue = $this->_defaultFilters[$filterName];
            }

            if (isset($this->_mandatoryFilters[$filterName])) {
                $filterValue = $this->_mandatoryFilters[$filterName];
            }
        }

        return new $this->_filterLibrary[$filterName]($filterValue);
    }

    /**
     * @param $filterName
     * @return string
     */
    public function getDefaultValue($filterName)
    {
        if (isset($this->_mandatoryFilters[$filterName])) {
            return $this->_mandatoryFilters[$filterName];
        }

        if (isset($this->_defaultFilters[$filterName])) {
            return $this->_defaultFilters[$filterName];
        }

        return '';
    }

    /**
     * Returns the set of set of filters requested (by filter name) or all filters if no names were provided
     *
     * @param array $filters
     * @return array
     */
    public function getFilters(array $filters = array()) {
        if (empty($filters)) {
            $filters = array_keys($this->_filterLibrary);
        }

        $filters_prepared = array();
        foreach ($filters as $filter) {
            $filters_prepared[$filter] = $this->getFilter($filter);
        }

        return $filters_prepared;
    }

    /**
     * @param $filterName
     * @return bool
     */
    public function hasFilter($filterName) {
        return isset($this->_filterLibrary[$filterName]);
    }

    /**
     * Gets the filter set based on provided parameters from the request POST data
     *
     * @param array $params
     * @return Wm_Isa_Filter[]
     */
    public function getFilterSet(array $params) {

        /** Get rid of params that are not in the filter library */
        $params = array_intersect_key($params, $this->_filterLibrary);

        /** Filter out empty values */
        $params = $this->filterParams($params);

        /** Grab the default filter values, making sure that values of $params overwrites them */
        $params = array_merge($this->_defaultFilters, $params);

        /** This makes sure we always grab the mandatory filters too */
        $params = array_merge($params, $this->_mandatoryFilters);

        $filters = array();
        foreach ($params as $filter => $value) {
            if (is_array($value)) {
                $value = array_filter($value);

                if (empty($value)) {
                    continue;
                }
            }
            $filters[$filter] = $this->getFilter($filter, $value);
        }

        return $filters;
    }

    public function filterParams($input)
    {
        foreach ($input as &$value)
        {
            if (is_array($value))
            {
                $value = $this->filterParams($value);
            }
        }

        return array_filter($input, function($val) {
                if (is_array($val) && empty($val)) {
                    return false;
                }
                return $val !== '' && $val !== null;
            });
    }
}
