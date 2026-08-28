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
using Nitrocid.Files.Editors.TextEdit;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Misc.Reflection;
using Terminaux.Base.Extensions;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;

namespace Nitrocid.Shell.Shells.Text.Commands
{
    /// <summary>
    /// Queries a word in a specified line or all lines
    /// </summary>
    /// <remarks>
    /// You can use this command to query a word and get its number from the specified line or all lines.
    /// </remarks>
    class QueryWordCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "queryword";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_TEXT_COMMAND_QUERYWORD_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "word/phrase", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_QUERYWORD_ARGUMENT_WORD_DESC"
                    }),
                    new CommandArgumentPart(true, "lineNum/all", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_QUERYCHAR_ARGUMENT_LINENUM_DESC"
                    }),
                    new CommandArgumentPart(false, "lineNum2", new CommandArgumentPartOptions()
                    {
                        IsNumeric = true,
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_DELLINE_ARGUMENT_LINENUM2_DESC"
                    })
                ])
            ];

        public override CommandFlags Flags =>
            CommandFlags.Wrappable;

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string targetStr = parameters.ArgumentsList[0];
            string lineNumStr = parameters.ArgumentsList[1];
            if (parameters.ArgumentsList.Length == 2)
            {
                if (TextTools.IsStringNumeric(lineNumStr))
                {
                    if (Convert.ToInt32(lineNumStr) <= TextEditShellCommon.FileLines.Count)
                    {
                        int LineIndex = Convert.ToInt32(lineNumStr);
                        var QueriedChars = TextEditTools.QueryWord(targetStr, LineIndex);
                        TextWriterColor.Write("- {0}: ", false, ThemeColorType.ListEntry, LineIndex);

                        // Process the output
                        string text = TextEditShellCommon.FileLines[LineIndex - 1];
                        var Words = text.Split(' ');
                        for (int wordIndex = 0; wordIndex < Words.Length; wordIndex++)
                        {
                            string word = Words[wordIndex];
                            TextWriterColor.Write($"{(QueriedChars.Contains(wordIndex) ? ThemeColorsTools.GetColor(ThemeColorType.Success).VTSequenceForeground() : "")}{word} ", false, ThemeColorType.ListValue);
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
                    var QueriedWords = TextEditTools.QueryWord(targetStr);
                    foreach (var QueriedWord in QueriedWords)
                    {
                        int LineIndex = QueriedWord.Item1;
                        var QueriedChars = TextEditTools.QueryWord(targetStr, LineIndex + 1);
                        TextWriterColor.Write("- {0}: ", false, ThemeColorType.ListEntry, LineIndex + 1);

                        // Process the output
                        string text = TextEditShellCommon.FileLines[LineIndex];
                        var Words = text.Split(' ');
                        for (int wordIndex = 0; wordIndex < Words.Length; wordIndex++)
                        {
                            string word = Words[wordIndex];
                            TextWriterColor.Write($"{(QueriedChars.Contains(wordIndex) ? ThemeColorsTools.GetColor(ThemeColorType.Success).VTSequenceForeground() : "")}{word} ", false, ThemeColorType.ListValue);
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
                    if (Convert.ToInt32(lineNumStr) <= TextEditShellCommon.FileLines.Count & Convert.ToInt32(lineNumSecondStr) <= TextEditShellCommon.FileLines.Count)
                    {
                        int LineNumberStart = Convert.ToInt32(lineNumStr);
                        int LineNumberEnd = Convert.ToInt32(lineNumSecondStr);
                        LineNumberStart.SwapIfSourceLarger(ref LineNumberEnd);
                        for (int LineNumber = LineNumberStart; LineNumber <= LineNumberEnd; LineNumber++)
                        {
                            var QueriedChars = TextEditTools.QueryWord(targetStr, LineNumber);
                            int LineIndex = LineNumber - 1;
                            TextWriterColor.Write("- {0}: ", false, ThemeColorType.ListEntry, LineIndex);

                            // Process the output
                            string text = TextEditShellCommon.FileLines[LineIndex];
                            var Words = text.Split(' ');
                            for (int wordIndex = 0; wordIndex < Words.Length; wordIndex++)
                            {
                                string word = Words[wordIndex];
                                TextWriterColor.Write($"{(QueriedChars.Contains(wordIndex) ? ThemeColorsTools.GetColor(ThemeColorType.Success).VTSequenceForeground() : "")}{word} ", false, ThemeColorType.ListValue);
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
