﻿//
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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using System.Linq;
using Nitrocid.ConsoleBase.Inputs;
using Nettify.Weather;
using Nitrocid.Shell.ShellBase.Switches;
using Terminaux.Inputs.Interactive;
using Nitrocid.Extras.Forecast.Forecast.Interactive;
using System;
using Terminaux.Writer.CyclicWriters;

namespace Nitrocid.Extras.Forecast.Forecast.Commands
{
    /// <summary>
    /// Shows weather information for a specified city
    /// </summary>
    /// <remarks>
    /// We credit IBM for their decent API service for weather information for the cities around the world from The Weather Channel.
    /// <br></br>
    /// This command lets you get current weather information for a specified city by city ID from The Weather Channel servers. If you want a list, use the switch indicated below.
    /// <br></br>
    /// <list type="table">
    /// <listheader>
    /// <term>Switches</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>-list</term>
    /// <description>Searches for the written city name and returns the list of longitudes and latitudes</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// </remarks>
    class WeatherCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (SwitchManager.ContainsSwitch(parameters.SwitchesList, "-tui"))
            {
                var tui = new WeatherCli();
                tui.Bindings.Add(new InteractiveTuiBinding<(double, double)>(Translate.DoTranslation("Add"), ConsoleKey.F1, (_, _, _, _) => tui.Add(), true));
                tui.Bindings.Add(new InteractiveTuiBinding<(double, double)>(Translate.DoTranslation("Add Manually"), ConsoleKey.F1, ConsoleModifiers.Shift, (_, _, _, _) => tui.AddManually(), true));
                tui.Bindings.Add(new InteractiveTuiBinding<(double, double)>(Translate.DoTranslation("Remove"), ConsoleKey.F2, (_, idx, _, _) => tui.Remove(idx)));
                tui.Bindings.Add(new InteractiveTuiBinding<(double, double)>(Translate.DoTranslation("Remove All"), ConsoleKey.F3, (_, _, _, _) => tui.RemoveAll()));
                InteractiveTuiTools.OpenInteractiveTui(tui);
                return 0;
            }
            if (parameters.ArgumentsList.Length <= 1)
            {
                TextWriters.Write(Translate.DoTranslation("Enter the city latitude and longitude."), KernelColorType.Error);
                return 38;
            }
            string APIKey = Forecast.ApiKey;
            if (parameters.ArgumentsList.Length > 2)
            {
                APIKey = parameters.ArgumentsList[2];
            }
            else if (string.IsNullOrEmpty(APIKey))
            {
                TextWriterColor.Write(Translate.DoTranslation("You can get your own API key by consulting the IBM website for guidance."));
                TextWriters.Write(Translate.DoTranslation("Enter your API key:") + " ", false, KernelColorType.Input);
                APIKey = InputTools.ReadLineNoInput();
                Forecast.ApiKey = APIKey;
            }
            var ListMode = false;
            if (parameters.SwitchesList.Contains("-list"))
                ListMode = true;
            if (ListMode)
            {
                var Cities = WeatherForecast.ListAllCities(SwitchManager.GetSwitchValue(parameters.SwitchesList, "-list"), APIKey);
                var listing = new Listing()
                {
                    Objects = Cities,
                    KeyColor = KernelColorTools.GetColor(KernelColorType.ListEntry),
                    ValueColor = KernelColorTools.GetColor(KernelColorType.ListValue),
                };
                TextWriterRaw.WriteRaw(listing.Render());
            }
            else
            {
                double latitude = double.Parse(parameters.ArgumentsList[0]);
                double longitude = double.Parse(parameters.ArgumentsList[1]);
                Forecast.PrintWeatherInfo(latitude, longitude, APIKey);
            }
            return 0;
        }

    }
}
