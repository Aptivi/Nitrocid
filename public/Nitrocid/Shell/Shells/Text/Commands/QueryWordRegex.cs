
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
using KS.Languages;
using KS.Misc.Editors.TextEdit;
using KS.Misc.Reflection;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;
using System;

namespace KS.Shell.Shells.Text.Commands
{
    /// <summary>
    /// Queries a word in a specified line or all lines using regular expressions
    /// </summary>
    /// <remarks>
    /// You can use this command to query a word and get its number from the specified line or all lines.
    /// </remarks>
    class TextEdit_QueryWordRegexCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (ListArgsOnly.Length == 2)
            {
                if (TextTools.IsStringNumeric(ListArgsOnly[1]))
                {
                    if (Convert.ToInt32(ListArgsOnly[1]) <= TextEditShellCommon.TextEdit_FileLines.Count)
                    {
                        var QueriedChars = TextEditTools.TextEdit_QueryWordRegex(ListArgsOnly[0], Convert.ToInt32(ListArgsOnly[1]));
                        foreach (int WordIndex in QueriedChars.Keys)
                        {
                            TextWriterColor.Write("- {0}: ", false, KernelColorType.ListEntry, WordIndex);
                            TextWriterColor.Write("{0} ({1})", true, KernelColorType.ListValue, ListArgsOnly[0], TextEditShellCommon.TextEdit_FileLines[Convert.ToInt32(ListArgsOnly[1])]);
                        }
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("The specified line number may not be larger than the last file line number."), true, KernelColorType.Error);
                    }
                }
                else if (ListArgsOnly[1].ToLower() == "all")
                {
                    var QueriedWords = TextEditTools.TextEdit_QueryWordRegex(ListArgsOnly[0]);
                    foreach (int LineIndex in QueriedWords.Keys)
                    {
                        foreach (int WordIndex in QueriedWords[LineIndex].Keys)
                        {
                            TextWriterColor.Write("- {0}:{1}: ", false, KernelColorType.ListEntry, LineIndex, WordIndex);
                            TextWriterColor.Write("{0} ({1})", true, KernelColorType.ListValue, ListArgsOnly[0], TextEditShellCommon.TextEdit_FileLines[LineIndex]);
                        }
                    }
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
                            var QueriedChars = TextEditTools.TextEdit_QueryWordRegex(ListArgsOnly[0], LineNumber);
                            foreach (int WordIndex in QueriedChars.Keys)
                            {
                                TextWriterColor.Write("- {0}:{1}: ", false, KernelColorType.ListEntry, LineNumber, WordIndex);
                                TextWriterColor.Write("{0} ({1})", true, KernelColorType.ListValue, ListArgsOnly[0], TextEditShellCommon.TextEdit_FileLines[Convert.ToInt32(ListArgsOnly[1])]);
                            }
                        }
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("The specified line number may not be larger than the last file line number."), true, KernelColorType.Error);
                    }
                }
            }
        }

    }
}
