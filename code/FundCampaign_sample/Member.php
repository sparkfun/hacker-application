<?php
namespace FundCampaign\app\components\Models;

/**
 * Member
 * @author William Barstad
 * @package FundCampaign\app\components\Models
 */
class Member
{
    /**
     * @property $m -  memberID
     * @var int
     */
    private $m = 0;

    /**
     * @property $u -  memberUsername
     * @var string
     */
    private $u = "";

    /**
     * @property $fn -  memberFirstName
     * @var string
     */
    private $fn = "";

    /**
     * @property $ln -  memberLastName
     * @var string
     */
    private $ln = "";

    /**
     * @property $em -  email
     * @var string
     */
    private $em = "";

    /**
     * @method __construct
     * @param memberID
     * @var int
     */
    public function __construct($m)
    {
        $this->m = $m;    //memberID

    }

    /**
     * @method getMemberId
     * @return int
     */
    public function getMemberId()
    {
        return $this->m;
    }

    /**
     * @method getFirstName
     * @return string
     */
    public function getFirstName()
    {
        return $this->fn;
    }

    /**
     * @method setFirstName
     * @param $fn
     */
    public function setFirstName($fn)
    {
        $this->fn = $fn;
    }

    /**
     * @method getLastName
     * @return string
     */
    public function getLastName()
    {
        return $this->ln;
    }

    /**
     * @method setLastName
     * @param $ln
     */
    public function setLastName($ln)
    {
        $this->ln = $ln;
    }

    /**
     * @method getEmail
     * @return string
     */
    public function getEmail()
    {
        return $this->em;
    }

    /**
     * @method setEmail
     * @param $em
     */
    public function setEmail($em)
    {
        $this->em = $em;
    }

    public function __destruct()
    {
        unset($this->m);
        unset($this->u);
        unset($this->fn);
        unset($this->ln);
        unset($this->em);
    }

}