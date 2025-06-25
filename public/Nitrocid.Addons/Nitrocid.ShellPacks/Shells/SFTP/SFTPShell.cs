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
using Terminaux.Colors.Themes.Colors;
using Nitrocid.Base.ConsoleBase.Inputs;

namespace Nitrocid.ShellPacks.Shells.SFTP
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
            SftpClient? client = (SftpClient?)sftpConnection.ConnectionInstance ??
                throw new KernelException(KernelExceptionType.SFTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_NOCLIENT"));

            // Finalize current connection
            SFTPShellCommon.clientConnection = sftpConnection;

            // Prepare to print current SFTP directory
            SFTPShellCommon.SFTPCurrentRemoteDir = client.WorkingDirectory;
            DebugWriter.WriteDebug(DebugLevel.I, "Working directory: {0}", vars: [SFTPShellCommon.SFTPCurrentRemoteDir ?? ""]);
            SFTPShellCommon.SFTPSite = client.ConnectionInfo.Host;
            SFTPShellCommon.SFTPUser = client.ConnectionInfo.Username;

            // Write connection information to Speed Dial file if it doesn't exist there
            SpeedDialTools.TryAddEntryToSpeedDial(SFTPShellCommon.SFTPSite, client.ConnectionInfo.Port, NetworkConnectionType.SFTP, false, SFTPShellCommon.SFTPUser);

            // Populate SFTP current directory
            SFTPShellCommon.SFTPCurrDirect = PathsManagement.HomePath;

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
                    InputTools.DetectKeypress();
                    Bail = true;
                }

                // Check if the shell is going to exit
                if (Bail)
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Exiting shell...");
                    if (!detaching)
                    {
                        ((SftpClient?)SFTPShellCommon.ClientSFTP?.ConnectionInstance)?.Disconnect();
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
