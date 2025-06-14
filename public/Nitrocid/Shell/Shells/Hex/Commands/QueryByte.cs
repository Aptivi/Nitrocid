//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Misc.Reflection;
using Nitrocid.Shell.ShellBase.Commands;
using Textify.General;

namespace Nitrocid.Shell.Shells.Hex.Commands
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
            var FileBytes = HexEditShellCommon.FileBytes ??
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
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
                    if (Convert.ToInt64(parameters.ArgumentsList[1]) <= FileBytes.LongLength)
                    {
                        byte ByteContent = Convert.ToByte(parameters.ArgumentsList[0], 16);
                        HexEditTools.QueryByteAndDisplay(ByteContent, Convert.ToInt64(parameters.ArgumentsList[1]));
                        return 0;
                    }
                    else
                    {
                        TextWriters.Write(LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_BYTENUMTOOLARGE"), true, KernelColorType.Error);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.HexEditor);
                    }
                }
            }
            else if (parameters.ArgumentsList.Length > 2)
            {
                if (TextTools.IsStringNumeric(parameters.ArgumentsList[1]) & TextTools.IsStringNumeric(parameters.ArgumentsList[2]))
                {
                    if (Convert.ToInt64(parameters.ArgumentsList[1]) <= FileBytes.LongLength & Convert.ToInt64(parameters.ArgumentsList[2]) <= HexEditShellCommon.FileBytes.LongLength)
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
                        TextWriters.Write(LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_BYTENUMTOOLARGE"), true, KernelColorType.Error);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.HexEditor);
                    }
                }
            }
            return 0;
        }

    }
}
