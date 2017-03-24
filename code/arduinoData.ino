// *********************************************************************
// The code below is used to convert data readings from Arduino sensors to a format that is friendly for Electric Imp's language, Squirrel, to read from the serial bus before sending to my API.   The code below is a condensed snippet to show personal work.
// Deployed version:
// https://dinkydinky.herokuapp.com
// The full repository can be found at:
// https://github.com/StephenHanzlik/capstone-let-it-grow
//Specific document:
// https://github.com/StephenHanzlik/capstone-let-it-grow/blob/master/arduino-let-it-grow/arduino-let-it-grow.ino
// *********************************************************************

//Set up pin A2 on the Arduino to get our reading for soil moisture
int sensorPin = A2;

//Soil sensor is powered via digital pin and a reading is taken.  Then power is turned off to the sensor.
digitalWrite(7, HIGH);
int soil;
soil = analogRead(sensorPin);
digitalWrite(7, LOW);

//Sensor readings are taken and converted to an integer to make passing data between multiple languages easier
float tempf = myPressure.readTempF();
int temp = (int) tempf;

float humidity myHumidity.readHumidity();
int hum = (int) humidity;

float light_lvl = get_light_level();
int light = (int) light_lvl;

//Values are concatenated into a single string.   Using 'S' to designate the start of the string and "X" to designate the end.
String stringOne = "S,";
String stringTemp = stringOne + light;
String stringTwo = ",";
String stringHum = stringTwo + temp;
String stringThree = ",";
String stringLight = stringThree + hum;
String stringFour = ",";
String stringSoil = stringFour + soil;
String stringEnd = ",X";

//The string is sent to the Electric Imp via software serial
mySerial.println(stringTemp + stringHum + stringLight + stringSoil + stringEnd);

//Delayed for 3.5 seconds.   This leads to POST requests to my API every 3.5 seconds.
delay(3500);
