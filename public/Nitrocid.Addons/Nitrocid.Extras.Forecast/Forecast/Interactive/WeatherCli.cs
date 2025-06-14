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
            InfoBoxNonModalColor.WriteInfoBox(LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_LOADING", "Nitrocid.Extras.Forecast") + $" {item.Item1}, {item.Item2}...", Settings.InfoBoxSettings);
            var WeatherInfo = Forecast.GetWeatherInfo(item.Item1, item.Item2);
            T Adjust<T>(string dayPartData)
            {
                var dayPartArray = WeatherInfo.WeatherToken["daypart"]?[0]?[dayPartData] ??
                    throw new Exception(LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_EXCEPTION_NODAYPARTARRAY", "Nitrocid.Extras.Forecast"));
                var adjusted = dayPartArray[0] ?? dayPartArray[1] ??
                    throw new Exception(LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_EXCEPTION_NODAYPART", "Nitrocid.Extras.Forecast"));
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
                $"{LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_CONDITION", "Nitrocid.Extras.Forecast")}: {WeatherInfo.Weather}{WeatherSpecifier}\n" +
                $"{LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_TEMPERATURE", "Nitrocid.Extras.Forecast")}: {WeatherInfo.Temperature:N2}\n" +
                $"{LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_WINDSPEED", "Nitrocid.Extras.Forecast")}: {WeatherInfo.WindSpeed:N2} {WindSpeedSpecifier}\n" +
                $"{LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_WINDDIRECTION", "Nitrocid.Extras.Forecast")}: {WeatherInfo.WindDirection:N2}°\n" +
                $"{LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_HUMIDITY", "Nitrocid.Extras.Forecast")}: {WeatherInfo.Humidity:N2}%\n" +
                $"{LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_INFO_CLOUDCOVER", "Nitrocid.Extras.Forecast")}: {cloudCover}\n" +
                $"{LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_INFO_TIMEOFDAY", "Nitrocid.Extras.Forecast")}: {dayIndicator}\n" +
                $"{LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_INFO_MINTEMP", "Nitrocid.Extras.Forecast")}: {min}, {WeatherSpecifier}\n" +
                $"{LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_INFO_MAXTEMP", "Nitrocid.Extras.Forecast")}: {max}, {WeatherSpecifier}\n" +
                $"{LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_INFO_PCPCHANGE", "Nitrocid.Extras.Forecast")}: {precipitation}%\n" +
                $"{LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_INFO_PCPTYPE", "Nitrocid.Extras.Forecast")}: {precipitationType}\n" +
                $"{LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_INFO_HEATIDX", "Nitrocid.Extras.Forecast")}: {heatIdx}, {WeatherSpecifier}\n" +
                $"{LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_INFO_WINDCHILL", "Nitrocid.Extras.Forecast")}: {windIdx}, {WeatherSpecifier}\n" +
                $"{LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_INFO_UVINDEX", "Nitrocid.Extras.Forecast")} [0-10]: {uvIdx}\n" +
                $"{LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_INFO_UVDESC", "Nitrocid.Extras.Forecast")}: {uvDesc}\n" +
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
                LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_LAT", "Nitrocid.Extras.Forecast") + $": {item.Item1} | " +
                LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_LON", "Nitrocid.Extras.Forecast") + $": {item.Item2}";
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem((double, double) item)
        {
            return
                LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_LAT", "Nitrocid.Extras.Forecast") + $": {item.Item1} | " +
                LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_LON", "Nitrocid.Extras.Forecast") + $": {item.Item2}";
        }

        internal void Add()
        {
            CheckApiKey();

            // Search for a specific city
            string cityName = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_CITYNAMEPROMPT", "Nitrocid.Extras.Forecast"), Settings.InfoBoxSettings);
            var cities = WeatherForecast.ListAllCities(cityName, Forecast.ApiKey);
            if (cities.Count == 0)
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_NOCITIES", "Nitrocid.Extras.Forecast"), Settings.InfoBoxSettings);
                return;
            }

            // Now, let the user select a city
            int cityIdx = InfoBoxSelectionColor.WriteInfoBoxSelection(cities.Select((kvp, idx) => new InputChoiceInfo($"{idx + 1}", $"[{kvp.Value.Item1}, {kvp.Value.Item2}] {kvp.Key}")).ToArray(), LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_SELECTCITY", "Nitrocid.Extras.Forecast"), Settings.InfoBoxSettings);
            if (cityIdx < 0)
                return;
            var cityLatsLongs = cities.ElementAt(cityIdx).Value;
            latsLongs.Add(cityLatsLongs);
        }

        internal void AddManually()
        {
            CheckApiKey();

            // Let the user input the latitude and the longitude data
            string latString = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_LATPROMPT", "Nitrocid.Extras.Forecast"), Settings.InfoBoxSettings);
            string lngString = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_LONPROMPT", "Nitrocid.Extras.Forecast"), Settings.InfoBoxSettings);
            if (!double.TryParse(latString, out var lat))
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_LATINVALID", "Nitrocid.Extras.Forecast"), Settings.InfoBoxSettings);
                return;
            }
            if (!double.TryParse(lngString, out var lng))
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_LONINVALID", "Nitrocid.Extras.Forecast"), Settings.InfoBoxSettings);
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
                    Forecast.ApiKey = InfoBoxInputColor.WriteInfoBoxInputPassword(LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_APIKEYPROMPT", "Nitrocid.Extras.Forecast"), Settings.InfoBoxSettings);
                    if (string.IsNullOrEmpty(Forecast.ApiKey))
                        InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_APIKEYNOTPROVIDED", "Nitrocid.Extras.Forecast"), Settings.InfoBoxSettings);
                } while (string.IsNullOrEmpty(Forecast.ApiKey));
            }
        }
    }
}
