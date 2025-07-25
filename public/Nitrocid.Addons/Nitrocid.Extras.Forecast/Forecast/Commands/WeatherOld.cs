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

using System;
using System.Linq;
using Nettify.Weather;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Commands;
using Nitrocid.Base.ConsoleBase.Inputs;

namespace Nitrocid.Extras.Forecast.Forecast.Commands
{
    /// <summary>
    /// Shows weather information for a specified city
    /// </summary>
    /// <remarks>
    /// We credit OpenWeatherMap for their decent free API service for weather information for the cities around the world. It requires that you have your own API key for OpenWeatherMap. Don't worry, Nitrocid KS only accesses free features; all you have to do is make an account and generate your own API key.
    /// <br></br>
    /// This command lets you get current weather information for a specified city by city ID as recommended by OpenWeatherMap. If you want a list, use the switch indicated below.
    /// <br></br>
    /// <list type="table">
    /// <listheader>
    /// <term>Switches</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>-list</term>
    /// <description>Lists the available cities</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// </remarks>
    class WeatherOldCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var ListMode = false;
            if (parameters.SwitchesList.Contains("-list"))
                ListMode = true;
            if (ListMode)
            {
                var Cities = WeatherForecastOwm.ListAllCities();
                ListWriterColor.WriteList(Cities);
            }
            else
            {
                string APIKey = Forecast.ApiKeyOwm;
                if (parameters.ArgumentsList.Length > 1)
                {
                    APIKey = parameters.ArgumentsList[1];
                }
                else if (string.IsNullOrEmpty(APIKey))
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FORECAST_WEATHEROLD_APIKEY"));
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FORECAST_APIKEYPROMPT") + " ", false, ThemeColorType.Input);
                    APIKey = InputTools.ReadLineNoInput();
                    Forecast.ApiKeyOwm = APIKey;
                }
                Forecast.PrintWeatherInfoOwm(parameters.ArgumentsList[0], APIKey);
            }
            return 0;
        }

        public override void HelpHelper()
        {
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FORECAST_WEATHEROLD_CITYLISTLINK"));
            TextWriterColor.Write("http://bulk.openweathermap.org/sample/city.list.json.gz");
        }

    }
}
