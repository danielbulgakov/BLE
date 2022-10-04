#include <type_traits>
typedef uint8_t cbyte;

template<class DataType>
class DataPackage
{
private:
    cbyte * dataPackage;       // Массив хранения данных
    int packageSize;    // Размер массива
    int endIndex;       // Индекс последнего не нулевого элемента

public:

    /**
     * @brief Конструкор DataPackage
     * 
     * @param size размер пакета в байтах
     */
    DataPackage(int size)
    {
        this->dataPackage = new cbyte[size];
        this->packageSize = size;
        this->endIndex = 0;
        for (int i = 0; i < packageSize; i++){
            dataPackage[i] = 0;
        }
        // memset(dataPackage, atoi("0"), packageSize);
    }

    cbyte * GetData() {
        return dataPackage;
    }

    int GetLength(){
        return packageSize;
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


    bool isFittable(int length){
        return length <= (packageSize - endIndex);
    }

    

};