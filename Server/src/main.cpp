#include "TitledSend/BLESend.h"
#include "TitledSend/ECGpackage.h"
#include "Config/BLEConfigs.h"
#include "Tools/toString.h"
#include "ECG/read.h"

#include <Arduino.h>

// Characteristics values
float sinstep = 1, cosstep = 1;
int battery = 100;
std::string date = "18.09.2022";
float pi = 3.141592653589793238;

const int size = 400/sizeof(float);
float array[size];

void setup(){
    Serial.begin(115200);

    pinMode(2, INPUT); // Setup for leads off detection LO +
    pinMode(4, INPUT); // Setup for leads off detection LO -

    SetupBLE();
    BatteryCharacteristics.setValue(to_str(battery));
    TimeCharacteristics.setValue(date);
    SinStepCharacteristics.setValue(sinstep);
    CosStepCharacteristics.setValue(cosstep);
    BLEStart();
}


ECGPackage pack ("ECG_DATA", 0, -10);
BLESend Sender((PackageTemplate&)pack, SinXCharacteristics);
ECG ecg;

int staticIndex = 0;

void loop() {
    if (deviceConnected) {

        // float* sinstep, *cosstep;


        // sinstep = reinterpret_cast<float*>(SinStepCharacteristics.getData());
        // cosstep = reinterpret_cast<float*>(CosStepCharacteristics.getData());

        for (int i = 0; i < size; i++){
            array[i] = ecg.Read();
            delay(10);
        }

        Sender.SendSingle(reinterpret_cast<uint8_t*>(&array), 400, staticIndex++);
        
    
    }
    else {
        pServer->getAdvertising()->start();
        delay(3000);
    }

    

}