
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
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Network.FTP.Transfer;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.FTP.Commands
{
    /// <summary>
    /// Downloads a folder from the current working directory
    /// </summary>
    /// <remarks>
    /// Downloads the binary or text folder and saves it to the current working local directory for you to use the downloaded folder that is provided in the FTP server.
    /// </remarks>
    class FTP_GetFolderCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string RemoteFolder = ListArgsOnly[0];
            string LocalFolder = ListArgsOnly.Length > 1 ? ListArgsOnly[1] : "";
            TextWriterColor.Write(Translate.DoTranslation("Downloading folder {0}..."), true, ColorTools.ColTypes.Progress, RemoteFolder);
            bool Result = !string.IsNullOrWhiteSpace(LocalFolder) ? FTPTransfer.FTPGetFolder(RemoteFolder, LocalFolder) : FTPTransfer.FTPGetFolder(RemoteFolder);
            if (Result)
            {
                Console.WriteLine();
                TextWriterColor.Write(Translate.DoTranslation("Downloaded folder {0}."), true, ColorTools.ColTypes.Success, RemoteFolder);
            }
            else
            {
                Console.WriteLine();
                TextWriterColor.Write(Translate.DoTranslation("Download failed for folder {0}."), true, ColorTools.ColTypes.Error, RemoteFolder);
            }
        }

    }
}