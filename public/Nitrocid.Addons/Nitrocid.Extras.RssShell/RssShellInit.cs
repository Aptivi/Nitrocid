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

using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Shell.ShellBase.Switches;
using Nitrocid.Extras.RssShell.RSS;
using Nitrocid.Extras.RssShell.Settings;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Shell.ShellBase.Commands;
using System.Collections.Generic;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Shell.ShellBase.Shells;
using System.Linq;

namespace Nitrocid.Extras.RssShell
{
    internal class RssShellInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("rss", /* Localizable */ "Opens an RSS shell to read the feeds",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "feedlink", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "RSS feed link"
                        }),
                    ],
                    [
                        new SwitchInfo("tui", /* Localizable */ "Opens an interactive RSS feed reader TUI"),
                    ])
                ], new RssCommandExec())
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasRssShell);

        internal static RssConfig RssConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(RssConfig)) ? (RssConfig)Config.baseConfigurations[nameof(RssConfig)] : Config.GetFallbackKernelConfig<RssConfig>();

        void IAddon.FinalizeAddon()
        {
            var config = new RssConfig();
            ConfigTools.RegisterBaseSetting(config);
            ShellManager.RegisterAddonShell("RSSShell", new RSSShellInfo());
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);
        }

        void IAddon.StartAddon()
        { }

        void IAddon.StopAddon()
        {
            ShellManager.UnregisterAddonShell("RSSShell");
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(RssConfig));
        }
    }
}
