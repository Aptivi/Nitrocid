
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
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Editors.HexEdit;
using KS.Misc.Text;
using KS.Shell.ShellBase.Commands;
using System;

namespace KS.Shell.Shells.Hex.Commands
{
    /// <summary>
    /// Removes the specified byte number
    /// </summary>
    /// <remarks>
    /// You can use this command to remove a specified byte by number. You can use the print command to take a look at the unneeded byte and its number.
    /// </remarks>
    class HexEdit_DelByteCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, ref string variableValue)
        {
            if (TextTools.IsStringNumeric(ListArgsOnly[0]))
            {
                if (Convert.ToInt32(ListArgsOnly[0]) <= HexEditShellCommon.HexEdit_FileBytes.LongLength)
                {
                    HexEditTools.HexEdit_DeleteByte(Convert.ToInt64(ListArgsOnly[0]));
                    TextWriterColor.Write(Translate.DoTranslation("Byte deleted."), true, KernelColorType.Success);
                    return 0;
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("The specified byte number may not be larger than the file size."), true, KernelColorType.Error);
                    return 10000 + (int)KernelExceptionType.HexEditor;
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("The byte number is not numeric."), true, KernelColorType.Error);
                DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", ListArgsOnly[0]);
                return 10000 + (int)KernelExceptionType.HexEditor;
            }
        }

    }
}
