//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using System.Linq;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using Nitrocid.Base.Files;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Combines the two text files or more into the console.
    /// </summary>
    /// <remarks>
    /// If you have two or more fragments of a complete text file, you can combine them using this command to print the complete text file to toe console.
    /// </remarks>
    class CombineStrCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string InputPath = parameters.ArgumentsList[0];
            var CombineInputPaths = parameters.ArgumentsList.Skip(1).ToArray();

            // Make a combined content array
            var CombinedContents = FilesystemTools.CombineTextFiles(InputPath, CombineInputPaths);
            string combinedContentsStr = string.Join("\n", CombinedContents);
            TextWriterColor.Write(combinedContentsStr);
            variableValue = combinedContentsStr;
            return 0;
        }

    }
}
