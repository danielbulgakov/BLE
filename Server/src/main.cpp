#include "TitledSend/BLESend.h"
#include "TitledSend/ECGpackage.h"
#include "Config/BLEConfigs.h"
#include "Tools/toString.h"

#include <Arduino.h>

// Characteristics values
float sinstep = 1, cosstep = 1;
int battery = 100;
std::string date = "18.09.2022";
float pi = 3.141592653589793238;

const int size = 10000/sizeof(float);
float array[size];

void setup(){
    Serial.begin(115200);

    SetupBLE();
    BatteryCharacteristics.setValue(to_str(battery));
    TimeCharacteristics.setValue(date);
    SinStepCharacteristics.setValue(sinstep);
    CosStepCharacteristics.setValue(cosstep);
    BLEStart();
}

int staticIndex = 0;

ECGPackage pack ("ECG_DATA", 0, -10);
BLESend Sender((PackageTemplate&)pack, SinXCharacteristics);

void loop() {
    if (deviceConnected) {
        delay(3000);
        float* sinstep, *cosstep;


        sinstep = reinterpret_cast<float*>(SinStepCharacteristics.getData());
        cosstep = reinterpret_cast<float*>(CosStepCharacteristics.getData());

        for (int i = 0; i < size; i++){
            array[i] = ++staticIndex;
        }

        Sender.Send(reinterpret_cast<uint8_t*>(&array), 10000, 400, 0);
    
    }
    else {
        pServer->getAdvertising()->start();
        delay(1000);
    }

}