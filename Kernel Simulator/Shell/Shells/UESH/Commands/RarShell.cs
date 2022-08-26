
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
using KS.Files;
using KS.Files.Querying;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Opens an RAR shell
    /// </summary>
    /// <remarks>
    /// If you wanted to interact with an RAR file more thoroughly, you can use this command to open a shell to an RAR file.
    /// </remarks>
    class RarShellCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            ListArgsOnly[0] = Filesystem.NeutralizePath(ListArgsOnly[0]);
            DebugWriter.Wdbg(DebugLevel.I, "File path is {0} and .Exists is {0}", ListArgsOnly[0], Checking.FileExists(ListArgsOnly[0]));
            if (Checking.FileExists(ListArgsOnly[0]))
            {
                ShellStart.StartShell(ShellType.RARShell, ListArgsOnly[0]);
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("File doesn't exist."), true, ColorTools.ColTypes.Error);
            }
        }

    }
}