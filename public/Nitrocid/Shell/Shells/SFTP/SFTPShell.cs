
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
using KS.Files;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Network.Base.Connections;
using KS.Network.SpeedDial;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using Renci.SshNet;

namespace KS.Shell.Shells.SFTP
{
    /// <summary>
    /// The SFTP shell
    /// </summary>
    public class SFTPShell : BaseShell, IShell
    {

        /// <inheritdoc/>
        public override string ShellType => "SFTPShell";

        /// <inheritdoc/>
        public override bool Bail { get; set; }

        internal bool detaching = false;

        /// <inheritdoc/>
        public override void InitializeShell(params object[] ShellArgs)
        {
            // Parse shell arguments
            NetworkConnection sftpConnection = (NetworkConnection)ShellArgs[0];
            SftpClient client = (SftpClient)sftpConnection.ConnectionInstance;

            // Finalize current connection
            SFTPShellCommon.clientConnection = sftpConnection;

            // Prepare to print current SFTP directory
            SFTPShellCommon.SFTPCurrentRemoteDir = client.WorkingDirectory;
            DebugWriter.WriteDebug(DebugLevel.I, "Working directory: {0}", SFTPShellCommon.SFTPCurrentRemoteDir);
            SFTPShellCommon.SFTPSite = client.ConnectionInfo.Host;
            SFTPShellCommon.SFTPUser = client.ConnectionInfo.Username;

            // Write connection information to Speed Dial file if it doesn't exist there
            SpeedDialTools.TryAddEntryToSpeedDial(SFTPShellCommon.SFTPSite, client.ConnectionInfo.Port, NetworkConnectionType.SFTP, false, SFTPShellCommon.SFTPUser);

            // Populate SFTP current directory
            SFTPShellCommon.SFTPCurrDirect = Paths.HomePath;

            // Actual shell logic
            while (!Bail)
            {
                try
                {
                    ShellManager.GetLine();
                }
                catch (ThreadInterruptedException)
                {
                    CancellationHandlers.CancelRequested = false;
                    Bail = true;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    throw new KernelException(KernelExceptionType.SFTPShell, Translate.DoTranslation("There was an error in the SFTP shell:") + " {0}", ex, ex.Message);
                }

                // Check if the shell is going to exit
                if (Bail)
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Exiting shell...");
                    if (!detaching)
                    {
                        ((SftpClient)SFTPShellCommon.ClientSFTP.ConnectionInstance)?.Disconnect();
                        int connectionIndex = NetworkConnectionTools.GetConnectionIndex(SFTPShellCommon.ClientSFTP);
                        NetworkConnectionTools.CloseConnection(connectionIndex);
                        SFTPShellCommon.clientConnection = null;
                    }
                    detaching = false;
                    SFTPShellCommon.SFTPSite = "";
                    SFTPShellCommon.SFTPCurrDirect = "";
                    SFTPShellCommon.SFTPCurrentRemoteDir = "";
                    SFTPShellCommon.SFTPUser = "";
                    SFTPShellCommon.SFTPPass = "";
                }
            }
        }

    }
}
