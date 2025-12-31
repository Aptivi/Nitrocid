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
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using System;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Users;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Users.Login;
using Nitrocid.Base.Security.Permissions;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Runs a command as the superuser
    /// </summary>
    /// <remarks>
    /// You can run a command as "root," the superuser, using just your password.
    /// </remarks>
    class SudoCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            bool sudoDone = false;
            PermissionsTools.Demand(PermissionTypes.UseSudo);

            // First, check to see if we're already root
            var root = UserManagement.GetUser("root") ??
                throw new KernelException(KernelExceptionType.UserManagement, LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SUDO_EXCEPTION_USERINFOSYSTEM"));
            if (UserManagement.CurrentUserInfo == root)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SUDO_ALREADYROOT"));
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.ShellOperation);
            }

            // Now, prompt for the current username's password
            string currentUsername = UserManagement.CurrentUser.Username;
            bool failed = false;
            try
            {
                if (Login.ShowPasswordPrompt(currentUsername))
                {
                    sudoDone = true;
                    DebugWriter.WriteDebug(DebugLevel.I, "Switching to root user...");
                    UserManagement.CurrentUserInfo = root;
                    UserManagement.LockUser(currentUsername);
                    UserManagement.LockUser("root");
                    ShellManager.AddAlternateThread();
                    ShellManager.GetLine(parameters.ArgumentsText);
                }
                else
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.ShellOperation);
            }
            catch (Exception ex)
            {
                failed = true;
                DebugWriter.WriteDebug(DebugLevel.I, "Executing command {0} as superuser failed: {1}", vars: [parameters.ArgumentsText, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SUDO_CANTEXECUTE") + $" {ex.Message}");
            }
            finally
            {
                if (sudoDone)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Sudo is done. Switching to user {0}...", vars: [currentUsername]);
                    UserManagement.CurrentUserInfo = UserManagement.GetUser(currentUsername) ??
                        throw new KernelException(KernelExceptionType.UserManagement, LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SUDO_EXCEPTION_USERINFO") + $" {currentUsername}");
                    UserManagement.UnlockUser(currentUsername);
                    UserManagement.UnlockUser("root");
                }
            }
            return failed ? KernelExceptionTools.GetErrorCode(KernelExceptionType.ShellOperation) : 0;
        }

    }
}
