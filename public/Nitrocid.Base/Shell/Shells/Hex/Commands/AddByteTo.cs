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
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Files.Editors.HexEdit;
using Nitrocid.Base.Kernel.Exceptions;

namespace Nitrocid.Base.Shell.Shells.Hex.Commands
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
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
                if (Convert.ToInt32(parameters.ArgumentsList[1]) <= FileBytes.LongLength)
                {
                    HexEditTools.AddNewByte(ByteContent, Convert.ToInt64(parameters.ArgumentsList[1]));
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_HEX_DELBYTE_SUCCESS"), true, ThemeColorType.Success);
                    return 0;
                }
                else
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_BYTENUMTOOLARGE"), true, ThemeColorType.Error);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.HexEditor);
                }
            }
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_HEX_COMMON_NOTNUMERIC"), true, ThemeColorType.Error);
                DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", vars: [parameters.ArgumentsList[1]]);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.HexEditor);
            }
        }

    }
}
