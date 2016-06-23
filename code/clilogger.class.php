<?php
namespace rp\phpcli;

/*****
 * Example Class
 *
 * This is a open source class I wrote to manage logging in a PHP CLI application.
 *
 * Thank you for your consideration!
 *****/

class Logger
{

    // Log Levels
    const DEBUG   = 0; // Debug information only
    const INFO    = 1; // General information from the script like whats going on and variables
    const NOTICE  = 2; // For such messages as script start/end
    const WARNING = 3; // General warnings
    const ERROR   = 4; // Error messages
    const FATAL   = 5; // Fatal, end script
    const DISABLE = 6; // Disable normal logging

    private $log_level_name = array(
        self::DEBUG   => 'DEBUG',
        self::INFO    => 'INFO',
        self::NOTICE  => 'NOTICE',
        self::WARNING => 'WARN',
        self::ERROR   => 'ERROR',
        self::FATAL   => 'FATAL',
    );

    const NAGIOS_OK       = 0;
    const NAGIOS_WARN     = 1;
    const NAGIOS_CRITICAL = 2;
    const NAGIOS_UNKNOWN  = 3;

    private $nagios_status_name = array(
        self::NAGIOS_OK       => 'OK',
        self::NAGIOS_WARN     => 'WARN',
        self::NAGIOS_CRITICAL => 'CRITICAL',
        self::NAGIOS_UNKNOWN  => 'UNKNOWN',
    );

    //custom error codes
    const E_DB_CONNECT = -10; // -10 has no real significance, just not already taken by another error code

    private $debug_vals = array();

    private $log_level = self::INFO;
    private $err_level = self::NOTICE;

    private $var_padding = 15;

    private $timezone;

    public function __construct() {
        if(!defined('STDIN'))  define('STDIN',  fopen('php://stdin',  'r'));
        if(!defined('STDOUT')) define('STDOUT', fopen('php://stdout', 'w'));
        if(!defined('STDERR')) define('STDERR', fopen('php://stderr', 'w'));

        $this->timezone = new \DateTimeZone('UTC');
    }

    public function setTimeZone($value) {
        $this->timezone = $value;
    }

    public function setLogLevel($value) {
        $value = (int)$value;
        if (self::DEBUG <= $value && $value <= self::DISABLE) {
            $this->log_level = $value;
        }
    }

    public function setErrLevel($value) {
        $value = (int)$value;
        if (self::DEBUG <= $value && $value <= self::DISABLE) {
            $this->err_level = $value;
        }
    }

    public function setPadding($value) {
        $value = (int)$value;
        if ($value > 0) {
            $this->var_padding = $value;
        }
    }

    // Get display name for type of error
    // Return: string
    private function getErrorType($errno)
    {
        $error_types = array (
            E_ERROR             => 'Error',
            E_WARNING           => 'Warning',
            E_PARSE             => 'Parsing Error',
            E_NOTICE            => 'Notice',
            E_CORE_ERROR        => 'Core Error',
            E_CORE_WARNING      => 'Core Warning',
            E_COMPILE_ERROR     => 'Compile Error',
            E_COMPILE_WARNING   => 'Compile Warning',
            E_USER_ERROR        => 'User Error',
            E_USER_WARNING      => 'User Warning',
            E_USER_NOTICE       => 'User Notice',
            E_STRICT            => 'Runtime Notice',
            self::E_DB_CONNECT  => 'DB Connect Error',
       );

        //get error type
        $error_type_name = 'Unknown Error';
        if (isset($error_types[$errno])) {
            $error_type_name = $error_types[$errno];
        }

        return $error_type_name;
    }

    // Figure out what severity level based on the type of PHP error being thrown
    // Return: string
    private function getSeverityLevel($errno, $errstr)
    {
        switch ($errno) {
            case E_ERROR:
            case E_PARSE:
            case E_CORE_ERROR:
            case E_COMPILE_ERROR:
            case self::E_DB_CONNECT:
                return self::FATAL;

            case E_USER_ERROR:
                return self::ERROR;

            case E_WARNING:
                if (preg_match('/ORA-/', $errstr)) {
                    return self::ERROR;
                }
                return self::WARNING;

            case E_CORE_WARNING:
            case E_COMPILE_WARNING:
            case E_NOTICE:
            case E_USER_WARNING:
            case E_USER_NOTICE:
            case E_STRICT:
            case E_DEPRECATED:
                return self::WARNING;

            break;
        }
        return self::ERROR;
    }

    // Return: string
    private function getDebugValStr()
    {

        $vals = '';

        if (!empty(self::$debug_vals)) {
            foreach (self::$debug_vals as $key => $value) {
                $vals .= "$key=";
                if (is_array($value)) {
                    $vals .= var_export($value, true);
                } else {
                    $vals .= $value;
                }
                $vals .= ', ';
            }
            $vals = substr($vals, 0, (strlen($vals)-2));
        }
        return $vals;
    }


    // Tries to determine the correct root cause of error in backtrace, and returns key to that error
    // Return: int
    private function findErrorOrigin($backtrace)
    {
        $origin_key = (count($backtrace)-1);

        //try to find the original error
        //skip over include/require statements
        $functions_to_skip = array('require', 'require_once', 'include', 'include_once');
        for ($i=$origin_key; $i>=0; $i--) {
            if (isset($backtrace[$i]['function']) &&
                    !in_array($backtrace[$i]['function'], $functions_to_skip) &&
                    isset($backtrace[$i]['file']) && isset($backtrace[$i]['line'])) {
                $origin_key = $i;
                break(1);
            }
        }

        return $origin_key;
    }


