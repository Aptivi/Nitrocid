
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using Extensification.IntegerExts;
using KS.ConsoleBase.Colors;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;
using System;

namespace KS.Shell.Shells.Text.Commands
{
    /// <summary>
    /// Prints the contents of the file
    /// </summary>
    /// <remarks>
    /// Prints the contents of the file with line numbers to the console. This is useful if you need to view the contents before and after editing.
    /// </remarks>
    class TextEdit_PrintCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            int LineNumber = 1;
            if (ListArgsOnly.Length > 0)
            {
                if (ListArgsOnly.Length == 1)
                {
                    // We've only provided one line number
                    DebugWriter.WriteDebug(DebugLevel.I, "Line number provided: {0}", ListArgsOnly[0]);
                    DebugWriter.WriteDebug(DebugLevel.I, "Is it numeric? {0}", StringQuery.IsStringNumeric(ListArgsOnly[0]));
                    if (StringQuery.IsStringNumeric(ListArgsOnly[0]))
                    {
                        LineNumber = Convert.ToInt32(ListArgsOnly[0]);
                        DebugWriter.WriteDebug(DebugLevel.I, "File lines: {0}", TextEditShellCommon.TextEdit_FileLines.Count);
                        if (Convert.ToInt32(ListArgsOnly[0]) <= TextEditShellCommon.TextEdit_FileLines.Count)
                        {
                            string Line = TextEditShellCommon.TextEdit_FileLines[LineNumber - 1];
                            DebugWriter.WriteDebug(DebugLevel.I, "Line number: {0} ({1})", LineNumber, Line);
                            TextWriterColor.Write("- {0}: ", false, KernelColorType.ListEntry, LineNumber);
                            TextWriterColor.Write(Line, true, KernelColorType.ListValue);
                        }
                        else
                        {
                            TextWriterColor.Write(Translate.DoTranslation("The specified line number may not be larger than the last file line number."), true, KernelColorType.Error);
                        }
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Specified line number {0} is not a valid number."), true, KernelColorType.Error, ListArgsOnly[0]);
                        DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", ListArgsOnly[0]);
                    }
                }
                else
                {
                    // We've provided two line numbers in the range
                    DebugWriter.WriteDebug(DebugLevel.I, "Line numbers provided: {0}, {1}", ListArgsOnly[0], ListArgsOnly[1]);
                    DebugWriter.WriteDebug(DebugLevel.I, "Is it numeric? {0}", StringQuery.IsStringNumeric(ListArgsOnly[0]), StringQuery.IsStringNumeric(ListArgsOnly[1]));
                    if (StringQuery.IsStringNumeric(ListArgsOnly[0]) & StringQuery.IsStringNumeric(ListArgsOnly[1]))
                    {
                        int LineNumberStart = Convert.ToInt32(ListArgsOnly[0]);
                        int LineNumberEnd = Convert.ToInt32(ListArgsOnly[1]);
                        LineNumberStart.SwapIfSourceLarger(ref LineNumberEnd);
                        DebugWriter.WriteDebug(DebugLevel.I, "File lines: {0}", TextEditShellCommon.TextEdit_FileLines.Count);
                        if (LineNumberStart <= TextEditShellCommon.TextEdit_FileLines.Count & LineNumberEnd <= TextEditShellCommon.TextEdit_FileLines.Count)
                        {
                            for (LineNumber = LineNumberStart; LineNumber <= LineNumberEnd; LineNumber++)
                            {
                                string Line = TextEditShellCommon.TextEdit_FileLines[LineNumber - 1];
                                DebugWriter.WriteDebug(DebugLevel.I, "Line number: {0} ({1})", LineNumber, Line);
                                TextWriterColor.Write("- {0}: ", false, KernelColorType.ListEntry, LineNumber);
                                TextWriterColor.Write(Line, true, KernelColorType.ListValue);
                            }
                        }
                        else
                        {
                            TextWriterColor.Write(Translate.DoTranslation("The specified line number may not be larger than the last file line number."), true, KernelColorType.Error);
                        }
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Specified line number {0} is not a valid number."), true, KernelColorType.Error, ListArgsOnly[0]);
                        DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", ListArgsOnly[0]);
                    }
                }
            }
            else
            {
                foreach (string Line in TextEditShellCommon.TextEdit_FileLines)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Line number: {0} ({1})", LineNumber, Line);
                    TextWriterColor.Write("- {0}: ", false, KernelColorType.ListEntry, LineNumber);
                    TextWriterColor.Write(Line, true, KernelColorType.ListValue);
                    LineNumber += 1;
                }
            }
        }

    }
}
