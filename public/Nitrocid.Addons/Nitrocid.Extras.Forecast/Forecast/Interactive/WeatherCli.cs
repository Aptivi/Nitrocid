//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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
using Nitrocid.Files.Editors.TextEdit;
using System.Linq;
using Terminaux.Inputs.Styles.Infobox;
using Nitrocid.ConsoleBase.Colors;
using Nettify.Weather;
using Terminaux.Inputs;

namespace Nitrocid.Extras.Forecast.Forecast.Interactive
{
    /// <summary>
    /// Weather TUI class
    /// </summary>
    public class WeatherCli : BaseInteractiveTui<(double, double)>, IInteractiveTui<(double, double)>
    {
        private static readonly List<(double, double)> latsLongs = [];
        /// <inheritdoc/>
        public override InteractiveTuiBinding[] Bindings { get; } =
        [
            // Operations
            new InteractiveTuiBinding("Add", ConsoleKey.F1,
                (_, _) => Add(), true),
            new InteractiveTuiBinding("Add Manually", ConsoleKey.F1, ConsoleModifiers.Shift,
                (_, _) => AddManually(), true),
            new InteractiveTuiBinding("Remove", ConsoleKey.F2,
                (_, idx) => Remove(idx)),
            new InteractiveTuiBinding("Remove All", ConsoleKey.F3,
                (_, _) => RemoveAll()),
        ];

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
            InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Loading weather info from geographical location") + $" {item.Item1}, {item.Item2}...", false, KernelColorTools.GetColor(KernelColorType.TuiBoxForeground), KernelColorTools.GetColor(KernelColorType.TuiBoxBackground));
            var WeatherInfo = Forecast.GetWeatherInfo(item.Item1, item.Item2);

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
            string info =
                $"{Translate.DoTranslation("Weather condition")}: {WeatherInfo.Weather}{WeatherSpecifier}\n" +
                $"{Translate.DoTranslation("Temperature")}: {WeatherInfo.Temperature:N2}\n" +
                $"{Translate.DoTranslation("Wind speed")}: {WeatherInfo.WindSpeed:N2} {WindSpeedSpecifier}\n" +
                $"{Translate.DoTranslation("Wind direction")}: {WeatherInfo.WindDirection:N2}°\n" +
                $"{Translate.DoTranslation("Humidity rate")}: {WeatherInfo.Humidity:N2}%\n" +
                $"{Translate.DoTranslation("Geographical location")}: {item.Item1}, {item.Item2}";

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

        private static void Add()
        {
            CheckApiKey();

            // Search for a specific city
            string cityName = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter a city name to search."), KernelColorTools.GetColor(KernelColorType.TuiBoxForeground), KernelColorTools.GetColor(KernelColorType.TuiBoxBackground));
            var cities = WeatherForecast.ListAllCities(cityName, Forecast.ApiKey);
            if (cities.Count == 0)
            {
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("There are no cities by this name. If you are trying to search for a specific city, try searching for a broader location that is closest to the city that you're looking for."), KernelColorTools.GetColor(KernelColorType.TuiBoxForeground), KernelColorTools.GetColor(KernelColorType.TuiBoxBackground));
                return;
            }

            // Now, let the user select a city
            int cityIdx = InfoBoxSelectionColor.WriteInfoBoxSelectionColorBack(cities.Select((kvp, idx) => new InputChoiceInfo($"{idx + 1}", $"[{kvp.Value.Item1}, {kvp.Value.Item2}] {kvp.Key}")).ToArray(), Translate.DoTranslation("Select a city from the list below. The two numbers represent geographical locations."), KernelColorTools.GetColor(KernelColorType.TuiBoxForeground), KernelColorTools.GetColor(KernelColorType.TuiBoxBackground));
            if (cityIdx < 0)
                return;
            var cityLatsLongs = cities.ElementAt(cityIdx).Value;
            latsLongs.Add(cityLatsLongs);
        }

        private static void AddManually()
        {
            CheckApiKey();

            // Let the user input the latitude and the longitude data
            string latString = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter city latitude."), KernelColorTools.GetColor(KernelColorType.TuiBoxForeground), KernelColorTools.GetColor(KernelColorType.TuiBoxBackground));
            string lngString = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter city longitude."), KernelColorTools.GetColor(KernelColorType.TuiBoxForeground), KernelColorTools.GetColor(KernelColorType.TuiBoxBackground));
            if (!double.TryParse(latString, out var lat))
            {
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Latitude is invalid."), KernelColorTools.GetColor(KernelColorType.TuiBoxForeground), KernelColorTools.GetColor(KernelColorType.TuiBoxBackground));
                return;
            }
            if (!double.TryParse(lngString, out var lng))
            {
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Longitude is invalid."), KernelColorTools.GetColor(KernelColorType.TuiBoxForeground), KernelColorTools.GetColor(KernelColorType.TuiBoxBackground));
                return;
            }
            latsLongs.Add((lat, lng));
        }

        private static void Remove(int idx) =>
            latsLongs.RemoveAt(idx);

        private static void RemoveAll() =>
            latsLongs.Clear();

        private static void CheckApiKey()
        {
            if (string.IsNullOrEmpty(Forecast.ApiKey))
            {
                do
                {
                    Forecast.ApiKey = InfoBoxInputPasswordColor.WriteInfoBoxInputPasswordColorBack(Translate.DoTranslation("You can get your own API key by consulting the IBM website for guidance. Enter The Weather Channel API key."), KernelColorTools.GetColor(KernelColorType.TuiBoxForeground), KernelColorTools.GetColor(KernelColorType.TuiBoxBackground));
                    if (string.IsNullOrEmpty(Forecast.ApiKey))
                        InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("The Weather Channel API key is not provided. If you need help, consult the IBM website."), KernelColorTools.GetColor(KernelColorType.TuiBoxForeground), KernelColorTools.GetColor(KernelColorType.TuiBoxBackground));
                } while (string.IsNullOrEmpty(Forecast.ApiKey));
            }
        }
    }
}
