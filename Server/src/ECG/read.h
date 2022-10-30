
#ifndef _ECG_
#define _ECG_

#include "Arduino.h"

class ECG{

public:
    float Read(){
        if((digitalRead(2) == 1)||(digitalRead(4) == 1)){
            // Serial.println('!');
            return 0.0;
        }
        else{
            // send the value of analog input 0:
            // Serial.println(analogRead(32));
            return (float)analogRead(32);
        }


    }


};

#endif // _ECG_