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

using Terminaux.Colors.Themes.Colors;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.ShellPacks.Tools.Transfer;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;

namespace Nitrocid.ShellPacks.Shells.FTP.Commands
{
    /// <summary>
    /// Downloads a file from the current working directory
    /// </summary>
    /// <remarks>
    /// Downloads the binary or text file and saves it to the current working local directory for you to use the downloaded file that is provided in the FTP server.
    /// </remarks>
    class GetCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string RemoteFile = parameters.ArgumentsList[0];
            string LocalFile = parameters.ArgumentsList.Length > 1 ? parameters.ArgumentsList[1] : "";
            TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_FS_DOWNLOADINGFILE"), false, ThemeColorType.Progress, RemoteFile);
            bool Result = !string.IsNullOrWhiteSpace(LocalFile) ? FTPTransfer.FTPGetFile(RemoteFile, LocalFile) : FTPTransfer.FTPGetFile(RemoteFile);
            if (Result)
            {
                TextWriterRaw.Write();
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_FS_DOWNLOADEDFILE"), true, ThemeColorType.Success, RemoteFile);
                return 0;
            }
            else
            {
                TextWriterRaw.Write();
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_GET_FAILED"), true, ThemeColorType.Error, RemoteFile);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.FTPNetwork);
            }
        }

    }
}
