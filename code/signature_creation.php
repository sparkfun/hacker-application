<?php

/**
 *  This function takes a body of text and converts it into a small image representing a users signature,
 *  it will take options including font, size, and weight. These signatures are used in the PDF creation
 *  process and overlayed onto the signature area, allowing a user to "sign" a document digitally.
 *
 *  I feel like the calculation of the size of the signature (to make sure the signature is as large as
 *  possible without overflowing) is a bit messy, and could be more efficient.
**/

public function generateTypeSignature($text, Wm_Customer $user, $options = array()) {
    $defaultOptions = array(
        'imageSize' => array(500, 200),
        'bgColour'  => array(0xff, 0xff, 0xff),
        'penColour' => array(0x14, 0x53, 0x94),
        'font'      => self::$fonts[0]
    );

    $options = array_merge($defaultOptions, $options);

    $img = imagecreatetruecolor($options['imageSize'][0], $options['imageSize'][1]);
    $bg  = imagecolorallocate($img, $options['bgColour'][0], $options['bgColour'][1], $options['bgColour'][2]);
    $pen = imagecolorallocate($img, $options['penColour'][0], $options['penColour'][1], $options['penColour'][2]);
    imagefill($img, 0, 0, $bg);

    $fontPath = $this->basepath . $options['font'];

    for ($size = 1; $size < 100; $size++) {
        $bbox = imagettfbbox($size, 0, $fontPath, $text);
        $width = abs($bbox[4] - $bbox[0]);
        if ($width - $options['imageSize'][0] > 0) {
            break;
        }
        $lastBbox = $bbox;
    }
    $size -= 5;

    $width = abs($lastBbox[4] - $lastBbox[0]);
    $height = abs($lastBbox[5] - $lastBbox[1]);

    //imagettftext sets the coordinates at the lower left corner of the font baseline; NOT the bottom of the text bounding box
    $x = (($options['imageSize'][0] - $width) / 2) + 15;
    $y = (($options['imageSize'][1] - $height) / 2) + abs($lastBbox[5]);

    imagettftext($img, $size, 0, $x, $y, $pen, $fontPath, $text);
    imagestring($img, 5, 10, $options['imageSize'][1] - 15, $user->getEsignatureHash(true), $pen);

    return $img;
}