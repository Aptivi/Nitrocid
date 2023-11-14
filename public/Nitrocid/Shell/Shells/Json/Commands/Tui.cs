//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.Files.Editors.JsonShell;
using KS.Files.Editors.TextEdit;
using KS.Files.Operations;
using KS.Shell.ShellBase.Commands;
using System.Collections.Generic;
using System.Linq;

namespace KS.Shell.Shells.Json.Commands
{
    /// <summary>
    /// Opens the JSON file in the interactive text editor TUI
    /// </summary>
    /// <remarks>
    /// This command will open the currently open JSON file in the interactive text editor.
    /// </remarks>
    class TuiCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string path = JsonShellCommon.FileStream.Name;
            List<string> lines = Reading.ReadAllLinesNoBlock(path).ToList();
            TextEditInteractive.OpenInteractive(JsonShellCommon.FileStream.Name, ref lines);

            // Save the changes
            JsonTools.CloseJsonFile();
            Writing.WriteAllLinesNoBlock(path, lines.ToArray());
            JsonTools.OpenJsonFile(path);
            return 0;
        }
    }
}
