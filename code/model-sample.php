<?php defined('SYSPATH')  or die('No direct script access.');

class Model_Video extends ORM {

	// Relationships
	protected $_has_one = array('media' => array());
	protected $_has_many = array('videochapters' => array());

	/*
	Video HTTP URL
	Returns the HTTP CDN URL for videos.
	*/
	public function http_url($stream = TRUE) {
        $a = 'http://0f837d73c52260b6ff9d-eaef829eae7c04fd12005cc3ad780db0.r48';
        $b = '.cf1.rackcdn.com/';

        return $a . $b;
	}

	public function https_url($stream = TRUE) {
		$a = 'https://424ab3360cd45b4ab42b-eaef829eae7c04fd12005cc3ad780db0.ssl.cf1.rackcdn.com/';
		return $a;
	}

	/*
	API JSON Object
	Generates a JSON compatible object for this video.
	Includes all pertinent information for video.
	*/
	public function api_json_object() {
		$return_object = new StdClass;
		
		$return_object->http_url = $this->http_url();
		$return_object->v270p = $this->v270p_file;
		$return_object->v360p = $this->v360p_file;
		$return_object->v480p = $this->v480p_file;
		$return_object->v720p = $this->v720p_file;
		$return_object->v1080p = $this->v1080p_file;

		return $return_object;
	}


	/*
	Generate Hash Code
	Generates this video's hash code based on the following factors:
	id, media_id, timestamp
	*/
	public function generate_hash() {
		$media_hash = base_convert($this->media_id, 10, 32);
		$image_hash = base_convert($this->id, 10, 32);
		$time_hash = substr(md5($this->timestamp),0,8);

		return $media_hash.$time_hash.$image_hash;
	}

	/*
	Delete All Files
	Deletes all files directly tied to this media.
	*/
	public function delete_all_files() {
		if ($this->preserve_files )
			return TRUE;

		try {
			Helper_Cloudfiles::Connect();
			$container = Kohana::config('site.name.abbreviation') . '_video';

			if ($this->v270p_file) {
				Helper_Cloudfiles::DeleteFile($container, $this->v270p_file);
				$this->v270p_file = NULL;
			}

			if ($this->v360p_file) {
				Helper_Cloudfiles::DeleteFile($container, $this->v360p_file);
				$this->v360p_file = NULL;
			}

			if ($this->v480p_file) {
				Helper_Cloudfiles::DeleteFile($container, $this->v480p_file);
				$this->v480p_file = NULL;
			}

			if ($this->v720p_file) {
				Helper_Cloudfiles::DeleteFile($container, $this->v720p_file);
				$this->v720p_file = NULL;
			}

			if ($this->v1080p_file) {
				Helper_Cloudfiles::DeleteFile($container, $this->v1080p_file);
				$this->v1080p_file = NULL;
			}

			$this->save();
			return TRUE;

		// Error!
		} catch (Exception $e) {
			$this->save();
			return FALSE;
		}
	}

	/*
	Video Length
	returned in x:xx format
	*/
	public function length() {
        if ($this->length)
		    return Helper_Wellcomemat::format_time($this->length);
        else
            return 'unknown';
	}

    public function length_iso_8601() {
        $duration = $this->length;
        $hours = floor($duration / 3600);
        $minutes = floor(($duration - ($hours * 3600)) / 60);
        $seconds = floor($duration % 60);
        $value = array('PT');

        if ($hours >= 1)
            $value[] = $hours . 'H';
        if ($minutes >= 1)
            $value[] = $minutes . 'M';
        if ($seconds > 0) {
            $value[] = $seconds;
        } else {
            $value[] = 0;
        }

        $value[] = 'S';
        return join('', $value);
    }

    public function storage_size_bytes() {
        return $this->storage_size * 1024;
    }
}

?>
