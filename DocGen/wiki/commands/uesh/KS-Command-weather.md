## weather command

### Summary

Shows weather information for a specified city

### Description

We credit OpenWeatherMap for their decent free API service for weather information for the cities around the world. It requires that you have your own API key for OpenWeatherMap. Don't worry, Nitrocid KS only accesses free features; all you have to do is make an account and generate your own API key [here](https://home.openweathermap.org/api_keys).

This command lets you get current weather information for a specified city by city ID as recommended by OpenWeatherMap. If you want a list, use the switch s indicated below.

The following information will be displayed:

- Temperature: Temperature in either Kelvin, Celsius, or Fahrenheit.*
- Feels like: Feels like temperature in either Kelvin, Celsius, or Fahrenheit.*
- Wind speed: Wind speed in either m.s or mph.*
- Wind direction: Wind direction in degrees.
- Pressure: Pressure in hPa.
- Humidity: Humidity in percent.

*: You can change this in config entry `Preferred Unit for Temperature` in Misc section.

| Switches | Description
|:---------|:------------
| -list    | Lists the available cities

### Command usage

* `weather [-list] <CityID/CityName> [apikey]`

### Examples

* `weather 1261481`: Displays the weather information for New Delhi.
* `weather 4183849`: Displays the weather information for Boston in US.