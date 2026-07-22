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

using Nitrocid.Extras.HttpShell.HTTP;
using Nitrocid.Extras.HttpShell.Settings;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Extensions;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using System.Collections.Generic;
using System.Linq;
using Nitrocid.Languages;

namespace Nitrocid.Extras.HttpShell
{
    internal class HttpShellInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("http", LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_COMMAND_HTTP_DESC"), new HttpCommandExec())
        ];

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasHttpShell);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasHttpShell);

        internal static HttpConfig HttpConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(HttpConfig)) ? (HttpConfig)Config.baseConfigurations[nameof(HttpConfig)] : Config.GetFallbackKernelConfig<HttpConfig>();

        public void FinalizeAddon()
        {
            LanguageTools.AddCustomAction(AddonName, new("Nitrocid.Extras.HttpShell.Resources.Languages.Output.Localizations", typeof(HttpShellInit).Assembly));
            var config = new HttpConfig();
            ConfigTools.RegisterBaseSetting(config);
            ShellManager.RegisterShell("HTTPShell", new HTTPShellInfo());
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
        }

        public void StartAddon()
        { }

        public void StopAddon()
        {
            LanguageTools.RemoveCustomAction(AddonName);
            ShellManager.UnregisterShell("HTTPShell");
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(HttpConfig));
        }
    }
}
