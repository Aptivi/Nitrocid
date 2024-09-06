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

using Nitrocid.Kernel.Configuration;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Switches;
using Nitrocid.Languages;
using Terminaux.Writer.FancyWriters;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Lists all configurations
    /// </summary>
    /// <remarks>
    /// This command lists all the configurations.
    /// </remarks>
    class LsConfigsCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var configs = Config.GetKernelConfigs();
            bool deep = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-deep");
            foreach (var config in configs)
            {
                if (config is null || config.SettingsEntries is null)
                    continue;
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Info for") + $" {config.GetType().Name}", true);
                ListEntryWriterColor.WriteListEntry(Translate.DoTranslation("Entries count"), $"{config.SettingsEntries.Length}");
                if (deep)
                {
                    foreach (var entry in config.SettingsEntries)
                    {
                        SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Entry name") + $": {entry.Name}", true);
                        ListEntryWriterColor.WriteListEntry(Translate.DoTranslation("Displaying as"), entry.DisplayAs, 1);
                        ListEntryWriterColor.WriteListEntry(Translate.DoTranslation("Description"), entry.Desc, 1);
                        ListEntryWriterColor.WriteListEntry(Translate.DoTranslation("Keys count"), $"{entry.Keys.Length}", 1);
                    }
                }
            }
            TextWriterColor.Write(Translate.DoTranslation("Use the {0} command to get the individual keys"), "lsconfigvalues");
            return 0;
        }

    }
}
