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

using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Users;
using Nitrocid.Base.Kernel.Exceptions;

namespace Nitrocid.Base.Shell.Shells.Admin.Commands
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
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ADMIN_USERFULLNAME_SUCCESS"), fullName);
            }
            else if (!string.IsNullOrWhiteSpace(fullName))
            {
                // Now, change the name in the user config
                UserManagement.Users[userIndex].FullName = fullName;
                UserManagement.SaveUsers();
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ADMIN_USERFULLNAME_SUCCESS"), fullName);
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
