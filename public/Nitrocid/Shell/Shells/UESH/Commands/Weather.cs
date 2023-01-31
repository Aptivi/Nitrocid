
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

using System;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Languages;
using KS.Misc.Forecast;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
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
    class WeatherCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            var ListMode = false;
            if (ListSwitchesOnly.Contains("-list"))
                ListMode = true;
            if (ListMode)
            {
                var Cities = ManagedWeatherMap.Core.Forecast.ListAllCities();
                ListWriterColor.WriteList(Cities);
            }
            else
            {
                string APIKey = Forecast.ApiKey;
                if (ListArgsOnly.Length > 1)
                {
                    APIKey = ListArgsOnly[1];
                }
                else if (string.IsNullOrEmpty(APIKey))
                {
                    TextWriterColor.Write(Translate.DoTranslation("You can get your own API key at https://home.openweathermap.org/api_keys."));
                    TextWriterColor.Write(Translate.DoTranslation("Enter your API key:") + " ", false, KernelColorType.Input);
                    APIKey = Input.ReadLineNoInput();
                    Forecast.ApiKey = APIKey;
                }
                Forecast.PrintWeatherInfo(ListArgsOnly[0], APIKey);
            }
        }

        public override void HelpHelper()
        {
            TextWriterColor.Write(Translate.DoTranslation("You can always consult http://bulk.openweathermap.org/sample/city.list.json.gz for the list of cities with their IDs.") + " " + Translate.DoTranslation("Or, pass \"listcities\" to this command."));
            TextWriterColor.Write(Translate.DoTranslation("This command has the below switches that change how it works:"));
            TextWriterColor.Write("  -list: ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Shows all the available cities"), true, KernelColorType.ListValue);
        }

    }
}