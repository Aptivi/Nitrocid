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
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using System;
using Textify.General;
using Nitrocid.Base.Files.Editors.TextEdit;
using Nitrocid.Base.Misc.Reflection;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Exceptions;

namespace Nitrocid.Base.Shell.Shells.Text.Commands
{
    /// <summary>
    /// Queries a word in a specified line or all lines using regular expressions
    /// </summary>
    /// <remarks>
    /// You can use this command to query a word and get its number from the specified line or all lines.
    /// </remarks>
    class QueryWordRegexCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (parameters.ArgumentsList.Length == 2)
            {
                if (TextTools.IsStringNumeric(parameters.ArgumentsList[1]))
                {
                    if (Convert.ToInt32(parameters.ArgumentsList[1]) <= TextEditShellCommon.FileLines.Count)
                    {
                        int LineIndex = Convert.ToInt32(parameters.ArgumentsList[1]);
                        var QueriedChars = TextEditTools.QueryWordRegex(parameters.ArgumentsList[0], LineIndex);
                        TextWriterColor.Write("- {0}: ", false, ThemeColorType.ListEntry, LineIndex);

                        // Process the output
                        string text = TextEditShellCommon.FileLines[LineIndex - 1];
                        var Words = text.Split(' ');
                        for (int wordIndex = 0; wordIndex < Words.Length; wordIndex++)
                        {
                            string word = Words[wordIndex];
                            TextWriterColor.Write($"{(QueriedChars.Contains(wordIndex) ? ThemeColorsTools.GetColor(ThemeColorType.Success).VTSequenceForeground : "")}{word} ", false, ThemeColorType.ListValue);
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
                else if (parameters.ArgumentsList[1].Equals("all", StringComparison.OrdinalIgnoreCase))
                {
                    var QueriedWords = TextEditTools.QueryWordRegex(parameters.ArgumentsList[0]);
                    foreach (var QueriedWord in QueriedWords)
                    {
                        int LineIndex = QueriedWord.Item1;
                        var QueriedChars = TextEditTools.QueryWordRegex(parameters.ArgumentsList[0], LineIndex + 1);
                        TextWriterColor.Write("- {0}: ", false, ThemeColorType.ListEntry, LineIndex + 1);

                        // Process the output
                        string text = TextEditShellCommon.FileLines[LineIndex];
                        var Words = text.Split(' ');
                        for (int wordIndex = 0; wordIndex < Words.Length; wordIndex++)
                        {
                            string word = Words[wordIndex];
                            TextWriterColor.Write($"{(QueriedChars.Contains(wordIndex) ? ThemeColorsTools.GetColor(ThemeColorType.Success).VTSequenceForeground : "")}{word} ", false, ThemeColorType.ListValue);
                        }
                        TextWriterRaw.Write();
                    }
                    return 0;
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
                            var QueriedChars = TextEditTools.QueryWordRegex(parameters.ArgumentsList[0], LineNumber);
                            int LineIndex = LineNumber - 1;
                            TextWriterColor.Write("- {0}: ", false, ThemeColorType.ListEntry, LineIndex);

                            // Process the output
                            string text = TextEditShellCommon.FileLines[LineIndex];
                            var Words = text.Split(' ');
                            for (int wordIndex = 0; wordIndex < Words.Length; wordIndex++)
                            {
                                string word = Words[wordIndex];
                                TextWriterColor.Write($"{(QueriedChars.Contains(wordIndex) ? ThemeColorsTools.GetColor(ThemeColorType.Success).VTSequenceForeground : "")}{word} ", false, ThemeColorType.ListValue);
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
