
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
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Opens the SQL editor shell
    /// </summary>
    /// <remarks>
    /// If you want to edit an SQL database, this command will let you open the SQL editor shell to a specified database so you can edit it.
    /// </remarks>
    class SqlCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            ListArgsOnly[0] = Filesystem.NeutralizePath(ListArgsOnly[0]);
            DebugWriter.WriteDebug(DebugLevel.I, "File path is {0} and .Exists is {0}", ListArgsOnly[0], Checking.FileExists(ListArgsOnly[0]));
            if (Checking.FileExists(ListArgsOnly[0]))
                ShellStart.StartShell(ShellType.SqlShell, ListArgsOnly[0]);
            else
                TextWriterColor.Write(Translate.DoTranslation("File doesn't exist."), true, KernelColorType.Error);
        }

    }
}
