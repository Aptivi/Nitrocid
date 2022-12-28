
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

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
            if (FTPShellCommon.FtpConnected)
            {
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("FTP server information"), true);
                TextWriterColor.Write(Translate.DoTranslation("Server address:") + " ", false, KernelColorType.ListEntry);
                TextWriterColor.Write(FTPShellCommon.ClientFTP.Host, false, KernelColorType.ListEntry);
                TextWriterColor.Write(Translate.DoTranslation("Server port:") + " ", false, KernelColorType.ListEntry);
                TextWriterColor.Write(FTPShellCommon.ClientFTP.Port.ToString(), false, KernelColorType.ListEntry);
                TextWriterColor.Write(Translate.DoTranslation("Server type:") + " ", false, KernelColorType.ListEntry);
                TextWriterColor.Write(FTPShellCommon.ClientFTP.ServerType.ToString(), false, KernelColorType.ListEntry);
                TextWriterColor.Write(Translate.DoTranslation("Server system type:") + " ", false, KernelColorType.ListEntry);
                TextWriterColor.Write(FTPShellCommon.ClientFTP.SystemType, false, KernelColorType.ListEntry);
                TextWriterColor.Write(Translate.DoTranslation("Server system:") + " ", false, KernelColorType.ListEntry);
                TextWriterColor.Write(FTPShellCommon.ClientFTP.ServerOS.ToString(), false, KernelColorType.ListEntry);
                TextWriterColor.Write(Translate.DoTranslation("Server encryption mode:") + " ", false, KernelColorType.ListEntry);
                TextWriterColor.Write(FTPShellCommon.ClientFTP.Config.EncryptionMode.ToString(), false, KernelColorType.ListEntry);
                TextWriterColor.Write(Translate.DoTranslation("Server data connection type:") + " ", false, KernelColorType.ListEntry);
                TextWriterColor.Write(FTPShellCommon.ClientFTP.Config.DataConnectionType.ToString(), false, KernelColorType.ListEntry);
                TextWriterColor.Write(Translate.DoTranslation("Server download data type:") + " ", false, KernelColorType.ListEntry);
                TextWriterColor.Write(FTPShellCommon.ClientFTP.Config.DownloadDataType.ToString(), false, KernelColorType.ListEntry);
                TextWriterColor.Write(Translate.DoTranslation("Server upload data type:") + " ", false, KernelColorType.ListEntry);
                TextWriterColor.Write(FTPShellCommon.ClientFTP.Config.UploadDataType.ToString(), false, KernelColorType.ListEntry);
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("You haven't connected to any server yet"), true, KernelColorType.Error);
            }
        }

    }
}
