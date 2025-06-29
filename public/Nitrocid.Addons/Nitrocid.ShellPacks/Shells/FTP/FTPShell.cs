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

using System;
using System.Threading;
using Nitrocid.ShellPacks.Tools;
using Nitrocid.ShellPacks.Tools.Transfer;
using FluentFTP;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Nitrocid.Base.Kernel.Debugging;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Files.Paths;
using Terminaux.Colors.Themes.Colors;
using Nitrocid.Base.Network.SpeedDial;
using Nitrocid.Base.Network.Connections;
using Nitrocid.Base.ConsoleBase.Inputs;

namespace Nitrocid.ShellPacks.Shells.FTP
{
    /// <summary>
    /// The FTP shell
    /// </summary>
    public class FTPShell : BaseShell, IShell
    {

        /// <inheritdoc/>
        public override string ShellType => "FTPShell";

        /// <inheritdoc/>
        public override bool Bail { get; set; }

        internal bool detaching = false;

        /// <inheritdoc/>
        public override void InitializeShell(params object[] ShellArgs)
        {
            // Parse shell arguments
            NetworkConnection ftpConnection = (NetworkConnection)ShellArgs[0];
            FtpClient? clientFTP = (FtpClient?)ftpConnection.ConnectionInstance ??
                throw new KernelException(KernelExceptionType.FTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_NOCLIENT"));

            // Finalize current connection
            FTPShellCommon.clientConnection = ftpConnection;

            // If MOTD exists, show it
            if (ShellsInit.ShellsConfig.FtpShowMotd)
            {
                if (clientFTP.FileExists("welcome.msg"))
                    TextWriterColor.Write(FTPTransfer.FTPDownloadToString("welcome.msg"), true, ThemeColorType.Banner);
                else if (clientFTP.FileExists(".message"))
                    TextWriterColor.Write(FTPTransfer.FTPDownloadToString(".message"), true, ThemeColorType.Banner);
            }

            // Prepare to print current FTP directory
            FTPShellCommon.FtpCurrentRemoteDir = clientFTP.GetWorkingDirectory();
            DebugWriter.WriteDebug(DebugLevel.I, "Working directory: {0}", vars: [FTPShellCommon.FtpCurrentRemoteDir]);
            FTPShellCommon.FtpSite = clientFTP.Host;
            FTPShellCommon.FtpUser = clientFTP.Credentials.UserName;

            // Write connection information to Speed Dial file if it doesn't exist there
            SpeedDialTools.TryAddEntryToSpeedDial(FTPShellCommon.FtpSite, clientFTP.Port, NetworkConnectionType.FTP, false, clientFTP.Credentials.UserName, (long)clientFTP.Config.EncryptionMode);

            // Initialize logging
            clientFTP.Logger = new FTPLogger();
            clientFTP.Config.LogUserName = ShellsInit.ShellsConfig.FtpLoggerUsername;
            clientFTP.Config.LogHost = ShellsInit.ShellsConfig.FtpLoggerIP;

            // Don't remove this, make a config entry for it, or set it to True! It will introduce security problems.
            clientFTP.Config.LogPassword = false;

            // Populate FTP current directory
            FTPShellCommon.FtpCurrentDirectory = PathsManagement.HomePath;

            // Actual shell logic
            while (!Bail)
            {
                try
                {
                    ShellManager.GetLine();
                }
                catch (ThreadInterruptedException)
                {
                    CancellationHandlers.DismissRequest();
                    Bail = true;
                }
                catch (Exception ex)
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_EXCEPTION_SHELLERROR") + " {0}", true, ThemeColorType.Error, ex.Message);
                    DebugWriter.WriteDebug(DebugLevel.E, "Shell will have to exit: {0}", vars: [ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    InputTools.DetectKeypress();
                    Bail = true;
                }

                // Check if the shell is going to exit
                if (Bail)
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Exiting shell...");
                    if (!detaching)
                    {
                        clientFTP?.Disconnect();
                        int connectionIndex = NetworkConnectionTools.GetConnectionIndex(FTPShellCommon.ClientFTP);
                        NetworkConnectionTools.CloseConnection(connectionIndex);
                        FTPShellCommon.clientConnection = null;
                    }
                    detaching = false;
                    FTPShellCommon.FtpSite = "";
                    FTPShellCommon.FtpCurrentDirectory = "";
                    FTPShellCommon.FtpCurrentRemoteDir = "";
                    FTPShellCommon.FtpUser = "";
                    FTPShellCommon.FtpPass = "";
                }
            }
        }

    }
}
