//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;

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
        public override string Command =>
            "addbyteto";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_HEX_COMMAND_ADDBYTESTO_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "byte", new()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_HEX_ADDBYTE_ARGUMENT_BYTE_DESC"
                    }),
                    new CommandArgumentPart(true, "pos", new()
                    {
                        IsNumeric = true,
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_HEX_ADDBYTETO_ARGUMENT_BYTEPOS_DESC"
                    })
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string byteStr = parameters.ArgumentsList[0];
            string posStr = parameters.ArgumentsList[1];

            var hexShell = (HexShell?)shell ??
                throw new KernelException(KernelExceptionType.Archive, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_LASTSHELLTYPEMISMATCH"));
            byte ByteContent = Convert.ToByte(byteStr, 16);
            if (TextTools.IsStringNumeric(posStr))
            {
                var FileBytes = hexShell.FileBytes ??
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
                if (Convert.ToInt32(posStr) <= FileBytes.LongLength)
                {
                    hexShell.AddNewByte(ByteContent, Convert.ToInt64(posStr));
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
                DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", vars: [posStr]);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.HexEditor);
            }
        }

    }
}
