//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using Terminaux.Writer.FancyWriters;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Kernel.Exceptions;

namespace Nitrocid.Extras.FtpShell.FTP.Commands
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
            SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("FTP server information"), true);
            TextWriters.Write(Translate.DoTranslation("Server address:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write(client.Host, true, KernelColorType.ListValue);
            TextWriters.Write(Translate.DoTranslation("Server port:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write(client.Port.ToString(), true, KernelColorType.ListValue);
            TextWriters.Write(Translate.DoTranslation("Server type:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write(client.ServerType.ToString(), true, KernelColorType.ListValue);
            TextWriters.Write(Translate.DoTranslation("Server system type:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write(client.SystemType, true, KernelColorType.ListValue);
            TextWriters.Write(Translate.DoTranslation("Server system:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write(client.ServerOS.ToString(), true, KernelColorType.ListValue);
            TextWriters.Write(Translate.DoTranslation("Server encryption mode:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write(client.Config.EncryptionMode.ToString(), true, KernelColorType.ListValue);
            TextWriters.Write(Translate.DoTranslation("Server data connection type:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write(client.Config.DataConnectionType.ToString(), true, KernelColorType.ListValue);
            TextWriters.Write(Translate.DoTranslation("Server download data type:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write(client.Config.DownloadDataType.ToString(), true, KernelColorType.ListValue);
            TextWriters.Write(Translate.DoTranslation("Server upload data type:") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write(client.Config.UploadDataType.ToString(), true, KernelColorType.ListValue);
            return 0;
        }

    }
}
