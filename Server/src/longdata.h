#ifndef _LONG_DATA_
#define _LONG_DATA_

#include <BLECharacteristic.h>
#include <Arduino.h>

// #define DEBUG

void SendLongData (BLECharacteristic& ch, uint8_t * data, int size, int split = 514, int delayTime = 20){


    int packageIndex = 1;
    int packageCount = ceil(size / (float)split);

    int leftBytes = size;

    // uint8_t * bytePackageIndex;

    for (int i = 0; i < packageCount; i++){
        // bytePackageIndex = static_cast<uint8_t*>(static_cast<void*>(&packageIndex));

#ifdef DEBUG
        Serial.println();
        Serial.print("Package ["); Serial.print(i); Serial.print("] = ");
        Serial.print("Start Index = "); Serial.print(split * i);
        Serial.print(" End Index = "); Serial.print(split * i + split);
        Serial.println();
#endif
        ch.setValue(&data[split * i], (split > leftBytes ?  leftBytes : split ));
        ch.notify();
        
        leftBytes -= split;
        if (leftBytes < 0) break;



        delay(delayTime);
    }



}



#endif //_LONG_DATA_