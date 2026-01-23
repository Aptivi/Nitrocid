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

using Nitrocid.Extras.HttpShell.HTTP;
using Nitrocid.Extras.HttpShell.Settings;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Extensions;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Extras.HttpShell
{
    internal class HttpShellInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("http", /* Localizable */ "Starts the HTTP shell", new HttpCommandExec())
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasHttpShell);

        internal static HttpConfig HttpConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(HttpConfig)) ? (HttpConfig)Config.baseConfigurations[nameof(HttpConfig)] : Config.GetFallbackKernelConfig<HttpConfig>();

        void IAddon.FinalizeAddon()
        {
            var config = new HttpConfig();
            ConfigTools.RegisterBaseSetting(config);
            ShellManager.RegisterShell("HTTPShell", new HTTPShellInfo());
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
        }

        void IAddon.StartAddon()
        { }

        void IAddon.StopAddon()
        {
            ShellManager.UnregisterShell("HTTPShell");
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(HttpConfig));
        }
    }
}
