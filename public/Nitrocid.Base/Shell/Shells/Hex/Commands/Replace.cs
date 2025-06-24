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
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using Textify.General;
using Nitrocid.Base.Misc.Reflection;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Files.Editors.HexEdit;
using Nitrocid.Base.Kernel.Exceptions;

namespace Nitrocid.Base.Shell.Shells.Hex.Commands
{
    /// <summary>
    /// Replaces a byte with another one
    /// </summary>
    /// <remarks>
    /// You can use this command to replace a byte with another one.
    /// </remarks>
    class ReplaceCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var FileBytes = HexEditShellCommon.FileBytes ??
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
            if (parameters.ArgumentsList.Length == 2)
            {
                byte ByteFrom = Convert.ToByte(parameters.ArgumentsList[0], 16);
                byte ByteWith = Convert.ToByte(parameters.ArgumentsList[1], 16);
                HexEditTools.Replace(ByteFrom, ByteWith);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_HEX_REPLACE_SUCCESS"), true, ThemeColorType.Success);
                return 0;
            }
            else if (parameters.ArgumentsList.Length == 3)
            {
                if (TextTools.IsStringNumeric(parameters.ArgumentsList[2]))
                {
                    if (Convert.ToInt64(parameters.ArgumentsList[2]) <= FileBytes.LongLength)
                    {
                        byte ByteFrom = Convert.ToByte(parameters.ArgumentsList[0], 16);
                        byte ByteWith = Convert.ToByte(parameters.ArgumentsList[1], 16);
                        HexEditTools.Replace(ByteFrom, ByteWith, Convert.ToInt64(parameters.ArgumentsList[2]));
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_HEX_REPLACE_SUCCESS"), true, ThemeColorType.Success);
                        return 0;
                    }
                    else
                    {
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_BYTENUMTOOLARGE"), true, ThemeColorType.Error);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.HexEditor);
                    }
                }
            }
            else if (parameters.ArgumentsList.Length > 3)
            {
                if (TextTools.IsStringNumeric(parameters.ArgumentsList[2]) & TextTools.IsStringNumeric(parameters.ArgumentsList[3]))
                {
                    if (Convert.ToInt64(parameters.ArgumentsList[2]) <= FileBytes.LongLength & Convert.ToInt64(parameters.ArgumentsList[3]) <= FileBytes.LongLength)
                    {
                        byte ByteFrom = Convert.ToByte(parameters.ArgumentsList[0], 16);
                        byte ByteWith = Convert.ToByte(parameters.ArgumentsList[1], 16);
                        long ByteNumberStart = Convert.ToInt64(parameters.ArgumentsList[2]);
                        long ByteNumberEnd = Convert.ToInt64(parameters.ArgumentsList[3]);
                        ByteNumberStart.SwapIfSourceLarger(ref ByteNumberEnd);
                        HexEditTools.Replace(ByteFrom, ByteWith, ByteNumberStart, ByteNumberEnd);
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_HEX_REPLACE_SUCCESS"), true, ThemeColorType.Success);
                        return 0;
                    }
                    else
                    {
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_BYTENUMTOOLARGE"), true, ThemeColorType.Error);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.HexEditor);
                    }
                }
            }
            return 0;
        }

    }
}
