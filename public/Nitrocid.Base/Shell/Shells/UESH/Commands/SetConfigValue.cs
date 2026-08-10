//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.Linq;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Configuration.Settings;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Sets a configuration value
    /// </summary>
    /// <remarks>
    /// This command prints a configuration value.
    /// </remarks>
    class SetConfigValueCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "setconfigvalue";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SETCONFIGVALUE_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(new CommandArgumentPart[]
                {
                    new(true, "config", new CommandArgumentPartOptions()
                    {
                        AutoCompleter = (_) => Config.GetKernelConfigs().Select((bkc) => bkc.GetType().Name).ToArray(),
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_GETCONFIGVALUE_ARGUMENT_CONFIG_DESC"
                    }),
                    new(true, "variable", new CommandArgumentPartOptions()
                    {
                        AutoCompleter = (arg) => ConfigTools.GetSettingsKeys(arg[0]).Select((sk) => sk.Variable).ToArray(),
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SETCONFIGVALUE_ARGUMENT_VARIABLE_DESC"
                    }),
                    new(true, "value", new CommandArgumentPartOptions()
                    {
                        // TODO: NKS_SHELL_SHELLS_UESH_COMMAND_SETCONFIGVALUE_ARGUMENT_VALUE_DESC -> Value to set to
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SETCONFIGVALUE_ARGUMENT_VALUE_DESC"
                    })
                })
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            var configs = Config.GetKernelConfigs();
            string configName = parameters.ArgumentsList[0];
            string varName = parameters.ArgumentsList[1];
            string valueSet = parameters.ArgumentsList[1];
            if (ConfigTools.IsCustomSettingRegistered(configName))
            {
                var config = configs.Single((bkc) => bkc.GetType().Name == configName);
                var keys = ConfigTools.GetSettingsKeys(config);
                if (keys.Any((sk) => sk.Variable == varName))
                {
                    var key = ConfigTools.GetSettingsKey(config, varName);
                    var finalValue = key.KeyInput.TranslateStringValue(key, valueSet);
                    SettingsAppTools.SetPropertyValue(key.Variable, finalValue, config);
                }
                else
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETCONFIGVALUE_KEYNOTFOUND"), ThemeColorType.Error);
                    return 28;
                }
            }
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETCONFIGVALUE_CONFIGNOTFOUND"), ThemeColorType.Error);
                return 28;
            }
            return 0;
        }

        public override void HelpHelper(IShell? shell)
        {
            var names = Config.GetKernelConfigs().Select((bkc) => bkc.GetType().Name).ToArray();
            ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETCONFIGVALUE_CONFIGTYPES"), string.Join(", ", names));
        }

    }
}
