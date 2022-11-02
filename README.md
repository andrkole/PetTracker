# PetTracker

Pet tracking application which gathers data from ESP32 microcontroller setup as a server.
Sensors connected to microcontroller are: 
- Temperature and humidity sensor (KY-015)
- Flame sensor (KY-026)
- GPS module (U-BLOX NEO-7M)

Application displays pets' live locations using Google Maps. User is also able to list nearby veterinary clinics and see each pet's statistics for given time frame.
Administrators have dedicated admin panel available for user management.

Application is developed using Blazor. Microcontroller as a server is developed in Arduino IDE (C++).
Duende Identity Server is used for user data management.