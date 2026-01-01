//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Users;

namespace Nitrocid.Shell.Shells.Admin.Commands
{
    class UserFullNameCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string userName = parameters.ArgumentsList[0];
            string fullName = parameters.ArgumentsList[1];
            int userIndex = UserManagement.GetUserIndex(userName);
            if (fullName == "clear")
            {
                // Now, change the name in the user config
                UserManagement.Users[userIndex].FullName = "";
                UserManagement.SaveUsers();
                TextWriterColor.Write(Translate.DoTranslation("Full name set to {0}. You may want to log in again."), fullName);
            }
            else if (!string.IsNullOrWhiteSpace(fullName))
            {
                // Now, change the name in the user config
                UserManagement.Users[userIndex].FullName = fullName;
                UserManagement.SaveUsers();
                TextWriterColor.Write(Translate.DoTranslation("Full name set to {0}. You may want to log in again."), fullName);
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("The full name is empty."));
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.UserManagement);
            }
            return 0;
        }
    }
}
