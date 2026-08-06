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

using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using System;
using System.Linq;
using Textify.General;
using Nitrocid.Base.Misc.Reflection;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Exceptions;
using Terminaux.Base.Extensions;
using Terminaux.Shell.Shells;

namespace Nitrocid.Base.Shell.Shells.Text.Commands
{
    /// <summary>
    /// Queries a character in a specified line or all lines
    /// </summary>
    /// <remarks>
    /// You can use this command to query a character and get its number from the specified line or all lines. This is useful for some commands like delcharnum.
    /// </remarks>
    class QueryCharCommand : BaseCommand, ICommand
    {

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
                        int LineIndex = Convert.ToInt32(lineNumStr);
                        var QueriedChars = textShell.QueryChar(Convert.ToChar(targetStr), LineIndex);
                        TextWriterColor.Write("- {0}: ", false, ThemeColorType.ListEntry, LineIndex);

                        // Process the output
                        string text = textShell.FileLines[LineIndex - 1];
                        for (int charIndex = 0; charIndex < text.Length; charIndex++)
                        {
                            char Character = text[charIndex];
                            TextWriterColor.Write($"{(QueriedChars.Contains(charIndex) ? ThemeColorsTools.GetColor(ThemeColorType.Success).VTSequenceForeground() : "")}{Character}", false, ThemeColorType.ListValue);
                        }
                        TextWriterRaw.Write();
                        return 0;
                    }
                    else
                    {
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FILES_EDITORS_TEXTEDITOR_EXCEPTION_LINENUMEXCEEDSLASTNUM"), true, ThemeColorType.Error);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.TextEditor);
                    }
                }
                else if (lineNumStr.Equals("all", StringComparison.OrdinalIgnoreCase))
                {
                    var QueriedChars = textShell.QueryChar(Convert.ToChar(targetStr));
                    foreach (var QueriedChar in QueriedChars)
                    {
                        int LineIndex = QueriedChar.Item1;
                        TextWriterColor.Write("- {0}: ", false, ThemeColorType.ListEntry, LineIndex + 1);

                        // Process the output
                        string text = textShell.FileLines[LineIndex];
                        var queried = QueriedChar.Item2;
                        for (int charIndex = 0; charIndex < text.Length; charIndex++)
                        {
                            char Character = text[charIndex];
                            TextWriterColor.Write($"{(queried.Contains(charIndex) ? ThemeColorsTools.GetColor(ThemeColorType.Success).VTSequenceForeground() : "")}{Character}", false, ThemeColorType.ListValue);
                        }
                        TextWriterRaw.Write();
                    }
                    return 0;
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
                            var QueriedChars = textShell.QueryChar(Convert.ToChar(targetStr), LineNumber);
                            int LineIndex = LineNumber - 1;
                            TextWriterColor.Write("- {0}: ", false, ThemeColorType.ListEntry, LineNumber);

                            // Process the output
                            string text = textShell.FileLines[LineIndex];
                            for (int charIndex = 0; charIndex < text.Length; charIndex++)
                            {
                                char Character = text[charIndex];
                                TextWriterColor.Write($"{(QueriedChars.Contains(charIndex) ? ThemeColorsTools.GetColor(ThemeColorType.Success).VTSequenceForeground() : "")}{Character}", false, ThemeColorType.ListValue);
                            }
                            TextWriterRaw.Write();
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
