//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using Terminaux.Inputs.Interactive;
using Nitrocid.Languages;
using System.Collections.Generic;
using System;
using System.Linq;
using Terminaux.Inputs.Styles.Infobox;
using Nitrocid.ConsoleBase.Colors;
using Nettify.Weather;
using Terminaux.Inputs.Styles;

namespace Nitrocid.Extras.Forecast.Forecast.Interactive
{
    /// <summary>
    /// Weather TUI class
    /// </summary>
    public class WeatherCli : BaseInteractiveTui<(double, double)>, IInteractiveTui<(double, double)>
    {
        internal readonly List<(double, double)> latsLongs = [];

        /// <inheritdoc/>
        public override IEnumerable<(double, double)> PrimaryDataSource =>
            latsLongs;

        /// <inheritdoc/>
        public override bool AcceptsEmptyData =>
            true;

        /// <inheritdoc/>
        public override string GetInfoFromItem((double, double) item)
        {
            // Load the weather information, given the API key provided by the command line. Prompt for it if empty.
            CheckApiKey();
            InfoBoxNonModalColor.WriteInfoBox(Translate.DoTranslation("Loading weather info from geographical location") + $" {item.Item1}, {item.Item2}...", Settings.InfoBoxSettings);
            var WeatherInfo = Forecast.GetWeatherInfo(item.Item1, item.Item2);
            T Adjust<T>(string dayPartData)
            {
                var dayPartArray = WeatherInfo.WeatherToken["daypart"]?[0]?[dayPartData] ??
                    throw new Exception(Translate.DoTranslation("Can't get day part array"));
                var adjusted = dayPartArray[0] ?? dayPartArray[1] ??
                    throw new Exception(Translate.DoTranslation("Can't get day part"));
                return adjusted.GetValue<T>();
            }

            // Print them to a string
            string WeatherSpecifier = "°";
            string WindSpeedSpecifier = "m.s";
            if (WeatherInfo.TemperatureMeasurement == UnitMeasurement.Metric)
                WeatherSpecifier += "C";
            else
            {
                WeatherSpecifier += "F";
                WindSpeedSpecifier = "mph";
            }
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
            string info =
                $"{Translate.DoTranslation("Weather condition")}: {WeatherInfo.Weather}{WeatherSpecifier}\n" +
                $"{Translate.DoTranslation("Temperature")}: {WeatherInfo.Temperature:N2}\n" +
                $"{Translate.DoTranslation("Wind speed")}: {WeatherInfo.WindSpeed:N2} {WindSpeedSpecifier}\n" +
                $"{Translate.DoTranslation("Wind direction")}: {WeatherInfo.WindDirection:N2}°\n" +
                $"{Translate.DoTranslation("Humidity rate")}: {WeatherInfo.Humidity:N2}%\n" +
                $"{Translate.DoTranslation("Cloud cover")}: {cloudCover}\n" +
                $"{Translate.DoTranslation("Time of day")}: {dayIndicator}\n" +
                $"{Translate.DoTranslation("Minimum temperature")}: {min}, {WeatherSpecifier}\n" +
                $"{Translate.DoTranslation("Maximum temperature")}: {max}, {WeatherSpecifier}\n" +
                $"{Translate.DoTranslation("Precipitation chance")}: {precipitation}%\n" +
                $"{Translate.DoTranslation("Precipitation type")}: {precipitationType}\n" +
                $"{Translate.DoTranslation("Heat index")}: {heatIdx}, {WeatherSpecifier}\n" +
                $"{Translate.DoTranslation("Wind chill")}: {windIdx}, {WeatherSpecifier}\n" +
                $"{Translate.DoTranslation("Ultraviolet index")} [0-10]: {uvIdx}\n" +
                $"{Translate.DoTranslation("Ultraviolet description")}: {uvDesc}\n" +
                $"======================\n" +
                $"{narrative} - {windNarrative}"
            ;

            // Render them to the second pane
            return info;
        }

        /// <inheritdoc/>
        public override string GetStatusFromItem((double, double) item)
        {
            return
                Translate.DoTranslation("Latitude") + $": {item.Item1} | " +
                Translate.DoTranslation("Longitude") + $": {item.Item2}";
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem((double, double) item)
        {
            return
                Translate.DoTranslation("Latitude") + $": {item.Item1} | " +
                Translate.DoTranslation("Longitude") + $": {item.Item2}";
        }

        internal void Add()
        {
            CheckApiKey();

            // Search for a specific city
            string cityName = InfoBoxInputColor.WriteInfoBoxInput(Translate.DoTranslation("Enter a city name to search."), Settings.InfoBoxSettings);
            var cities = WeatherForecast.ListAllCities(cityName, Forecast.ApiKey);
            if (cities.Count == 0)
            {
                InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("There are no cities by this name. If you are trying to search for a specific city, try searching for a broader location that is closest to the city that you're looking for."), Settings.InfoBoxSettings);
                return;
            }

            // Now, let the user select a city
            int cityIdx = InfoBoxSelectionColor.WriteInfoBoxSelection(cities.Select((kvp, idx) => new InputChoiceInfo($"{idx + 1}", $"[{kvp.Value.Item1}, {kvp.Value.Item2}] {kvp.Key}")).ToArray(), Translate.DoTranslation("Select a city from the list below. The two numbers represent geographical locations."), Settings.InfoBoxSettings);
            if (cityIdx < 0)
                return;
            var cityLatsLongs = cities.ElementAt(cityIdx).Value;
            latsLongs.Add(cityLatsLongs);
        }

        internal void AddManually()
        {
            CheckApiKey();

            // Let the user input the latitude and the longitude data
            string latString = InfoBoxInputColor.WriteInfoBoxInput(Translate.DoTranslation("Enter city latitude."), Settings.InfoBoxSettings);
            string lngString = InfoBoxInputColor.WriteInfoBoxInput(Translate.DoTranslation("Enter city longitude."), Settings.InfoBoxSettings);
            if (!double.TryParse(latString, out var lat))
            {
                InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("Latitude is invalid."), Settings.InfoBoxSettings);
                return;
            }
            if (!double.TryParse(lngString, out var lng))
            {
                InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("Longitude is invalid."), Settings.InfoBoxSettings);
                return;
            }
            latsLongs.Add((lat, lng));
        }

        internal void Remove(int idx) =>
            latsLongs.RemoveAt(idx);

        internal void RemoveAll() =>
            latsLongs.Clear();

        internal void CheckApiKey()
        {
            if (string.IsNullOrEmpty(Forecast.ApiKey))
            {
                do
                {
                    Forecast.ApiKey = InfoBoxInputColor.WriteInfoBoxInputPassword(Translate.DoTranslation("You can get your own API key by consulting the IBM website for guidance. Enter The Weather Channel API key."), Settings.InfoBoxSettings);
                    if (string.IsNullOrEmpty(Forecast.ApiKey))
                        InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("The Weather Channel API key is not provided. If you need help, consult the IBM website."), Settings.InfoBoxSettings);
                } while (string.IsNullOrEmpty(Forecast.ApiKey));
            }
        }
    }
}
