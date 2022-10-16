#include "longdata.h"

#include <BLEDevice.h>
#include <BLEServer.h>
#include <BLEUtils.h>
#include <BLE2902.h>

#include <Arduino.h>
#include <string.h>
#include <toString.h>
#include <cmath>

#define IFPACKAGE
#define DEBUG

#ifdef IFPACKAGE
  // #include "package.h"
  #include "Packaging/titletedsending.h"
#endif

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

BLECharacteristic SinXCharacteristics(CHARACTER_SINX_UUID, BLECharacteristic::PROPERTY_NOTIFY );
BLEDescriptor SinXDescriptor(BLEUUID((uint16_t)0x2910));

BLECharacteristic CosXCharacteristics(CHARACTER_COSX_UUID, BLECharacteristic::PROPERTY_NOTIFY );
BLEDescriptor CosXDescriptor(BLEUUID((uint16_t)0x2909));

BLECharacteristic SinStepCharacteristics(CHARACTER_SIN_STEP_UUID, BLECharacteristic::PROPERTY_WRITE | BLECharacteristic::PROPERTY_READ);
BLEDescriptor SinStepDescriptor(BLEUUID((uint16_t)0x2906));

BLECharacteristic CosStepCharacteristics(CHARACTER_COS_STEP_UUID, BLECharacteristic::PROPERTY_WRITE | BLECharacteristic::PROPERTY_READ);
BLEDescriptor CosStepDescriptor(BLEUUID((uint16_t)0x2907));

// Characteristics values
float sinstep = 1, cosstep = 1;
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
const int size = 10000/sizeof(float);
float sinxarray[size];
// float cosxarray[size];

#ifdef IFPACKAGE
  // DataPackage<float> dataCos(size);
  // DataPackage<float> dataSin(size);
#endif

void setup(){

  Serial.begin(115200);
  
  // BLE-device init
  BLEDevice::init(bleServerName);
  BLEDevice::setMTU(517);


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



  BatteryCharacteristics.setValue(to_str(battery));
  TimeCharacteristics.setValue(date);
  SinStepCharacteristics.setValue(sinstep);
  CosStepCharacteristics.setValue(cosstep);


  ServiceDevice->start();
  ServiceSensor->start();

  pServer->getAdvertising()->start();
  
  Serial.println("Waiting a client connection to notify...");       



}


float ps = 0;
// float pc = 1;

void loop() {
  // function is f = cosstep * sin (sinstep * x)
  // sinstep * x = [-p; -p/2;  0; p/2; p/2]
  //        y = [   0;    -1;  0;  -1;    0]
  if (deviceConnected) {
    float* sinstep, *cosstep;


    sinstep = reinterpret_cast<float*>(SinStepCharacteristics.getData());
    cosstep = reinterpret_cast<float*>(CosStepCharacteristics.getData());

    #ifdef DEBUG
      // Serial.print("sinstep ");
      // Serial.println(*sinstep);
      // Serial.print("cosstep ");
      // Serial.println(*cosstep);
    #endif 

    // float x1 = 0, x2 = 0;
    for (int i = 0; i < size; i++){
      // sinxarray[i] = sin(x1);
      // cosxarray[i] = cos(x2);
      sinxarray[i] = ps++;
      // cosxarray[i] = pc++;
      // x1 += *sinstep;
      // x2 += *cosstep;
    }

    #ifdef DEBUG
      // for (int i = 0 ; i < size; i ++){
      //   Serial.print(sinxarray[i]); Serial.print( ' '); Serial.println(cosxarray[i]);
      // }
      Serial.print("Start Index = ");
      Serial.print(sinxarray[0]); Serial.print( ' '); /*Serial.println(cosxarray[0]);*/
      Serial.print("End Index = ");
      Serial.print(sinxarray[size - 1]); Serial.print( ' '); /*Serial.println(cosxarray[size - 1]);*/
      Serial.println();
    #endif
    
    #ifdef IFPACKAGE
      
      // dataSin.AddData(sinxarray, size);

      
      TitledSend(SinXCharacteristics, reinterpret_cast<uint8_t*>(sinxarray), 10000, 400, -2, 0);
      // dataCos.AddData(cosxarray, size);
      // SendLongData(SinXCharacteristics, dataSin.GetData(), dataSin.GetLength(), 508, 20 ); 

      
      // dataSin.Clear();
      // SendLongData(CosXCharacteristics, dataCos.GetData(), dataCos.GetLength() );
    #else
      SinXCharacteristics.setValue(reinterpret_cast<uint8_t*>(&sinxarray), sizeof(sinxarray));
      CosXCharacteristics.setValue(reinterpret_cast<uint8_t*>(&cosxarray), sizeof(cosxarray));
    #endif
    
    // SinXCharacteristics.notify();
    // CosXCharacteristics.notify();

    delay(1000);
  }
  else {
    #ifdef DEBUG
      Serial.println("Disconnected");
    #endif
    pServer->getAdvertising()->start();
    delay(1000);

  }
}