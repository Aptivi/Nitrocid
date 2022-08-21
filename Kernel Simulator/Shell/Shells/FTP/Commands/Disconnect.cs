
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

using System;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.FTP.Commands
{
    /// <summary>
    /// Disconnects from the current working server
    /// </summary>
    /// <remarks>
    /// This command sends the quit command to the FTP server so the server knows that you're going away. It basically disconnects you from the server to connect to the server again or re-connect to the last server connected.
    /// <br></br>
    /// <list type="table">
    /// <listheader>
    /// <term>Switches</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>-f</term>
    /// <description>Force disconnection</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// </remarks>
    class FTP_DisconnectCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (FTPShellCommon.FtpConnected == true)
            {
                // Set a connected flag to False
                FTPShellCommon.FtpConnected = false;
                FTPShellCommon.ClientFTP.DisconnectWithQuit = ListSwitchesOnly.Contains("-f");
                FTPShellCommon.ClientFTP.Disconnect();
                TextWriterColor.Write(Translate.DoTranslation("Disconnected from {0}"), true, ColorTools.ColTypes.Success, FTPShellCommon.FtpSite);

                // Clean up everything
                FTPShellCommon.FtpSite = "";
                FTPShellCommon.FtpCurrentRemoteDir = "";
                FTPShellCommon.FtpUser = "";
                FTPShellCommon.FtpPass = "";
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("You haven't connected to any server yet"), true, ColorTools.ColTypes.Error);
            }
        }

        public override void HelpHelper()
        {
            TextWriterColor.Write(Translate.DoTranslation("This command has the below switches that change how it works:"), true, ColorTools.ColTypes.Neutral);
            TextWriterColor.Write("  -f: ", false, ColorTools.ColTypes.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Disconnects from server disgracefully"), true, ColorTools.ColTypes.ListValue);
        }

    }
}