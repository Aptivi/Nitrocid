
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Files.Editors.TextEdit;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Misc.Text;
using KS.Shell.ShellBase.Commands;
using System;

namespace KS.Shell.Shells.Text.Commands
{
    /// <summary>
    /// Queries a word in a specified line or all lines
    /// </summary>
    /// <remarks>
    /// You can use this command to query a word and get its number from the specified line or all lines.
    /// </remarks>
    class TextEdit_QueryWordCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, ref string variableValue)
        {
            if (ListArgsOnly.Length == 2)
            {
                if (TextTools.IsStringNumeric(ListArgsOnly[1]))
                {
                    if (Convert.ToInt32(ListArgsOnly[1]) <= TextEditShellCommon.TextEdit_FileLines.Count)
                    {
                        int LineIndex = Convert.ToInt32(ListArgsOnly[1]);
                        var QueriedChars = TextEditTools.TextEdit_QueryWord(ListArgsOnly[0], LineIndex);
                        TextWriterColor.Write("- {0}: ", false, KernelColorType.ListEntry, LineIndex);

                        // Process the output
                        string text = TextEditShellCommon.TextEdit_FileLines[LineIndex - 1];
                        var Words = text.Split(' ');
                        for (int wordIndex = 0; wordIndex < Words.Length; wordIndex++)
                        {
                            string word = Words[wordIndex];
                            TextWriterColor.Write($"{(QueriedChars.Contains(wordIndex) ? KernelColorTools.GetColor(KernelColorType.Success).VTSequenceForeground : "")}{word} ", false, KernelColorType.ListValue);
                        }
                        TextWriterColor.Write();
                        return 0;
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("The specified line number may not be larger than the last file line number."), true, KernelColorType.Error);
                        return 10000 + (int)KernelExceptionType.TextEditor;
                    }
                }
                else if (ListArgsOnly[1].ToLower() == "all")
                {
                    var QueriedWords = TextEditTools.TextEdit_QueryWord(ListArgsOnly[0]);
                    foreach (var QueriedWord in QueriedWords)
                    {
                        int LineIndex = QueriedWord.Item1;
                        var QueriedChars = TextEditTools.TextEdit_QueryWord(ListArgsOnly[0], LineIndex + 1);
                        TextWriterColor.Write("- {0}: ", false, KernelColorType.ListEntry, LineIndex + 1);

                        // Process the output
                        string text = TextEditShellCommon.TextEdit_FileLines[LineIndex];
                        var Words = text.Split(' ');
                        for (int wordIndex = 0; wordIndex < Words.Length; wordIndex++)
                        {
                            string word = Words[wordIndex];
                            TextWriterColor.Write($"{(QueriedChars.Contains(wordIndex) ? KernelColorTools.GetColor(KernelColorType.Success).VTSequenceForeground : "")}{word} ", false, KernelColorType.ListValue);
                        }
                        TextWriterColor.Write();
                    }
                    return 0;
                }
            }
            else if (ListArgsOnly.Length > 2)
            {
                if (TextTools.IsStringNumeric(ListArgsOnly[1]) & TextTools.IsStringNumeric(ListArgsOnly[2]))
                {
                    if (Convert.ToInt32(ListArgsOnly[1]) <= TextEditShellCommon.TextEdit_FileLines.Count & Convert.ToInt32(ListArgsOnly[2]) <= TextEditShellCommon.TextEdit_FileLines.Count)
                    {
                        int LineNumberStart = Convert.ToInt32(ListArgsOnly[1]);
                        int LineNumberEnd = Convert.ToInt32(ListArgsOnly[2]);
                        LineNumberStart.SwapIfSourceLarger(ref LineNumberEnd);
                        for (int LineNumber = LineNumberStart; LineNumber <= LineNumberEnd; LineNumber++)
                        {
                            var QueriedChars = TextEditTools.TextEdit_QueryWord(ListArgsOnly[0], LineNumber);
                            int LineIndex = LineNumber - 1;
                            TextWriterColor.Write("- {0}: ", false, KernelColorType.ListEntry, LineIndex);

                            // Process the output
                            string text = TextEditShellCommon.TextEdit_FileLines[LineIndex];
                            var Words = text.Split(' ');
                            for (int wordIndex = 0; wordIndex < Words.Length; wordIndex++)
                            {
                                string word = Words[wordIndex];
                                TextWriterColor.Write($"{(QueriedChars.Contains(wordIndex) ? KernelColorTools.GetColor(KernelColorType.Success).VTSequenceForeground : "")}{word} ", false, KernelColorType.ListValue);
                            }
                            TextWriterColor.Write();
                        }
                        return 0;
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("The specified line number may not be larger than the last file line number."), true, KernelColorType.Error);
                        return 10000 + (int)KernelExceptionType.TextEditor;
                    }
                }
            }
            return 0;
        }

    }
}
