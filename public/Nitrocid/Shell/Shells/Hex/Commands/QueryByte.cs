//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Misc.Text;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.Hex.Commands
{
    /// <summary>
    /// Queries a byte in a specified byte, a range of bytes, or entirely
    /// </summary>
    /// <remarks>
    /// You can use this command to query a byte and get its number from the specified byte, a range of bytes, or entirely.
    /// </remarks>
    class QueryByteCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (parameters.ArgumentsList.Length == 1)
            {
                byte ByteContent = Convert.ToByte(parameters.ArgumentsList[0], 16);
                HexEditTools.QueryByteAndDisplay(ByteContent);
                return 0;
            }
            else if (parameters.ArgumentsList.Length == 2)
            {
                if (TextTools.IsStringNumeric(parameters.ArgumentsList[1]))
                {
                    if (Convert.ToInt64(parameters.ArgumentsList[1]) <= HexEditShellCommon.FileBytes.LongLength)
                    {
                        byte ByteContent = Convert.ToByte(parameters.ArgumentsList[0], 16);
                        HexEditTools.QueryByteAndDisplay(ByteContent, Convert.ToInt64(parameters.ArgumentsList[1]));
                        return 0;
                    }
                    else
                    {
                        TextWriterColor.WriteKernelColor(Translate.DoTranslation("The specified byte number may not be larger than the file size."), true, KernelColorType.Error);
                        return 10000 + (int)KernelExceptionType.HexEditor;
                    }
                }
            }
            else if (parameters.ArgumentsList.Length > 2)
            {
                if (TextTools.IsStringNumeric(parameters.ArgumentsList[1]) & TextTools.IsStringNumeric(parameters.ArgumentsList[2]))
                {
                    if (Convert.ToInt64(parameters.ArgumentsList[1]) <= HexEditShellCommon.FileBytes.LongLength & Convert.ToInt64(parameters.ArgumentsList[2]) <= HexEditShellCommon.FileBytes.LongLength)
                    {
                        byte ByteContent = Convert.ToByte(parameters.ArgumentsList[0], 16);
                        long ByteNumberStart = Convert.ToInt64(parameters.ArgumentsList[1]);
                        long ByteNumberEnd = Convert.ToInt64(parameters.ArgumentsList[2]);
                        ByteNumberStart.SwapIfSourceLarger(ref ByteNumberEnd);
                        HexEditTools.QueryByteAndDisplay(ByteContent, ByteNumberStart, ByteNumberEnd);
                        return 0;
                    }
                    else
                    {
                        TextWriterColor.WriteKernelColor(Translate.DoTranslation("The specified byte number may not be larger than the file size."), true, KernelColorType.Error);
                        return 10000 + (int)KernelExceptionType.HexEditor;
                    }
                }
            }
            return 0;
        }

    }
}
