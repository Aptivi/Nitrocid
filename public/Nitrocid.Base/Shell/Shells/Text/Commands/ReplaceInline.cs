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

using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using System;
using Textify.General;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Files.Editors.TextEdit;
using Nitrocid.Base.Misc.Reflection;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Exceptions;

namespace Nitrocid.Base.Shell.Shells.Text.Commands
{
    /// <summary>
    /// Replaces a word or phrase with another one in a line
    /// </summary>
    /// <remarks>
    /// You can use this command to replace a word or a complete phrase enclosed in double quotes with another one (enclosed in double quotes again) in a line.
    /// </remarks>
    class ReplaceInlineCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (parameters.ArgumentsList.Length == 3)
            {
                if (TextTools.IsStringNumeric(parameters.ArgumentsList[2]))
                {
                    if (Convert.ToInt32(parameters.ArgumentsList[2]) <= TextEditShellCommon.FileLines.Count)
                    {
                        TextEditTools.Replace(parameters.ArgumentsList[0], parameters.ArgumentsList[1], Convert.ToInt32(parameters.ArgumentsList[2]));
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_TEXT_REPLACE_SUCCESS"), true, ThemeColorType.Success);
                        return 0;
                    }
                    else
                    {
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_LINENUMEXCEEDSLASTNUM"), true, ThemeColorType.Error);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.TextEditor);
                    }
                }
                else
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_TEXT_DELLINE_NUMINVALID"), true, ThemeColorType.Error, parameters.ArgumentsList[2]);
                    DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", vars: [parameters.ArgumentsList[2]]);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.TextEditor);
                }
            }
            else if (parameters.ArgumentsList.Length > 3)
            {
                if (TextTools.IsStringNumeric(parameters.ArgumentsList[2]) & TextTools.IsStringNumeric(parameters.ArgumentsList[3]))
                {
                    if (Convert.ToInt32(parameters.ArgumentsList[2]) <= TextEditShellCommon.FileLines.Count & Convert.ToInt32(parameters.ArgumentsList[3]) <= TextEditShellCommon.FileLines.Count)
                    {
                        int LineNumberStart = Convert.ToInt32(parameters.ArgumentsList[2]);
                        int LineNumberEnd = Convert.ToInt32(parameters.ArgumentsList[3]);
                        LineNumberStart.SwapIfSourceLarger(ref LineNumberEnd);
                        for (int LineNumber = LineNumberStart; LineNumber <= LineNumberEnd; LineNumber++)
                        {
                            TextEditTools.Replace(parameters.ArgumentsList[0], parameters.ArgumentsList[1], LineNumber);
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_TEXT_REPLACEINLINE_SUCCESSINLINE"), true, ThemeColorType.Success, LineNumber);
                        }
                        return 0;
                    }
                    else
                    {
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_LINENUMEXCEEDSLASTNUM"), true, ThemeColorType.Error);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.TextEditor);
                    }
                }
            }
            return 0;
        }

    }
}
