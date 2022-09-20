#include <BLEDevice.h>
#include <BLEServer.h>
#include <BLEUtils.h>
#include <BLE2902.h>

#include <Arduino.h>
#include <string.h>
#include <toString.h>
#include <cmath>

#define bleServerName "TEST_ESP32"

// UUID
#define SERVICE_SETTINGS_UUID "17b5248e-2e8b-11ed-a261-0242ac120002"
#define SERVICE_SENSOR_UUID "17b5290c-2e8b-11ed-a261-0242ac120002"

// SERVICE_SETTINGS_UUID characteristic uuid
#define CHARACTER_BATTERY_UUID "17b52a4c-2e8b-11er-a261-0242ac120002"
#define CHARACTER_TIME_UUID "17b52a4c-2e8b-11ed-a261-0242ac120002"

// SERVICE_SENSOR_UUID characteristic uuid
#define CHARACTER_SINX_UUID "17b52c72-2e8b-11er-a261-0242ac120002"
#define CHARACTER_SINY_UUID "17b52c72-2e8b-11ef-a261-0242ac120002"
#define CHARACTER_SIN_WIDE_UUID "17b52ea2-2e8b-11ed-a261-0242ac120002"
#define CHARACTER_SIN_HEIGHT_UUID "17b52fc4-2e8b-11ed-a261-0242ac120002"

// Descrports and characterisitic init
BLECharacteristic BatteryCharacteristics(CHARACTER_BATTERY_UUID, BLECharacteristic::PROPERTY_READ | BLECharacteristic::PROPERTY_WRITE);
BLEDescriptor BatteryDescriptor(BLEUUID((uint16_t)0x2903));

BLECharacteristic TimeCharacteristics(CHARACTER_TIME_UUID, BLECharacteristic::PROPERTY_READ | BLECharacteristic::PROPERTY_WRITE);
BLEDescriptor TimeDescriptor(BLEUUID((uint16_t)0x2904));

BLECharacteristic SinXCharacteristics(CHARACTER_SINX_UUID, BLECharacteristic::PROPERTY_READ | BLECharacteristic::PROPERTY_NOTIFY);
BLEDescriptor SinXDescriptor(BLEUUID((uint16_t)0x2910));

BLECharacteristic SinYCharacteristics(CHARACTER_SINY_UUID, BLECharacteristic::PROPERTY_READ | BLECharacteristic::PROPERTY_NOTIFY);
BLEDescriptor SinYDescriptor(BLEUUID((uint16_t)0x2909));

BLECharacteristic SinWideCharacteristics(CHARACTER_SIN_WIDE_UUID, BLECharacteristic::PROPERTY_WRITE | BLECharacteristic::PROPERTY_READ);
BLEDescriptor SinWideDescriptor(BLEUUID((uint16_t)0x2906));

BLECharacteristic SinHeightCharacteristics(CHARACTER_SIN_HEIGHT_UUID, BLECharacteristic::PROPERTY_WRITE | BLECharacteristic::PROPERTY_READ);
BLEDescriptor SinHeightDescriptor(BLEUUID((uint16_t)0x2907));

// Characteristics values
float wide = 1, height = 1;
int battery = 100;
std::string date = "18.09.2022";
float pi = 3.141592653589793238;

bool deviceConnected = false;

class MyServerCallbacks: public BLEServerCallbacks {
  void onConnect(BLEServer* pServer) {
    deviceConnected = true;
  };
  void onDisconnect(BLEServer* pServer) {
    deviceConnected = false;
  }
};

BLEServer *pServer;

