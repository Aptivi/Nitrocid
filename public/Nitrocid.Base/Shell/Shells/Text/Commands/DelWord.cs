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

using System;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Reflection;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;

namespace Nitrocid.Base.Shell.Shells.Text.Commands
{
    /// <summary>
    /// Deletes a word or phrase from line number
    /// </summary>
    /// <remarks>
    /// You can use this command to remove an extraneous word or phrase enclosed in double quotes in a specified line number. You can use the print command to review the changes and line numbers.
    /// </remarks>
    class DelWordCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "delword";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_TEXT_COMMAND_DELWORD_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "word/phrase", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_DELWORD_ARGUMENT_WORD_DESC"
                    }),
                    new CommandArgumentPart(true, "lineNum", new CommandArgumentPartOptions()
                    {
                        IsNumeric = true,
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_DELLINE_ARGUMENT_LINENUM_DESC"
                    }),
                    new CommandArgumentPart(false, "lineNum2", new CommandArgumentPartOptions()
                    {
                        IsNumeric = true,
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_DELLINE_ARGUMENT_LINENUM2_DESC"
                    })
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string targetStr = parameters.ArgumentsList[0];
            string lineNumStr = parameters.ArgumentsList[1];
            var textShell = (TextShell?)shell ??
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_LASTSHELLTYPEMISMATCH"));
            if (parameters.ArgumentsList.Length == 2)
            {
                if (TextTools.IsStringNumeric(lineNumStr))
                {
                    if (Convert.ToInt32(lineNumStr) <= textShell.FileLines.Count)
                    {
                        textShell.DeleteWord(targetStr, Convert.ToInt32(lineNumStr));
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_TEXT_DELWORD_SUCCESS"), true, ThemeColorType.Success);
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
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_TEXT_DELLINE_NUMINVALID"), true, ThemeColorType.Error, lineNumStr);
                    DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", vars: [lineNumStr]);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.TextEditor);
                }
            }
            else if (parameters.ArgumentsList.Length > 2)
            {
                string lineNumSecondStr = parameters.ArgumentsList[2];
                if (TextTools.IsStringNumeric(lineNumStr) & TextTools.IsStringNumeric(lineNumSecondStr))
                {
                    if (Convert.ToInt32(lineNumStr) <= textShell.FileLines.Count & Convert.ToInt32(lineNumSecondStr) <= textShell.FileLines.Count)
                    {
                        int LineNumberStart = Convert.ToInt32(lineNumStr);
                        int LineNumberEnd = Convert.ToInt32(lineNumSecondStr);
                        LineNumberStart.SwapIfSourceLarger(ref LineNumberEnd);
                        for (int LineNumber = LineNumberStart; LineNumber <= LineNumberEnd; LineNumber++)
                        {
                            textShell.DeleteWord(targetStr, LineNumber);
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_TEXT_DELWORD_SUCCESSINLINE"), true, ThemeColorType.Success, LineNumber);
                        }
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
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_TEXT_DELLINE_NUMINVALID"), true, ThemeColorType.Error, lineNumStr);
                    DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", vars: [lineNumStr]);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.TextEditor);
                }
            }
            return 0;
        }

    }
}
