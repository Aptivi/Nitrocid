
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
    /// Removes the specified line number
    /// </summary>
    /// <remarks>
    /// You can use this command to remove a specified line by number. You can use the print command to take a look at the unneeded line and its number.
    /// </remarks>
    class TextEdit_DelLineCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (ListArgsOnly.Length == 1)
            {
                if (StringQuery.IsStringNumeric(ListArgsOnly[0]))
                {
                    if (Convert.ToInt32(ListArgsOnly[0]) <= TextEditShellCommon.TextEdit_FileLines.Count)
                    {
                        TextEditTools.TextEdit_RemoveLine(Convert.ToInt32(ListArgsOnly[0]));
                        TextWriterColor.Write(Translate.DoTranslation("Removed line."), true, KernelColorType.Success);
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
            else if (ListArgsOnly.Length > 1)
            {
                if (StringQuery.IsStringNumeric(ListArgsOnly[0]) & StringQuery.IsStringNumeric(ListArgsOnly[1]))
                {
                    if (Convert.ToInt32(ListArgsOnly[0]) <= TextEditShellCommon.TextEdit_FileLines.Count & Convert.ToInt32(ListArgsOnly[1]) <= TextEditShellCommon.TextEdit_FileLines.Count)
                    {
                        int LineNumberStart = Convert.ToInt32(ListArgsOnly[0]);
                        int LineNumberEnd = Convert.ToInt32(ListArgsOnly[1]);
                        LineNumberStart.SwapIfSourceLarger(ref LineNumberEnd);
                        for (int LineNumber = LineNumberStart; LineNumber <= LineNumberEnd; LineNumber++)
                        {
                            TextEditTools.TextEdit_RemoveLine(LineNumber);
                            TextWriterColor.Write(Translate.DoTranslation("Removed line number {0}."), true, KernelColorType.Success, LineNumber);
                        }
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("The specified line number may not be larger than the last file line number."), true, KernelColorType.Error);
                    }
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("Specified line number {0} is not a valid number."), true, KernelColorType.Error, ListArgsOnly[1]);
                    DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", ListArgsOnly[1]);
                }
            }
        }

    }
}
