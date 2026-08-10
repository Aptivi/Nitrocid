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
    /// Deletes the bytes
    /// </summary>
    /// <remarks>
    /// You can use this command to remove a extraneous bytes in a specified range. You can use the print command to review the changes.
    /// </remarks>
    class DelBytesCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "delbytes";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_HEX_COMMAND_DELBYTES_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "startbyte", new CommandArgumentPartOptions()
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

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            var hexShell = (HexShell?)shell ??
                throw new KernelException(KernelExceptionType.Archive, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_LASTSHELLTYPEMISMATCH"));
            var FileBytes = hexShell.FileBytes ??
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
            if (parameters.ArgumentsList.Length == 1)
            {
                string startByteStr = parameters.ArgumentsList[0];

                if (TextTools.IsStringNumeric(startByteStr))
                {
                    if (Convert.ToInt64(startByteStr) <= FileBytes.LongLength)
                    {
                        hexShell.DeleteBytes(Convert.ToInt64(startByteStr));
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_HEX_DELBYTES_SUCCESS"), true, ThemeColorType.Success);
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
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_HEX_DELBYTES_INVALIDNUM"), true, ThemeColorType.Error, startByteStr);
                    DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", vars: [startByteStr]);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.HexEditor);
                }
            }
            else if (parameters.ArgumentsList.Length > 1)
            {
                string startByteStr = parameters.ArgumentsList[0];
                string endByteStr = parameters.ArgumentsList[1];

                if (TextTools.IsStringNumeric(startByteStr) & TextTools.IsStringNumeric(endByteStr))
                {
                    if (Convert.ToInt64(startByteStr) <= FileBytes.LongLength & Convert.ToInt64(endByteStr) <= hexShell.FileBytes.LongLength)
                    {
                        long ByteNumberStart = Convert.ToInt64(startByteStr);
                        long ByteNumberEnd = Convert.ToInt64(endByteStr);
                        ByteNumberStart.SwapIfSourceLarger(ref ByteNumberEnd);
                        hexShell.DeleteBytes(ByteNumberStart, ByteNumberEnd);
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
                    DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a numeric value.", vars: [endByteStr]);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.HexEditor);
                }
            }
            return 0;
        }

    }
}
