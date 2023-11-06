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
using Nitrocid.Extras.ArchiveShell.Archive.Commands;
using Nitrocid.Extras.ArchiveShell.Archive.Shell;
using Nitrocid.Extras.ArchiveShell.Settings;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Extras.ArchiveShell
{
    internal class ArchiveShellInit : IAddon
    {
        private readonly Dictionary<string, CommandInfo> addonCommands = new()
        {
            { "archive",
                new CommandInfo("archive", /* Localizable */ "Opens the archive file to the archive shell",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "archivefile"),
                        })
                    }, new ArchiveCommand())
            },
        };

        string IAddon.AddonName => "Extras - Archive Shell";

        AddonType IAddon.AddonType => AddonType.Optional;

        internal static ArchiveConfig ArchiveConfig =>
            (ArchiveConfig)Config.baseConfigurations[nameof(ArchiveConfig)];

        void IAddon.FinalizeAddon()
        {
            var config = new ArchiveConfig();
            ConfigTools.RegisterBaseSetting(config);
            ShellManager.reservedShells.Add("ArchiveShell");
            ShellManager.RegisterShell("ArchiveShell", new ArchiveShellInfo());
        }

        void IAddon.StartAddon() =>
            CommandManager.RegisterAddonCommands(ShellType.Shell, addonCommands.Values.ToArray());

        void IAddon.StopAddon()
        {
            ShellManager.availableShells.Remove("ArchiveShell");
            PromptPresetManager.CurrentPresets.Remove("ArchiveShell");
            ShellManager.reservedShells.Remove("ArchiveShell");
            CommandManager.UnregisterAddonCommands(ShellType.Shell, addonCommands.Keys.ToArray());
            ConfigTools.UnregisterBaseSetting(nameof(ArchiveConfig));
        }
    }
}
