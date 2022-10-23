#ifndef _TITLED_PACKAGE_
#define _TITLED_PACKAGE_

#include <stdint.h>
#include <string>
#include <cstring>
#include "PackageTemplate.h"


/**
 * Этот класс описывает шаблонный пакет со следующими параметрами
 * title - загловок пакета (8Б)
 * number - номер пакета (4Б)
 * mcu - х-ка (2Б)
 * packArray - массив полезных данных (400Б)
 * 
 * Размер всего пакета составляет (416Б)
 * |HEAD(8Б)|NUM(4Б)|MCU(2Б)|'<'|DATA(400Б)|'>'|
 */
class ECGPackage : PackageTemplate{
private:

    enum{
        packLen  = 416,
        titleLen = 8,
        dataLen  = 400,
    };
    enum indices{
        ihead = 0,
        inum = titleLen + 1,
        imnu = inum + 4,
        il = imnu + 2,
        ir = packLen - 1,
        idata = 16
    };

    uint8_t title[titleLen];
    const char l = '<';
    const char r = '>';
    int32_t number;
    int16_t mnu;    


    uint8_t packArray[packLen];

public:

    ECGPackage(std::string name = "", int32_t number = -1, int16_t mnu = -1, uint8_t* data = nullptr, int len = -1){
        VirtualSetTitle(name);
        VirtualSetNumber(number);
        VirtualSetMNU(mnu);
        VirtualSetData(data, len);

        memcpy(&packArray[indices::il], &l, sizeof(l));
        memcpy(&packArray[indices::ir], &r, sizeof(r));
    }

    void VirtualSetTitle(std::string name){
        if (name.length() < 1) return;
        int len = titleLen > name.length() ? name.length() : titleLen;
        for (int i = 0; i < len; i++) title[i] = name[i];
        memcpy(&packArray[indices::ihead], title, titleLen);
    }

    void VirtualSetNumber(int32_t num){
        number = num;
        uint8_t * toWrite = reinterpret_cast<uint8_t *>(&num);
        memcpy(&packArray[indices::inum], toWrite, sizeof(num));
    }

    void VirtualSetMNU(int16_t vmnu){
        mnu = vmnu;
        uint8_t * toWrite = reinterpret_cast<uint8_t *>(&vmnu);
        memcpy(&packArray[indices::imnu], toWrite, sizeof(vmnu));
    }

    void VirtualSetData(uint8_t * data, int size){
        if (data == nullptr || size == -1) return;
        memcpy(&packArray[indices::idata], data, (size < dataLen ? size : dataLen));
    }

    uint8_t * VirtualGetUsefulData(){
        return this->packArray + indices::idata;
    }

    uint8_t * VirtualGetRawData(){
        return this->packArray;
    }

    int VirtualGetLen(){
        return packLen;
    }


};


#endif //_TITLED_PACKAGE_