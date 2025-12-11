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

using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Security.Permissions;
using Nitrocid.Base.Users;
using Nitrocid.Base.Users.TwoFactorAuth;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Help;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Manages 2FA authentication for users
    /// </summary>
    class TwoFactorCommand : BaseCommand, ICommand
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

            string CommandMode = parameters.ArgumentsList[0].ToLower();
            string userName = parameters.ArgumentsList[1];

            // Now, the actual logic
            switch (CommandMode)
            {
                case "add":
                    {
                        var userInfo = UserManagement.GetUser(userName) ??
                            throw new KernelException(KernelExceptionType.NoSuchUser);
                        TwoFactorAuthTools.EnrollUser(userName);
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_USERS_2FA_ENROLLMENTCOMPLETE") + $": {userInfo.TwoFactorSecret}");
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_USERS_2FA_ENROLLMENTCOMPLETE_QR"));
                        string qrCodeRendered = TwoFactorAuthTools.RenderQRCodeMatrix(userName);
                        TextWriterColor.Write(qrCodeRendered, false);
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_USERS_2FA_ENROLLMENTCOMPLETE_REMINDER") + $": {userInfo.TwoFactorSecret}");
                        break;
                    }
                case "delete":
                    {
                        TwoFactorAuthTools.UnenrollUser(userName);
                        break;
                    }
                case "check":
                    {
                        bool enrolled = TwoFactorAuthTools.IsUserEnrolled(userName);
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_USERS_2FA_ENROLLMENTSTATUS") + $": {enrolled}");
                        break;
                    }
                case "setupkey":
                    {
                        bool enrolled = TwoFactorAuthTools.IsUserEnrolled(userName);
                        if (!enrolled)
                        {
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_USERS_2FA_EXCEPTION_USERNOTENROLLED"));
                            break;
                        }

                        // Write the setup key
                        var userInfo = UserManagement.GetUser(userName) ??
                            throw new KernelException(KernelExceptionType.NoSuchUser);
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_USERS_2FA_SETUPKEY") + $": {userInfo.TwoFactorSecret}");
                        break;
                    }
                case "setupqr":
                    {
                        bool enrolled = TwoFactorAuthTools.IsUserEnrolled(userName);
                        if (!enrolled)
                        {
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_USERS_2FA_EXCEPTION_USERNOTENROLLED"));
                            break;
                        }

                        // Write the setup key
                        string qrCodeRendered = TwoFactorAuthTools.RenderQRCodeMatrix(userName);
                        TextWriterColor.Write(qrCodeRendered, false);
                        break;
                    }

                default:
                    {
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_BASE_COMMANDS_INVALIDCOMMAND_BRANCHED"), true, ThemeColorType.Error, CommandMode);
                        HelpPrint.ShowHelp("alarm");
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.Alarm);
                    }
            }
            return 0;
        }

        internal static int CheckArgument(CommandParameters parameters, string commandMode)
        {
            // These command modes require arguments to be passed, so re-check here and there.
            switch (commandMode.ToLower())
            {
                case "add":
                case "delete":
                case "check":
                case "setupkey":
                case "setupqr":
                    {
                        if (parameters.ArgumentsList.Length > 1)
                        {
                            string userName = parameters.ArgumentsList[1];
                            if (!UserManagement.UserExists(userName))
                            {
                                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_USERS_EXCEPTION_USERNOTFOUND2"), true, ThemeColorType.Error);
                                return KernelExceptionTools.GetErrorCode(KernelExceptionType.NoSuchUser);
                            }
                        }
                        else
                        {
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_USERS_USERNOTPROVIDED"), true, ThemeColorType.Error);
                            return KernelExceptionTools.GetErrorCode(KernelExceptionType.UserManagement);
                        }

                        break;
                    }
            }
            return 0;
        }
    }
}