    // Add value to debug array, that gets included in error output if there's an error
    // Return: void
    public function addDebugValue($key, $value)
    {
        self::$debug_vals[$key] = $value;
    }

    // Logger error handler
    public function errorHandler($errno, $errstr, $errfile, $errline)
    {
        //ignore errors suppressed by '@'
        $error_level = error_reporting();
        if ($error_level == 0) {
            return;
        }

        $error['error'] = strip_tags($errstr);
        $error['level'] = self::getSeverityLevel($errno, $errstr);

        // If fatal or error, throw an exception, which always stops script execution
        if ($error['level'] == self::FATAL || $error['level'] == self::ERROR) {
            throw new \ErrorException($errstr, 0, $errno, $errfile, $errline);
        } else {
            $error['type'] = self::getErrorType($errno);
            $error['info'] = self::getDebugValStr();
            $error['file'] = $errfile;
            $error['line'] = $errline;

            $backtrace = debug_backtrace();

            //find true error from back-trace, replace error if it's different
            $origin_key = self::findErrorOrigin($backtrace);
            if (isset($backtrace[$origin_key]['file']) &&
                    isset($backtrace[$origin_key]['line']) &&
                    $backtrace[$origin_key]['file']!=$error['file'] &&
                    $backtrace[$origin_key]['line']!=$error['line']) {
                $origin_file = $backtrace[$origin_key]['file'];
                $origin_line = $backtrace[$origin_key]['line'];
            }

            //if this is a warning (or below), just log it and return, don't stop script execution
            $log_vals['file'] = $error['file'];
            $log_vals['line'] = $error['line'];
            if (isset($origin_file)) {
                $log_vals['origin'] = "$origin_file($origin_line)";
            }
            $log_vals['info'] = self::getDebugValStr();
            self::log($error['level'], $errstr, $log_vals);

            return;
        }
    }

    // Logger exception handler
    public function exceptionHandler($e)
    {
        restore_error_handler();

        $error['error'] = strip_tags($e->getMessage());
        $error['level'] = self::FATAL; //all exceptions are fatal!
        $error['info'] = self::getDebugValStr();
        $error['file'] = $e->getFile();
        $error['line'] = $e->getLine();

        $backtrace = $e->getTrace();

        // Find true error from backtrace, replace error if it's different
        $origin_key = self::findErrorOrigin($backtrace);
        if (isset($backtrace[$origin_key]['file']) &&
                isset($backtrace[$origin_key]['line']) &&
                $backtrace[$origin_key]['file'] != $error['file'] &&
                $backtrace[$origin_key]['line'] != $error['line']) {
            $origin_file = $backtrace[$origin_key]['file'];
            $origin_line = $backtrace[$origin_key]['line'];
        }

        // Set values for logging
        $log_vals['file'] = $error['file'];
        $log_vals['line'] = $error['line'];
        if (isset($origin_file)) {
            $log_vals['origin'] = "$origin_file($origin_line)";
        }
        $log_vals['info'] = $error['info'];

        self::log($error['level'], $error['error'], $log_vals);

        exit();
    }

    // One line nagios exit
    public function logNagios($status, $msg) {
        $status = (int)$status;

        $msg = $this->nagios_status_name[$status].' - '.$msg;

        $this->logExit($status, $msg);
    }

    // One line exit call
    public function logExit($status, $msg) {
        $status = (int)$status;

        fwrite(STDOUT, $msg);
        exit($status);
    }

    // Add a line break to the log
    public function logEcho($level = STDOUT, $text, $newline = true) {
        if ($newline) {
            $text = "\n".$text;
        }
        $this->outputToLog($level, $text);
    }

    // Add a line break to the log
    public function logBreak($level = STDOUT) {
        $this->outputToLog($level, "\n");
    }

    // Format and output to stdout/stderr
    public function log($level = self::INFO, $msg, $vals = null)
    {
        $date = new \DateTime("now", $this->timezone);
        $output = "\n[".$date->format('Y-m-d H:i:s T')."] ".str_pad("[".$this->log_level_name[$level], 7, ' ', STR_PAD_LEFT)."]: ";

        if (is_array($vals)) {
            if (isset($vals['file']) && isset($vals['line'])) {
                $output .= $vals['file'].' ('.$vals['line'].")\n";
            }
            if (isset($vals['file'])) unset($vals['file']);
            if (isset($vals['line'])) unset($vals['line']);
        }

        $output .= $msg;

        if (!empty($vals)) {
            if (is_array($vals) || is_object($vals)) {
                $output .= "\n".print_r($vals, true);
            } else {
                $output .= " (".$vals.")";
            }
        }

        $this->outputToLog($level, $output);
    }

    private function outputToLog($level = self::INFO, $text) {
        if ($level === STDOUT || $level === STDERR) {
            fwrite($level, $text);
            return;
        }

        if ($level >= $this->log_level) {
            fwrite(STDOUT, $text);
        }

        if ($level >= $this->err_level) {
            fwrite(STDERR, $text);
        }
    }
}
