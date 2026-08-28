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

using Nitrocid.Kernel.Configuration;
using Terminaux.Shell.Commands;
using Nitrocid.Kernel.Extensions;
using System.Linq;
using Nitrocid.Extras.Ssh.Settings;
using Nitrocid.Extras.Ssh.Commands;
using Nitrocid.Languages;

namespace Nitrocid.Extras.Ssh
{
    internal class SshInit : IAddon
    {
        private readonly BaseCommand[] addonCommands =
        [
            new SshellCommand(),
            new SshcmdCommand(),
        ];

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasSsh);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasSsh);

        internal static SshConfig SshConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(SshConfig)) ? (SshConfig)Config.baseConfigurations[nameof(SshConfig)] : Config.GetFallbackKernelConfig<SshConfig>();

        public void FinalizeAddon()
        {
            LanguageTools.AddCustomAction(AddonName, new("Nitrocid.Extras.Ssh.Resources.Languages.Output.Localizations", typeof(SshInit).Assembly));
            var config = new SshConfig();
            ConfigTools.RegisterBaseSetting(config);
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
        }

        public void StartAddon()
        { }

        public void StopAddon()
        {
            LanguageTools.RemoveCustomAction(AddonName);
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(SshConfig));
        }
    }
}
