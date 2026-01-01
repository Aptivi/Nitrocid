//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Misc.Reflection;
using Nitrocid.Shell.ShellBase.Commands;
using System;
using Textify.General;

namespace Nitrocid.Shell.Shells.Text.Commands
{
    /// <summary>
    /// Prints the contents of the file
    /// </summary>
    /// <remarks>
    /// Prints the contents of the file with line numbers to the console. This is useful if you need to view the contents before and after editing.
    /// </remarks>
    class PrintCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            int LineNumber = 1;
            if (parameters.ArgumentsList.Length > 0)
            {
                if (parameters.ArgumentsList.Length == 1)
                {
                    // We've only provided one line number
                    DebugWriter.WriteDebug(DebugLevel.I, "Line number provided: {0}", parameters.ArgumentsList[0]);
                    DebugWriter.WriteDebug(DebugLevel.I, "Is it numeric? {0}", TextTools.IsStringNumeric(parameters.ArgumentsList[0]));
                    if (TextTools.IsStringNumeric(parameters.ArgumentsList[0]))
                    {
                        LineNumber = Convert.ToInt32(parameters.ArgumentsList[0]);
                        DebugWriter.WriteDebug(DebugLevel.I, "File lines: {0}", TextEditShellCommon.FileLines.Count);
                        if (Convert.ToInt32(parameters.ArgumentsList[0]) <= TextEditShellCommon.FileLines.Count)
                        {
                            string Line = TextEditShellCommon.FileLines[LineNumber - 1];
                            DebugWriter.WriteDebug(DebugLevel.I, "Line number: {0} ({1})", LineNumber, Line);
                            TextWriters.Write("- {0}: ", false, KernelColorType.ListEntry, LineNumber);
                            TextWriters.Write(Line, true, KernelColorType.ListValue);
                            return 0;
                        }
                        else
                        {
                            TextWriters.Write(Translate.DoTranslation("The specified line number may not be larger than the last file line number."), true, KernelColorType.Error);
                            return KernelExceptionTools.GetErrorCode(KernelExceptionType.TextEditor);
                        }
                    }
                    else
                    {
                        TextWriters.Write(Translate.DoTranslation("Specified line number {0} is not a valid number."), true, KernelColorType.Error, parameters.ArgumentsList[0]);
                        DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", parameters.ArgumentsList[0]);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.TextEditor);
                    }
                }
                else
                {
                    // We've provided two line numbers in the range
                    DebugWriter.WriteDebug(DebugLevel.I, "Line numbers provided: {0}, {1}", parameters.ArgumentsList[0], parameters.ArgumentsList[1]);
                    DebugWriter.WriteDebug(DebugLevel.I, "Is it numeric? {0}", TextTools.IsStringNumeric(parameters.ArgumentsList[0]), TextTools.IsStringNumeric(parameters.ArgumentsList[1]));
                    if (TextTools.IsStringNumeric(parameters.ArgumentsList[0]) & TextTools.IsStringNumeric(parameters.ArgumentsList[1]))
                    {
                        int LineNumberStart = Convert.ToInt32(parameters.ArgumentsList[0]);
                        int LineNumberEnd = Convert.ToInt32(parameters.ArgumentsList[1]);
                        LineNumberStart.SwapIfSourceLarger(ref LineNumberEnd);
                        DebugWriter.WriteDebug(DebugLevel.I, "File lines: {0}", TextEditShellCommon.FileLines.Count);
                        if (LineNumberStart <= TextEditShellCommon.FileLines.Count & LineNumberEnd <= TextEditShellCommon.FileLines.Count)
                        {
                            for (LineNumber = LineNumberStart; LineNumber <= LineNumberEnd; LineNumber++)
                            {
                                string Line = TextEditShellCommon.FileLines[LineNumber - 1];
                                DebugWriter.WriteDebug(DebugLevel.I, "Line number: {0} ({1})", LineNumber, Line);
                                TextWriters.Write("- {0}: ", false, KernelColorType.ListEntry, LineNumber);
                                TextWriters.Write(Line, true, KernelColorType.ListValue);
                            }
                            return 0;
                        }
                        else
                        {
                            TextWriters.Write(Translate.DoTranslation("The specified line number may not be larger than the last file line number."), true, KernelColorType.Error);
                            return KernelExceptionTools.GetErrorCode(KernelExceptionType.TextEditor);
                        }
                    }
                    else
                    {
                        TextWriters.Write(Translate.DoTranslation("Specified line number {0} is not a valid number."), true, KernelColorType.Error, parameters.ArgumentsList[0]);
                        DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", parameters.ArgumentsList[0]);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.TextEditor);
                    }
                }
            }
            else
            {
                foreach (string Line in TextEditShellCommon.FileLines)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Line number: {0} ({1})", LineNumber, Line);
                    TextWriters.Write("- {0}: ", false, KernelColorType.ListEntry, LineNumber);
                    TextWriters.Write(Line, true, KernelColorType.ListValue);
                    LineNumber += 1;
                }
                return 0;
            }
        }

    }
}
