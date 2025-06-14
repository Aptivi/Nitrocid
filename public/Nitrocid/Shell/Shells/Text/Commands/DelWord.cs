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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Files.Editors.TextEdit;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Misc.Reflection;
using Nitrocid.Shell.ShellBase.Commands;
using System;
using Textify.General;

namespace Nitrocid.Shell.Shells.Text.Commands
{
    /// <summary>
    /// Deletes a word or phrase from line number
    /// </summary>
    /// <remarks>
    /// You can use this command to remove an extraneous word or phrase enclosed in double quotes in a specified line number. You can use the print command to review the changes and line numbers.
    /// </remarks>
    class DelWordCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (parameters.ArgumentsList.Length == 2)
            {
                if (TextTools.IsStringNumeric(parameters.ArgumentsList[1]))
                {
                    if (Convert.ToInt32(parameters.ArgumentsList[1]) <= TextEditShellCommon.FileLines.Count)
                    {
                        TextEditTools.DeleteWord(parameters.ArgumentsList[0], Convert.ToInt32(parameters.ArgumentsList[1]));
                        TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_TEXT_DELWORD_SUCCESS"), true, KernelColorType.Success);
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
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_TEXT_DELLINE_NUMINVALID"), true, KernelColorType.Error, parameters.ArgumentsList[1]);
                    DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", vars: [parameters.ArgumentsList[1]]);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.TextEditor);
                }
            }
            else if (parameters.ArgumentsList.Length > 2)
            {
                if (TextTools.IsStringNumeric(parameters.ArgumentsList[1]) & TextTools.IsStringNumeric(parameters.ArgumentsList[2]))
                {
                    if (Convert.ToInt32(parameters.ArgumentsList[1]) <= TextEditShellCommon.FileLines.Count & Convert.ToInt32(parameters.ArgumentsList[2]) <= TextEditShellCommon.FileLines.Count)
                    {
                        int LineNumberStart = Convert.ToInt32(parameters.ArgumentsList[1]);
                        int LineNumberEnd = Convert.ToInt32(parameters.ArgumentsList[2]);
                        LineNumberStart.SwapIfSourceLarger(ref LineNumberEnd);
                        for (int LineNumber = LineNumberStart; LineNumber <= LineNumberEnd; LineNumber++)
                        {
                            TextEditTools.DeleteWord(parameters.ArgumentsList[0], LineNumber);
                            TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_TEXT_DELWORD_SUCCESSINLINE"), true, KernelColorType.Success, LineNumber);
                        }
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
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_TEXT_DELLINE_NUMINVALID"), true, KernelColorType.Error, parameters.ArgumentsList[1]);
                    DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", vars: [parameters.ArgumentsList[1]]);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.TextEditor);
                }
            }
            return 0;
        }

    }
}
