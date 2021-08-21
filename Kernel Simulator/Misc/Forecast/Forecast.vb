
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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
Imports System.IO
Imports System.IO.Compression
Imports Extensification.ArrayExts
Imports System.Text

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

    ''' <summary>
    ''' Gets current weather info from OpenWeatherMap
    ''' </summary>
    ''' <param name="CityName">City name</param>
    ''' <param name="APIKey">API Key</param>
    ''' <returns>A class containing properties of weather information</returns>
    Public Function GetWeatherInfo(ByVal CityName As String, ByVal APIKey As String, Optional ByVal Unit As UnitMeasurement = UnitMeasurement.Metric) As ForecastInfo
        Dim WeatherInfo As New ForecastInfo With {.CityName = CityName, .TemperatureMeasurement = Unit}
        Dim WeatherURL As String = $"http://api.openweathermap.org/data/2.5/weather?q={CityName}&appid={APIKey}"
        Dim WeatherDownloader As New WebClient
        Dim WeatherData As String
        Dim WeatherToken As JToken
        Wdbg("I", "Made new instance of class with {0} and {1}", CityName, Unit)
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
        WeatherInfo.CityID = WeatherToken.SelectToken("id").ToObject(GetType(Long))
        Return WeatherInfo
    End Function

    ''' <summary>
    ''' Prints the weather information to the console
    ''' </summary>
    ''' <param name="CityID">City ID or name</param>
    ''' <param name="APIKey">API Key</param>
    Public Sub PrintWeatherInfo(ByVal CityID As String, ByVal APIKey As String)
        Dim WeatherInfo As ForecastInfo
        Dim WeatherSpecifier As String = "°"
        Dim WindSpeedSpecifier As String = "m.s"
        If IsNumeric(CityID) Then
            WeatherInfo = GetWeatherInfo(CLng(CityID), APIKey, PreferredUnit)
        Else
            WeatherInfo = GetWeatherInfo(CityID, APIKey, PreferredUnit)
        End If
        Wdbg("I", "City name: {0}, City ID: {1}", WeatherInfo.CityName, WeatherInfo.CityID)
        WriteSeparator(DoTranslation("-- Weather info for {0} --"), False, ColTypes.Stage, WeatherInfo.CityName)
        W(DoTranslation("Weather: {0}"), True, ColTypes.Neutral, WeatherInfo.Weather)
        If WeatherInfo.TemperatureMeasurement = UnitMeasurement.Metric Then
            WeatherSpecifier += "C"
        ElseIf WeatherInfo.TemperatureMeasurement = UnitMeasurement.Kelvin Then
            WeatherSpecifier += "K"
        ElseIf WeatherInfo.TemperatureMeasurement = UnitMeasurement.Imperial Then
            WeatherSpecifier += "F"
            WindSpeedSpecifier = "mph"
        End If
        W(DoTranslation("Temperature: {0}") + WeatherSpecifier, True, ColTypes.Neutral, FormatNumber(WeatherInfo.Temperature, 2))
        W(DoTranslation("Feels like: {0}") + WeatherSpecifier, True, ColTypes.Neutral, FormatNumber(WeatherInfo.FeelsLike, 2))
        W(DoTranslation("Wind speed: {0}") + " {1}", True, ColTypes.Neutral, FormatNumber(WeatherInfo.WindSpeed, 2), WindSpeedSpecifier)
        W(DoTranslation("Wind direction: {0}") + "°", True, ColTypes.Neutral, FormatNumber(WeatherInfo.WindDirection, 2))
        W(DoTranslation("Pressure: {0}") + " hPa", True, ColTypes.Neutral, FormatNumber(WeatherInfo.Pressure, 2))
        W(DoTranslation("Humidity: {0}") + "%", True, ColTypes.Neutral, FormatNumber(WeatherInfo.Humidity, 2))
    End Sub

    ''' <summary>
    ''' Lists all the available cities
    ''' </summary>
    Public Function ListAllCities() As Dictionary(Of Long, String)
        Dim WeatherCityListURL As String = $"http://bulk.openweathermap.org/sample/city.list.json.gz"
        Dim WeatherCityListDownloader As New WebClient
        Dim WeatherCityListData As GZipStream
        Dim WeatherCityListDataStream As Stream
        Dim WeatherCityListUncompressed As New List(Of Byte)
        Dim WeatherCityListReadByte As Integer = 0
        Dim WeatherCityListToken As JToken
        Dim WeatherCityList As New Dictionary(Of Long, String)
        Wdbg("I", "Weather City List URL: {0}", WeatherCityListURL)

        'Download and parse JSON data
        WeatherCityListDataStream = WeatherCityListDownloader.OpenRead(WeatherCityListURL)
        WeatherCityListData = New GZipStream(WeatherCityListDataStream, CompressionMode.Decompress, False)
        Do Until WeatherCityListReadByte = -1
            WeatherCityListReadByte = WeatherCityListData.ReadByte
            If WeatherCityListReadByte <> -1 Then WeatherCityListUncompressed.Add(WeatherCityListReadByte)
        Loop
        WeatherCityListToken = JToken.Parse(Encoding.Default.GetString(WeatherCityListUncompressed.ToArray))

        'Put needed data to the class
        For Each WeatherCityToken As JToken In WeatherCityListToken
            WeatherCityList.AddIfNotFound(WeatherCityToken("id"), WeatherCityToken("name"))
        Next

        'Return list
        Return WeatherCityList
    End Function

End Module