void setup(){
  Serial.begin(115200);
  
  // BLE-device init
  BLEDevice::init(bleServerName);

  pServer = BLEDevice::createServer();
  // BLE-server init
  pServer->setCallbacks(new MyServerCallbacks());

  // BLE-service init
  BLEService *ServiceDevice = pServer->createService(SERVICE_SETTINGS_UUID);
  BLEService *ServiceSensor = pServer->createService(SERVICE_SENSOR_UUID);

  // SERVICE_SETTINGS_UUID characters add 
  ServiceDevice->addCharacteristic(&BatteryCharacteristics);
  BatteryDescriptor.setValue("Percantage of charge left in battery"); 
  BatteryCharacteristics.addDescriptor(new BLE2902());
  BatteryCharacteristics.addDescriptor(&BatteryDescriptor);

  ServiceDevice->addCharacteristic(&TimeCharacteristics);
  TimeDescriptor.setValue("Current time"); 
  TimeCharacteristics.addDescriptor(new BLE2902());
  TimeCharacteristics.addDescriptor(&TimeDescriptor);

  ServiceDevice->start();

  // SERVICE_SENSOR_UUID characters add
  ServiceSensor->addCharacteristic(&SinXCharacteristics);
  SinXDescriptor.setValue("sin arg x function"); 
  SinXCharacteristics.addDescriptor(new BLE2902());
  SinXCharacteristics.addDescriptor(&SinXDescriptor);

  ServiceSensor->addCharacteristic(&SinYCharacteristics);
  SinYDescriptor.setValue("sin arg y function"); 
  SinYCharacteristics.addDescriptor(new BLE2902());
  SinYCharacteristics.addDescriptor(&SinYDescriptor);

  ServiceSensor->addCharacteristic(&SinWideCharacteristics);
  SinWideDescriptor.setValue("wide arg for sin function"); 
  SinWideCharacteristics.addDescriptor(new BLE2902());
  SinWideCharacteristics.addDescriptor(&SinWideDescriptor);

  ServiceSensor->addCharacteristic(&SinHeightCharacteristics);
  SinHeightDescriptor.setValue("height arg for sin function"); 
  SinHeightCharacteristics.addDescriptor(new BLE2902());
  SinHeightCharacteristics.addDescriptor(&SinHeightDescriptor);

  ServiceSensor->start();

  BatteryCharacteristics.setValue(to_str(battery));
  TimeCharacteristics.setValue(date);
  SinWideCharacteristics.setValue(wide);
  SinHeightCharacteristics.setValue(height);


  ServiceDevice->start();
  ServiceSensor->start();

  pServer->getAdvertising()->start();
  
  Serial.println("Waiting a client connection to notify...");       



}

void loop() {
  // function is f = height * sin (wide * x)
  // wide * x = [-p; -p/2;  0; p/2; p/2]
  //        y = [   0;    -1;  0;  -1;    0]
  if (deviceConnected) {

    float sinxarray[5];
    float sinyarray[5];
    float* wide, *height;

    wide = reinterpret_cast<float*>(SinWideCharacteristics.getData());
    height = reinterpret_cast<float*>(SinHeightCharacteristics.getData());

    Serial.print("wide ");
    Serial.println(*wide);
    Serial.print("height ");
    Serial.println(*height);

    
    // Serial.println(wide); Serial.println(height);

    float x = -pi;
    for (int i = 0; i < 5; i++){
      sinxarray[i] = x / (*wide);
      sinyarray[i] = (*height) * sin(x * (*wide));
      x += (pi/2);
    }

    for (int i = 0 ; i < 5; i ++){
      Serial.print(sinxarray[i]); Serial.print( ' '); Serial.println(sinyarray[i]);
    }
    

    SinXCharacteristics.setValue(reinterpret_cast<uint8_t*>(&sinxarray), sizeof(sinxarray));
    SinYCharacteristics.setValue(reinterpret_cast<uint8_t*>(&sinyarray), sizeof(sinyarray));
    // SinXCharacteristics.notify();
    // //SinYCharacteristics.notify();
    // SinWideCharacteristics.notify();
    // SinHeightCharacteristics.notify();

    delay(5000);
  }
  else {
    Serial.println("Disconnected");
    pServer->getAdvertising()->start();
    delay(2000);

  }
}