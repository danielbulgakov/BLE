#ifndef _BLE_SEND_
#define _BLE_SEND_

#include "PackageTemplate.h"
#include "BLECharacteristic.h"

#include <Arduino.h>

constexpr int STD_DELAY = (int)1000;


class BLESend {
    PackageTemplate * packageRef;
    BLECharacteristic * chrRef;

    int packInd = 0;
public:
    
    BLESend (PackageTemplate& pt, BLECharacteristic &ch){
        this->packageRef = &pt;
        this->chrRef = &ch;
    }
    void SendSingle (uint8_t* buff, int size,  int ind){
        (*packageRef).SetNumber(ind);
        (*packageRef).SetData(buff, size);
        Serial.print("Package {"); Serial.print(ind); Serial.println("} send");
        Serial.print("[");for (int i = 0; i < 100; i++ ) {Serial.print(reinterpret_cast<float*>((*packageRef).GetUsefulData())[i]); Serial.print(',');}Serial.println("]");

        Fire();
    };

    void Send(uint8_t* buff, int size, int stride, int indoffset)
    {
        int packageOffset = 0;
        int bytesLeft = size;
        for (;bytesLeft > 0;packageOffset += 1 + indoffset){

            (*packageRef).SetNumber(packInd++);
            (*packageRef).SetData(&buff[packageOffset * stride], stride);

            Fire();

            Serial.print("Package {"); Serial.print(packInd); Serial.println("} send");
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