//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using Nettify.Weather;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;
using Nitrocid.ConsoleBase.Colors;

namespace Nitrocid.Extras.Forecast.Forecast
{
    /// <summary>
    /// Forecast module
    /// </summary>
    public static class Forecast
    {

        internal static string ApiKey = "";
        internal static string ApiKeyOwm = "";

        /// <summary>
        /// Preferred unit for forecast measurements
        /// </summary>
        public static UnitMeasurement PreferredUnit =>
            (UnitMeasurement)ForecastInit.ForecastConfig.PreferredUnit;

        /// <summary>
        /// Gets current weather info from The Weather Channel by IBM
        /// </summary>
        /// <param name="latitude">Latitude</param>
        /// <param name="longitude">Longitude</param>
        /// <returns>A class containing properties of weather information</returns>
        public static WeatherForecastInfo GetWeatherInfo(double latitude, double longitude) =>
            WeatherForecast.GetWeatherInfo(latitude, longitude, ApiKey, PreferredUnit);

        /// <summary>
        /// Gets current weather info from The Weather Channel by IBM
        /// </summary>
        /// <param name="latitude">Latitude</param>
        /// <param name="longitude">Longitude</param>
        /// <param name="APIKey">API key</param>
        /// <returns>A class containing properties of weather information</returns>
        public static WeatherForecastInfo GetWeatherInfo(double latitude, double longitude, string APIKey) =>
            WeatherForecast.GetWeatherInfo(latitude, longitude, APIKey, PreferredUnit);

        /// <summary>
        /// Prints the weather information to the console
        /// </summary>
        /// <param name="latitude">Latitude</param>
        /// <param name="longitude">Longitude</param>
        public static void PrintWeatherInfo(double latitude, double longitude) =>
            PrintWeatherInfo(latitude, longitude, ApiKey);

        /// <summary>
        /// Prints the weather information to the console
        /// </summary>
        /// <param name="latitude">Latitude</param>
        /// <param name="longitude">Longitude</param>
        /// <param name="APIKey">API Key</param>
        public static void PrintWeatherInfo(double latitude, double longitude, string APIKey)
        {
            WeatherForecastInfo WeatherInfo = GetWeatherInfo(latitude, longitude, APIKey);
            T Adjust<T>(string dayPartData)
            {
                var dayPartArray = WeatherInfo.WeatherToken["daypart"]?[0]?[dayPartData] ??
                    throw new Exception(Translate.DoTranslation("Can't get day part array"));
                var adjusted = dayPartArray[0] ?? dayPartArray[1] ??
                    throw new Exception(Translate.DoTranslation("Can't get day part"));
                return adjusted.GetValue<T>();
            }

            string WeatherSpecifier = "°";
            string WindSpeedSpecifier = "m.s";
            SeparatorWriterColor.WriteSeparatorColor(Translate.DoTranslation("-- Weather info --"), KernelColorTools.GetColor(KernelColorType.ListTitle));
            TextWriterColor.Write(Translate.DoTranslation("Weather: {0}"), WeatherInfo.Weather);
            if (WeatherInfo.TemperatureMeasurement == UnitMeasurement.Metric)
                WeatherSpecifier += "C";
            else
            {
                WeatherSpecifier += "F";
                WindSpeedSpecifier = "mph";
            }
            TextWriterColor.Write(Translate.DoTranslation("Temperature: {0}") + WeatherSpecifier, WeatherInfo.Temperature.ToString("N2"));
            TextWriterColor.Write(Translate.DoTranslation("Wind speed: {0}") + " {1}", WeatherInfo.WindSpeed.ToString("N2"), WindSpeedSpecifier);
            TextWriterColor.Write(Translate.DoTranslation("Wind direction: {0}") + "°", WeatherInfo.WindDirection.ToString("N2"));
            TextWriterColor.Write(Translate.DoTranslation("Humidity: {0}") + "%", WeatherInfo.Humidity.ToString("N2"));
            TextWriterColor.Write(Translate.DoTranslation("Latitude and Longitude") + ": {0}, {1}", latitude, longitude);

            // More info
            int cloudCover = Adjust<int>("cloudCover");
            string dayIndicator = Adjust<string>("dayOrNight");
            string narrative = Adjust<string>("narrative");
            int precipitation = Adjust<int>("precipChance");
            string precipitationType = Adjust<string>("precipType");
            int heatIdx = Adjust<int>("temperatureHeatIndex");
            int windIdx = Adjust<int>("temperatureWindChill");
            int uvIdx = Adjust<int>("uvIndex");
            string uvDesc = Adjust<string>("uvDescription");
            string windNarrative = Adjust<string>("windPhrase");
            int min = (int?)WeatherInfo.WeatherToken?["calendarDayTemperatureMin"]?[0] ?? 0;
            int max = (int?)WeatherInfo.WeatherToken?["calendarDayTemperatureMax"]?[0] ?? 0;
            TextWriterColor.Write(Translate.DoTranslation("Cloud cover") + ": {0}%", cloudCover);
            TextWriterColor.Write(Translate.DoTranslation("Time of day") + ": {0}", dayIndicator);
            TextWriterColor.Write(Translate.DoTranslation("Minimum temperature") + ": {0}{1}", min, WeatherSpecifier);
            TextWriterColor.Write(Translate.DoTranslation("Maximum temperature") + ": {0}{1}", max, WeatherSpecifier);
            TextWriterColor.Write(Translate.DoTranslation("Precipitation chance") + ": {0}%", precipitation);
            TextWriterColor.Write(Translate.DoTranslation("Precipitation type") + ": {0}", precipitationType);
            TextWriterColor.Write(Translate.DoTranslation("Heat index") + ": {0}{1}", heatIdx, WeatherSpecifier);
            TextWriterColor.Write(Translate.DoTranslation("Wind chill") + ": {0}{1}", windIdx, WeatherSpecifier);
            TextWriterColor.Write(Translate.DoTranslation("Ultraviolet index") + " [0-10]: {0}", uvIdx);
            TextWriterColor.Write(Translate.DoTranslation("Ultraviolet description") + ": {0}", uvDesc);
            TextWriterColor.Write("\n======================\n");
            TextWriterColor.Write($"{narrative} - {windNarrative}");
        }

