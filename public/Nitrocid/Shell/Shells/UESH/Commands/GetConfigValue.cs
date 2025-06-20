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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using System.Linq;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Gets a configuration value
    /// </summary>
    /// <remarks>
    /// This command prints a configuration value.
    /// </remarks>
    class GetConfigValueCommand : BaseCommand, ICommand
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
                    var value = ConfigTools.GetValueFromEntry(key, config);
                    TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETCONFIGVALUE_KEYNAME"), key.Name);
                    TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETCONFIGVALUE_KEYDESC"), key.Description);
                    TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETCONFIGVALUE_KEYTYPE"), $"{key.Type}");
                    TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETCONFIGVALUE_KEYVAR"), $"{key.Variable} [{value}]");
                    variableValue = $"{value}";
                }
                else
                {
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETCONFIGVALUE_KEYNOTFOUND"), KernelColorType.Error);
                    return 28;
                }
            }
            else
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETCONFIGVALUE_CONFIGNOTFOUND"), KernelColorType.Error);
                return 28;
            }
            return 0;
        }

        public override void HelpHelper()
        {
            var names = Config.GetKernelConfigs().Select((bkc) => bkc.GetType().Name).ToArray();
            TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETCONFIGVALUE_CONFIGTYPES"), string.Join(", ", names));
        }

    }
}
