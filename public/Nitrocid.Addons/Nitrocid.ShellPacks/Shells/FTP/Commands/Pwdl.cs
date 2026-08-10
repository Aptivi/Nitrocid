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
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.ShellPacks.Shells.FTP.Commands
{
    /// <summary>
    /// Gets local directory
    /// </summary>
    /// <remarks>
    /// If you want to get the local directory, use this command.
    /// </remarks>
    class PwdlCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "pwdl";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_FS_COMMAND_PWDL_DESC");

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            var ftpShell = (FTPShell?)shell ??
                throw new KernelException(KernelExceptionType.FTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_LASTSHELLTYPEMISMATCH"));
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_FS_LOCALDIR"), ftpShell.FtpCurrentDirectory);
            return 0;
        }
    }
}
