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

using FluentFTP;
using Terminaux.Shell.Commands;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using Terminaux.Colors.Themes.Colors;
using Nitrocid.Kernel.Exceptions;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.ShellPacks.Shells.FTP.Commands
{
    /// <summary>
    /// Gets server info
    /// </summary>
    /// <remarks>
    /// To get the server info, including the operating system and server type, use this command.
    /// </remarks>
    class InfoCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var client = (FtpClient?)FTPShellCommon.ClientFTP?.ConnectionInstance;
            if (client is null)
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.FTPShell);
            SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_SERVERINFO_TITLE"), ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
            TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_SERVERINFO_ADDRESS") + " ", false, ThemeColorType.ListEntry);
            TextWriters.Write(client.Host, true, ThemeColorType.ListValue);
            TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_SERVERINFO_PORT") + " ", false, ThemeColorType.ListEntry);
            TextWriters.Write(client.Port.ToString(), true, ThemeColorType.ListValue);
            TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_SERVERINFO_TYPE") + " ", false, ThemeColorType.ListEntry);
            TextWriters.Write(client.ServerType.ToString(), true, ThemeColorType.ListValue);
            TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_SERVERINFO_SYSTYPE") + " ", false, ThemeColorType.ListEntry);
            TextWriters.Write(client.SystemType, true, ThemeColorType.ListValue);
            TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_SERVERINFO_SYSTEM") + " ", false, ThemeColorType.ListEntry);
            TextWriters.Write(client.ServerOS.ToString(), true, ThemeColorType.ListValue);
            TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_SERVERINFO_ENCRYPTION") + " ", false, ThemeColorType.ListEntry);
            TextWriters.Write(client.Config.EncryptionMode.ToString(), true, ThemeColorType.ListValue);
            TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_SERVERINFO_DATACONNECTION") + " ", false, ThemeColorType.ListEntry);
            TextWriters.Write(client.Config.DataConnectionType.ToString(), true, ThemeColorType.ListValue);
            TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_SERVERINFO_DOWNLOADDATATYPE") + " ", false, ThemeColorType.ListEntry);
            TextWriters.Write(client.Config.DownloadDataType.ToString(), true, ThemeColorType.ListValue);
            TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_SERVERINFO_UPLOADDATATYPE") + " ", false, ThemeColorType.ListEntry);
            TextWriters.Write(client.Config.UploadDataType.ToString(), true, ThemeColorType.ListValue);
            return 0;
        }

    }
}
