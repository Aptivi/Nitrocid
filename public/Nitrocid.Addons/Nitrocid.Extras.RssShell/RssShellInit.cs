//
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
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

using KS.Kernel.Configuration;
using KS.Kernel.Extensions;
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Arguments;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.ShellBase.Switches;
using Nitrocid.Extras.RssShell.RSS;
using Nitrocid.Extras.RssShell.Settings;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Extras.RssShell
{
    internal class RssShellInit : IAddon
    {
        private readonly Dictionary<string, CommandInfo> addonCommands = new()
        {
            { "rss",
                new CommandInfo("rss", /* Localizable */ "Opens an RSS shell to read the feeds",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "feedlink"),
                        }, new[]
                        {
                            new SwitchInfo("tui", /* Localizable */ "Opens an interactive RSS feed reader TUI"),
                        })
                    }, new RssCommandExec())
            },
        };

        string IAddon.AddonName => "Extras - RSS Shell";

        AddonType IAddon.AddonType => AddonType.Optional;

        internal static RssConfig RssConfig =>
            (RssConfig)Config.baseConfigurations[nameof(RssConfig)];

        void IAddon.FinalizeAddon()
        {
            var config = new RssConfig();
            ConfigTools.RegisterBaseSetting(config);
            ShellManager.reservedShells.Add("RSSShell");
            ShellManager.RegisterShell("RSSShell", new RSSShellInfo());
            CommandManager.RegisterAddonCommands(ShellType.Shell, addonCommands.Values.ToArray());
        }

        void IAddon.StartAddon()
        { }

        void IAddon.StopAddon()
        {
            ShellManager.availableShells.Remove("RSSShell");
            PromptPresetManager.CurrentPresets.Remove("RSSShell");
            ShellManager.reservedShells.Remove("RSSShell");
            CommandManager.UnregisterAddonCommands(ShellType.Shell, addonCommands.Keys.ToArray());
            ConfigTools.UnregisterBaseSetting(nameof(RssConfig));
        }
    }
}
