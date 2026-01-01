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

using System.Linq;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using Terminaux.Writer.FancyWriters;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Lists all configuration values
    /// </summary>
    /// <remarks>
    /// This command lists all the configuration values.
    /// </remarks>
    class LsConfigValuesCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var configs = Config.GetKernelConfigs();
            string configName = parameters.ArgumentsList[0];
            if (ConfigTools.IsCustomSettingRegistered(configName))
            {
                var config = configs.Single((bkc) => bkc.GetType().Name == configName);
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Configuration variables for") + $" {configName}", true);
                foreach (var entry in config.SettingsEntries ?? [])
                {
                    SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Entry name") + $": {entry.Name}");
                    foreach (var key in entry.Keys)
                    {
                        var value = ConfigTools.GetValueFromEntry(key, config);
                        TextWriters.WriteListEntry(Translate.DoTranslation("Key name"), key.Name, indent: 1);
                        TextWriters.WriteListEntry(Translate.DoTranslation("Key description"), key.Description, indent: 1);
                        TextWriters.WriteListEntry(Translate.DoTranslation("Key type"), $"{key.Type}", indent: 1);
                        TextWriters.WriteListEntry(Translate.DoTranslation("Key variable"), $"{key.Variable} [{value}]", indent: 1);
                    }
                }
            }
            else
            {
                TextWriters.Write(Translate.DoTranslation("Config not found."), KernelColorType.Error);
                return 28;
            }
            return 0;
        }

        public override void HelpHelper()
        {
            var names = Config.GetKernelConfigs().Select((bkc) => bkc.GetType().Name).ToArray();
            TextWriters.WriteListEntry(Translate.DoTranslation("Available configuration types"), string.Join(", ", names));
        }

    }
}
