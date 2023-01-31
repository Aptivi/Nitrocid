
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
using Extensification.LongExts;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Editors.HexEdit;
using KS.Misc.Reflection;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.Hex.Commands
{
    /// <summary>
    /// Replaces a byte with another one
    /// </summary>
    /// <remarks>
    /// You can use this command to replace a byte with another one.
    /// </remarks>
    class HexEdit_ReplaceCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (ListArgsOnly.Length == 2)
            {
                byte ByteFrom = Convert.ToByte(ListArgsOnly[0], 16);
                byte ByteWith = Convert.ToByte(ListArgsOnly[1], 16);
                HexEditTools.HexEdit_Replace(ByteFrom, ByteWith);
                TextWriterColor.Write(Translate.DoTranslation("Byte replaced."), true, KernelColorType.Success);
            }
            else if (ListArgsOnly.Length == 3)
            {
                if (StringQuery.IsStringNumeric(ListArgsOnly[2]))
                {
                    if (Convert.ToInt64(ListArgsOnly[2]) <= HexEditShellCommon.HexEdit_FileBytes.LongLength)
                    {
                        byte ByteFrom = Convert.ToByte(ListArgsOnly[0], 16);
                        byte ByteWith = Convert.ToByte(ListArgsOnly[1], 16);
                        HexEditTools.HexEdit_Replace(ByteFrom, ByteWith, Convert.ToInt64(ListArgsOnly[2]));
                        TextWriterColor.Write(Translate.DoTranslation("Byte replaced."), true, KernelColorType.Success);
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("The specified byte number may not be larger than the file size."), true, KernelColorType.Error);
                    }
                }
            }
            else if (ListArgsOnly.Length > 3)
            {
                if (StringQuery.IsStringNumeric(ListArgsOnly[2]) & StringQuery.IsStringNumeric(ListArgsOnly[3]))
                {
                    if (Convert.ToInt64(ListArgsOnly[2]) <= HexEditShellCommon.HexEdit_FileBytes.LongLength & Convert.ToInt64(ListArgsOnly[3]) <= HexEditShellCommon.HexEdit_FileBytes.LongLength)
                    {
                        byte ByteFrom = Convert.ToByte(ListArgsOnly[0], 16);
                        byte ByteWith = Convert.ToByte(ListArgsOnly[1], 16);
                        long ByteNumberStart = Convert.ToInt64(ListArgsOnly[2]);
                        long ByteNumberEnd = Convert.ToInt64(ListArgsOnly[3]);
                        ByteNumberStart.SwapIfSourceLarger(ref ByteNumberEnd);
                        HexEditTools.HexEdit_Replace(ByteFrom, ByteWith, ByteNumberStart, ByteNumberEnd);
                        TextWriterColor.Write(Translate.DoTranslation("Byte replaced."), true, KernelColorType.Success);
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("The specified byte number may not be larger than the file size."), true, KernelColorType.Error);
                    }
                }
            }
        }

    }
}