<?php

/********************************************************* 
**************** HireStarts YouTube Parser ***************
**************** Written by: Tyler Bailey  ***************
**********************************************************/

//Parse YouTube URL and get details about video (thumbnail, title, ID, etc...)
class YoutubeParser{
    var $source    = '';
    var $unique    = false;
    var $suggested = false;
    var $https     = false;
    var $privacy   = false;
    var $width     = 640;
    var $height    = 360;
 
    function __construct(){
    }
 
    function set($key,$val){
        return $this->$key = $val;
    }
    function getall(){
        $return = Array();
        $domain = 'http'.($this->https?'s':'').'://www.youtube'.($this->privacy?'-nocookie':'').'.com';
        $size   = 'width="'.$this->width.'" height="'.$this->height.'"';
 
        preg_match_all('/(youtu.be\/|\/watch\?v=|\/embed\/)([a-z0-9\-_]+)/i',$this->source,$matches);
        if(isset($matches[2])){
            if($this->unique){
                $matches[2] = array_values(array_unique($matches[2]));
            }
            foreach($matches[2] as $key=>$id) {
                $return[$key]['id']       = $id;
                $return[$key]['embed']    = '<iframe '.$size.' src="'.$domain.'/embed/'.$id.($this->suggested?'':'?rel=0').'" frameborder="0" allowfullscreen></iframe>';
                $return[$key]['embedold'] = '<object '.$size.'>
                <param name="movie" value="'.$domain.'/v/'.$id.'?version=3'.($this->suggested?'':'&amp;rel=0').'"></param>
                <param name="allowFullScreen" value="true"></param>
                <param name="allowscriptaccess" value="always"></param>
                <embed src="'.$domain.'/v/'.$id.'?version=3'.($this->suggested?'':'&amp;rel=0').'" type="application/x-shockwave-flash" '.$size.' allowscriptaccess="always" allowfullscreen="true"></embed>
                </object>';
                $return[$key]['thumb']    = 'http://i4.ytimg.com/vi/'.$id.'/default.jpg';
                $return[$key]['hqthumb']  = 'http://i4.ytimg.com/vi/'.$id.'/hqdefault.jpg';
            }
        }
        return $return;
    }
}

?>