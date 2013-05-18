<?php
/**
 * vim: set expandtab tabstop=4 shiftwidth=4 autoindent smartindent:
 *
 * Generator for Credit Card numbers. This is for use of testing credit card processing
 * applications
 */

class GenerateCC {
    private $prefix;
    private $length;
    
    public function __construct($type = 'visa') {
        switch($type) {
            case 'disc':
            case 'discover':
                $picker = mt_rand(1,4);
                switch($picker) {
                    case '1':
                        $this->prefix = '6011';
                        break;
                    case '2':
                        $this->prefix = mt_rand(622126, 622925);
                        break;
                    case '3':
                        $this->prefix = mt_rand(644, 649);
                        break;
                    case '4':
                        $this->prefix = '65';
                        break;
                }

                $this->length = 16;
                break;
            case 'amex':
                $this->prefix = '3' . ((mt_rand(1,10) > 5) ? '4' : '7');
                $this->length = 15;
                break;
            case 'mc':
                $this->prefix = '5' . mt_rand(1, 5);
                $this->length = 16;
                break;
            default:
            case 'visa':
                $this->prefix = '4';
                $this->length = 16;
        }
    }
    
    private function is_luhn($pan) {
        $sum = 0;
        $i = strlen($pan); // Find the last character
        $o = $i % 2; // Shift odd/even for odd-length $s

        while($i-- > 0) {
            //work from right-to-left & sum the digits
            $sum += $pan[$i];
        
            //digit position is even, add it again. Adjust for digits 10+ by subtracting 9.
            if($o == ($i % 2)) {
                if($pan[$i] > 4) {
                    $sum += ($pan[$i] - 9);
                }
                else {
                    $sum += $pan[$i];
                }
            }
        }
        
        if(($sum % 10) != 0) {
            return false;
        }
        
        return true;
    }
    
    private function genDigits($num) {
        $digits = '';
        $pool = '0123456789';
        for($i = 0; $i < $num; $i++) {
            $digits .= $pool[mt_rand(0, 9)];
        }
        return $digits;
    }
    
    public function getPAN() {
        $pan = '';
        do {
            $pan = $this->prefix;
            $pan .= $this->genDigits($this->length - strlen($pan));
        }
        while(!$this->is_luhn($pan));
        //while(false);

        return $pan;
    }
};

//testing
if(PHP_SAPI == 'cli') {
    $cc = new GenerateCC();
    echo 'Generated: ' . $cc->getPAN() . PHP_EOL;
}
