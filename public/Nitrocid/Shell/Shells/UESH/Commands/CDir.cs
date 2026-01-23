//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using Nitrocid.Files;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// This command prints your current directory
    /// </summary>
    /// <remarks>
    /// This command prints your current directory. If invoked with -set, will also set the indicated variable to the current directory
    /// </remarks>
    class CDirCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string currentDir = FilesystemTools.CurrentDir;
            TextWriterColor.Write(currentDir);
            variableValue = currentDir;
            return 0;
        }

    }
}
