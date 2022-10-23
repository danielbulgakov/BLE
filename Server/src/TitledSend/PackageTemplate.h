#ifndef _PACKAGE_TEMPLATE_
#define _PACKAGE_TEMPLATE_

#include <string>

class PackageTemplate{
protected:
    virtual void VirtualSetTitle(std::string name) = 0;

    virtual void VirtualSetNumber(int32_t num) = 0;

    virtual void VirtualSetMNU(int16_t vmnu) = 0;

    virtual void VirtualSetData(uint8_t * data, int size) = 0;

    virtual uint8_t * VirtualGetUsefulData() = 0;

    virtual uint8_t * VirtualGetRawData() = 0;

    virtual int VirtualGetLen() = 0;
public :

    void SetTitle(std::string name){
        VirtualSetTitle(name);
    }

    void SetNumber(int32_t num){
        VirtualSetNumber(num);
    }

    void SetMNU(int16_t vmnu){
        VirtualSetMNU(vmnu);
    }

    void SetData(uint8_t * data, int size){
        VirtualSetData(data, size);
    }

    uint8_t * GetUsefulData(){
        return VirtualGetUsefulData();
    }

    uint8_t * GetRawData(){
        return VirtualGetRawData();
    }

    int GetLen(){
        return VirtualGetLen();
    }
};

#endif // _PACKAGE_TEMPLATE_