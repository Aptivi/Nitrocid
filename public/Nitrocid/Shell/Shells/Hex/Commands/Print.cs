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
using Nitrocid.Files.Editors.HexEdit;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Misc.Reflection;
using Terminaux.Shell.Commands;
using System;
using Textify.General;

namespace Nitrocid.Shell.Shells.Hex.Commands
{
    /// <summary>
    /// Prints the contents of the file
    /// </summary>
    /// <remarks>
    /// Prints the contents of the file with bytes to the console. This is useful if you need to view the contents before and after editing.
    /// </remarks>
    class PrintCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            long ByteNumber;
            if (parameters.ArgumentsList.Length > 0)
            {
                if (parameters.ArgumentsList.Length == 1)
                {
                    // We've only provided one range
                    DebugWriter.WriteDebug(DebugLevel.I, "Byte number provided: {0}", vars: [parameters.ArgumentsList[0]]);
                    DebugWriter.WriteDebug(DebugLevel.I, "Is it numeric? {0}", vars: [TextTools.IsStringNumeric(parameters.ArgumentsList[0])]);
                    if (TextTools.IsStringNumeric(parameters.ArgumentsList[0]))
                    {
                        ByteNumber = Convert.ToInt64(parameters.ArgumentsList[0]);
                        HexEditTools.DisplayHex(ByteNumber);
                        return 0;
                    }
                    else
                    {
                        TextWriters.Write(Translate.DoTranslation("The byte number is not numeric."), true, KernelColorType.Error);
                        DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", vars: [parameters.ArgumentsList[0]]);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.HexEditor);
                    }
                }
                else
                {
                    // We've provided two Byte numbers in the range
                    DebugWriter.WriteDebug(DebugLevel.I, "Byte numbers provided: {0}, {1}", vars: [parameters.ArgumentsList[0], parameters.ArgumentsList[1]]);
                    DebugWriter.WriteDebug(DebugLevel.I, "Is it numeric? {0}", vars: [TextTools.IsStringNumeric(parameters.ArgumentsList[0]), TextTools.IsStringNumeric(parameters.ArgumentsList[1])]);
                    if (TextTools.IsStringNumeric(parameters.ArgumentsList[0]) & TextTools.IsStringNumeric(parameters.ArgumentsList[1]))
                    {
                        long ByteNumberStart = Convert.ToInt64(parameters.ArgumentsList[0]);
                        long ByteNumberEnd = Convert.ToInt64(parameters.ArgumentsList[1]);
                        ByteNumberStart.SwapIfSourceLarger(ref ByteNumberEnd);
                        HexEditTools.DisplayHex(ByteNumberStart, ByteNumberEnd);
                        return 0;
                    }
                    else
                    {
                        TextWriters.Write(Translate.DoTranslation("The byte number is not numeric."), true, KernelColorType.Error);
                        DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", vars: [parameters.ArgumentsList[0]]);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.HexEditor);
                    }
                }
            }
            else
            {
                HexEditTools.DisplayHex();
                return 0;
            }
        }

    }
}
