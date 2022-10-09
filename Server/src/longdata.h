#ifndef _LONG_DATA_
#define _LONG_DATA_

#include <BLECharacteristic.h>
#include <Arduino.h>

void SendData (BLECharacteristic ch, uint8_t * data, int size, int split = 514, int delayTime = 20){

    //Not implemented yet.
    return ; 


    int packageIndex = 1;
    int packageCount = size + 4 / split;

    uint8_t * bytePackageIndex;

    for (int i = 0; i < packageCount; i++){
        bytePackageIndex = static_cast<uint8_t*>(static_cast<void*>(&packageIndex));
        
        
    }



}

#endif //_LONG_DATA_