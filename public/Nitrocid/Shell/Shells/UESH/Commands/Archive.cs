
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
using KS.Files;
using KS.Files.Querying;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Opens an archive file to the archive shell
    /// </summary>
    /// <remarks>
    /// If you want to interact with the archive files, like extracting them, use this command. For now, only RAR and ZIP files are supported.
    /// </remarks>
    class ArchiveCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, ref string variableValue)
        {
            ListArgsOnly[0] = Filesystem.NeutralizePath(ListArgsOnly[0]);
            DebugWriter.WriteDebug(DebugLevel.I, "File path is {0} and .Exists is {0}", ListArgsOnly[0], Checking.FileExists(ListArgsOnly[0]));
            if (Checking.FileExists(ListArgsOnly[0]))
            {
                ShellStart.StartShell(ShellType.ArchiveShell, ListArgsOnly[0]);
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("File doesn't exist."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Filesystem;
            }
            return 0;
        }

    }
}
