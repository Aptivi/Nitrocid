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

using Terminaux.Colors.Themes.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Files.Editors.TextEdit;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Misc.Reflection;
using Terminaux.Shell.Commands;
using System;
using Textify.General;

namespace Nitrocid.Shell.Shells.Text.Commands
{
    /// <summary>
    /// Removes the specified line number
    /// </summary>
    /// <remarks>
    /// You can use this command to remove a specified line by number. You can use the print command to take a look at the unneeded line and its number.
    /// </remarks>
    class DelLineCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (parameters.ArgumentsList.Length == 1)
            {
                if (TextTools.IsStringNumeric(parameters.ArgumentsList[0]))
                {
                    if (Convert.ToInt32(parameters.ArgumentsList[0]) <= TextEditShellCommon.FileLines.Count)
                    {
                        TextEditTools.RemoveLine(Convert.ToInt32(parameters.ArgumentsList[0]));
                        TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_TEXT_DELLINE_SUCCESS"), true, ThemeColorType.Success);
                        return 0;
                    }
                    else
                    {
                        TextWriters.Write(LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_LINENUMEXCEEDSLASTNUM"), true, ThemeColorType.Error);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.TextEditor);
                    }
                }
                else
                {
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_TEXT_DELLINE_NUMINVALID"), true, ThemeColorType.Error, parameters.ArgumentsList[0]);
                    DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", vars: [parameters.ArgumentsList[0]]);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.TextEditor);
                }
            }
            else if (parameters.ArgumentsList.Length > 1)
            {
                if (TextTools.IsStringNumeric(parameters.ArgumentsList[0]) & TextTools.IsStringNumeric(parameters.ArgumentsList[1]))
                {
                    if (Convert.ToInt32(parameters.ArgumentsList[0]) <= TextEditShellCommon.FileLines.Count & Convert.ToInt32(parameters.ArgumentsList[1]) <= TextEditShellCommon.FileLines.Count)
                    {
                        int LineNumberStart = Convert.ToInt32(parameters.ArgumentsList[0]);
                        int LineNumberEnd = Convert.ToInt32(parameters.ArgumentsList[1]);
                        LineNumberStart.SwapIfSourceLarger(ref LineNumberEnd);
                        for (int LineNumber = LineNumberStart; LineNumber <= LineNumberEnd; LineNumber++)
                        {
                            TextEditTools.RemoveLine(LineNumber);
                            TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_TEXT_DELLINE_SUCCESSINLINE"), true, ThemeColorType.Success, LineNumber);
                        }
                        return 0;
                    }
                    else
                    {
                        TextWriters.Write(LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_LINENUMEXCEEDSLASTNUM"), true, ThemeColorType.Error);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.TextEditor);
                    }
                }
                else
                {
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_TEXT_DELLINE_NUMINVALID"), true, ThemeColorType.Error, parameters.ArgumentsList[1]);
                    DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", vars: [parameters.ArgumentsList[1]]);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.TextEditor);
                }
            }
            return 0;
        }

    }
}
