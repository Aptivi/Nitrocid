
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
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Misc.Text;
using KS.Shell.ShellBase.Commands;
using System;

namespace KS.Shell.Shells.Text.Commands
{
    /// <summary>
    /// Deletes a word or phrase from line number
    /// </summary>
    /// <remarks>
    /// You can use this command to remove an extraneous word or phrase enclosed in double quotes in a specified line number. You can use the print command to review the changes and line numbers.
    /// </remarks>
    class TextEdit_DelWordCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, ref string variableValue)
        {
            if (ListArgsOnly.Length == 2)
            {
                if (TextTools.IsStringNumeric(ListArgsOnly[1]))
                {
                    if (Convert.ToInt32(ListArgsOnly[1]) <= TextEditShellCommon.TextEdit_FileLines.Count)
                    {
                        TextEditTools.TextEdit_DeleteWord(ListArgsOnly[0], Convert.ToInt32(ListArgsOnly[1]));
                        TextWriterColor.Write(Translate.DoTranslation("Word deleted."), true, KernelColorType.Success);
                        return 0;
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("The specified line number may not be larger than the last file line number."), true, KernelColorType.Error);
                        return 10000 + (int)KernelExceptionType.TextEditor;
                    }
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("Specified line number {0} is not a valid number."), true, KernelColorType.Error, ListArgsOnly[1]);
                    DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", ListArgsOnly[1]);
                    return 10000 + (int)KernelExceptionType.TextEditor;
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
                            TextEditTools.TextEdit_DeleteWord(ListArgsOnly[0], LineNumber);
                            TextWriterColor.Write(Translate.DoTranslation("Word deleted in line {0}."), true, KernelColorType.Success, LineNumber);
                        }
                        return 0;
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("The specified line number may not be larger than the last file line number."), true, KernelColorType.Error);
                        return 10000 + (int)KernelExceptionType.TextEditor;
                    }
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("Specified line number {0} is not a valid number."), true, KernelColorType.Error, ListArgsOnly[1]);
                    DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", ListArgsOnly[1]);
                    return 10000 + (int)KernelExceptionType.TextEditor;
                }
            }
            return 0;
        }

    }
}
