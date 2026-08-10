//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.Collections.Generic;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Terminaux.Reader;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Base.Shell.Shells.Text.Commands
{
    /// <summary>
    /// Adds new lines with text at the end of the file
    /// </summary>
    /// <remarks>
    /// You can use this command to add new lines at the end of the file.
    /// </remarks>
    class AddLinesCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "addlines";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_TEXT_COMMAND_ADDLINES_DESC");

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            var textShell = (TextShell?)shell ??
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_LASTSHELLTYPEMISMATCH"));
            var FinalLines = new List<string>();
            string FinalLine = "";

            // Keep prompting for lines until the user finishes
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_TEXT_ADDLINES_LINEPROMPT"));
            while (FinalLine != "EOF")
            {
                TextWriterColor.Write(">> ", false, ThemeColorType.Input);
                FinalLine = TermReader.Read();
                if (FinalLine != "EOF")
                {
                    FinalLines.Add(FinalLine);
                }
            }

            // Add the new lines
            textShell.AddNewLines([.. FinalLines]);
            return 0;
        }

    }
}
