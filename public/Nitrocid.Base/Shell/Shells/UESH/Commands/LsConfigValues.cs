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
using Terminaux.Shell.Commands;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors.Themes.Colors;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Languages;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
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
                SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_LSCONFIGVALUES_LISTING") + $" {configName}", ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
                foreach (var entry in config.SettingsEntries ?? [])
                {
                    SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_LSCONFIGS_NAME") + $": {entry.Name}", ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
                    foreach (var key in entry.Keys)
                    {
                        var value = ConfigTools.GetValueFromEntry(key, config);
                        ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETCONFIGVALUE_KEYNAME"), key.Name, indent: 1);
                        ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETCONFIGVALUE_KEYDESC"), key.Description, indent: 1);
                        ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETCONFIGVALUE_KEYTYPE"), $"{key.Type}", indent: 1);
                        ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETCONFIGVALUE_KEYVAR"), $"{key.Variable} [{value}]", indent: 1);
                    }
                }
            }
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETCONFIGVALUE_CONFIGNOTFOUND"), ThemeColorType.Error);
                return 28;
            }
            return 0;
        }

        public override void HelpHelper()
        {
            var names = Config.GetKernelConfigs().Select((bkc) => bkc.GetType().Name).ToArray();
            ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETCONFIGVALUE_CONFIGTYPES"), string.Join(", ", names));
        }

    }
}
