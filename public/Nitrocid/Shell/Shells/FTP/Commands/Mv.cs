
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
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Text;
using KS.Network.FTP.Filesystem;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.FTP.Commands
{
    /// <summary>
    /// Moves a file or directory to another destination in the server
    /// </summary>
    /// <remarks>
    /// If you manage the FTP server and wanted to move a file or a directory from a remote directory to another remote directory, use this command.
    /// <br></br>
    /// The authenticated user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class FTP_MvCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string StringArgsOrig, string[] ListArgsOnlyOrig, string[] ListSwitchesOnly, ref string variableValue)
        {
            TextWriterColor.Write(Translate.DoTranslation("Moving {0} to {1}..."), true, KernelColorType.Progress, ListArgsOnly[0], ListArgsOnly[1]);
            if (FTPFilesystem.FTPMoveItem(ListArgsOnly[0], ListArgsOnly[1]))
            {
                TextWriterColor.Write(CharManager.NewLine + Translate.DoTranslation("Moved successfully"), true, KernelColorType.Success);
                return 0;
            }
            else
            {
                TextWriterColor.Write(CharManager.NewLine + Translate.DoTranslation("Failed to move {0} to {1}."), true, KernelColorType.Error, ListArgsOnly[0], ListArgsOnly[1]);
                return 10000 + (int)KernelExceptionType.FTPFilesystem;
            }
        }

    }
}
