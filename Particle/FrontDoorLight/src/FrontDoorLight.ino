// This #include statement was automatically added by the Particle IDE.
#include "Sunrise.h"
#include "math.h"

int led1 = D0;
int led2 = D7;

Thread* lightThread;
int lightIsOn;
volatile int lightCounterReset;
Sunrise sunrise(-36.8485,174.7633, 12);

double sunRiseTime;
double sunSetTime;

void setup()
{
    pinMode(led1, OUTPUT);
    pinMode(led2, OUTPUT);
    digitalWrite(led1, LOW);
    digitalWrite(led2, LOW);
    lightIsOn = 0;
    lightCounterReset = 0;
    Time.zone(12);
    sunrise.Astronomical();

    sunRiseTime = sunrise.Rise(Time.month(),Time.day());
    sunRiseTime /= 60.0;
    sunSetTime = sunrise.Set(Time.month(),Time.day());
    sunSetTime /= 60.0;

    Particle.variable("sunRiseTime", sunRiseTime);
    Particle.variable("sunSetTime", sunSetTime);
    Particle.variable("lightIsOn", lightIsOn);

    Particle.function("turnOn30s",turnOn30s);
}

void loop()
{
   // Nothing to do here
}

void lightOn30s(){
    int lightCounter = 30;
    lightIsOn = 1;
    digitalWrite(led1,HIGH);
    digitalWrite(led2,HIGH);
    while (lightCounter > 0) {
        delay(1000);
        lightCounter --;
        if (lightCounterReset == 1) {
            lightCounter = 30;
            lightCounterReset = 0;
        }
    }
    digitalWrite(led1,LOW);
    digitalWrite(led2,LOW);
    lightIsOn = 0;
}

int turnOn30s(String Command) {
    int hour = Time.hour();
    int min = Time.minute();

    sunRiseTime = sunrise.Rise(Time.month(),Time.day()) / 60.0;
    int hour_rise = sunrise.sun_Hour();
    int min_rise = sunrise.sun_Minute();

    sunSetTime = sunrise.Set(Time.month(),Time.day()) / 60.0;
    int hour_set = sunrise.sun_Hour();
    int min_set = sunrise.sun_Minute();

    if (
        ((hour > hour_rise)||((hour==hour_rise)&&(min>min_rise)))  &&
        ((hour < hour_set)||((hour==hour_set)&&(min<min_set)))
    ) {
        return 3;
    }
    if (lightIsOn==0) {
        lightThread = new Thread("lightOn30s", lightOn30s);
        return 1;
    } else {
        lightCounterReset = 1;
        return 2;
    }
}
