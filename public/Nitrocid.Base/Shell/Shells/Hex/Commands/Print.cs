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

using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using System;
using Textify.General;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Reflection;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Exceptions;
using Terminaux.Shell.Shells;

namespace Nitrocid.Base.Shell.Shells.Hex.Commands
{
    /// <summary>
    /// Prints the contents of the file
    /// </summary>
    /// <remarks>
    /// Prints the contents of the file with bytes to the console. This is useful if you need to view the contents before and after editing.
    /// </remarks>
    class PrintCommand : BaseCommand, ICommand
    {

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            var hexShell = (HexShell?)shell ??
                throw new KernelException(KernelExceptionType.Archive, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_LASTSHELLTYPEMISMATCH"));
            long ByteNumber;
            if (parameters.ArgumentsList.Length > 0)
            {
                if (parameters.ArgumentsList.Length == 1)
                {
                    string startByteStr = parameters.ArgumentsList[0];

                    // We've only provided one range
                    DebugWriter.WriteDebug(DebugLevel.I, "Byte number provided: {0}", vars: [startByteStr]);
                    DebugWriter.WriteDebug(DebugLevel.I, "Is it numeric? {0}", vars: [TextTools.IsStringNumeric(startByteStr)]);
                    if (TextTools.IsStringNumeric(startByteStr))
                    {
                        ByteNumber = Convert.ToInt64(startByteStr);
                        hexShell.DisplayHex(ByteNumber);
                        return 0;
                    }
                    else
                    {
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_HEX_COMMON_NOTNUMERIC"), true, ThemeColorType.Error);
                        DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", vars: [startByteStr]);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.HexEditor);
                    }
                }
                else
                {
                    string startByteStr = parameters.ArgumentsList[0];
                    string endByteStr = parameters.ArgumentsList[1];

                    // We've provided two Byte numbers in the range
                    DebugWriter.WriteDebug(DebugLevel.I, "Byte numbers provided: {0}, {1}", vars: [startByteStr, endByteStr]);
                    DebugWriter.WriteDebug(DebugLevel.I, "Is it numeric? {0}", vars: [TextTools.IsStringNumeric(startByteStr), TextTools.IsStringNumeric(endByteStr)]);
                    if (TextTools.IsStringNumeric(startByteStr) & TextTools.IsStringNumeric(endByteStr))
                    {
                        long ByteNumberStart = Convert.ToInt64(startByteStr);
                        long ByteNumberEnd = Convert.ToInt64(endByteStr);
                        ByteNumberStart.SwapIfSourceLarger(ref ByteNumberEnd);
                        hexShell.DisplayHex(ByteNumberStart, ByteNumberEnd);
                        return 0;
                    }
                    else
                    {
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_HEX_COMMON_NOTNUMERIC"), true, ThemeColorType.Error);
                        DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", vars: [startByteStr]);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.HexEditor);
                    }
                }
            }
            else
            {
                hexShell.DisplayHex();
                return 0;
            }
        }

    }
}
