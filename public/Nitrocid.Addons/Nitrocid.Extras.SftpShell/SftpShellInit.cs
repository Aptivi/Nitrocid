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
using Nitrocid.Extras.SftpShell.Commands;
using Nitrocid.Extras.SftpShell.Settings;
using Nitrocid.Extras.SftpShell.SFTP;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Shell.ShellBase.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Modifications;
using System.Linq;
using Nitrocid.Extras.SftpShell.Tools.Filesystem;
using Nitrocid.Extras.SftpShell.Tools.Transfer;
using Nitrocid.Extras.SftpShell.Tools;
using Nitrocid.Network.Connections;
using Renci.SshNet;

namespace Nitrocid.Extras.SftpShell
{
    internal class SftpShellInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("sftp", /* Localizable */ "Lets you use an SSH FTP server",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "server", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "SFTP server to connect to"
                        }),
                    ])
                ], new SftpCommandExec()),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasSftpShell);

        internal static SftpConfig SftpConfig =>
            (SftpConfig)Config.baseConfigurations[nameof(SftpConfig)];

        ReadOnlyDictionary<string, Delegate>? IAddon.PubliclyAvailableFunctions => new(new Dictionary<string, Delegate>()
        {
            { nameof(SFTPFilesystem.SFTPListRemote), new Func<string, List<string>>(SFTPFilesystem.SFTPListRemote) },
            { nameof(SFTPFilesystem.SFTPListRemote) + "2", new Func<string, bool, List<string>>(SFTPFilesystem.SFTPListRemote) },
            { nameof(SFTPFilesystem.SFTPDeleteRemote), new Func<string, bool>(SFTPFilesystem.SFTPDeleteRemote) },
            { nameof(SFTPFilesystem.SFTPChangeRemoteDir), new Func<string, bool>(SFTPFilesystem.SFTPChangeRemoteDir) },
            { nameof(SFTPFilesystem.SFTPChangeLocalDir), new Func<string, bool>(SFTPFilesystem.SFTPChangeLocalDir) },
            { nameof(SFTPFilesystem.SFTPGetCanonicalPath), new Func<string, string>(SFTPFilesystem.SFTPGetCanonicalPath) },
            { nameof(SFTPTransfer.SFTPGetFile), new Func<string, bool>(SFTPTransfer.SFTPGetFile) },
            { nameof(SFTPTransfer.SFTPUploadFile), new Func<string, bool>(SFTPTransfer.SFTPUploadFile) },
            { nameof(SFTPTransfer.SFTPDownloadToString), new Func<string, string>(SFTPTransfer.SFTPDownloadToString) },
            { nameof(SFTPTools.PromptConnectionInfo), new Func<string, int, string, ConnectionInfo>(SFTPTools.PromptConnectionInfo) },
            { nameof(SFTPTools.SFTPTryToConnect), new Func<string, NetworkConnection?>(SFTPTools.SFTPTryToConnect) },
        });

        ReadOnlyDictionary<string, PropertyInfo>? IAddon.PubliclyAvailableProperties => null;

        ReadOnlyDictionary<string, FieldInfo>? IAddon.PubliclyAvailableFields => null;

        void IAddon.FinalizeAddon()
        {
            var config = new SftpConfig();
            ConfigTools.RegisterBaseSetting(config);
            ShellManager.RegisterAddonShell("SFTPShell", new SFTPShellInfo());
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);
        }

        void IAddon.StartAddon()
        { }

        void IAddon.StopAddon()
        {
            ShellManager.UnregisterAddonShell("SFTPShell");
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(SftpConfig));
        }
    }
}
