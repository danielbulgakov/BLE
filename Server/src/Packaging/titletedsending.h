#ifndef _TITLED_SENDING_
#define _TITLED_SENDING_

#include <BLECharacteristic.h>
#include "titledpackage.h"
#include <cmath>
#include <Arduino.h>

constexpr int STD_DELAY = (int)20;

/**
 * @brief Отправка пакета определенной структуры. 
 * 
 * @param ch ссылка на х-ку.
 * @param buff массив данных.
 * @param size размер данных.
 * @param mcu дополнительная х-ка
 * @param offset смещение номера пакета.
 */
void TitledSend(BLECharacteristic &ch, uint8_t* buff, int size, int stride, int mcu, int packNumOffset){
    int packageInd = 0;
    int bytesLeft = size;
    TitledPackage::Init();
    Serial.print(TitledPackage::GetNum()); Serial.print(TitledPackage::GetMnu()); 
    for (;bytesLeft > 0;packageInd += 1 + packNumOffset){
        TitledPackage::SetNumber(packageInd);
        TitledPackage::SetMNU(mcu);
        TitledPackage::SetData((buff + packageInd * stride), stride);

        ch.setValue(TitledPackage::GetData(), TitledPackage::GetLenght());
        ch.notify();

        Serial.print("Package {"); Serial.print(packageInd); Serial.println("} send");

        bytesLeft-=stride;
        delay(STD_DELAY);
    }

}



#endif //_TITLED_SENDING_