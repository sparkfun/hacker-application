<?php
/**
 * This is a super simple example that I quickly put together just for you...
 * Simple example of formatting a word... can multiselect the formats.
 * The purpose is to demonstrate some very basic fundamental skillz... that wouldn't take you long to see.
 * In the real (virtual?) world: I would probably be using a PHP framework, sanitize form inputs, etc.
 */

class FormatWord
{
    private $word, $format;

    public function __construct($word, $format){
        $this->word = $word;
        $this->format = $format;
    }

    public function formatWord($formatType, $formattedWord = ""){
        if (!empty($formattedWord)){
            $word = $formattedWord;
        }
        else{
            $word = $this->word;
        }

        switch ($formatType) {
            case 'randomize':
                return str_shuffle($word);
                break;
            case 'firstcaps':
                return strtoupper($word);
                break;
            case 'extraspace':
                return implode(' ', str_split($word));
                break;
        }
    }

    public function getFormatWord(){
        $formattedWord = "";

        foreach ($this->format as $formatType){
            if ($formattedWord === $this->word){
                $formattedWord = "";
            }
            $formattedWord = $this->formatWord($formatType, $formattedWord);
        }

        return $formattedWord;
    }
}

if (!empty($_POST)){
    $word = $_POST['word'];
    $format = $_POST['format'];
    $formatWord = new FormatWord($word, $format);
    echo "<h1>" . $formatWord->getFormatWord() . "</h1>";
}
?>

<!DOCTYPE html>
<html>
<body>
<form action="" method="post">
    Word: <input type="text" name="word" value="sparkfun"/>
    <hr/>
    <select multiple name="format[]">
        <option value="randomize">Randomize</option>
        <option value="firstcaps" selected="selected">Capitalize</option>
        <option value="extraspace">Add extra space</option>
    </select>
    <hr/>
    <input type="submit" value="Submit"/>
</form>
</body>
</html>