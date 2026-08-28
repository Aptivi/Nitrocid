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

using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using Nitrocid.Languages;
using Nitrocid.Users;
using Nitrocid.Kernel.Exceptions;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Arguments;

namespace Nitrocid.Shell.Shells.Admin.Commands
{
    class UserFullNameCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "userfullname";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ADMIN_COMMAND_USERFULLNAME_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "user", new()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_ADMIN_COMMAND_USERFLAG_ARGUMENT_USER_DESC"
                    }),
                    new CommandArgumentPart(true, "name/clear", new()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_ADMIN_COMMAND_USERFULLNAME_ARGUMENT_NEWNAME_DESC"
                    })
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string userName = parameters.ArgumentsList[0];
            string fullName = parameters.ArgumentsList[1];
            string finalFullName = "";
            int userIndex = UserManagement.GetUserIndex(userName);
            if (fullName != "clear")
                finalFullName = fullName;
            if (!string.IsNullOrWhiteSpace(fullName))
            {
                // Now, change the name in the user config
                UserManagement.Users[userIndex].FullName = finalFullName;
                UserManagement.SaveUsers();
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ADMIN_USERFULLNAME_SUCCESS"), finalFullName);
            }
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ADMIN_USERFULLNAME_EMPTY"));
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.UserManagement);
            }
            return 0;
        }
    }
}
