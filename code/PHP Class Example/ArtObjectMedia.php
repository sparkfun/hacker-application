<?php

/** Code not for re-use. Example code only with permission from the University of Kansas **/

/** This class is part of a project which uses Symfony & Doctrine. It's purpse is interacting/persisting database objects */

namespace AppBundle\Entity;

use Doctrine\ORM\Mapping as ORM;
use Symfony\Component\Validator\Constraints as Assert;

/**
 * @ORM\Entity()
 */
class ArtObjectMedia
	extends BaseMedia {

	/**
	 * @ORM\Column( type="text", nullable=true )
	 */
	protected $transcript;

	/**
	 * @ORM\ManyToOne( targetEntity="ArtObject", inversedBy="media" )
	 * @ORM\JoinColumn( name="artObj_id", referencedColumnName="id", nullable=false )
	 *
	 * @var $artObject ArtObject
	 **/
	protected $artObject;

	////////////////////////////////////////

	/**
	 * Get artObject
	 *
	 * @return ArtObject
	 */
	public function getArtObject() {

		return $this->artObject;
	}

	/**
	 * Set artObject
	 *
	 * @param ArtObject $artObject
	 *
	 * @return ArtObjectMedia
	 */
	public function setArtObject( ArtObject $artObject ) {

		if( is_null( $this->artObject ) || $this->artObject->getObjectID() != $artObject->getObjectID() ) {

			$this->artObject = $artObject;

			$this->touched = true;
		}

		return $this;
	}

	/**
	 * @return string
	 */
	public function getTranscript() {

		return $this->transcript;
	}

	/**
	 * @param string $transcript
	 *
	 * @return ArtObjectMedia
	 */
	public function setTranscript( $transcript ) {

		$this->transcript = $transcript;

		return $this;
	}
}