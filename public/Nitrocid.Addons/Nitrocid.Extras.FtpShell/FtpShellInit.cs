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

using Nitrocid.Extras.FtpShell.FTP;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using System.Collections.Generic;
using System.Linq;
using FtpConfig = Nitrocid.Extras.FtpShell.Settings.FtpConfig;

namespace Nitrocid.Extras.FtpShell
{
    internal class FtpShellInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("ftp", /* Localizable */ "Use an FTP shell to interact with servers",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "server", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "FTP server to connect to"
                        }),
                    ])
                ], new FtpCommandExec())
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasFtpShell);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        internal static FtpConfig FtpConfig =>
            (FtpConfig)Config.baseConfigurations[nameof(FtpConfig)];

        void IAddon.FinalizeAddon()
        {
            var config = new FtpConfig();
            ConfigTools.RegisterBaseSetting(config);
            ShellManager.RegisterAddonShell("FTPShell", new FTPShellInfo());
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);
        }

        void IAddon.StartAddon()
        { }

        void IAddon.StopAddon()
        {
            ShellManager.UnregisterAddonShell("FTPShell");
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(FtpConfig));
        }
    }
}
