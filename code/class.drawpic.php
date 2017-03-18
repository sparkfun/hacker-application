<?php
/* 
############################################################################################
############################################################################################
class.drawpic.php


This code copyright (c)2007 Russell E. Bertolette all rights reserved. rb@bertographics.com 

############################################################################################
############################################################################################

*/
set_time_limit(0);
ini_set("max_execution_time", "10800");
ini_set('memory_limit','512M');
ini_set('output_buffering', 0);
ini_set('implicit_flush', 1);
ob_start();

class drawpic {

	var $history;	
	var $version;	// see below and global_header.php	
	var $sz;		// normal, big, huge
	var $size_x;
	var $size_y;
	var $factor;
	var $offset_x;
	var $offset_y;
	
	var $im_tmp;
	var $thickness;
	var $drawdata;
	var $bl_col;
	var $br_im;
	var $im_dest;
	var $txfl;
	
	/* VERSIONS:
			
			versions change as layout changes, with different size (v.0) 
			and x,y offsets for rendering
		
			0 = 300x350 to 8/2007
			1 = 600x350 to 11/2007
			2 = 600x350 to 7/2011
			3 = 600x350 to 9/2012
			
			now current:
			4 = touch (HTML5) from 7/20/2012
			5 = 600x350 Flash from 9/2012 on new server
		
	*/

	function __construct($history, $version='5', $sz='normal', $txfl=null, $flip=null) {
	
		$this->history = $history;
		$this->version = $version;
		
		switch($sz) {
			case 'normal':
				$factor = 2;
				break;
			case 'big':
				$factor = 4;
				break;
			case 'huge':
				$factor = 8;
				break;	
		}
		$this->sz = $sz;
		$this->factor = $factor;
		
		$this->size_x = ($version > 0) ? 600 * $factor : 300 * $factor; 
		$this->size_y = 350 * $factor;
		
		switch ($version) {
			case 5:
				$offset_x = 170 * $factor;
				$offset_y = 425 * $factor;
				break;
			case 4:
				$offset_x = 0;
				$offset_y = 0;
				break;
			case 3 :
				$offset_x = 170 * $factor;
				$offset_y = 555 * $factor;
				break;
			case 2 :
				$offset_x = 90 * $factor;
				$offset_y = 410 * $factor;
				break;
			case 1 :
				$offset_x = 90 * $factor;
				$offset_y = 250 * $factor;
				break;
			case 0 :
				$offset_x = 100 * factor;
				$offset_y = 250 * factor;
				break;
		}
		$this->offset_x = $offset_x;
		$this->offset_y = $offset_y;
		
		$this->txfl = $txfl;
		$this->flip = $flip;
	}

	
    function renderPic() {

		if (!empty($this->history)) {
		
			// only works for normal - change func above if you create a prog bar for print pix
			$this->writeProgFile("prog=0");  
	
			$drawdata = explode(",",$this->history); 
		
			// make a temporary transparent image to create lines and merge with dest image
		
			$this->im_tmp = imagecreatetruecolor($this->size_x, $this->size_y);
			$bl_col = imagecolorallocate ($this->im_tmp, 0, 0, 0);
			imagecolortransparent($this->im_tmp, $bl_col);
		
			// make a white true color image for dest image
		
			$this->im_dest = imagecreatetruecolor($this->size_x, $this->size_y);
			$white_color = imagecolorallocate($this->im_dest, 255, 255, 255);
			imagefill($this->im_dest, 0, 0, $white_color);
			imageantialias($this->im_dest, true);
			imagealphablending($this->im_dest, true);
		
			// draw it $this->factor x the size
		
			for ($loop=0; $loop < sizeof($drawdata); $loop++) {
			
				$progfrac = floatval($loop / (sizeof($drawdata) - 1));
	
				$new_data = (strpos($drawdata[$loop], " ") !== false) ? explode(" ",$drawdata[$loop]) : false;
				$next_data = ($loop != (sizeof($drawdata) - 1)) ? explode(" ",$drawdata[$loop+1]) : false;
				$old_data = ($loop != 0) ? explode(" ",$drawdata[$loop-1]) : false;
		
				if ($new_data) {
		
					$thickness = ($new_data[0] == "0") ? $this->factor: (round($new_data[0]) * $this->factor);
			
					$x1 = (round($new_data[3]) * $this->factor) - $this->offset_x;
					$y1 = (round($new_data[4]) * $this->factor) - $this->offset_y; 
					$x2 = (round($new_data[5]) * $this->factor) - $this->offset_x; 
					$y2 = (round($new_data[6]) * $this->factor) - $this->offset_y; 
			
					if ((!$old_data) || ((isset($old_data[7]))&&($new_data[7] !== $old_data[7]))) {
						$this->createBrush($thickness, $new_data[1]);
					}
					if ($new_data) {
	
						imageline($this->im_tmp, $x1, $y1, $x2, $y2, IMG_COLOR_BRUSHED);
					}
					
					$this->writeProgFile("prog=".$progfrac);
							
					if ((($new_data) && (!$next_data)) || ((isset($next_data[7]))&&($new_data[7] !== $next_data[7]))) {
						$pct = intval($new_data[2]);
						imagecopymerge ($this->im_dest, $this->im_tmp, 0, 0, 0, 0, $this->size_x, $this->size_y, $pct);
						$this->createTempImg();
					}
								
					ob_flush();
					flush();
				}
			} 
			
			$this->writeProgFile("prog=1.0");	
		
			// declare image name and place image in appropriate folder
		
			srand ((double) microtime () * 1000000);
			$str = "";
			for ($i = 0; $i < 4; ++$i) {
				$str .= chr(rand() % 26 + 97);
			}
			$thisid = uniqid($str);
	
	
			if ($this->sz == 'normal') {		// going into the entries folder on site
				// resample it down to orig. size
	
				$fin_im = imagecreatetruecolor(600, 350);
		
				imageantialias($fin_im, true);
		
				imagealphablending($fin_im, true);
	
				imagecopyresampled ($fin_im, $this->im_dest, 0, 0, 0, 0, 600, 350, 1200, 700);
				
				$targetfile = "images2/".$thisid.".gif";
				imagegif($fin_im,"$targetfile");
				
				imagedestroy ($fin_im);
				
				$_SESSION['pic_url'] = $thisid.".gif";
				
			} else {							// getting downloaded - a printable jpeg
						
				if ($this->flip !== null) {	
					$this->im_dest = imagerotate($this->im_dest, 180, 0);
				}

				$tmpfolder = ((strpos($_SERVER['PHP_SELF'], "admin/") !== false)||($this->flip !== null)) ? "../tmp/" : "tmp/";
				
				$targetfile = $tmpfolder.$thisid.".jpg";
				imagejpeg($this->im_dest,"$targetfile",100);	

				
				$_SESSION['new_pic_url'] = $thisid.".jpg";
			}
		
			// delete the temp images used
		
			imagedestroy($this->br_im);
			imagedestroy($this->im_tmp);
			imagedestroy($this->im_dest);
		
		} else { // error for no history
		
			$this->error("Sorry, your drawing could not be rendered");
		
		}
        
	}
	
		
	private function calcClr($colr) {
		$theVal = array();
		switch ($colr) {
		case "0x000000" :
			$theVal[0] = "1";
			$theVal[1] = "1";
			$theVal[2] = "1";
			break;
		case "0x660099" :
			$theVal[0] = "102";
			$theVal[1] = "0";
			$theVal[2] = "153";
			break;
		case "0x9933CC" :
			$theVal[0] = "153";
			$theVal[1] = "51";
			$theVal[2] = "204";
			break;
		case "0xCC33CC" :
			$theVal[0] = "204";
			$theVal[1] = "51";
			$theVal[2] = "204";
			break;
		case "0xFF99FF" :
			$theVal[0] = "255";
			$theVal[1] = "153";
			$theVal[2] = "255";
			break;
		case "0xFFFFFF" :
			$theVal[0] = "255";
			$theVal[1] = "255";
			$theVal[2] = "255";
			break;
		case "0x66CCFF" :
			$theVal[0] = "102";
			$theVal[1] = "204";
			$theVal[2] = "255";
			break;
		case "0x99FF99" :
			$theVal[0] = "153";
			$theVal[1] = "255";
			$theVal[2] = "153";
			break;
		case "0xFFFF00" :
			$theVal[0] = "255";
			$theVal[1] = "255";
			$theVal[2] = "0";
			break;
		case "0xFFCC00" :
			$theVal[0] = "255";
			$theVal[1] = "204";
			$theVal[2] = "0";
			break;
		case "0xFF6600" :
			$theVal[0] = "255";
			$theVal[1] = "102";
			$theVal[2] = "0";
			break;
		case "0x999999" :
			$theVal[0] = "153";
			$theVal[1] = "153";
			$theVal[2] = "153";
			break;
		case "0xFF0000" :
			$theVal[0] = "255";
			$theVal[1] = "0";
			$theVal[2] = "0";
			break;
		case "0xCC3300" :
			$theVal[0] = "204";
			$theVal[1] = "51";
			$theVal[2] = "0";
			break;
		case "0x003399" :
			$theVal[0] = "0";
			$theVal[1] = "51";
			$theVal[2] = "153";
			break;
		case "0x3366FF" :
			$theVal[0] = "51";
			$theVal[1] = "102";
			$theVal[2] = "255";
			break;
		case "0x00CC00" :
			$theVal[0] = "0";
			$theVal[1] = "204";
			$theVal[2] = "0";
			break;
		case "0x006600" :
			$theVal[0] = "0";
			$theVal[1] = "102";
			$theVal[2] = "0";
			break;
		}
		return ($theVal);
	}
	
