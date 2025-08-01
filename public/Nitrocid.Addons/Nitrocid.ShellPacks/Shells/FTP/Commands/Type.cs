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
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Commands;

namespace Nitrocid.ShellPacks.Shells.FTP.Commands
{
    /// <summary>
    /// Sets data transfer type
    /// </summary>
    /// <remarks>
    /// If you need to change how the data transfer is made, you can use this command to switch between the ASCII transfer and the binary transfer. Please note that the ASCII transfer is highly discouraged in many conditions except if you're only transferring text.
    /// </remarks>
    class TypeCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var client = (FtpClient?)FTPShellCommon.ClientFTP?.ConnectionInstance;
            if (client is null)
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.FTPShell);
            if (parameters.ArgumentsList[0].Equals("a", System.StringComparison.OrdinalIgnoreCase))
            {
                client.Config.DownloadDataType = FtpDataType.ASCII;
                client.Config.ListingDataType = FtpDataType.ASCII;
                client.Config.UploadDataType = FtpDataType.ASCII;
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_TYPE_ASCII"), true, ThemeColorType.Success);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_TYPE_ASCII_WARNING"), true, ThemeColorType.Warning);
                return 0;
            }
            else if (parameters.ArgumentsList[0].Equals("b", System.StringComparison.OrdinalIgnoreCase))
            {
                client.Config.DownloadDataType = FtpDataType.Binary;
                client.Config.ListingDataType = FtpDataType.Binary;
                client.Config.UploadDataType = FtpDataType.Binary;
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_TYPE_BINARY"), true, ThemeColorType.Success);
                return 0;
            }
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_TYPE_INVALID"), true, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.FTPFilesystem);
            }
        }

    }
}
