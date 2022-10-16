#ifndef _TITLED_PACKAGE_
#define _TITLED_PACKAGE_

#include <stdint.h>
#include <cstring>

class TitledPackage{

public:

    // constant header values
    static constexpr char titleHead[8] = {'E','C','G','_','D','A','T','A'};
    static const char l = '<';
    static const char r = '>';

    // useful values
    static int32_t packNum ;
    static int16_t mnu ;

    // package characteristics
    static const int dataLen = 400;
    static constexpr int packageLen = sizeof(titleHead) + 2 * sizeof(l) + sizeof(packNum) + sizeof(mnu) + dataLen;

    // useful values
    static uint8_t packData[packageLen];
    
    TitledPackage() {packNum = -1; mnu = -1; };
public:

    static void Init(){
        
        int offset = 0;
        memcpy((packData + offset), titleHead, sizeof(titleHead));
        offset += sizeof(titleHead);
        memcpy((packData + offset), reinterpret_cast<uint8_t *>(&packNum), sizeof(packNum));
        offset += sizeof(packNum);
        memcpy((packData + offset), reinterpret_cast<uint8_t *>(&mnu), sizeof(mnu));
        offset += sizeof(mnu);
        memcpy((packData + offset), &l, sizeof(l));
        offset = packageLen - 1;
        memcpy((packData + offset), &r, sizeof(r));
    }

    static std::string GetName(){return std::string(titleHead);} 
    static int32_t GetNum() {return packNum;}
    static int16_t GetMnu() {return mnu;}
    static uint8_t* GetData() {return packData;}
    static int GetLenght() {return packageLen;}

    static void SetNumber(int32_t num){
        packNum = num;
        uint8_t * toWrite = reinterpret_cast<uint8_t *>(&num);
        int offset = sizeof(titleHead);
        memcpy((packData + offset), toWrite, sizeof(num));
    }

    static void SetMNU(int16_t vmnu){
        mnu = vmnu;
        uint8_t * toWrite = reinterpret_cast<uint8_t *>(&vmnu);
        int offset = sizeof(titleHead) + sizeof(packNum);
        memcpy((packData + offset), toWrite, sizeof(vmnu));
    }

    static void SetData(uint8_t * data, int size){
        int offset = sizeof(titleHead) + sizeof(packNum) + sizeof(mnu) + sizeof(l);
        memcpy((packData + offset), data, (size < dataLen ? size : dataLen));
    }





};

int16_t TitledPackage :: mnu = -1;
int32_t TitledPackage :: packNum = - 1;
uint8_t TitledPackage :: packData[TitledPackage::packageLen] = {0};
constexpr char TitledPackage::titleHead[];

#endif //_TITLED_PACKAGE_