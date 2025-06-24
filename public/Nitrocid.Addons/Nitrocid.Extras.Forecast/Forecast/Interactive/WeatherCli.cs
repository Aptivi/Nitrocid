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
using Nitrocid.Base.Languages;
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
            InfoBoxNonModalColor.WriteInfoBox(LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_LOADING") + $" {item.Item1}, {item.Item2}...", Settings.InfoBoxSettings);
            var WeatherInfo = Forecast.GetWeatherInfo(item.Item1, item.Item2);
            T Adjust<T>(string dayPartData)
            {
                var dayPartArray = WeatherInfo.WeatherToken["daypart"]?[0]?[dayPartData] ??
                    throw new Exception(LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_EXCEPTION_NODAYPARTARRAY"));
                var adjusted = dayPartArray[0] ?? dayPartArray[1] ??
                    throw new Exception(LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_EXCEPTION_NODAYPART"));
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
                $"{LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_CONDITION")}: {WeatherInfo.Weather}{WeatherSpecifier}\n" +
                $"{LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_TEMPERATURE")}: {WeatherInfo.Temperature:N2}\n" +
                $"{LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_WINDSPEED")}: {WeatherInfo.WindSpeed:N2} {WindSpeedSpecifier}\n" +
                $"{LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_WINDDIRECTION")}: {WeatherInfo.WindDirection:N2}°\n" +
                $"{LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_HUMIDITY")}: {WeatherInfo.Humidity:N2}%\n" +
                $"{LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_INFO_CLOUDCOVER")}: {cloudCover}\n" +
                $"{LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_INFO_TIMEOFDAY")}: {dayIndicator}\n" +
                $"{LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_INFO_MINTEMP")}: {min}, {WeatherSpecifier}\n" +
                $"{LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_INFO_MAXTEMP")}: {max}, {WeatherSpecifier}\n" +
                $"{LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_INFO_PCPCHANGE")}: {precipitation}%\n" +
                $"{LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_INFO_PCPTYPE")}: {precipitationType}\n" +
                $"{LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_INFO_HEATIDX")}: {heatIdx}, {WeatherSpecifier}\n" +
                $"{LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_INFO_WINDCHILL")}: {windIdx}, {WeatherSpecifier}\n" +
                $"{LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_INFO_UVINDEX")} [0-10]: {uvIdx}\n" +
                $"{LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_INFO_UVDESC")}: {uvDesc}\n" +
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
                LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_LAT") + $": {item.Item1} | " +
                LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_LON") + $": {item.Item2}";
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem((double, double) item)
        {
            return
                LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_LAT") + $": {item.Item1} | " +
                LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_LON") + $": {item.Item2}";
        }

        internal void Add()
        {
            CheckApiKey();

            // Search for a specific city
            string cityName = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_CITYNAMEPROMPT"), Settings.InfoBoxSettings);
            var cities = WeatherForecast.ListAllCities(cityName, Forecast.ApiKey);
            if (cities.Count == 0)
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_NOCITIES"), Settings.InfoBoxSettings);
                return;
            }

            // Now, let the user select a city
            int cityIdx = InfoBoxSelectionColor.WriteInfoBoxSelection(cities.Select((kvp, idx) => new InputChoiceInfo($"{idx + 1}", $"[{kvp.Value.Item1}, {kvp.Value.Item2}] {kvp.Key}")).ToArray(), LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_SELECTCITY"), Settings.InfoBoxSettings);
            if (cityIdx < 0)
                return;
            var cityLatsLongs = cities.ElementAt(cityIdx).Value;
            latsLongs.Add(cityLatsLongs);
        }

        internal void AddManually()
        {
            CheckApiKey();

            // Let the user input the latitude and the longitude data
            string latString = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_LATPROMPT"), Settings.InfoBoxSettings);
            string lngString = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_LONPROMPT"), Settings.InfoBoxSettings);
            if (!double.TryParse(latString, out var lat))
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_LATINVALID"), Settings.InfoBoxSettings);
                return;
            }
            if (!double.TryParse(lngString, out var lng))
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_LONINVALID"), Settings.InfoBoxSettings);
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
                    Forecast.ApiKey = InfoBoxInputColor.WriteInfoBoxInputPassword(LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_APIKEYPROMPT"), Settings.InfoBoxSettings);
                    if (string.IsNullOrEmpty(Forecast.ApiKey))
                        InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_FORECAST_WEATHER_TUI_APIKEYNOTPROVIDED"), Settings.InfoBoxSettings);
                } while (string.IsNullOrEmpty(Forecast.ApiKey));
            }
        }
    }
}
