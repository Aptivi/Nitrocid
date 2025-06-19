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

using System;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Inputs;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using Textify.General;

namespace Nitrocid.Shell.Shells.Text.Commands
{
    /// <summary>
    /// Edits a line
    /// </summary>
    /// <remarks>
    /// You can use this command to edit a line seamlessly.
    /// </remarks>
    class EditLineCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (TextTools.IsStringNumeric(parameters.ArgumentsList[0]))
            {
                int lineNum = Convert.ToInt32(parameters.ArgumentsList[0]);
                if (lineNum <= TextEditShellCommon.FileLines.Count)
                {
                    string OriginalLine = TextEditShellCommon.FileLines[lineNum - 1];
                    TextWriters.Write(">> ", false, KernelColorType.Input);
                    string EditedLine = InputTools.ReadLine("", OriginalLine);
                    TextEditShellCommon.FileLines[lineNum - 1] = EditedLine;
                    return 0;
                }
                else
                {
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_LINENUMEXCEEDSLASTNUM"), true, KernelColorType.Error);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.TextEditor);
                }
            }
            else
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_TEXT_DELLINE_NUMINVALID"), true, KernelColorType.Error);
                DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", vars: [parameters.ArgumentsList[0]]);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.TextEditor);
            }
        }

    }
}
