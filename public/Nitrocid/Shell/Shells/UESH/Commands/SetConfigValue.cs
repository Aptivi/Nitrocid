﻿//
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers;
using KS.Kernel.Configuration;
using KS.Kernel.Configuration.Settings;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using System.Linq;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Sets a configuration value
    /// </summary>
    /// <remarks>
    /// This command prints a configuration value.
    /// </remarks>
    class SetConfigValueCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var configs = Config.GetKernelConfigs();
            string configName = parameters.ArgumentsList[0];
            string varName = parameters.ArgumentsList[1];
            if (ConfigTools.IsCustomSettingRegistered(configName))
            {
                var config = configs.Single((bkc) => bkc.GetType().Name == configName);
                var keys = ConfigTools.GetSettingsKeys(config);
                if (keys.Any((sk) => sk.Variable == varName))
                {
                    var key = ConfigTools.GetSettingsKey(config, varName);
                    var finalValue = key.KeyInput.TranslateStringValue(key, parameters.ArgumentsList[2]);
                    SettingsAppTools.SetPropertyValue(key.Variable, finalValue, config);
                }
                else
                {
                    TextWriters.Write(Translate.DoTranslation("Key not found."), KernelColorType.Error);
                    return 28;
                }
            }
            else
            {
                TextWriters.Write(Translate.DoTranslation("Config not found."), KernelColorType.Error);
                return 28;
            }
            return 0;
        }

    }
}