
'    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
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

Imports ManagedWeatherMap.Core
Imports KS.Misc.Reflection

Namespace Misc.Forecast
    Public Module Forecast

        Public PreferredUnit As UnitMeasurement = UnitMeasurement.Metric
        Friend ApiKey As String = ""

        ''' <summary>
        ''' Gets current weather info from OpenWeatherMap
        ''' </summary>
        ''' <param name="CityID">City ID</param>
        ''' <returns>A class containing properties of weather information</returns>
        Public Function GetWeatherInfo(CityID As Long) As ForecastInfo
            Return ManagedWeatherMap.Core.Forecast.GetWeatherInfo(CityID:=CityID, ApiKey, PreferredUnit)
        End Function

        ''' <summary>
        ''' Gets current weather info from OpenWeatherMap
        ''' </summary>
        ''' <param name="CityID">City ID</param>
        ''' <param name="APIKey">API key</param>
        ''' <returns>A class containing properties of weather information</returns>
        Public Function GetWeatherInfo(CityID As Long, APIKey As String) As ForecastInfo
            Return ManagedWeatherMap.Core.Forecast.GetWeatherInfo(CityID:=CityID, APIKey, PreferredUnit)
        End Function

        ''' <summary>
        ''' Gets current weather info from OpenWeatherMap
        ''' </summary>
        ''' <param name="CityName">City name</param>
        ''' <returns>A class containing properties of weather information</returns>
        Public Function GetWeatherInfo(CityName As String) As ForecastInfo
            Return ManagedWeatherMap.Core.Forecast.GetWeatherInfo(CityName:=CityName, ApiKey, PreferredUnit)
        End Function

        ''' <summary>
        ''' Gets current weather info from OpenWeatherMap
        ''' </summary>
        ''' <param name="CityName">City name</param>
        ''' <param name="APIKey">API key</param>
        ''' <returns>A class containing properties of weather information</returns>
        Public Function GetWeatherInfo(CityName As String, APIKey As String) As ForecastInfo
            Return ManagedWeatherMap.Core.Forecast.GetWeatherInfo(CityName:=CityName, APIKey, PreferredUnit)
        End Function

        ''' <summary>
        ''' Prints the weather information to the console
        ''' </summary>
        ''' <param name="CityID">City ID or name</param>
        Public Sub PrintWeatherInfo(CityID As String)
            PrintWeatherInfo(CityID, ApiKey)
        End Sub

        ''' <summary>
        ''' Prints the weather information to the console
        ''' </summary>
        ''' <param name="CityID">City ID or name</param>
        ''' <param name="APIKey">API Key</param>
        Public Sub PrintWeatherInfo(CityID As String, APIKey As String)
            Dim WeatherInfo As ForecastInfo
            Dim WeatherSpecifier As String = "°"
            Dim WindSpeedSpecifier As String = "m.s"
            If IsStringNumeric(CityID) Then
                WeatherInfo = GetWeatherInfo(CLng(CityID), APIKey)
            Else
                WeatherInfo = GetWeatherInfo(CityID, APIKey)
            End If
            Wdbg(DebugLevel.I, "City name: {0}, City ID: {1}", WeatherInfo.CityName, WeatherInfo.CityID)
            WriteSeparator(DoTranslation("-- Weather info for {0} --"), False, WeatherInfo.CityName)
            Write(DoTranslation("Weather: {0}"), True, ColTypes.Neutral, WeatherInfo.Weather)
            If WeatherInfo.TemperatureMeasurement = UnitMeasurement.Metric Then
                WeatherSpecifier += "C"
            ElseIf WeatherInfo.TemperatureMeasurement = UnitMeasurement.Kelvin Then
                WeatherSpecifier += "K"
            ElseIf WeatherInfo.TemperatureMeasurement = UnitMeasurement.Imperial Then
                WeatherSpecifier += "F"
                WindSpeedSpecifier = "mph"
            End If
            Write(DoTranslation("Temperature: {0}") + WeatherSpecifier, True, ColTypes.Neutral, WeatherInfo.Temperature.ToString("N2"))
            Write(DoTranslation("Feels like: {0}") + WeatherSpecifier, True, ColTypes.Neutral, WeatherInfo.FeelsLike.ToString("N2"))
            Write(DoTranslation("Wind speed: {0}") + " {1}", True, ColTypes.Neutral, WeatherInfo.WindSpeed.ToString("N2"), WindSpeedSpecifier)
            Write(DoTranslation("Wind direction: {0}") + "°", True, ColTypes.Neutral, WeatherInfo.WindDirection.ToString("N2"))
            Write(DoTranslation("Pressure: {0}") + " hPa", True, ColTypes.Neutral, WeatherInfo.Pressure.ToString("N2"))
            Write(DoTranslation("Humidity: {0}") + "%", True, ColTypes.Neutral, WeatherInfo.Humidity.ToString("N2"))
        End Sub

    End Module
End Namespace