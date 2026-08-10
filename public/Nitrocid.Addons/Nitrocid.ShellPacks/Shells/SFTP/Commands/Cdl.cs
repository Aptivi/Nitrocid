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

namespace Nitrocid.ShellPacks.Shells.SFTP.Commands
{
    /// <summary>
    /// Changes your local directory
    /// </summary>
    /// <remarks>
    /// This command lets you change your local directory in your hard drives to another directory that exists in the subdirectory. However, when specifying .., it goes backwards.
    /// </remarks>
    class CdlCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "cdl";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FS_COMMAND_CDL_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "directory", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_LOCALDIR_DESC"
                    })
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            var sftpShell = (SFTPShell?)shell ??
                throw new KernelException(KernelExceptionType.SFTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_LASTSHELLTYPEMISMATCH"));
            sftpShell.SFTPChangeLocalDir(parameters.ArgumentsList[0]);
            return 0;
        }
    }
}
