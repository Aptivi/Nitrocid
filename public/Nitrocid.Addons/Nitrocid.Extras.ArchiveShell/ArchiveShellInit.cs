﻿//
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
using Nitrocid.Extras.ArchiveShell.Archive.Commands;
using Nitrocid.Extras.ArchiveShell.Archive.Shell;
using Nitrocid.Extras.ArchiveShell.Settings;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Shell.ShellBase.Commands;
using System.Collections.Generic;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Shell.ShellBase.Shells;
using System.Linq;

namespace Nitrocid.Extras.ArchiveShell
{
    internal class ArchiveShellInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("archive", /* Localizable */ "Opens the archive file to the archive shell",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "archivefile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to archive file"
                        }),
                    ])
                ], new ArchiveCommand())
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasArchiveShell);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        internal static ArchiveConfig ArchiveConfig =>
            (ArchiveConfig)Config.baseConfigurations[nameof(ArchiveConfig)];

        void IAddon.FinalizeAddon()
        {
            var config = new ArchiveConfig();
            ConfigTools.RegisterBaseSetting(config);
            ShellManager.RegisterAddonShell("ArchiveShell", new ArchiveShellInfo());
        }

        void IAddon.StartAddon() =>
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);

        void IAddon.StopAddon()
        {
            ShellManager.UnregisterAddonShell("ArchiveShell");
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(ArchiveConfig));
        }
    }
}
