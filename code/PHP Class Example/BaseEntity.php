<?php

/** This class is part of a project which uses Symfony & Doctrine. It's purpse is interacting/persisting database objects */

namespace AppBundle\Entity;

use Doctrine\ORM\Mapping as ORM;
use Symfony\Component\Validator\Constraints as Assert;

/**
 * BaseEntity class. Doctrine handles DB sanitation. Further sanitation should be handled outside of subclasses.
 *
 * @ORM\MappedSuperclass
 * @ORM\HasLifecycleCallbacks
 */
abstract class BaseEntity {

	/**
	 * @var integer
	 * @ORM\Column(name="id", type="integer")
	 * @ORM\Id
	 * @ORM\GeneratedValue(strategy="AUTO")
	 */
	private $id;

	/**
	 * @var integer
	 * @ORM\Column( name="enabled", type="boolean", options={"default":true} )
	 */
	protected $enabled = true;

	/**
	 * @var \DateTime
	 * @ORM\Column( name="created_at", type="datetime", nullable=true )
	 */
	protected $createdAt;

	/**
	 * @var \DateTime
	 * @ORM\Column( name="updated_at", type="datetime", nullable=true )
	 */
	protected $updatedAt;

	abstract public function __toString();

	/**
	 * Method automatically called before initially saving to the database. Will automatically keep track of created time.
	 *
	 * @ORM\PrePersist
	 *
	 * @internal param LifecycleEventArgs $lifeCycleEventArgs
	 */
	public function prePersistEvent() {

		$this->setCreatedAt( new \DateTime )->setUpdatedAt( new \DateTime );
	}

	/**
	 * Method automatically called before saving to the database. Will automatically keep track of last updated time.
	 *
	 * @ORM\PreUpdate
	 *
	 * @internal param PreUpdateEventArgs $preUpdateEventArgs
	 */
	public function preUpdateEvent() {

		$this->setUpdatedAt( new \DateTime() );
	}

	/**
	 * set id
	 *
	 * @param integer $id
	 *
	 * @return BaseEntity
	 */
	public function setId( $id ) {

		$this->id = $id;

		return $this;
	}

	/**
	 * Get id
	 *
	 * @return integer
	 */
	public function getId() {

		return $this->id;
	}

	/**
	 * set enabled
	 *
	 * @param boolean $enabled
	 *
	 * @return BaseEntity
	 */
	public function setEnabled( $enabled ) {

		$this->enabled = $enabled;

		return $this;
	}

	/**
	 * get enabled
	 *
	 * @return integer
	 */
	public function getEnabled() {

		return $this->enabled;
	}

	/**
	 *
	 * @param \DateTime $createdAt
	 *
	 * @return BaseEntity
	 */
	public function setCreatedAt( $createdAt ) {

		$this->createdAt = $createdAt;

		return $this;
	}

	/**
	 * Get createdAt
	 *
	 * @return BaseEntity
	 */
	public function getCreatedAt() {

		return $this->createdAt;
	}

	/**
	 *
	 * @param \DateTime $updatedAt
	 *
	 * @return BaseEntity
	 */
	public function setUpdatedAt( $updatedAt ) {

		$this->updatedAt = $updatedAt;

		return $this;
	}

	/**
	 * Get updatedAt
	 *
	 * @return BaseEntity
	 */
	public function getUpdatedAt() {

		return $this->updatedAt;
	}
}