<?php 

	// 1. Start with the original image  
	$source = imagecreatefromjpeg("$orig_img");  
    // $img_magicpink = imagecolorallocatealpha($img, 255, 0, 255, 127);  
    // imagecolortransparent($img, $img_magicpink);  

    // (Get its dimensions for copying)  
    list($orig_w, $orig_h) = getimagesize("$orig_img");

	// 2. copy and crop to a square (164 x 164)  

	$img_w = 164;
	$img_h = 164;
	
	// ($circ_x and y are passed from admin_funcs.php)
	
	$sc_x = $circ_x - 26; // allow for new margin (164 vs. 112, the orig. circle diameter)
	$sc_y = $circ_y - 26;
	
	$copy = imagecreatetruecolor($img_w, $img_h);
	imagecopy($copy, $source, 0, 0, $sc_x, $sc_y, $orig_w, $orig_h);

	// 3. copy reduced square image
	$target_w = 73;
	$target_h = 73;
	
	$img = imagecreatetruecolor($target_w, $target_h);
	imagecopyresampled($img, $copy, 0, 0, 0, 0, $target_w, $target_h, $img_w, $img_h);
	
	$r_magicpink = imagecolorallocatealpha($img, 255, 0, 255, 127);

	// 4. Create the mask  
    $mask = imagecreatetruecolor($target_w, $target_h);  
    imagealphablending($mask, true);

	$mask_black = imagecolorallocate($mask, 0, 0, 0);  
    $mask_magicpink = imagecolorallocate($mask, 255, 0, 255);  
    imagecolortransparent($mask, $mask_black);  
    imagefill($mask, 0, 0, $mask_magicpink);

	// 4-2. Draw the circle for the mask 
	$c_w = 54;  
    $c_h = 54;
	$ctr_c_x = 37;
	$ctr_c_y = 36; 
    imagefilledellipse($mask, $ctr_c_x, $ctr_c_y, $c_w, $c_h, $mask_black);  

	// 5. Copy the mask over the top of the copied image, and apply the mask as an alpha layer  
    imagecopymerge($img, $mask, 0, 0, 0, 0, $target_w, $target_h, 100);
	$img_magicpink = imagecolorallocate($img, 255, 0, 255);
	imagecolortransparent($img, $img_magicpink);
	
	// 6. save the gif image  

	$targetfile = $tmp_folder."/".$picture_name.".gif";
	imagegif($img,"$targetfile");  // final
	
	imagedestroy($img);
	imagedestroy($mask);
	imagedestroy($copy);
	imagedestroy($source);

?>