
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

using System;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Text;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.Text.Commands
{
    /// <summary>
    /// Edits a line
    /// </summary>
    /// <remarks>
    /// You can use this command to edit a line seamlessly.
    /// </remarks>
    class EditLineCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (TextTools.IsStringNumeric(parameters.ArgumentsList[0]))
            {
                int lineNum = Convert.ToInt32(parameters.ArgumentsList[0]);
                if (lineNum <= TextEditShellCommon.FileLines.Count)
                {
                    string OriginalLine = TextEditShellCommon.FileLines[lineNum - 1];
                    TextWriterColor.WriteKernelColor(">> ", false, KernelColorType.Input);
                    string EditedLine = Input.ReadLine("", OriginalLine);
                    TextEditShellCommon.FileLines[lineNum - 1] = EditedLine;
                    return 0;
                }
                else
                {
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("The specified line number may not be larger than the last file line number."), true, KernelColorType.Error);
                    return 10000 + (int)KernelExceptionType.TextEditor;
                }
            }
            else
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Specified line number {0} is not a valid number."), true, KernelColorType.Error);
                DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", parameters.ArgumentsList[0]);
                return 10000 + (int)KernelExceptionType.TextEditor;
            }
        }

    }
}
