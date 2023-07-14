
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using FluentFTP;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.FTP.Commands
{
    /// <summary>
    /// Gets server info
    /// </summary>
    /// <remarks>
    /// To get the server info, including the operating system and server type, use this command.
    /// </remarks>
    class FTP_InfoCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("FTP server information"), true);
            TextWriterColor.Write(Translate.DoTranslation("Server address:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).Host, false, KernelColorType.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Server port:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).Port.ToString(), false, KernelColorType.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Server type:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).ServerType.ToString(), false, KernelColorType.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Server system type:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).SystemType, false, KernelColorType.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Server system:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).ServerOS.ToString(), false, KernelColorType.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Server encryption mode:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).Config.EncryptionMode.ToString(), false, KernelColorType.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Server data connection type:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).Config.DataConnectionType.ToString(), false, KernelColorType.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Server download data type:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).Config.DownloadDataType.ToString(), false, KernelColorType.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Server upload data type:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).Config.UploadDataType.ToString(), false, KernelColorType.ListEntry);
        }

    }
}
