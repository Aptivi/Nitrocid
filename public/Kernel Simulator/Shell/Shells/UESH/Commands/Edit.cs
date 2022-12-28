
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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
    /// Opens the text editor shell
    /// </summary>
    /// <remarks>
    /// If you want to edit a text document, this command will let you open the text editor shell to a specified document so you can edit it. Currently, it's on the basic stage, so it doesn't have advanced options yet.
    /// <br></br>
    /// It can also open binary files, but we don't recommend doing that, because it isn't a hex editor yet. Editing a binary file may or may not cause file corruptions. Use hexedit for such tasks.
    /// </remarks>
    class EditCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            ListArgsOnly[0] = Filesystem.NeutralizePath(ListArgsOnly[0]);
            DebugWriter.WriteDebug(DebugLevel.I, "File path is {0} and .Exists is {0}", ListArgsOnly[0], Checking.FileExists(ListArgsOnly[0]));
            if (Checking.FileExists(ListArgsOnly[0]))
            {
                // Determine the type
                if (Parsing.IsBinaryFile(ListArgsOnly[0]))
                    ShellStart.StartShell(ShellType.HexShell, ListArgsOnly[0]);
                else if (Parsing.IsJson(ListArgsOnly[0]))
                    ShellStart.StartShell(ShellType.JsonShell, ListArgsOnly[0]);
                else
                    ShellStart.StartShell(ShellType.TextShell, ListArgsOnly[0]);
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("File doesn't exist."), true, KernelColorType.Error);
            }
        }

    }
}
