#include <Wire.h>
#include "RTClib.h"

RTC_DS1307 RTC;
int heart_beat = 13;

void setup() {
  // RTC setup code from 
  // https://github.com/adafruit/RTClib/blob/master/examples/ds1307/ds1307.pde
  
  Serial.begin(57600);
  Wire.begin();
  RTC.begin();
  
  // If the real time clock is not running
  // then set it to this code's compilation date.
  if(!RTC.isrunning()) {
    //Serial.println("Setting the RTC...");
    //RTC.adjust(DateTime(__DATE__, __TIME__));
  }
  
  // Set pins 2 to 7 as outputs
  // http://www.arduino.cc/en/Reference/PortManipulation
  DDRD = DDRD | B11111100;
  
  pinMode(heart_beat, OUTPUT);
}

int get_phase(DateTime now) {
  // Lunar phase calculation from http://www.ben-daglish.net/moon.shtml
  int phase = now.year() % 100;
  
  phase %= 19;
  
  if(phase > 9) { phase -= 19; }
  
  phase = ((phase * 11) % 30) + now.month() + now.day();
  
  if(now.month() < 3) { phase += 2; }
  
  phase -= ((now.year() < 2000) ? 4 : 8.3);
  phase = int(floor(phase + 0.5)) % 30;
  phase = (phase < 0) ? phase + 30 : phase;
  
  // The value 3.6 is from the
  // maximum output from John Conways equation (29) / number of lunar phases (8)
  return round(phase / 3.6);
}

void print_datetime(DateTime now) {
  Serial.print(now.year(), DEC);
  Serial.print('/');
  Serial.print(now.month(), DEC);
  Serial.print('/');
  Serial.print(now.day(), DEC);
  Serial.print(' ');
  Serial.print(now.hour(), DEC);
  Serial.print(':');
  Serial.print(now.minute(), DEC);
  Serial.print(':');
  Serial.print(now.second(), DEC);
  Serial.println();
}

void flash_heart_beat() {
  digitalWrite(heart_beat, HIGH);
  delay(500);
  digitalWrite(heart_beat, LOW);
  delay(500);
}

void loop() {
  // Reference for different phases from http://www.calculatorcat.com/moon_phases/moon_phases.phtml
  DateTime now = RTC.now();
  
  print_datetime(now);
  
  switch(get_phase(now)) {
    case 0:
      // new moon
      Serial.println("new moon");
      PORTD = B00000000;
      break;
    case 1:
      // waxing crescent
      Serial.println("waxing crescent");
      PORTD = B10000000;
      break;
    case 2:
      // first quarter
      Serial.println("first quarter");
      PORTD = B11100000;
      break;
    case 3:
      // waxing gibbous
      Serial.println("waxing gibbous");
      PORTD = B11111000;
      break;
    case 4:
      // full moon
      Serial.println("full moon");
      PORTD = B11111100;
      break;
    case 5:
      // waning gibbous
      Serial.println("waning gibbous");
      PORTD = B01111100;
      break;
    case 6:
      // last quarter
      Serial.println("last quarter");
      PORTD = B00011100;
      break;
    default:
      // waning crescent
      Serial.println("waning crescent");
      PORTD = B00000100;
      break;
  }
  
  flash_heart_beat();
  
  delay(10 * 1000); // update every 10 seconds... why not? :p
}
