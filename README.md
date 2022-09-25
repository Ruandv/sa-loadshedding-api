# eskom-calendar-api

This is an API that query various sources for Loadshedding data

## Description

 As the time goes on this api will be updated to include various sources to reflect data that can be used for loadshedding. 
 Currently this api query the following sites : 
 - [beyarkay/eskom-calendar](https://github.com/beyarkay/eskom-calendar/releases/tag/latest) 
 - [Eskom] (https://loadshedding.eskom.co.za/loadshedding) No UI/api interface

## Getting Started
- create an .env file in the root of the project with the following values in it
```
ESKOM_CALENDAR_BASE_URL = "https://github.com/beyarkay/eskom-calendar/releases/download/latest/"
ESKOM_SITE_BASE_URL="https://loadshedding.eskom.co.za/loadshedding/"
```
- install all the NuGet packages for the project from the package manager

### Dependencies

* .Net Core 6.0

### Executing program

* Start the project (eskom-calendar-api) from the IDE

## Authors

Ruan de Villiers [Ruandv](https://github.com/Ruandv)

## Version History

* 0.1
    * Initial Release


## Acknowledgments

Inspiration, code snippets, etc.
* beyarkay/eskom-calendar https://github.com/beyarkay/eskom-calendar/
* Eskom https://www.eskom.co.za/