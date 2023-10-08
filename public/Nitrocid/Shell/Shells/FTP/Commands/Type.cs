
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
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.FTP.Commands
{
    /// <summary>
    /// Sets data transfer type
    /// </summary>
    /// <remarks>
    /// If you need to change how the data transfer is made, you can use this command to switch between the ASCII transfer and the binary transfer. Please note that the ASCII transfer is highly discouraged in many conditions except if you're only transferring text.
    /// </remarks>
    class FTP_TypeCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (parameters.ArgumentsList[0].ToLower() == "a")
            {
                ((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).Config.DownloadDataType = FtpDataType.ASCII;
                ((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).Config.ListingDataType = FtpDataType.ASCII;
                ((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).Config.UploadDataType = FtpDataType.ASCII;
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Data type set to ASCII!"), true, KernelColorType.Success);
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Beware that most files won't download or upload properly using this mode, so we highly recommend using the Binary mode on most situations."), true, KernelColorType.Warning);
                return 0;
            }
            else if (parameters.ArgumentsList[0].ToLower() == "b")
            {
                ((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).Config.DownloadDataType = FtpDataType.Binary;
                ((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).Config.ListingDataType = FtpDataType.Binary;
                ((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).Config.UploadDataType = FtpDataType.Binary;
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Data type set to Binary!"), true, KernelColorType.Success);
                return 0;
            }
            else
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Invalid data type."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.FTPFilesystem;
            }
        }

    }
}
