
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
using KS.Files.Operations;
using KS.Files.Querying;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Removes directory or file from current working directory
    /// </summary>
    /// <remarks>
    /// This command lets you remove a directory or a file from your current working directory.
    /// </remarks>
    class RmCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            foreach (string Path in ListArgsOnly)
            {
                string NeutPath = Filesystem.NeutralizePath(Path);
                if (Checking.FileExists(NeutPath))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "{0} is a file. Removing...", Path);
                    Removing.RemoveFile(Path);
                }
                else if (Checking.FolderExists(NeutPath))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "{0} is a folder. Removing...", Path);
                    Removing.RemoveDirectory(Path);
                }
                else
                {
                    DebugWriter.Wdbg(DebugLevel.W, "Trying to remove {0} which is not found.", Path);
                    TextWriterColor.Write(Translate.DoTranslation("Can't remove {0} because it doesn't exist."), true, ColorTools.ColTypes.Error, Path);
                }
            }
        }

    }
}