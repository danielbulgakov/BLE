// #include <BLEDevice.h>
// #include <BLEServer.h>
// #include <BLEUtils.h>
// #include <BLE2902.h>

// #include <Arduino.h>

// #define bleServerName "dESP32"

// // UUID
// #define SERVICE_UUID "17b5248e-2e8b-11ed-a261-0242ac120002"
// #define SERVICE_UUID1 "17b5290c-2e8b-11ed-a261-0242ac120002"

// #define CHARACTER_UUID_READ "17b52a4c-2e8b-11ed-a261-0242ac120002"
// #define CHARACTER_UUID_READ_WRITE "17b52b64-2e8b-11ed-a261-0242ac120002"
// #define CHARACTER_UUID_WRITE "17b52c72-2e8b-11ed-a261-0242ac120002"
// #define CHARACTER_UUID_NOTIFY "17b530c8-2e8b-11ed-a261-0242ac120002"
// #define CHARACTER_UUID_BROADCAST "17b52ea2-2e8b-11ed-a261-0242ac120002"
// #define CHARACTER_UUID_INDICATE "17b52fc4-2e8b-11ed-a261-0242ac120002"

// // Инициализация характеристик и дескрипторов
// BLECharacteristic ReadCharacteristics(CHARACTER_UUID_READ, BLECharacteristic::PROPERTY_READ);
// BLEDescriptor ReadDescriptor(BLEUUID((uint16_t)0x2903));

// BLECharacteristic ReadWriteCharacteristics(CHARACTER_UUID_READ_WRITE, BLECharacteristic::PROPERTY_READ | BLECharacteristic::PROPERTY_WRITE);
// BLEDescriptor ReadWriteDescriptor(BLEUUID((uint16_t)0x2904));

// BLECharacteristic WriteCharacteristics(CHARACTER_UUID_WRITE, BLECharacteristic::PROPERTY_WRITE);
// BLEDescriptor WriteDescriptor(BLEUUID((uint16_t)0x2905));

// BLECharacteristic NotifyCharacteristics(CHARACTER_UUID_NOTIFY, BLECharacteristic::PROPERTY_READ | BLECharacteristic::PROPERTY_NOTIFY);
// BLEDescriptor NotifyDescriptor(BLEUUID((uint16_t)0x2906));

// BLECharacteristic BroadcastCharacteristics(CHARACTER_UUID_BROADCAST, BLECharacteristic::PROPERTY_READ | BLECharacteristic::PROPERTY_BROADCAST);
// BLEDescriptor BroadcastDescriptor(BLEUUID((uint16_t)0x2907));

// BLECharacteristic IndicateCharacteristics(CHARACTER_UUID_INDICATE, BLECharacteristic::PROPERTY_READ | BLECharacteristic::PROPERTY_INDICATE);
// BLEDescriptor IndicateDescriptor(BLEUUID((uint16_t)0x2908));

// bool deviceConnected = false;

// // задаем функции обратного вызова onConnect() и onDisconnect():
// class MyServerCallbacks: public BLEServerCallbacks {
//   void onConnect(BLEServer* pServer) {
//     deviceConnected = true;
//   };
//   void onDisconnect(BLEServer* pServer) {
//     deviceConnected = false;
//   }
// };

// void setup(){
//   Serial.begin(115200);
  
//   // Иницализация BLE-устройства 
//   BLEDevice::init(bleServerName);

//   // Инициализация BLE-сервера
//   BLEServer *pServer = BLEDevice::createServer();
//   pServer->setCallbacks(new MyServerCallbacks());

//   // Инициализация BLE-сервисов
//   BLEService *Service = pServer->createService(SERVICE_UUID);
//   BLEService *Service1 = pServer->createService(SERVICE_UUID1);

//   // Иницализация характеристик Service
//   Service->addCharacteristic(&ReadCharacteristics);
//   ReadDescriptor.setValue("test read-only value"); 
//   ReadCharacteristics.addDescriptor(new BLE2902());
//   ReadCharacteristics.addDescriptor(&ReadDescriptor);

//   Service->addCharacteristic(&ReadWriteCharacteristics);
//   ReadWriteDescriptor.setValue("test read/write value"); 
//   ReadWriteCharacteristics.addDescriptor(new BLE2902());
//   ReadWriteCharacteristics.addDescriptor(&ReadWriteDescriptor);

//   Service->addCharacteristic(&WriteCharacteristics);
//   WriteDescriptor.setValue("test write value"); 
//   WriteCharacteristics.addDescriptor(new BLE2902());
//   WriteCharacteristics.addDescriptor(&WriteDescriptor);
  
//   Service->start();

//   // Иницализация характеристик Service1
//   Service1->addCharacteristic(&NotifyCharacteristics);
//   NotifyDescriptor.setValue("test notify value"); 
//   NotifyCharacteristics.addDescriptor(new BLE2902());
//   NotifyCharacteristics.addDescriptor(&NotifyDescriptor);

//   Service1->addCharacteristic(&BroadcastCharacteristics);
//   BroadcastDescriptor.setValue("test broadcast value"); 
//   BroadcastCharacteristics.addDescriptor(new BLE2902());
//   BroadcastCharacteristics.addDescriptor(&BroadcastDescriptor);

//   Service1->addCharacteristic(&IndicateCharacteristics);
//   IndicateDescriptor.setValue("test indicate value"); 
//   IndicateCharacteristics.addDescriptor(new BLE2902());
//   IndicateCharacteristics.addDescriptor(&IndicateDescriptor);

//   Service1->start();

//   // запускаем рассылку оповещений:
//   pServer->getAdvertising()->start();
//   Serial.println("Waiting a client connection to notify...");
//              //  "Ждем подключения клиента, чтобы отправить уведомление..."
// }

// void loop() {
//   if (deviceConnected) {
    
//   }
// }















