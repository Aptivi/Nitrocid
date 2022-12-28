
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
using KS.Misc.Editors.TextEdit;
using KS.Misc.Reflection;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;
using System;

namespace KS.Shell.Shells.Text.Commands
{
    /// <summary>
    /// Replaces a word or phrase with another one in a line
    /// </summary>
    /// <remarks>
    /// You can use this command to replace a word or a complete phrase enclosed in double quotes with another one (enclosed in double quotes again) in a line.
    /// </remarks>
    class TextEdit_ReplaceInlineCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (ListArgsOnly.Length == 3)
            {
                if (StringQuery.IsStringNumeric(ListArgsOnly[2]))
                {
                    if (Convert.ToInt32(ListArgsOnly[2]) <= TextEditShellCommon.TextEdit_FileLines.Count)
                    {
                        TextEditTools.TextEdit_Replace(ListArgsOnly[0], ListArgsOnly[1], Convert.ToInt32(ListArgsOnly[2]));
                        TextWriterColor.Write(Translate.DoTranslation("String replaced."), true, KernelColorType.Success);
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("The specified line number may not be larger than the last file line number."), true, KernelColorType.Error);
                    }
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("Specified line number {0} is not a valid number."), true, KernelColorType.Error, ListArgsOnly[2]);
                    DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", ListArgsOnly[2]);
                }
            }
            else if (ListArgsOnly.Length > 3)
            {
                if (StringQuery.IsStringNumeric(ListArgsOnly[2]) & StringQuery.IsStringNumeric(ListArgsOnly[3]))
                {
                    if (Convert.ToInt32(ListArgsOnly[2]) <= TextEditShellCommon.TextEdit_FileLines.Count & Convert.ToInt32(ListArgsOnly[3]) <= TextEditShellCommon.TextEdit_FileLines.Count)
                    {
                        int LineNumberStart = Convert.ToInt32(ListArgsOnly[2]);
                        int LineNumberEnd = Convert.ToInt32(ListArgsOnly[3]);
                        LineNumberStart.SwapIfSourceLarger(ref LineNumberEnd);
                        for (int LineNumber = LineNumberStart; LineNumber <= LineNumberEnd; LineNumber++)
                        {
                            TextEditTools.TextEdit_Replace(ListArgsOnly[0], ListArgsOnly[1], LineNumber);
                            TextWriterColor.Write(Translate.DoTranslation("String replaced in line {0}."), true, KernelColorType.Success, LineNumber);
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
