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

using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using System;
using Textify.General;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Files.Editors.HexEdit;
using Nitrocid.Base.Kernel.Exceptions;

namespace Nitrocid.Base.Shell.Shells.Hex.Commands
{
    /// <summary>
    /// Removes the specified byte number
    /// </summary>
    /// <remarks>
    /// You can use this command to remove a specified byte by number. You can use the print command to take a look at the unneeded byte and its number.
    /// </remarks>
    class DelByteCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (TextTools.IsStringNumeric(parameters.ArgumentsList[0]))
            {
                var FileBytes = HexEditShellCommon.FileBytes ??
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
                if (Convert.ToInt32(parameters.ArgumentsList[0]) <= FileBytes.LongLength)
                {
                    HexEditTools.DeleteByte(Convert.ToInt64(parameters.ArgumentsList[0]));
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
                DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", vars: [parameters.ArgumentsList[0]]);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.HexEditor);
            }
        }

    }
}
