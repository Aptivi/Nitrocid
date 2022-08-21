using System;

// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using Extensification.LongExts;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Editors.HexEdit;
using KS.Misc.Reflection;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;
using Microsoft.VisualBasic.CompilerServices;

namespace KS.Shell.Shells.Hex.Commands
{
    /// <summary>
    /// Queries a byte in a specified byte, a range of bytes, or entirely
    /// </summary>
    /// <remarks>
    /// You can use this command to query a byte and get its number from the specified byte, a range of bytes, or entirely.
    /// </remarks>
    class HexEdit_QueryByteCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (ListArgsOnly.Length == 1)
            {
                byte ByteContent = Convert.ToByte(ListArgsOnly[0], 16);
                HexEditTools.HexEdit_QueryByteAndDisplay(ByteContent);
            }
            else if (ListArgsOnly.Length == 2)
            {
                if (StringQuery.IsStringNumeric(ListArgsOnly[1]))
                {
                    if (Conversions.ToLong(ListArgsOnly[1]) <= HexEditShellCommon.HexEdit_FileBytes.LongLength)
                    {
                        byte ByteContent = Convert.ToByte(ListArgsOnly[0], 16);
                        HexEditTools.HexEdit_QueryByteAndDisplay(ByteContent, Conversions.ToLong(ListArgsOnly[1]));
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("The specified byte number may not be larger than the file size."), true, ColorTools.ColTypes.Error);
                    }
                }
            }
            else if (ListArgsOnly.Length > 2)
            {
                if (StringQuery.IsStringNumeric(ListArgsOnly[1]) & StringQuery.IsStringNumeric(ListArgsOnly[2]))
                {
                    if (Conversions.ToLong(ListArgsOnly[1]) <= HexEditShellCommon.HexEdit_FileBytes.LongLength & Conversions.ToLong(ListArgsOnly[2]) <= HexEditShellCommon.HexEdit_FileBytes.LongLength)
                    {
                        byte ByteContent = Convert.ToByte(ListArgsOnly[0], 16);
                        long ByteNumberStart = Conversions.ToLong(ListArgsOnly[1]);
                        long ByteNumberEnd = Conversions.ToLong(ListArgsOnly[2]);
                        ByteNumberStart.SwapIfSourceLarger(ref ByteNumberEnd);
                        HexEditTools.HexEdit_QueryByteAndDisplay(ByteContent, ByteNumberStart, ByteNumberEnd);
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("The specified byte number may not be larger than the file size."), true, ColorTools.ColTypes.Error);
                    }
                }
            }
        }

    }
}