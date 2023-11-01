﻿//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using KS.Users;

namespace KS.Shell.Shells.Admin.Commands
{
    class UserFlagCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
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
                    TextWriterColor.Write(Translate.DoTranslation("The specified main flag type is invalid") + ": {0}", type);
                    return 10000 + (int)KernelExceptionType.UserManagement;
            }
            UserManagement.Users[userIndex].Flags = flags;
            UserManagement.SaveUsers();
            return 0;
        }
    }
}