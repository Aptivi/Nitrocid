//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Files.Editors.HexEdit;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Text;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.Hex.Commands
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
                if (Convert.ToInt32(parameters.ArgumentsList[1]) <= HexEditShellCommon.FileBytes.LongLength)
                {
                    HexEditTools.AddNewByte(ByteContent, Convert.ToInt64(parameters.ArgumentsList[1]));
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("Byte deleted."), true, KernelColorType.Success);
                    return 0;
                }
                else
                {
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("The specified byte number may not be larger than the file size."), true, KernelColorType.Error);
                    return 10000 + (int)KernelExceptionType.HexEditor;
                }
            }
            else
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("The byte number is not numeric."), true, KernelColorType.Error);
                DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", parameters.ArgumentsList[1]);
                return 10000 + (int)KernelExceptionType.HexEditor;
            }
        }

    }
}
