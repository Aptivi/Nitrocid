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
using Nitrocid.Extras.SftpShell.Commands;
using Nitrocid.Extras.SftpShell.Settings;
using Nitrocid.Extras.SftpShell.SFTP;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Nitrocid.Extras.SftpShell
{
    internal class SftpShellInit : IAddon
    {
        private readonly Dictionary<string, CommandInfo> addonCommands = new()
        {
            { "sftp",
                new CommandInfo("sftp", /* Localizable */ "Lets you use an SSH FTP server",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "server"),
                        })
                    ], new SftpCommandExec())
            },

            { "sshell",
                new CommandInfo("sshell", /* Localizable */ "Connects to an SSH server.",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "address:port"),
                            new CommandArgumentPart(true, "username"),
                        })
                    ], new SshellCommand())
            },

            { "sshcmd",
                new CommandInfo("sshcmd", /* Localizable */ "Connects to an SSH server to execute a command.",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "address:port"),
                            new CommandArgumentPart(true, "username"),
                            new CommandArgumentPart(true, "command"),
                        })
                    ], new SshcmdCommand())
            },
        };

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasSftpShell);

        AddonType IAddon.AddonType => AddonType.Optional;

        internal static SftpConfig SftpConfig =>
            (SftpConfig)Config.baseConfigurations[nameof(SftpConfig)];

        ReadOnlyDictionary<string, Delegate> IAddon.PubliclyAvailableFunctions => null;

        ReadOnlyDictionary<string, PropertyInfo> IAddon.PubliclyAvailableProperties => null;

        ReadOnlyDictionary<string, FieldInfo> IAddon.PubliclyAvailableFields => null;

        void IAddon.FinalizeAddon()
        {
            var config = new SftpConfig();
            ConfigTools.RegisterBaseSetting(config);
            ShellManager.reservedShells.Add("SFTPShell");
            ShellManager.RegisterShell("SFTPShell", new SFTPShellInfo());
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands.Values]);
        }

        void IAddon.StartAddon()
        { }

        void IAddon.StopAddon()
        {
            ShellManager.availableShells.Remove("SFTPShell");
            PromptPresetManager.CurrentPresets.Remove("SFTPShell");
            ShellManager.reservedShells.Remove("SFTPShell");
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Keys]);
            ConfigTools.UnregisterBaseSetting(nameof(SftpConfig));
        }
    }
}
