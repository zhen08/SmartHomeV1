/*
 * Project GarageDoor
 * Description:
 * Author:
 * Date:
 */

 int relay = D0;
 int led = D7;

// setup() runs once, when the device is first turned on.
void setup() {
  pinMode(relay, OUTPUT);
  pinMode(led, OUTPUT);
  digitalWrite(relay, LOW);
  digitalWrite(led, LOW);

  Particle.function("remotebutton",remotebutton);

}

// loop() runs over and over again, as quickly as it can execute.
void loop() {
  // The core of your code will likely live here.

}

int remotebutton(String Command) {
  digitalWrite(relay,HIGH);
  digitalWrite(led,HIGH);
  delay(500);
  digitalWrite(relay, LOW);
  digitalWrite(led, LOW);
}
