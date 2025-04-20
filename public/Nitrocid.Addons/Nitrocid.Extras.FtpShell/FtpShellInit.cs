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

using FluentFTP;
using Nitrocid.Extras.FtpShell.FTP;
using Nitrocid.Extras.FtpShell.Tools;
using Nitrocid.Extras.FtpShell.Tools.Filesystem;
using Nitrocid.Extras.FtpShell.Tools.Transfer;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Languages;
using Nitrocid.Modifications;
using Nitrocid.Network.Connections;
using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
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

        ReadOnlyDictionary<string, Delegate>? IAddon.PubliclyAvailableFunctions => new(new Dictionary<string, Delegate>()
        {
            { nameof(FTPFilesystem.FTPListRemote), new Func<string, List<string>>(FTPFilesystem.FTPListRemote) },
            { nameof(FTPFilesystem.FTPListRemote) + "2", new Func<string, bool, List<string>>(FTPFilesystem.FTPListRemote) },
            { nameof(FTPFilesystem.FTPDeleteRemote), new Func<string, bool>(FTPFilesystem.FTPDeleteRemote) },
            { nameof(FTPFilesystem.FTPChangeRemoteDir), new Func<string, bool>(FTPFilesystem.FTPChangeRemoteDir) },
            { nameof(FTPFilesystem.FTPMoveItem), new Func<string, string, bool>(FTPFilesystem.FTPMoveItem) },
            { nameof(FTPFilesystem.FTPCopyItem), new Func<string, string, bool>(FTPFilesystem.FTPCopyItem) },
            { nameof(FTPFilesystem.FTPChangePermissions), new Func<string, int, bool>(FTPFilesystem.FTPChangePermissions) },
            { nameof(FTPHashing.FTPGetHash), new Func<string, FtpHashAlgorithm, FtpHash>(FTPHashing.FTPGetHash) },
            { nameof(FTPHashing.FTPGetHashes), new Func<string, FtpHashAlgorithm, Dictionary<string, FtpHash>>(FTPHashing.FTPGetHashes) },
            { nameof(FTPHashing.FTPGetHashes) + "2", new Func<string, FtpHashAlgorithm, bool, Dictionary<string, FtpHash>>(FTPHashing.FTPGetHashes) },
            { nameof(FTPTransfer.FTPGetFile), new Func<string, bool>(FTPTransfer.FTPGetFile) },
            { nameof(FTPTransfer.FTPGetFile) + "2", new Func<string, string, bool>(FTPTransfer.FTPGetFile) },
            { nameof(FTPTransfer.FTPGetFolder), new Func<string, bool>(FTPTransfer.FTPGetFolder) },
            { nameof(FTPTransfer.FTPGetFolder) + "2", new Func<string, string, bool>(FTPTransfer.FTPGetFolder) },
            { nameof(FTPTransfer.FTPUploadFile), new Func<string, bool>(FTPTransfer.FTPUploadFile) },
            { nameof(FTPTransfer.FTPUploadFile) + "2", new Func<string, string, bool>(FTPTransfer.FTPUploadFile) },
            { nameof(FTPTransfer.FTPUploadFolder), new Func<string, bool>(FTPTransfer.FTPUploadFolder) },
            { nameof(FTPTransfer.FTPUploadFolder) + "2", new Func<string, string, bool>(FTPTransfer.FTPUploadFolder) },
            { nameof(FTPTransfer.FTPDownloadToString), new Func<string, string>(FTPTransfer.FTPDownloadToString) },
            { nameof(FTPTools.PromptForPassword), new Func<FtpClient, string, string, int, FtpEncryptionMode, NetworkConnection?>(FTPTools.PromptForPassword) },
            { nameof(FTPTools.TryToConnect), new Func<string, NetworkConnection?>(FTPTools.TryToConnect) },
        });

        ReadOnlyDictionary<string, PropertyInfo>? IAddon.PubliclyAvailableProperties => new(new Dictionary<string, PropertyInfo>()
        {
            { nameof(FTPTransferProgress.FileProgress), typeof(FTPTransferProgress).GetProperty(nameof(FTPTransferProgress.FileProgress)) ?? throw new Exception(Translate.DoTranslation("There is no property info for") + $" {nameof(FTPTransferProgress.FileProgress)}") },
            { nameof(FTPTransferProgress.MultipleProgress), typeof(FTPTransferProgress).GetProperty(nameof(FTPTransferProgress.MultipleProgress)) ?? throw new Exception(Translate.DoTranslation("There is no property info for") + $" {nameof(FTPTransferProgress.MultipleProgress)}") },
        });

        ReadOnlyDictionary<string, FieldInfo>? IAddon.PubliclyAvailableFields => null;

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
