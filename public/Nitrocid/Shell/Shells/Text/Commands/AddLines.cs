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

using System.Collections.Generic;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Inputs;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Files.Editors.TextEdit;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;

namespace Nitrocid.Shell.Shells.Text.Commands
{
    /// <summary>
    /// Adds new lines with text at the end of the file
    /// </summary>
    /// <remarks>
    /// You can use this command to add new lines at the end of the file.
    /// </remarks>
    class AddLinesCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var FinalLines = new List<string>();
            string FinalLine = "";

            // Keep prompting for lines until the user finishes
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_TEXT_ADDLINES_LINEPROMPT"));
            while (FinalLine != "EOF")
            {
                TextWriters.Write(">> ", false, KernelColorType.Input);
                FinalLine = InputTools.ReadLine();
                if (FinalLine != "EOF")
                {
                    FinalLines.Add(FinalLine);
                }
            }

            // Add the new lines
            TextEditTools.AddNewLines([.. FinalLines]);
            return 0;
        }

    }
}
