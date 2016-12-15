<?php

/** Code not for re-use. Example code only with permission from the University of Kansas **/

/** This class is part of a project which uses Symfony & Doctrine. It's purpse is interacting/persisting database objects */

namespace AppBundle\Entity;

use Doctrine\ORM\Mapping as ORM;
use EWD\CoreBundle\Entity\BaseEntity;
use Symfony\Component\Validator\Constraints as Assert;

/**
 * BaseMedia class. Contains common variables for all Media types.
 *
 * @ORM\MappedSuperclass
 */
abstract class BaseMedia
	extends BaseEntity {

	/**
	 * @ORM\Column( type="string", length=32, nullable=true )
	 */
	protected $linkID;

	/**
	 * @ORM\Column( type="string", length=32, nullable=true )
	 */
	protected $name;

	/**
	 * @ORM\Column( type="string", length=128, nullable=true )
	 */
	protected $path;

	/**
	 * @ORM\Column( type="string", length=8, nullable=true )
	 */
	protected $ext;

	/**
	 * @ORM\Column( type="string", length=32, nullable=true )
	 */
	protected $typeDesc;

	/**
	 * @ORM\Column( type="text", nullable=true )
	 */
	protected $description;

	/**
	 * Save changed status that is accessible to user.
	 *
	 * @var bool
	 */
	protected $touched = false;

	////////////////////////////////////////

	/**
	 * True if entity modified
	 *
	 * @return bool
	 */
	public function isTouched() {

		return $this->touched;
	}

	public function __toString() {

		return $this->getName();
	}

	////////////////////////////////////////

	/**
	 * Get name
	 *
	 * @return string
	 */
	public function getName() {

		return $this->name;
	}

	/**
	 * Set name
	 *
	 * @param string $name
	 *
	 * @return BaseMedia
	 */
	public function setName( $name ) {

		if( $this->name != $name ) {

			$this->name = $name;
			$this->touched = true;
		}

		return $this;
	}

	/**
	 * Get linkID
	 *
	 * @return string
	 */
	public function getLinkID() {

		return $this->linkID;
	}

	/**
	 * Set linkID
	 *
	 * @param string $linkID
	 *
	 * @return BaseMedia
	 */
	public function setLinkID( $linkID ) {

		if( $this->linkID != $linkID ) {

			$this->linkID = $linkID;
			$this->touched = true;
		}

		return $this;
	}

	/**
	 * Get path
	 *
	 * @return string
	 */
	public function getPath() {

		return $this->path;
	}

	/**
	 * Set path
	 *
	 * @param string $path
	 *
	 * @return BaseMedia
	 */
	public function setPath( $path ) {

		if( $this->path != $path ) {

			$this->path = $path;
			$this->touched = true;
		}

		return $this;
	}

	/**
	 * Get ext
	 *
	 * @return string
	 */
	public function getExt() {

		return $this->ext;
	}

	/**
	 * Set ext
	 *
	 * @param string $ext
	 *
	 * @return BaseMedia
	 */
	public function setExt( $ext ) {

		if( $this->ext != $ext ) {

			$this->ext = $ext;
			$this->touched = true;
		}

		return $this;
	}

	/**
	 * Get typeDesc
	 *
	 * @return string
	 */
	public function getTypeDesc() {

		return $this->typeDesc;
	}

	/**
	 * Set typeDesc
	 *
	 * @param string $typeDesc
	 *
	 * @return BaseMedia
	 */
	public function setTypeDesc( $typeDesc ) {

		if( $this->typeDesc != $typeDesc ) {

			$this->typeDesc = $typeDesc;
			$this->touched = true;
		}

		return $this;
	}

	/**
	 * Get description
	 *
	 * @return string
	 */
	public function getDescription() {

		return $this->description;
	}

	/**
	 * Set description
	 *
	 * @param string $description
	 *
	 * @return BaseMedia
	 */
	public function setDescription( $description ) {

		if( $this->description != $description ) {

			$this->description = $description;
			$this->touched = true;
		}

		return $this;
	}
}