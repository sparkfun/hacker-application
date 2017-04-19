<?php

/********************************************************* 
**************** String Formatting Class *****************
**************** Written by: Tyler Bailey ****************
**********************************************************/


/* Formats Strings */
class str_format
{

	public function __construct() { }

	// truncate strings if they are longer than specified
	public function truncate($string, $limit, $break=".", $pad="... ") { 
	
		// return with no change if string is shorter than $limit 
		if(strlen($string) <= $limit) return $string;
		
		// is $break present between $limit and the end of the string? 
		if ( false !== ($breakpoint = strpos($string, $break, $limit))) {
			if($breakpoint < strlen($string) - 1) {
				$string = substr($string, 0, $breakpoint) . $pad;
			}
		}
		return $string;
	}
	
	//SEO Friendly URLS
	public function Seo($input){
		$input = str_replace(array("'", "-"), "", $input); 
		$input = mb_convert_case($input, MB_CASE_LOWER, "UTF-8"); 
		$input = preg_replace("#[^a-zA-Z0-9]+#", "-", $input); 
		$input = preg_replace("#(-){2,}#", "$1", $input); 
		$input = trim($input, "-"); 
		return $input; 
	}

	// make file size indicators look pretty
	public function bytesToSize1024($bytes, $precision = 2) {
		$unit = array('B','KB','MB');
		return @round($bytes / pow(1024, ($i = floor(log($bytes, 1024)))), $precision).' '.$unit[$i];
	}

	// converting time to "time-ago"
	public function time_ago($post_time){
		$time_ = time() - $post_time;
		 
		$seconds =$time_;
		$minutes = round($time_ / 60);
		$hours = round($time_ / 3600);
		$days = round($time_ / 86400);
		$weeks = round($time_ / 604800);
		$months = round($time_ / 2419200);
		$years = round($time_ / 29030400);
		 
		//Seconds
		if($seconds <= 60){
		 
		$time="$seconds seconds ago";   
		 
		//Minutes    
		}else if($minutes <= 60){
		 
			if($minutes == 1){
				$time="one minute ago";
			}else{
				$time="$minutes minutes ago";
			}
		 
		//Hours
		}else if($hours <= 24){
		 
			if($hours == 1){
				$time="one hour ago";
			}else{
				$time="$hours hours ago";
			}
		 
		//Days 
		}else if($days <= 7){
		 
			if($days == 1){
				$time="one day ago";
			}else{
				$time="$days days ago";
			}
		
		}else{
			//if more than 1 week ago, display the full date instead of just "1 Week Ago"
			$time = date('F jS, Y - g:ia', strtotime($post_time));
			 
		}
		return $time;
	}
	


}

?>