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
using Nitrocid.Base.Users;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Base.Shell.Shells.Admin.Commands
{
    class UserFlagCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "userflag";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ADMIN_COMMAND_USERFLAG_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "user", new()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_ADMIN_COMMAND_USERFLAG_ARGUMENT_USER_DESC"
                    }),
                    new CommandArgumentPart(true, "admin/anonymous/disabled", new()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_ADMIN_COMMAND_USERFLAG_ARGUMENT_TYPE_DESC"
                    }),
                    new CommandArgumentPart(true, "false/true", new()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_ADMIN_COMMAND_USERFLAG_ARGUMENT_GRANT_DESC"
                    })
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string userName = parameters.ArgumentsList[0];
            string type = parameters.ArgumentsList[1];
            bool enabled = bool.Parse(parameters.ArgumentsList[2]);
            int userIndex = UserManagement.GetUserIndex(userName);
            var flags = UserManagement.Users[userIndex].Flags;
            switch (type)
            {
                case "admin":
                    if (enabled)
                    {
                        if (!flags.HasFlag(UserFlags.Administrator))
                            flags |= UserFlags.Administrator;
                    }
                    else
                    {
                        if (flags.HasFlag(UserFlags.Administrator))
                            flags &= ~UserFlags.Administrator;
                    }
                    break;
                case "disabled":
                    if (enabled)
                    {
                        if (!flags.HasFlag(UserFlags.Disabled))
                            flags |= UserFlags.Disabled;
                    }
                    else
                    {
                        if (flags.HasFlag(UserFlags.Disabled))
                            flags &= ~UserFlags.Disabled;
                    }
                    break;
                case "anonymous":
                    if (enabled)
                    {
                        if (!flags.HasFlag(UserFlags.Anonymous))
                            flags |= UserFlags.Anonymous;
                    }
                    else
                    {
                        if (flags.HasFlag(UserFlags.Anonymous))
                            flags &= ~UserFlags.Anonymous;
                    }
                    break;
                default:
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ADMIN_USERFLAG_FLAGINVALID") + ": {0}", type);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.UserManagement);
            }
            UserManagement.Users[userIndex].Flags = flags;
            UserManagement.SaveUsers();
            return 0;
        }
    }
}