        #region Legacy (OWM)
        /// <summary>
        /// Gets current weather info from OpenWeatherMap
        /// </summary>
        /// <param name="CityID">City ID</param>
        /// <returns>A class containing properties of weather information</returns>
        public static WeatherForecastInfo GetWeatherInfoOwm(long CityID) =>
            WeatherForecastOwm.GetWeatherInfo(CityID: CityID, ApiKey, PreferredUnit);

        /// <summary>
        /// Gets current weather info from OpenWeatherMap
        /// </summary>
        /// <param name="CityID">City ID</param>
        /// <param name="APIKey">API key</param>
        /// <returns>A class containing properties of weather information</returns>
        public static WeatherForecastInfo GetWeatherInfoOwm(long CityID, string APIKey) =>
            WeatherForecastOwm.GetWeatherInfo(CityID: CityID, APIKey, PreferredUnit);

        /// <summary>
        /// Gets current weather info from OpenWeatherMap
        /// </summary>
        /// <param name="CityName">City name</param>
        /// <returns>A class containing properties of weather information</returns>
        public static WeatherForecastInfo GetWeatherInfoOwm(string CityName) =>
            WeatherForecastOwm.GetWeatherInfo(CityName: CityName, ApiKey, PreferredUnit);

        /// <summary>
        /// Gets current weather info from OpenWeatherMap
        /// </summary>
        /// <param name="CityName">City name</param>
        /// <param name="APIKey">API key</param>
        /// <returns>A class containing properties of weather information</returns>
        public static WeatherForecastInfo GetWeatherInfoOwm(string CityName, string APIKey) =>
            WeatherForecastOwm.GetWeatherInfo(CityName: CityName, APIKey, PreferredUnit);

        /// <summary>
        /// Prints the weather information to the console
        /// </summary>
        /// <param name="CityID">City ID or name</param>
        public static void PrintWeatherInfoOwm(string CityID) =>
            PrintWeatherInfoOwm(CityID, ApiKey);

        /// <summary>
        /// Prints the weather information to the console
        /// </summary>
        /// <param name="CityID">City ID or name</param>
        /// <param name="APIKey">API Key</param>
        public static void PrintWeatherInfoOwm(string CityID, string APIKey)
        {
            WeatherForecastInfo WeatherInfo;
            string WeatherSpecifier = "°";
            string WindSpeedSpecifier = "m.s";
            if (TextTools.IsStringNumeric(CityID))
                WeatherInfo = GetWeatherInfoOwm(Convert.ToInt64(CityID), APIKey);
            else
                WeatherInfo = GetWeatherInfoOwm(CityID, APIKey);
            string name = (string?)WeatherInfo.WeatherToken["name"] ?? "";
            double feelsLike = (double?)WeatherInfo.WeatherToken?["main"]?["feels_like"] ?? 0d;
            double pressure = (double?)WeatherInfo.WeatherToken?["main"]?["pressure"] ?? 0d;
            DebugWriter.WriteDebug(DebugLevel.I, "City name: {0}, City ID: {1}", vars: [name, CityID]);
            SeparatorWriterColor.WriteSeparatorColor(Translate.DoTranslation("-- Weather info for {0} --"), KernelColorTools.GetColor(KernelColorType.ListTitle), true, name);
            TextWriterColor.Write(Translate.DoTranslation("Weather: {0}"), WeatherInfo.Weather);
            if (WeatherInfo.TemperatureMeasurement == UnitMeasurement.Metric)
                WeatherSpecifier += "C";
            else if (WeatherInfo.TemperatureMeasurement == UnitMeasurement.Kelvin)
                WeatherSpecifier += "K";
            else if (WeatherInfo.TemperatureMeasurement == UnitMeasurement.Imperial)
            {
                WeatherSpecifier += "F";
                WindSpeedSpecifier = "mph";
            }
            TextWriterColor.Write(Translate.DoTranslation("Temperature: {0}") + WeatherSpecifier, WeatherInfo.Temperature.ToString("N2"));
            TextWriterColor.Write(Translate.DoTranslation("Feels like: {0}") + WeatherSpecifier, feelsLike.ToString("N2"));
            TextWriterColor.Write(Translate.DoTranslation("Wind speed: {0}") + " {1}", WeatherInfo.WindSpeed.ToString("N2"), WindSpeedSpecifier);
            TextWriterColor.Write(Translate.DoTranslation("Wind direction: {0}") + "°", WeatherInfo.WindDirection.ToString("N2"));
            TextWriterColor.Write(Translate.DoTranslation("Pressure: {0}") + " hPa", pressure.ToString("N2"));
            TextWriterColor.Write(Translate.DoTranslation("Humidity: {0}") + "%", WeatherInfo.Humidity.ToString("N2"));
        }
        #endregion
    }
}
