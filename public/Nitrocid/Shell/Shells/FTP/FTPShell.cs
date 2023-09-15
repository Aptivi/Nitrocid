
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Threading;
using FluentFTP;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Network.Base.Connections;
using KS.Network.FTP.Transfer;
using KS.Network.FTP;
using KS.Network.SpeedDial;
using KS.Shell.ShellBase.Shells;
using KS.ConsoleBase.Writers.ConsoleWriters;

namespace KS.Shell.Shells.FTP
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
            FtpClient clientFTP = (FtpClient)ftpConnection.ConnectionInstance;

            // Finalize current connection
            FTPShellCommon.clientConnection = ftpConnection;

            // If MOTD exists, show it
            if (FTPShellCommon.FtpShowMotd)
            {
                if (clientFTP.FileExists("welcome.msg"))
                    TextWriterColor.Write(FTPTransfer.FTPDownloadToString("welcome.msg"), true, KernelColorType.Banner);
                else if (clientFTP.FileExists(".message"))
                    TextWriterColor.Write(FTPTransfer.FTPDownloadToString(".message"), true, KernelColorType.Banner);
            }

            // Prepare to print current FTP directory
            FTPShellCommon.FtpCurrentRemoteDir = clientFTP.GetWorkingDirectory();
            DebugWriter.WriteDebug(DebugLevel.I, "Working directory: {0}", FTPShellCommon.FtpCurrentRemoteDir);
            FTPShellCommon.FtpSite = clientFTP.Host;
            FTPShellCommon.FtpUser = clientFTP.Credentials.UserName;

            // Write connection information to Speed Dial file if it doesn't exist there
            SpeedDialTools.TryAddEntryToSpeedDial(FTPShellCommon.FtpSite, clientFTP.Port, NetworkConnectionType.FTP, false, clientFTP.Credentials.UserName, (long)clientFTP.Config.EncryptionMode);

            // Initialize logging
            clientFTP.Logger = new FTPLogger();
            clientFTP.Config.LogUserName = Flags.FTPLoggerUsername;
            clientFTP.Config.LogHost = Flags.FTPLoggerIP;

            // Don't remove this, make a config entry for it, or set it to True! It will introduce security problems.
            clientFTP.Config.LogPassword = false;

            // Populate FTP current directory
            FTPShellCommon.FtpCurrentDirectory = Paths.HomePath;

            // Actual shell logic
            while (!Bail)
            {
                try
                {
                    ShellManager.GetLine();
                }
                catch (ThreadInterruptedException)
                {
                    Flags.CancelRequested = false;
                    Bail = true;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    throw new KernelException(KernelExceptionType.FTPShell, Translate.DoTranslation("There was an error in the FTP shell:") + " {0}", ex, ex.Message);
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
