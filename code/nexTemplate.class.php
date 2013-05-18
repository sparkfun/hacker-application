<?php
/**
 * vim: set expandtab tabstop=4 shiftwidth=4 autoindent smartindent:
 *
 * Nextraztus Industries simple PHP templating library
 *
 * @author       Sam Torno <sam@samtorno.com>
 * @copyright    2012 (c) Nextraztus Industries
 * @license      New BSD, see LICENSE
 * @file
 * 
 */

class nexTemplate {
	private $pieces;
	private $template;
	private $leftDelim;
	private $rightDelim;

	/**
	 * The template library requires a source template to instantiate, it will attempt
	 * to load the file from disk. The first block of text in a nex-template is metadata
	 * that will be stripped out. It allows you to set required pieces before render.
	 *
	 * @param string $file a path to the file to load
	 * @param string $type describes the type of template to expect (html|css|js)
	 * @throws InvalidArgumentException if the file could not be loaded
	 */  
	public function __construct($file, $type = 'html') {
		//check file type
		switch($type) {
			case 'html':
				$this->leftDelim = '<!--[[nt-';
				$this->rightDelim = ']]-->';
                break;
			case 'css':
			case 'js':
				$this->leftDelim = '/*[[nt-';
				$this->rightDelim = ']]*/';
				break;
			default:
				throw new InvalidArgumentException('"' . $type . '" is not an acceptable type.');
		}

		$this->pieces = array();
        
        if(is_null($file)) {
            $this->template = '';
            return;
        }

		if($filepath = realpath($file)) {
			$this->template = file_get_contents($filepath);
		}
		else {
			throw new InvalidArgumentException('File not resolvable.');
		}

		if($this->template === false) {
			throw new InvalidArgumentException('File could not be read.');
		}
	}

    public function setTemplate($value) {
        $this->template = $value;
        
        return $this;
    }

	public function setPiece($key, $value) {
		$this->pieces[$key] = $value;

		return $this;
	}

	public function clearPiece($key) {
		unset($this->pieces[$key]);

		return $this;
	}

	/**
	 * Render with filter will allow the user of the object to specify a filter to
	 * to run the output through. Their function should accept a single parameter
	 * which is the search/replaced text in it's entirety.
	 *
	 * @param callable $filter the callback to pass the rendered data to
	 * @return the result of the callback
	 */
	public function renderWithFilter($filter) {
		return call_user_func($filter, $this->render(true));
	}

	/**
	 * Produce the rendered text with interpolations done.
	 *
	 * @param boolean $ret if ret is true, returns instead of echo
	 * @return void|string either returns nothing and echos, or the interpolated text
	 */
	public function render($ret = false) {
        //if we haven't given it any pieces, no point in replacing
        if(count($this->pieces) < 1) {
            if($ret) {
                return $this->template;
            }
            else {
                echo $this->template;
                return;
            }
        }
		
        $searches = array();
		$replaces = array();

		foreach($this->pieces as $key => $value) {
			array_push($searches, $this->leftDelim . $key . $this->rightDelim);
			array_push($replaces, $value); 
		}

		if($ret) {
			return str_replace($searches, $replaces, $this->template);
		}
		else {
			echo str_replace($searches, $replaces, $this->template);
		}
	}
};


/*
  BASIC USAGE

  $t = new nexTemplate('somefile.tmpl', 'html');
  $t->setPiece('somevar', $var);
  $t->render();
*/
