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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using Nitrocid.Users;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// You can add the user's name whenever you need, with the password if required.
    /// </summary>
    /// <remarks>
    /// If you need to add a person that wants to use the kernel, you can add users for them, and let them specify the password if they need. This way, adduser will only create an account and gets the permissions for the new user ready, and the new user will be a normal account for security reasons.
    /// <br></br>
    /// However if you need to add a person that has admin rights, you should set the permission for the user to allow admin rights. If you want to temporarily disable an account so it blocks the log-on request to that account, you should set the disabled permission to Enabled.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class AddUserCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (parameters.ArgumentsList.Length == 1)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_ADDUSER_CREATEPROGRESS"), parameters.ArgumentsList[0]);
                UserManagement.AddUser(parameters.ArgumentsList[0]);
                return 0;
            }
            else if (parameters.ArgumentsList.Length > 2)
            {
                if (parameters.ArgumentsList[1] == parameters.ArgumentsList[2])
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_ADDUSER_CREATEPROGRESS"), parameters.ArgumentsList[0]);
                    UserManagement.AddUser(parameters.ArgumentsList[0], parameters.ArgumentsList[1]);
                    return 0;
                }
                else
                {
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_ADDUSER_PASSWORDMISMATCH"), true, KernelColorType.Error);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.UserManagement);
                }
            }
            return 0;
        }

    }
}
