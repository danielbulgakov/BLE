#ifndef _BLE_SEND_
#define _BLE_SEND_

#include "PackageTemplate.h"
#include "BLECharacteristic.h"

#include <Arduino.h>

constexpr int STD_DELAY = (int)50;


class BLESend {
    PackageTemplate * packageRef;
    BLECharacteristic * chrRef;

public:
    
    BLESend (PackageTemplate& pt, BLECharacteristic &ch){
        this->packageRef = &pt;
        this->chrRef = &ch;
    }
    void SendSingle (uint8_t* buff, int size,  int ind){
        (*packageRef).SetNumber(ind);
        (*packageRef).SetData(buff, size);
        Fire();
    };

    void Send(uint8_t* buff, int size, int stride, int indoffset)
    {
        int packageInd = 0;
        int bytesLeft = size;
        for (;bytesLeft > 0;packageInd += 1 + indoffset){

            (*packageRef).SetNumber(packageInd);
            (*packageRef).SetData(&buff[packageInd * stride], stride);

            Fire();

            Serial.print("Package {"); Serial.print(packageInd); Serial.println("} send");
            Serial.print("[");for (int i = 0; i < 100; i++ ) {Serial.print(reinterpret_cast<float*>((*packageRef).GetUsefulData())[i]); Serial.print(',');}Serial.println("]");

            bytesLeft-=stride;
            delay(STD_DELAY);
        }

    }

    void Fire(){
        (*chrRef).setValue((*packageRef).GetRawData(), (*packageRef).GetLen());
        (*chrRef).indicate();
    }


};


#endif //_BLE_SEND_