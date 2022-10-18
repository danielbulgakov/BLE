#ifndef _DATAPACKAGE_
#define _DATAPACKAGE_

#include "Template/standart_package.h"
#include <type_traits>
typedef uint8_t cbyte;

template<class DataType>
class DataPackage
{
private:
//STATIC
    cbyte * dataPackage;       // Массив хранения данных
    int packageSize;    // Размер массива в байтах
    int endIndex;       // Индекс последнего не нулевого элемента

public:

    /**
     * @brief Конструкор DataPackage
     * 
     * @param size размер пакета
     */
    DataPackage(int size)
    {
        this->dataPackage = new cbyte[size * sizeof(DataType)];
        this->packageSize = size* sizeof(DataType);
        this->endIndex = 0;
        for (int i = 0; i < packageSize; i++){
            dataPackage[i] = 0;
        }
        // memset(dataPackage, atoi("0"), packageSize);
    }

    ~DataPackage(){
        delete[] dataPackage;
    }

    cbyte * GetData() {
        return dataPackage;
    }

    int GetLength(){
        return packageSize;
    }

    void Clear(){
        delete[] dataPackage;
        this->dataPackage = new cbyte[this->packageSize];
        this->endIndex = 0;
        for (int i = 0; i < packageSize; i++){
            dataPackage[i] = 0;
        }
    }


    int AddData(DataType* data, int length = 1){
        if (!isFittable(sizeof(DataType) * length)) return -1;
        cbyte * localData;
        for (int i = 0; i < length; i ++){
            localData = reinterpret_cast<cbyte*>(&data[i]);
            this->pushBack(localData, sizeof(DataType) );
        }
        return 0;
    }

private:

    void pushBack (cbyte* dataByte, int length){
        for (int i = 0; i < length; i++){
            if (endIndex != packageSize){
                dataPackage[endIndex] = dataByte[i];
                endIndex++;
            }

        }
    }

    /**
     * @brief Проверка на вместимость в байтах.
     * 
     * @param length размер в байтах.
     * @return true если осталось место.
     * @return false если не осталось места.
     */
    bool isFittable(int length){
        return length <= (packageSize - endIndex);
    }

    

};

#endif // _DATAPACKAGE_