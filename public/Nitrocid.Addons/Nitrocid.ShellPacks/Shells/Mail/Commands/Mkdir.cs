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

namespace Nitrocid.ShellPacks.Shells.Mail.Commands
{
    /// <summary>
    /// Makes a mail directory
    /// </summary>
    /// <remarks>
    /// If you want an additional mail folder to organize your messages there, you can use this command. Works with the combination of the mv or mvall command.
    /// </remarks>
    class MkdirCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "mkdir";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMAND_MKDIR_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "foldername", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_MAIL_COMMAND_ARGUMENT_FOLDER_DESC"
                    })
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            var mailShell = (MailShell?)shell ??
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_LASTSHELLTYPEMISMATCH"));
            mailShell.CreateMailDirectory(parameters.ArgumentsList[0]);
            return 0;
        }
    }
}
