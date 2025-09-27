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

using Terminaux.Shell.Commands;
using Terminaux.Shell.Switches;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors.Themes.Colors;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Languages;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
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
            bool deep = parameters.ContainsSwitch("-deep");
            foreach (var config in configs)
            {
                if (config is null || config.SettingsEntries is null)
                    continue;
                SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_LSCONFIGS_INFOFOR") + $" {config.GetType().Name}", ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_LSCONFIGS_COUNT"), $"{config.SettingsEntries.Length}");
                if (deep)
                {
                    foreach (var entry in config.SettingsEntries)
                    {
                        SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_LSCONFIGS_NAME") + $": {entry.Name}", ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
                        ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_LSCONFIGS_DISPLAY"), entry.DisplayAs, indent: 1);
                        ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_LSCONFIGS_DESC"), entry.Desc, indent: 1);
                        ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_LSCONFIGS_KEYS"), $"{entry.Keys.Length}", indent: 1);
                    }
                }
            }
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_LSCONFIGS_COMMANDTIP"), "lsconfigvalues");
            return 0;
        }

    }
}
