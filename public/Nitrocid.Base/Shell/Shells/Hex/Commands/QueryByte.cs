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
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Reflection;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;

namespace Nitrocid.Base.Shell.Shells.Hex.Commands
{
    /// <summary>
    /// Queries a byte in a specified byte, a range of bytes, or entirely
    /// </summary>
    /// <remarks>
    /// You can use this command to query a byte and get its number from the specified byte, a range of bytes, or entirely.
    /// </remarks>
    class QueryByteCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "querybyte";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_HEX_COMMAND_QUERYBYTE_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "byte", new()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_HEX_ADDBYTE_ARGUMENT_BYTE_DESC"
                    }),
                    new CommandArgumentPart(false, "startbyte", new CommandArgumentPartOptions()
                    {
                        IsNumeric = true,
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_HEX_DELBYTES_ARGUMENT_STARTBYTE_DESC"
                    }),
                    new CommandArgumentPart(false, "endbyte", new CommandArgumentPartOptions()
                    {
                        IsNumeric = true,
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_HEX_DELBYTES_ARGUMENT_ENDBYTE_DESC"
                    })
                ])
            ];

        public override CommandFlags Flags =>
            CommandFlags.Wrappable;

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            var hexShell = (HexShell?)shell ??
                throw new KernelException(KernelExceptionType.Archive, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_LASTSHELLTYPEMISMATCH"));
            var FileBytes = hexShell.FileBytes ??
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
            if (parameters.ArgumentsList.Length == 1)
            {
                string byteStr = parameters.ArgumentsList[0];
                byte ByteContent = Convert.ToByte(byteStr, 16);
                hexShell.QueryByteAndDisplay(ByteContent);
                return 0;
            }
            else if (parameters.ArgumentsList.Length == 2)
            {
                string byteStr = parameters.ArgumentsList[0];
                string startByteStr = parameters.ArgumentsList[1];
                if (TextTools.IsStringNumeric(startByteStr))
                {
                    if (Convert.ToInt64(startByteStr) <= FileBytes.LongLength)
                    {
                        byte ByteContent = Convert.ToByte(byteStr, 16);
                        hexShell.QueryByteAndDisplay(ByteContent, Convert.ToInt64(startByteStr));
                        return 0;
                    }
                    else
                    {
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_BYTENUMTOOLARGE"), true, ThemeColorType.Error);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.HexEditor);
                    }
                }
            }
            else if (parameters.ArgumentsList.Length > 2)
            {
                string byteStr = parameters.ArgumentsList[0];
                string startByteStr = parameters.ArgumentsList[1];
                string endByteStr = parameters.ArgumentsList[2];
                if (TextTools.IsStringNumeric(startByteStr) & TextTools.IsStringNumeric(endByteStr))
                {
                    if (Convert.ToInt64(startByteStr) <= FileBytes.LongLength & Convert.ToInt64(endByteStr) <= hexShell.FileBytes.LongLength)
                    {
                        byte ByteContent = Convert.ToByte(byteStr, 16);
                        long ByteNumberStart = Convert.ToInt64(startByteStr);
                        long ByteNumberEnd = Convert.ToInt64(endByteStr);
                        ByteNumberStart.SwapIfSourceLarger(ref ByteNumberEnd);
                        hexShell.QueryByteAndDisplay(ByteContent, ByteNumberStart, ByteNumberEnd);
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
