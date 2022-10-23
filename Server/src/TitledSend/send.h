#ifndef _TITLED_SENDING_
#define _TITLED_SENDING_

#include <BLECharacteristic.h>
#include "ECGpackage.h"
#include <cmath>
#include <Arduino.h>

constexpr int STD_DELAY = (int)50;

/**
 * @brief Отправка пакета определенной структуры. 
 * 
 * @param ch ссылка на х-ку.
 * @param buff массив данных.
 * @param size размер данных.
 * @param mcu дополнительная х-ка
 * @param offset смещение номера пакета.
 */



// void TitledSend(BLECharacteristic &ch, uint8_t* buff, int size, int stride, int mcu, int packNumOffset){
//     TemplatePackage tp;
//     int packageInd = 0;
//     int bytesLeft = size;
//     tp.SetMNU(mcu);
//     // Serial.print("[");for (int i = 0; i < 2500; i++ ) {Serial.print(reinterpret_cast<float*>(buff)[i]); Serial.print(',');}Serial.println("]");
//     for (;bytesLeft > 0;packageInd += 1 + packNumOffset){
//         tp.SetTitle("ECG_DATA");
//         tp.SetNumber(packageInd);
        
//         tp.SetData(&buff[packageInd * stride], stride);

//         ch.setValue(tp.GetRawData(), tp.GetLen());
//         ch.indicate();
//         Serial.print("Package {"); Serial.print(packageInd); Serial.println("} send");
//         Serial.print("[");for (int i = 0; i < 100; i++ ) {Serial.print(reinterpret_cast<float*>(tp.GetUsefulData())[i]); Serial.print(',');}Serial.println("]");

//         bytesLeft-=stride;
//         delay(STD_DELAY);
//     }

// }



#endif //_TITLED_SENDING_