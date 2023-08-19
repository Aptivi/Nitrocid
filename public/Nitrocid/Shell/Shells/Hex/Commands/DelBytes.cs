
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
using KS.Misc.Reflection;
using KS.Misc.Text;
using KS.Shell.ShellBase.Commands;
using System;

namespace KS.Shell.Shells.Hex.Commands
{
    /// <summary>
    /// Deletes the bytes
    /// </summary>
    /// <remarks>
    /// You can use this command to remove a extraneous bytes in a specified range. You can use the print command to review the changes.
    /// </remarks>
    class HexEdit_DelBytesCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, ref string variableValue)
        {
            if (ListArgsOnly.Length == 1)
            {
                if (TextTools.IsStringNumeric(ListArgsOnly[0]))
                {
                    if (Convert.ToInt64(ListArgsOnly[0]) <= HexEditShellCommon.HexEdit_FileBytes.LongLength)
                    {
                        HexEditTools.HexEdit_DeleteBytes(Convert.ToInt64(ListArgsOnly[0]));
                        TextWriterColor.Write(Translate.DoTranslation("Deleted bytes."), true, KernelColorType.Success);
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
                    TextWriterColor.Write(Translate.DoTranslation("Specified Byte number {0} is not a valid number."), true, KernelColorType.Error, ListArgsOnly[0]);
                    DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", ListArgsOnly[0]);
                    return 10000 + (int)KernelExceptionType.HexEditor;
                }
            }
            else if (ListArgsOnly.Length > 1)
            {
                if (TextTools.IsStringNumeric(ListArgsOnly[0]) & TextTools.IsStringNumeric(ListArgsOnly[1]))
                {
                    if (Convert.ToInt64(ListArgsOnly[0]) <= HexEditShellCommon.HexEdit_FileBytes.LongLength & Convert.ToInt64(ListArgsOnly[1]) <= HexEditShellCommon.HexEdit_FileBytes.LongLength)
                    {
                        long ByteNumberStart = Convert.ToInt64(ListArgsOnly[0]);
                        long ByteNumberEnd = Convert.ToInt64(ListArgsOnly[1]);
                        ByteNumberStart.SwapIfSourceLarger(ref ByteNumberEnd);
                        HexEditTools.HexEdit_DeleteBytes(ByteNumberStart, ByteNumberEnd);
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
                    DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", ListArgsOnly[1]);
                    return 10000 + (int)KernelExceptionType.HexEditor;
                }
            }
            return 0;
        }

    }
}
