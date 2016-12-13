<?php

// -------------------- Global Variables --------------------
// Timezones in PHP
$SGD_PHP_TIMEZONES = array(
                  '-12:00'=>'Pacific/Kwajalein', 
                  '-11:00'=>'Pacific/Samoa', 
                  '-10:00'=>'Pacific/Honolulu', 
                  '-09:00'=>'America/Juneau', 
                  '-08:00'=>'America/Los_Angeles', 
                  '-07:00'=>'America/Denver', 
                  '-06:00'=>'America/Chicago', 
                  '-05:00'=>'America/New_York', 
                  '-04:00'=>'America/Caracas', 
                  '-03:30'=>'America/St_Johns', 
                  '-03:00'=>'America/Argentina/Buenos_Aires', 
                  '-02:00'=>'Atlantic/Azores',
                  '-01:00'=>'Atlantic/Azores', 
                  '+00:00'=>'GMT', 
                  '+01:00'=>'Europe/Paris', 
                  '+02:00'=>'Europe/Helsinki', 
                  '+03:00'=>'Europe/Moscow', 
                  '+03:30'=>'Asia/Tehran', 
                  '+04:00'=>'Asia/Baku', 
                  '+04:30'=>'Asia/Kabul', 
                  '+05:00'=>'Asia/Karachi', 
                  '+05:30'=>'Asia/Kolkata', 
                  '+05:45'=>'Asia/Kathmandu', 
                  '+06:00'=>'Asia/Colombo', 
                  '+07:00'=>'Asia/Bangkok', 
                  '+08:00'=>'Asia/Singapore', 
                  '+09:00'=>'Asia/Tokyo', 
                  '+10:00'=>'Pacific/Guam', 
                  '+11:00'=>'Asia/Magadan', 
                  '+12:00'=>'Asia/Kamchatka'
);
// Display name for Timezones       
$SGD_DISPLAY_TIMEZONES = array(
        '-12:00' => 'Eniwetok, Kwajalein',
        '-11:00' => 'Midway Island, Samoa',
        '-10:00' => 'Hawaii',
        '-09:00' => 'Alaska',
        '-08:00' => 'Pacific Time (US &amp; Canada)',
        '-07:00' => 'Mountain Time (US &amp; Canada)',
        '-06:00' => 'Central Time (US &amp; Canada)',
        '-05:00' => 'Eastern Time (US &amp; Canada)',
        '-04:00' => 'Atlantic Time (Canada), Caracas, La Paz',
        '-03:30' => 'Newfoundland',
        '-03:00' => 'Brazil, Buenos Aires, Georgetown',
        '-02:00' => 'Mid-Atlantic',
        '-01:00' => 'Azores, Cape Verde Islands',
        '+00:00' => '(GMT) Western Europe Time, London, Lisbon, Casablanca',
        '+01:00' => 'Brussels, Copenhagen, Madrid, Paris',
        '+02:00' => 'Kaliningrad, South Africa',
        '+03:00' => 'Baghdad, Riyadh, Moscow, St. Petersburg',
        '+03:30' => 'Tehran',
        '+04:00' => 'Abu Dhabi, Muscat, Baku, Tbilisi',
        '+05:00' => 'Ekaterinburg, Islamabad, Karachi, Tashkent',
        '+05:30' => 'Bombay, Kolkata, Madras, New Delhi',
        '+05:45' => 'Kathmandu',
        '+06:00' => 'Almaty, Dhaka, Colombo',
        '+07:00' => 'Bangkok, Hanoi, Jakarta',
        '+08:00' => 'Beijing, Perth, Singapore, Hong Kong',
        '+09:00' => 'Tokyo, Seoul, Osaka, Sapporo, Yakutsk',
        '+10:00' => 'Eastern Australia, Guam, Vladivostok',
        '+11:00' => 'Magadan, Solomon Islands, New Caledonia',
        '+12:00' => 'Auckland, Wellington, Fiji, Kamchatka' 
);


// returns true if string $needle is a prefix of the string $haystack; false otherwise.
function startsWith($haystack, $needle, $case=true) {
    if ($case) {
        return (strcmp(substr($haystack, 0, strlen($needle)), $needle) == 0);
    }
    return (strcasecmp(substr($haystack, 0, strlen($needle)), $needle) == 0);
}

// returns true if string $needle is a suffix of the string $haystack; false otherwise.
function endsWith($haystack, $needle, $case=true) {
    if ($case) {
        return (strcmp(substr($haystack, strlen($haystack) - strlen($needle)), $needle) == 0);
    }
    return (strcasecmp(substr($haystack, strlen($haystack) - strlen($needle)), $needle) == 0);
}

// get user's email.  Hard-code for demo purpose
function getUserEmail() {
    return 'toshiko.sesselmann@gmail.com';
}

// return HTML light green RGB
function lightGreen() {
    return '#81F781';
}

// return HTML light yellow RGB
function lightYellow() {
    return '#F3F781';
}

// return HTML light red RGB
function lightRed() {
    return '#F78181';
}

// return user's timezone.  Hard-code for the demo purpose.
function getDisplayTimezone($offset) {
    global $SGD_DISPLAY_TIMEZONES;
    $offset = '-07:00';
    if (array_key_exists($offset, $SGD_DISPLAY_TIMEZONES)) {
        return $SGD_DISPLAY_TIMEZONES[$offset];
    }
    return 'GMT';
}
?>
