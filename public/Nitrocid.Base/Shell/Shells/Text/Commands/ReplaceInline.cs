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
    /// Replaces a word or phrase with another one in a line
    /// </summary>
    /// <remarks>
    /// You can use this command to replace a word or a complete phrase enclosed in double quotes with another one (enclosed in double quotes again) in a line.
    /// </remarks>
    class ReplaceInlineCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "replaceinline";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_TEXT_COMMAND_REPLACEINLINE_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "word/phrase", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_REPLACE_ARGUMENT_SOURCE_DESC"
                    }),
                    new CommandArgumentPart(true, "word/phrase", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_REPLACE_ARGUMENT_TARGET_DESC"
                    }),
                    new CommandArgumentPart(true, "lineNum/all", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_REPLACEINLINE_ARGUMENT_LINENUM_DESC"
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
            string replacementStr = parameters.ArgumentsList[1];
            string lineNumStr = parameters.ArgumentsList[2];
            var textShell = (TextShell?)shell ??
                throw new KernelException(KernelExceptionType.TextEditor, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_LASTSHELLTYPEMISMATCH"));
            if (parameters.ArgumentsList.Length == 3)
            {
                if (TextTools.IsStringNumeric(lineNumStr))
                {
                    if (Convert.ToInt32(lineNumStr) <= textShell.FileLines.Count)
                    {
                        textShell.Replace(targetStr, replacementStr, Convert.ToInt32(lineNumStr));
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
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_TEXT_DELLINE_NUMINVALID"), true, ThemeColorType.Error, lineNumStr);
                    DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", vars: [lineNumStr]);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.TextEditor);
                }
            }
            else if (parameters.ArgumentsList.Length > 3)
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
                            textShell.Replace(targetStr, replacementStr, LineNumber);
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
