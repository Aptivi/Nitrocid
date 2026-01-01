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

using System;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Files.Editors.HexEdit;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using Textify.General;

namespace Nitrocid.Shell.Shells.Hex.Commands
{
    /// <summary>
    /// Adds a new byte to a specified position
    /// </summary>
    /// <remarks>
    /// You can use this command to add a new byte to the specified position.
    /// </remarks>
    class AddByteToCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            byte ByteContent = Convert.ToByte(parameters.ArgumentsList[0], 16);
            if (TextTools.IsStringNumeric(parameters.ArgumentsList[1]))
            {
                var FileBytes = HexEditShellCommon.FileBytes ??
                    throw new KernelException(KernelExceptionType.HexEditor, Translate.DoTranslation("Hex file is not open yet."));
                if (Convert.ToInt32(parameters.ArgumentsList[1]) <= FileBytes.LongLength)
                {
                    HexEditTools.AddNewByte(ByteContent, Convert.ToInt64(parameters.ArgumentsList[1]));
                    TextWriters.Write(Translate.DoTranslation("Byte deleted."), true, KernelColorType.Success);
                    return 0;
                }
                else
                {
                    TextWriters.Write(Translate.DoTranslation("The specified byte number may not be larger than the file size."), true, KernelColorType.Error);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.HexEditor);
                }
            }
            else
            {
                TextWriters.Write(Translate.DoTranslation("The byte number is not numeric."), true, KernelColorType.Error);
                DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", vars: [parameters.ArgumentsList[1]]);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.HexEditor);
            }
        }

    }
}
