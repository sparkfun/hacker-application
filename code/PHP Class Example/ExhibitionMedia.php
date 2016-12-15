<?php

/** Code not for re-use. Example code only with permission from the University of Kansas **/

/** This class is part of a project which uses Symfony & Doctrine. It's purpse is interacting/persisting database objects */

namespace AppBundle\Entity;

use Doctrine\ORM\Mapping as ORM;
use Symfony\Component\Validator\Constraints as Assert;

/**
 * @ORM\Entity()
 */
class ExhibitionMedia
	extends BaseMedia {

	/**
	 * @ORM\Column( type="integer", nullable=false )
	 */
	protected $objectID;

	/**
	 * @ORM\ManyToOne( targetEntity="Exhibition", inversedBy="media" )
	 * @ORM\JoinColumn( name="exhibition_id", referencedColumnName="id", nullable=false )
	 *
	 * @var $exhibition Exhibition
	 **/
	protected $exhibition;

	////////////////////////////////////////

	/**
	 * Get objectID
	 *
	 * @return int
	 */
	public function getObjectID() {

		return $this->objectID;
	}

	/**
	 * Set objectID
	 *
	 * @param int $objectID
	 *
	 * @return ArtObject
	 */
	public function setObjectID( $objectID ) {

		if( $this->objectID != $objectID ) {

			$this->objectID = $objectID;
			$this->touched = true;
		}

		return $this;
	}

	/**
	 * Get exhibition
	 *
	 * @return Exhibition
	 */
	public function getExhibition() {

		return $this->exhibition;
	}

	/**
	 * Set artObject
	 *
	 * @param Exhibition $exhibition
	 *
	 * @return ArtObjectMedia
	 */
	public function setExhibition( Exhibition $exhibition = NULL ) {

		if( $exhibition == NULL ) {

			$this->exhibition = NULL;
			$this->touched = true;
		} else if( is_null( $this->exhibition ) || $this->exhibition->getExhibitionID() != $exhibition->getExhibitionID() ) {

			$this->exhibition = $exhibition;

			$this->touched = true;
		}

		return $this;
	}
}