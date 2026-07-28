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
using Nitrocid.Base.Files.Paths;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Network.Connections;
using Nitrocid.Base.Network.SpeedDial;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Renci.SshNet;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Themes.Colors;
using Terminaux.Inputs;

namespace Nitrocid.ShellPacks.Shells.SFTP
{
    /// <summary>
    /// The SFTP shell
    /// </summary>
    public partial class SFTPShell : BaseShell, IShell
    {
        internal NetworkConnection? clientConnection;

        /// <summary>
        /// The SFTP client used to connect to the SFTP server
        /// </summary>
        public NetworkConnection? ClientSFTP =>
            clientConnection;

        /// <summary>
        /// SFTP current local directory
        /// </summary>
        public string SFTPCurrDirect { get; set; } = "";

        /// <summary>
        /// SFTP current remote directory
        /// </summary>
        public string SFTPCurrentRemoteDir { get; set; } = "";

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
            SftpClient? client = (SftpClient?)sftpConnection.ConnectionInstance ??
                throw new KernelException(KernelExceptionType.SFTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_NOCLIENT"));

            // Finalize current connection
            clientConnection = sftpConnection;

            // Prepare to print current SFTP directory
            SFTPCurrentRemoteDir = client.WorkingDirectory;
            DebugWriter.WriteDebug(DebugLevel.I, "Working directory: {0}", vars: [SFTPCurrentRemoteDir ?? ""]);

            // Write connection information to Speed Dial file if it doesn't exist there
            SpeedDialTools.TryAddEntryToSpeedDial(client.ConnectionInfo.Host, client.ConnectionInfo.Port, NetworkConnectionType.SFTP, client.ConnectionInfo.Username, "", false);

            // Populate SFTP current directory
            SFTPCurrDirect = PathsManagement.HomePath;

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
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SFTP_EXCEPTION_SHELLERROR") + " {0}", true, ThemeColorType.Error, ex.Message);
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
                        ((SftpClient?)ClientSFTP?.ConnectionInstance)?.Disconnect();
                        int connectionIndex = NetworkConnectionTools.GetConnectionIndex(ClientSFTP);
                        NetworkConnectionTools.CloseConnection(connectionIndex);
                        clientConnection = null;
                    }
                    detaching = false;
                }
            }
        }

    }
}
