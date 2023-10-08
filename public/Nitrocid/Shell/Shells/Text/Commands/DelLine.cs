
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
    /// Removes the specified line number
    /// </summary>
    /// <remarks>
    /// You can use this command to remove a specified line by number. You can use the print command to take a look at the unneeded line and its number.
    /// </remarks>
    class TextEdit_DelLineCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (parameters.ArgumentsList.Length == 1)
            {
                if (TextTools.IsStringNumeric(parameters.ArgumentsList[0]))
                {
                    if (Convert.ToInt32(parameters.ArgumentsList[0]) <= TextEditShellCommon.TextEdit_FileLines.Count)
                    {
                        TextEditTools.TextEdit_RemoveLine(Convert.ToInt32(parameters.ArgumentsList[0]));
                        TextWriterColor.WriteKernelColor(Translate.DoTranslation("Removed line."), true, KernelColorType.Success);
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
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("Specified line number {0} is not a valid number."), true, KernelColorType.Error, parameters.ArgumentsList[0]);
                    DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", parameters.ArgumentsList[0]);
                    return 10000 + (int)KernelExceptionType.TextEditor;
                }
            }
            else if (parameters.ArgumentsList.Length > 1)
            {
                if (TextTools.IsStringNumeric(parameters.ArgumentsList[0]) & TextTools.IsStringNumeric(parameters.ArgumentsList[1]))
                {
                    if (Convert.ToInt32(parameters.ArgumentsList[0]) <= TextEditShellCommon.TextEdit_FileLines.Count & Convert.ToInt32(parameters.ArgumentsList[1]) <= TextEditShellCommon.TextEdit_FileLines.Count)
                    {
                        int LineNumberStart = Convert.ToInt32(parameters.ArgumentsList[0]);
                        int LineNumberEnd = Convert.ToInt32(parameters.ArgumentsList[1]);
                        LineNumberStart.SwapIfSourceLarger(ref LineNumberEnd);
                        for (int LineNumber = LineNumberStart; LineNumber <= LineNumberEnd; LineNumber++)
                        {
                            TextEditTools.TextEdit_RemoveLine(LineNumber);
                            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Removed line number {0}."), true, KernelColorType.Success, LineNumber);
                        }
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
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("Specified line number {0} is not a valid number."), true, KernelColorType.Error, parameters.ArgumentsList[1]);
                    DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", parameters.ArgumentsList[1]);
                    return 10000 + (int)KernelExceptionType.TextEditor;
                }
            }
            return 0;
        }

    }
}
