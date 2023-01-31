
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;
using ManagedWeatherMap.Core;
using System;

namespace KS.Misc.Forecast
{
    /// <summary>
    /// Forecast module
    /// </summary>
    public static class Forecast
    {

        /// <summary>
        /// Preferred unit for forecast measurements
        /// </summary>
        public static UnitMeasurement PreferredUnit { get; set; } = UnitMeasurement.Metric;
        internal static string ApiKey = "";

        /// <summary>
        /// Gets current weather info from OpenWeatherMap
        /// </summary>
        /// <param name="CityID">City ID</param>
        /// <returns>A class containing properties of weather information</returns>
        public static ForecastInfo GetWeatherInfo(long CityID) => ManagedWeatherMap.Core.Forecast.GetWeatherInfo(CityID: CityID, ApiKey, PreferredUnit);

        /// <summary>
        /// Gets current weather info from OpenWeatherMap
        /// </summary>
        /// <param name="CityID">City ID</param>
        /// <param name="APIKey">API key</param>
        /// <returns>A class containing properties of weather information</returns>
        public static ForecastInfo GetWeatherInfo(long CityID, string APIKey) => ManagedWeatherMap.Core.Forecast.GetWeatherInfo(CityID: CityID, APIKey, PreferredUnit);

        /// <summary>
        /// Gets current weather info from OpenWeatherMap
        /// </summary>
        /// <param name="CityName">City name</param>
        /// <returns>A class containing properties of weather information</returns>
        public static ForecastInfo GetWeatherInfo(string CityName) => ManagedWeatherMap.Core.Forecast.GetWeatherInfo(CityName: CityName, ApiKey, PreferredUnit);

        /// <summary>
        /// Gets current weather info from OpenWeatherMap
        /// </summary>
        /// <param name="CityName">City name</param>
        /// <param name="APIKey">API key</param>
        /// <returns>A class containing properties of weather information</returns>
        public static ForecastInfo GetWeatherInfo(string CityName, string APIKey) => ManagedWeatherMap.Core.Forecast.GetWeatherInfo(CityName: CityName, APIKey, PreferredUnit);

        /// <summary>
        /// Prints the weather information to the console
        /// </summary>
        /// <param name="CityID">City ID or name</param>
        public static void PrintWeatherInfo(string CityID) => PrintWeatherInfo(CityID, ApiKey);

        /// <summary>
        /// Prints the weather information to the console
        /// </summary>
        /// <param name="CityID">City ID or name</param>
        /// <param name="APIKey">API Key</param>
        public static void PrintWeatherInfo(string CityID, string APIKey)
        {
            ForecastInfo WeatherInfo;
            string WeatherSpecifier = "°";
            string WindSpeedSpecifier = "m.s";
            if (StringQuery.IsStringNumeric(CityID))
            {
                WeatherInfo = GetWeatherInfo(Convert.ToInt64(CityID), APIKey);
            }
            else
            {
                WeatherInfo = GetWeatherInfo(CityID, APIKey);
            }
            DebugWriter.WriteDebug(DebugLevel.I, "City name: {0}, City ID: {1}", WeatherInfo.CityName, WeatherInfo.CityID);
            SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("-- Weather info for {0} --"), false, WeatherInfo.CityName);
            TextWriterColor.Write(Translate.DoTranslation("Weather: {0}"), WeatherInfo.Weather);
            if (WeatherInfo.TemperatureMeasurement == UnitMeasurement.Metric)
            {
                WeatherSpecifier += "C";
            }
            else if (WeatherInfo.TemperatureMeasurement == UnitMeasurement.Kelvin)
            {
                WeatherSpecifier += "K";
            }
            else if (WeatherInfo.TemperatureMeasurement == UnitMeasurement.Imperial)
            {
                WeatherSpecifier += "F";
                WindSpeedSpecifier = "mph";
            }
            TextWriterColor.Write(Translate.DoTranslation("Temperature: {0}") + WeatherSpecifier, WeatherInfo.Temperature.ToString("N2"));
            TextWriterColor.Write(Translate.DoTranslation("Feels like: {0}") + WeatherSpecifier, WeatherInfo.FeelsLike.ToString("N2"));
            TextWriterColor.Write(Translate.DoTranslation("Wind speed: {0}") + " {1}", WeatherInfo.WindSpeed.ToString("N2"), WindSpeedSpecifier);
            TextWriterColor.Write(Translate.DoTranslation("Wind direction: {0}") + "°", WeatherInfo.WindDirection.ToString("N2"));
            TextWriterColor.Write(Translate.DoTranslation("Pressure: {0}") + " hPa", WeatherInfo.Pressure.ToString("N2"));
            TextWriterColor.Write(Translate.DoTranslation("Humidity: {0}") + "%", WeatherInfo.Humidity.ToString("N2"));
        }

    }
}
