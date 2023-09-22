
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

using System.Collections.Generic;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Files.Editors.TextEdit;
using KS.Languages;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.Text.Commands
{
    /// <summary>
    /// Adds new lines with text at the end of the file
    /// </summary>
    /// <remarks>
    /// You can use this command to add new lines at the end of the file.
    /// </remarks>
    class TextEdit_AddLinesCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string StringArgsOrig, string[] ListArgsOnlyOrig, string[] ListSwitchesOnly, ref string variableValue)
        {
            var FinalLines = new List<string>();
            string FinalLine = "";

            // Keep prompting for lines until the user finishes
            TextWriterColor.Write(Translate.DoTranslation("Enter the text that you want to append to the end of the file. When you're done, write \"EOF\" on its own line."));
            while (FinalLine != "EOF")
            {
                TextWriterColor.Write(">> ", false, KernelColorType.Input);
                FinalLine = Input.ReadLine();
                if (!(FinalLine == "EOF"))
                {
                    FinalLines.Add(FinalLine);
                }
            }

            // Add the new lines
            TextEditTools.TextEdit_AddNewLines(FinalLines.ToArray());
            return 0;
        }

    }
}
