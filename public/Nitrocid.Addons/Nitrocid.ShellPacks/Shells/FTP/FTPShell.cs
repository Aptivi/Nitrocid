//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Threading;
using FluentFTP;
using Nitrocid.Base.Files.Paths;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Network.Connections;
using Nitrocid.Base.Network.SpeedDial;
using Nitrocid.ShellPacks.Shells.FTP.Tools;
using Renci.SshNet;
using Terminaux.Inputs;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.ShellPacks.Shells.FTP
{
    /// <summary>
    /// The FTP shell
    /// </summary>
    public partial class FTPShell : BaseShell, IShell
    {
        internal NetworkConnection? clientConnection;

        /// <summary>
        /// The FTP network connection instance
        /// </summary>
        public NetworkConnection? FTPNetwork =>
            clientConnection;

        /// <summary>
        /// The FTP client used to connect to the FTP server
        /// </summary>
        public FtpClient FTPClient =>
            (FtpClient?)FTPNetwork?.ConnectionInstance ??
                throw new KernelException(KernelExceptionType.FTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_NOTCONNECTED_2"));

        /// <summary>
        /// FTP current local directory
        /// </summary>
        public string FtpCurrentDirectory { get; set; } = "";

        /// <summary>
        /// FTP current remote directory
        /// </summary>
        public string FtpCurrentRemoteDir { get; set; } = "";

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
            clientConnection = ftpConnection;

            // If MOTD exists, show it
            if (ShellsInit.ShellsConfig.FtpShowMotd)
            {
                if (clientFTP.FileExists("welcome.msg"))
                    TextWriterColor.Write(FTPDownloadToString("welcome.msg"), true, ThemeColorType.Banner);
                else if (clientFTP.FileExists(".message"))
                    TextWriterColor.Write(FTPDownloadToString(".message"), true, ThemeColorType.Banner);
            }

            // Prepare to print current FTP directory
            FtpCurrentRemoteDir = clientFTP.GetWorkingDirectory();
            DebugWriter.WriteDebug(DebugLevel.I, "Working directory: {0}", vars: [FtpCurrentRemoteDir]);

            // Write connection information to Speed Dial file if it doesn't exist there
            SpeedDialTools.TryAddEntryToSpeedDial(clientFTP.Host, clientFTP.Port, NetworkConnectionType.FTP, clientFTP.Credentials.UserName, clientFTP.Credentials.Password, false, new()
            {
                { "FtpEncryptionMode", (long)clientFTP.Config.EncryptionMode }
            });

            // Initialize logging
            clientFTP.Logger = new FTPLogger();
            clientFTP.Config.LogUserName = ShellsInit.ShellsConfig.FtpLoggerUsername;
            clientFTP.Config.LogHost = ShellsInit.ShellsConfig.FtpLoggerIP;

            // Don't remove this, make a config entry for it, or set it to True! It will introduce security problems.
            clientFTP.Config.LogPassword = false;

            // Populate FTP current directory
            FtpCurrentDirectory = PathsManagement.HomePath;

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
                    Input.ReadKey();
                    Bail = true;
                }

                // Check if the shell is going to exit
                if (Bail)
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Exiting shell...");
                    if (!detaching)
                    {
                        clientFTP?.Disconnect();
                        int connectionIndex = NetworkConnectionTools.GetConnectionIndex(FTPNetwork);
                        NetworkConnectionTools.CloseConnection(connectionIndex);
                    }
                    detaching = false;
                }
            }
        }

    }
}
