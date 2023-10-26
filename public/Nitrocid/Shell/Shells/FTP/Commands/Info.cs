//
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
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
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Languages;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.FTP.Commands
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
            SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("FTP server information"), true);
            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Server address:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.WriteKernelColor(((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).Host, true, KernelColorType.ListValue);
            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Server port:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.WriteKernelColor(((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).Port.ToString(), true, KernelColorType.ListValue);
            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Server type:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.WriteKernelColor(((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).ServerType.ToString(), true, KernelColorType.ListValue);
            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Server system type:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.WriteKernelColor(((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).SystemType, true, KernelColorType.ListValue);
            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Server system:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.WriteKernelColor(((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).ServerOS.ToString(), true, KernelColorType.ListValue);
            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Server encryption mode:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.WriteKernelColor(((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).Config.EncryptionMode.ToString(), true, KernelColorType.ListValue);
            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Server data connection type:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.WriteKernelColor(((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).Config.DataConnectionType.ToString(), true, KernelColorType.ListValue);
            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Server download data type:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.WriteKernelColor(((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).Config.DownloadDataType.ToString(), true, KernelColorType.ListValue);
            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Server upload data type:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.WriteKernelColor(((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).Config.UploadDataType.ToString(), true, KernelColorType.ListValue);
            return 0;
        }

    }
}
