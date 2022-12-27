
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;
using KS.Shell.Shells.FTP;

namespace KS.Shell.Shells.SFTP.Commands
{
    /// <summary>
    /// Disconnects from the current working server
    /// </summary>
    /// <remarks>
    /// This command sends the quit command to the SFTP server so the server knows that you're going away. It basically disconnects you from the server to connect to the server again or re-connect to the last server connected.
    /// </remarks>
    class SFTP_DisconnectCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (SFTPShellCommon.SFTPConnected)
            {
                // Set a connected flag to False
                SFTPShellCommon.SFTPConnected = false;
                SFTPShellCommon.ClientSFTP.Disconnect();
                TextWriterColor.Write(Translate.DoTranslation("Disconnected from {0}"), FTPShellCommon.FtpSite);

                // Clean up everything
                SFTPShellCommon.SFTPSite = "";
                SFTPShellCommon.SFTPCurrentRemoteDir = "";
                SFTPShellCommon.SFTPUser = "";
                SFTPShellCommon.SFTPPass = "";
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("You haven't connected to any server yet"), true, KernelColorType.Error);
            }
        }

    }
}