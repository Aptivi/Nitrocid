
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Languages;
using KS.Network.FTP.Filesystem;
using KS.Shell.ShellBase.Commands;
using System;

namespace KS.Shell.Shells.FTP.Commands
{
    /// <summary>
    /// Sets file permissions
    /// </summary>
    /// <remarks>
    /// If you have administrative access to the FTP server, you can set the remote file permissions. The permnumber argument is inherited from CHMOD's permission number.
    /// <br></br>
    /// The authenticated user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class FTP_PermCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (FTPFilesystem.FTPChangePermissions(ListArgsOnly[0], Convert.ToInt32(ListArgsOnly[1])))
                TextWriterColor.Write(Translate.DoTranslation("Permissions set successfully for file") + " {0}", true, KernelColorType.Success, ListArgsOnly[0]);
            else
                TextWriterColor.Write(Translate.DoTranslation("Failed to set permissions of {0} to {1}."), true, KernelColorType.Error, ListArgsOnly[0], ListArgsOnly[1]);
        }

    }
}
