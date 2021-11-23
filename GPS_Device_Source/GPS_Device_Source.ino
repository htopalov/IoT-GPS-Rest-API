//GPRS credentials (leave empty, if not needed)
const char apn[]      = "internet.a1.bg"; // APN of A1 mobile network
const char gprsUser[] = ""; // GPRS User, no need for now
const char gprsPass[] = ""; // GPRS Password, no need for now
//String deviceId = "867372057405818";
// SIM card PIN (leave empty, if not defined)
const char simPIN[]   = "0000"; 

// Server details
// The server variable can be just a domain name or it can have a subdomain. It depends on the service you are using
const char server[] = "http://trailsapi.azurewebsites.net"; // domain name
const int  port = 80;                            // server port number
const char ip[]="20.43.43.32"; //Azure app service ip address for connection

// TTGO T-Call pins
#define MODEM_RST            5
#define MODEM_PWKEY          4
#define MODEM_POWER_ON       23
#define MODEM_TX             27
#define MODEM_RX             26
#define I2C_SDA              21
#define I2C_SCL              22

// Set serial for debug console (to Serial Monitor, default speed 115200)
#define SerialMon Serial
// Set serial for AT commands (to SIM800 module)
#define SerialAT Serial1

// Configure TinyGSM library
#define TINY_GSM_MODEM_SIM800      // Modem is SIM800
#define TINY_GSM_RX_BUFFER   1024  // Set RX buffer to 1Kb

// Define the serial console for debug prints, if needed
//#define DUMP_AT_COMMANDS

#include <Wire.h>
#include <TinyGsmClient.h>
#include <TinyGPS++.h>
#include <SoftwareSerial.h>
#include <ArduinoJson.h>

// The TinyGPS++ object
TinyGPSPlus gps;

DynamicJsonDocument doc(110); // allocates in the heap
String result;
float curr_latitude;
float curr_longitude;
float curr_altitude;
float curr_speed;


#ifdef DUMP_AT_COMMANDS
  #include <StreamDebugger.h>
  StreamDebugger debugger(SerialAT, SerialMon);
  TinyGsm modem(debugger);
#else
  TinyGsm modem(SerialAT);
#endif

// I2C for SIM800 (to keep it running when powered from battery)
TwoWire I2CPower = TwoWire(0);

// TinyGSM Client for Internet connection(2G GPRS from mobile network)
TinyGsmClient client(modem);

#define uS_TO_S_FACTOR 1000000UL   /* Conversion factor for micro seconds to seconds */
#define TIME_TO_SLEEP  30        /* Time ESP32 will go to sleep (in seconds) 3600 seconds = 1 hour */

#define IP5306_ADDR          0x75
#define IP5306_REG_SYS_CTL0  0x00

bool setPowerBoostKeepOn(int en){
  I2CPower.beginTransmission(IP5306_ADDR);
  I2CPower.write(IP5306_REG_SYS_CTL0);
  if (en) {
    I2CPower.write(0x37); // Set bit1: 1 enable 0 disable boost keep on
  } else {
    I2CPower.write(0x35); // 0x37 is default reg value
  }
  return I2CPower.endTransmission() == 0;
}

void setup() {
  Serial2.begin(9600, SERIAL_8N1, 14, 27); //RX, TX GPS SERIAL
  // Set serial monitor debugging window baud rate to 115200
  SerialMon.begin(115200);


  // Keep power when running from battery
  bool isOk = setPowerBoostKeepOn(1);
  SerialMon.println(String("IP5306 KeepOn ") + (isOk ? "OK" : "FAIL"));

  // Set modem reset, enable, power pins
  pinMode(MODEM_PWKEY, OUTPUT);
  pinMode(MODEM_RST, OUTPUT);
  pinMode(MODEM_POWER_ON, OUTPUT);
  digitalWrite(MODEM_PWKEY, LOW);
  digitalWrite(MODEM_RST, HIGH);
  digitalWrite(MODEM_POWER_ON, HIGH);

  // Set GSM module baud rate and UART pins
  SerialAT.begin(115200, SERIAL_8N1, MODEM_RX, MODEM_TX);

  // Restart SIM800 module, it takes quite some time
  // To skip it, call init() instead of restart()
  SerialMon.println("Initializing modem...");
  modem.restart();
  // use modem.init() if you don't need the complete restart

  // Unlock your SIM card with a PIN if needed
  if (strlen(simPIN) && modem.getSimStatus() != 3 ) {
    modem.simUnlock(simPIN);
  }

  // Configure the wake up source as timer wake up  
  esp_sleep_enable_timer_wakeup(TIME_TO_SLEEP * uS_TO_S_FACTOR);

}

void loop() {
 
 SerialMon.print("Connecting to APN: ");
  SerialMon.print(apn);
   if (!modem.gprsConnect(apn, gprsUser, gprsPass)) {
    SerialMon.println(" fail");
  }
  else {
    SerialMon.println(" OK");
    
    SerialMon.print("Connecting to ");
    SerialMon.print(server);
    if (!client.connect(ip, port)) {
      SerialMon.println(" fail");
    }
    else {
      SerialMon.println(" OK");


 while (Serial2.available() > 0){
    gps.encode(Serial2.read());
    if (gps.location.isUpdated()){ 
         curr_latitude = gps.location.lat();
         curr_longitude = gps.location.lng();
         curr_altitude = gps.altitude.meters();
         curr_speed = gps.speed.kmph();
    }
 }
         if(curr_latitude == 0 || curr_longitude == 0)
         {
           Serial.println("Incorrect GPS data!");
           Serial.println("Disconnecting from network!");
           client.stop();
           modem.gprsDisconnect();
           return;
         }

         doc["latitude"]   = curr_latitude;
         doc["longitude"]  = curr_longitude;
         doc["altitude"]   = curr_altitude;
         doc["speed"]      = curr_speed;
         doc["deviceId"]   = "867372057405818";
         //serializeJson(doc, Serial);
         serializeJson(doc, result);
         
      // Making an HTTP POST request
      SerialMon.println("Performing HTTP POST request...");
      // Prepare HTTP POST request data as JSON
      //Send the actual HTTP POST request to API
      client.print(String("POST ") + "http://trailsapi.azurewebsites.net/api/v1/position_data" + " HTTP/1.1\r\n");
      client.print(String("Host: ") + "http://trailsapi.azurewebsites.net" + "\r\n");
      client.println("Connection: close");
      client.println("Content-Type: application/json");
      client.print("Content-Length: ");
      client.println(result.length());
      client.println();
      client.println(result);


      unsigned long timeout = millis();
      while (client.connected() && millis() - timeout < 10000L) {
        // Print available data (HTTP response from server)-have to be 201 Created
        while (client.available()) {
          char c = client.read();
          SerialMon.print(c);
          timeout = millis();
        }
      }
      SerialMon.println();
      //delay(10000);
      // Close client and disconnect
      client.stop();
      SerialMon.println(F("Server disconnected"));
      modem.gprsDisconnect();
      SerialMon.println(F("GPRS disconnected"));

    }
  }
  // Put ESP32 into deep sleep mode (with timer wake up)
  esp_deep_sleep_start();
}
