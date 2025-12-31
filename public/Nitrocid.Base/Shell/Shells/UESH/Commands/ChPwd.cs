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

using System;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Users;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Security.Permissions;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// You can change your password or someone else's password
    /// </summary>
    /// <remarks>
    /// If the password for your account, or for someone else's account, needs to be changed, then you can use this command to change your password or someone else's password.
    /// <br></br>
    /// This is useful if you think that your account or someone else's account has a bad password or is in the easy password list located online.
    /// <br></br>
    /// This command requires you to specify your password or someone else's password before writing your new password.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class ChPwdCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (!PermissionsTools.IsPermissionGranted(PermissionTypes.RunStrictCommands) &&
                !UserManagement.CurrentUser.Flags.HasFlag(UserFlags.Administrator))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Cmd exec {0} failed: adminList(signedinusrnm) is False, strictCmds.Contains({0}) is True", vars: [parameters.CommandText]);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_NEEDSPERM"), true, ThemeColorType.Error, parameters.CommandText);
                return -4;
            }

            try
            {
                if (parameters.ArgumentsList[3].Contains(' '))
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHPWD_NOSPACES"), true, ThemeColorType.Error);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.UserManagement);
                }
                else if (parameters.ArgumentsList[3] == parameters.ArgumentsList[2])
                {
                    UserManagement.ChangePassword(parameters.ArgumentsList[0], parameters.ArgumentsList[1], parameters.ArgumentsList[2]);
                    return 0;
                }
                else if (parameters.ArgumentsList[3] != parameters.ArgumentsList[2])
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHPWD_MISMATCH"), true, ThemeColorType.Error);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.UserManagement);
                }
            }
            catch (Exception ex)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHPWD_FAILURE"), true, ThemeColorType.Error, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                return ex.GetHashCode();
            }
            return 0;
        }

    }
}
