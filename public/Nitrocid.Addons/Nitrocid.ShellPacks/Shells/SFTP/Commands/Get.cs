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

using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.ShellPacks.Shells.SFTP.Commands
{
    /// <summary>
    /// Downloads a file from the current working directory
    /// </summary>
    /// <remarks>
    /// Downloads the binary or text file and saves it to the current working local directory for you to use the downloaded file that is provided in the SFTP server.
    /// </remarks>
    class GetCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "get";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FS_COMMAND_GET_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEFILE_DESC"
                    })
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            var sftpShell = (SFTPShell?)shell ??
                throw new KernelException(KernelExceptionType.SFTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_LASTSHELLTYPEMISMATCH"));
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_FS_DOWNLOADINGFILE"), false, ThemeColorType.Progress, parameters.ArgumentsList[0]);
            if (sftpShell.SFTPGetFile(parameters.ArgumentsList[0]))
            {
                TextWriterRaw.Write();
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_FS_DOWNLOADEDFILE"), true, ThemeColorType.Success, parameters.ArgumentsList[0]);
                return 0;
            }
            else
            {
                TextWriterRaw.Write();
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_GET_FAILED"), true, ThemeColorType.Error, parameters.ArgumentsList[0]);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.SFTPFilesystem);
            }
        }

    }
}
