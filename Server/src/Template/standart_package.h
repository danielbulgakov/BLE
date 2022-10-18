#ifndef _PACKAGE_TEMPLATE_
#define _PACKAGE_TEMPLATE_

#include <stdint.h>
#include <cstring>


class StandartPackage{
private:
    // Заголовки пакетов
    static const int packLength = 417; // bytes
    const int usefulDataLength = 400; // bytes

    const char keyName[9] = "ECG_DATA";
    const char openBracket = '<';
    const char closeBracket = '>';

    int32_t packageNum = -1;
    int16_t umv = -1;
    // 
    uint8_t packArray[packLength];

public:


    StandartPackage(){
        memcpy(packArray, keyName, 8); // добавить имя закголовка
        int offset = 8;
        memcpy(&packArray[offset], reinterpret_cast<uint8_t*>(&packageNum), sizeof(packageNum));
        offset += sizeof(packageNum);
        memcpy(&packArray[offset], reinterpret_cast<uint8_t*>(&umv), sizeof(umv));
        offset += sizeof(umv);
        packArray[offset] = (uint8_t)openBracket;
        packArray[packLength - 1] = closeBracket;
    }

    int GetPackLength (){
        return this->packLength;
    }

    uint8_t * GetData(){
        return packArray;
    }

    void SetPackageNumber(int32_t num){
        this->packageNum = num;
        int offset = 8;
        memcpy(&packArray[offset], reinterpret_cast<uint8_t*>(&num), sizeof(num));
    }

    void SetUMV(int16_t umv){
        this->umv = umv;
        int offset = 8 + sizeof(packageNum);
        memcpy(&packArray[offset], reinterpret_cast<uint8_t*>(&umv), sizeof(umv));        
    }

    void SetUsefulData(float * data, int size){
        int offset = 8 + 4 + 2 + 1;
        int length = (size * sizeof(float) > 100 ? 100 : size * sizeof(float));
        memcpy(&packArray[offset], reinterpret_cast<uint8_t*>(&data), length);
        if (length < 100)
            for (int index = offset + length; index < packLength - 1; index++)
                packArray[index] = uint8_t(0);
    }

};


#endif //_PACKAGE_TEMPLATE_