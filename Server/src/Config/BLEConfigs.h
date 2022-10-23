#include <BLECharacteristic.h>
#include <BLEServer.h>
#include <BLEDevice.h>
#include <BLE2902.h>
#include <Arduino.h>

#define bleServerName "TEST_ESP32"

// UUID
#define SERVICE_SETTINGS_UUID "17b5248e-2e8b-11ed-a261-0242ac120002"
#define SERVICE_SENSOR_UUID "17b5290c-2e8b-11ed-a261-0242ac120002"

// SERVICE_SETTINGS_UUID characteristic uuid
#define CHARACTER_BATTERY_UUID "17b52a4c-2e8b-11er-a261-0242ac120002"
#define CHARACTER_TIME_UUID "17b52a4c-2e8b-11ed-a261-0242ac120002"

// SERVICE_SENSOR_UUID characteristic uuid
#define CHARACTER_SINX_UUID "17b52c72-2e8b-11er-a261-0242ac120002"
#define CHARACTER_COSX_UUID "17b52c72-2e8b-11ef-a261-0242ac120002"
#define CHARACTER_SIN_STEP_UUID "17b52ea2-2e8b-11ed-a261-0242ac120002"
#define CHARACTER_COS_STEP_UUID "17b52fc4-2e8b-11ed-a261-0242ac120002"

// Descrports and characterisitic init
BLECharacteristic BatteryCharacteristics(CHARACTER_BATTERY_UUID, BLECharacteristic::PROPERTY_READ | BLECharacteristic::PROPERTY_WRITE);
BLEDescriptor BatteryDescriptor(BLEUUID((uint16_t)0x2903));

BLECharacteristic TimeCharacteristics(CHARACTER_TIME_UUID, BLECharacteristic::PROPERTY_READ | BLECharacteristic::PROPERTY_WRITE);
BLEDescriptor TimeDescriptor(BLEUUID((uint16_t)0x2904));

BLECharacteristic SinXCharacteristics(CHARACTER_SINX_UUID, BLECharacteristic::PROPERTY_INDICATE );
BLEDescriptor SinXDescriptor(BLEUUID((uint16_t)0x2910));

BLECharacteristic CosXCharacteristics(CHARACTER_COSX_UUID, BLECharacteristic::PROPERTY_INDICATE );
BLEDescriptor CosXDescriptor(BLEUUID((uint16_t)0x2909));

BLECharacteristic SinStepCharacteristics(CHARACTER_SIN_STEP_UUID, BLECharacteristic::PROPERTY_WRITE | BLECharacteristic::PROPERTY_READ);
BLEDescriptor SinStepDescriptor(BLEUUID((uint16_t)0x2906));

BLECharacteristic CosStepCharacteristics(CHARACTER_COS_STEP_UUID, BLECharacteristic::PROPERTY_WRITE | BLECharacteristic::PROPERTY_READ);
BLEDescriptor CosStepDescriptor(BLEUUID((uint16_t)0x2907));


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
BLEService *ServiceDevice;
BLEService *ServiceSensor;

void SetupBLE(){
    // BLE-device init
  BLEDevice::init(bleServerName);
  BLEDevice::setMTU(517);


  pServer = BLEDevice::createServer();
  // BLE-server init
  pServer->setCallbacks(new MyServerCallbacks());

  // BLE-service init
  ServiceDevice = pServer->createService(SERVICE_SETTINGS_UUID);
  ServiceSensor = pServer->createService(SERVICE_SENSOR_UUID);

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
  SinXDescriptor.setValue("sin x function"); 
  SinXCharacteristics.addDescriptor(new BLE2902());
  SinXCharacteristics.addDescriptor(&SinXDescriptor);
  SinXCharacteristics.setIndicateProperty(1);

  ServiceSensor->addCharacteristic(&CosXCharacteristics);
  CosXDescriptor.setValue("cos x function"); 
  CosXCharacteristics.addDescriptor(new BLE2902());
  CosXCharacteristics.addDescriptor(&CosXDescriptor);
  CosXCharacteristics.setIndicateProperty(1);

  ServiceSensor->addCharacteristic(&SinStepCharacteristics);
  SinStepDescriptor.setValue("sinstep arg for sin function"); 
  // SinStepCharacteristics.addDescriptor(new BLE2902());
  SinStepCharacteristics.addDescriptor(&SinStepDescriptor);

  ServiceSensor->addCharacteristic(&CosStepCharacteristics);
  CosStepDescriptor.setValue("cosstep arg for sin function"); 
  // SinHeightCharacteristics.addDescriptor(new BLE2902());
  CosStepCharacteristics.addDescriptor(&CosStepDescriptor);



//   BatteryCharacteristics.setValue(to_str(battery));
//   TimeCharacteristics.setValue(date);
//   SinStepCharacteristics.setValue(sinstep);
//   CosStepCharacteristics.setValue(cosstep);



}

void BLEStart(){
  ServiceDevice->start();
  ServiceSensor->start();

  pServer->getAdvertising()->start();
  
  Serial.println("Waiting a client connection to notify...");       

}