	private function createTempImg() {
		if (is_resource($this->im_tmp)) imagedestroy($this->im_tmp);
		$this->im_tmp = imagecreatetruecolor($this->size_x, $this->size_y);
		$this->bl_col = imagecolorallocate ($this->im_tmp, 0, 0, 0);
		imagecolortransparent($this->im_tmp, $this->bl_col);	
	}
	
	private function createBrush($thickness, $clrnum) {
		if (is_resource($this->br_im)) imagedestroy($this->br_im);
		if ((!empty($thickness)) && (!empty($clrnum)) && (!empty($this->im_tmp))) {
			$this->br_im = imagecreatetruecolor($thickness, $thickness);
			$br_blk = imagecolorallocate($this->br_im, 0, 0, 0);
			imagecolortransparent($this->br_im, $br_blk);
			$clr = $this->calcClr($clrnum);
			$br_color = imagecolorallocate($this->br_im, $clr[0], $clr[1], $clr[2]);
			$ct = floor($thickness/2);
			imagefilledellipse ($this->br_im, $ct, $ct, $thickness, $thickness, $br_color);
			imageantialias($this->br_im, true);
			imagesetbrush($this->im_tmp, $this->br_im);
		}
	}
	
	private function writeProgFile($prog_txt) {
		if (($this->sz == 'normal')||($this->flip !== null)) {
			if (!is_file($this->txfl)) {
				touch($this->txfl);
				chmod($this->txfl, 0777);
			}
			$handle = fopen($this->txfl, 'w');
			if ($handle) {
				fwrite($handle, $prog_txt);
				fclose($handle);
			}
		}
	}
	
	// simple error handling

    private function error($message) {
        echo "&nbsp; Error: ".$message."<br>"; 
        return 1;
    }
	
}

?>
