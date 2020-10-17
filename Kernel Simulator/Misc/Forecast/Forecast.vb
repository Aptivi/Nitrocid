
'    Kernel Simulator  Copyright (C) 2018-2020  EoflaOE
'
'    This file is part of Kernel Simulator
'
'    Kernel Simulator is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 3 of the License, or
'    (at your option) any later version.
'
'    Kernel Simulator is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.
'
'    You should have received a copy of the GNU General Public License
'    along with this program.  If not, see <https://www.gnu.org/licenses/>.

Imports Newtonsoft.Json.Linq

Public Module Forecast

    Public PreferredUnit As UnitMeasurement = UnitMeasurement.Metric

    ''' <summary>
    ''' Gets current weather info from OpenWeatherMap
    ''' </summary>
    ''' <param name="CityID">City ID</param>
    ''' <param name="APIKey">API key</param>
    ''' <returns>A class containing properties of weather information</returns>
    Public Function GetWeatherInfo(ByVal CityID As Long, ByVal APIKey As String, Optional ByVal Unit As UnitMeasurement = UnitMeasurement.Metric) As ForecastInfo
        Dim WeatherInfo As New ForecastInfo With {.CityID = CityID, .TemperatureMeasurement = Unit}
        Dim WeatherURL As String = $"http://api.openweathermap.org/data/2.5/weather?id={CityID}&appid={APIKey}"
        Dim WeatherDownloader As New WebClient
        Dim WeatherData As String
        Dim WeatherToken As JToken
        Wdbg("I", "Made new instance of class with {0} and {1}", CityID, Unit)
        Wdbg("I", "Weather URL: {0}", WeatherURL)

        'Deal with measurements
        If Unit = UnitMeasurement.Imperial Then
            WeatherURL += "&units=imperial"
        Else
            WeatherURL += "&units=metric"
        End If

        'Download and parse JSON data
        WeatherData = WeatherDownloader.DownloadString(WeatherURL)
        WeatherToken = JToken.Parse(WeatherData)

        'Put needed data to the class
        WeatherInfo.Weather = WeatherToken.SelectToken("weather").First.SelectToken("id").ToObject(GetType(WeatherCondition))
        WeatherInfo.Temperature = WeatherToken.SelectToken("main").SelectToken("temp").ToObject(GetType(Double))
        WeatherInfo.FeelsLike = WeatherToken.SelectToken("main").SelectToken("feels_like").ToObject(GetType(Double))
        WeatherInfo.Pressure = WeatherToken.SelectToken("main").SelectToken("pressure").ToObject(GetType(Double))
        WeatherInfo.Humidity = WeatherToken.SelectToken("main").SelectToken("humidity").ToObject(GetType(Double))
        WeatherInfo.WindSpeed = WeatherToken.SelectToken("wind").SelectToken("speed").ToObject(GetType(Double))
        WeatherInfo.WindDirection = WeatherToken.SelectToken("wind").SelectToken("deg").ToObject(GetType(Double))
        WeatherInfo.CityName = WeatherToken.SelectToken("name").ToString
        Return WeatherInfo
    End Function

End Module
