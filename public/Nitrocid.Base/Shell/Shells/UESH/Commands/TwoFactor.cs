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

using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Security.Permissions;
using Nitrocid.Base.Users;
using Nitrocid.Base.Users.TwoFactorAuth;
using OtpNet;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Help;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Manages 2FA authentication for users
    /// </summary>
    class TwoFactorCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "2fa";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_2FA_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "add", new()
                    {
                        ExactWording = ["add"],
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_2FA_ARGUMENT_ADD_DESC"
                    }),
                    new CommandArgumentPart(true, "username", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_2FA_ARGUMENT_USERNAME_DESC"
                    }),
                ])
                {
                    ArgChecker = (cp) => TwoFactorCommand.CheckArgument(cp, "add")
                },
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "delete", new()
                    {
                        ExactWording = ["delete"],
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_2FA_ARGUMENT_DELETE_DESC"
                    }),
                    new CommandArgumentPart(true, "username", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_2FA_ARGUMENT_USERNAME_DESC"
                    }),
                ])
                {
                    ArgChecker = (cp) => TwoFactorCommand.CheckArgument(cp, "delete")
                },
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "check", new()
                    {
                        ExactWording = ["check"],
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_2FA_ARGUMENT_CHECK_DESC"
                    }),
                    new CommandArgumentPart(true, "username", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_2FA_ARGUMENT_USERNAME_DESC"
                    }),
                ])
                {
                    ArgChecker = (cp) => TwoFactorCommand.CheckArgument(cp, "check")
                },
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "setupkey", new()
                    {
                        ExactWording = ["setupkey"],
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_2FA_ARGUMENT_SETUPKEY_DESC"
                    }),
                    new CommandArgumentPart(true, "username", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_2FA_ARGUMENT_USERNAME_DESC"
                    }),
                ])
                {
                    ArgChecker = (cp) => TwoFactorCommand.CheckArgument(cp, "setupkey")
                },
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "setupqr", new()
                    {
                        ExactWording = ["setupqr"],
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_2FA_ARGUMENT_SETUPQR_DESC"
                    }),
                    new CommandArgumentPart(true, "username", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_2FA_ARGUMENT_USERNAME_DESC"
                    }),
                ])
                {
                    ArgChecker = (cp) => TwoFactorCommand.CheckArgument(cp, "setupqr")
                },
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
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
                        string secretDisplay = Base32Encoding.ToString(TwoFactorAuthTools.SecretToBytes(userInfo));
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_USERS_2FA_ENROLLMENTCOMPLETE") + $": {secretDisplay}");
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_USERS_2FA_ENROLLMENTCOMPLETE_QR"));
                        string qrCodeRendered = TwoFactorAuthTools.RenderQRCodeMatrix(userName);
                        TextWriterColor.Write(qrCodeRendered, false);
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_USERS_2FA_ENROLLMENTCOMPLETE_REMINDER") + $": {secretDisplay}");
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
                        string secretDisplay = Base32Encoding.ToString(TwoFactorAuthTools.SecretToBytes(userInfo));
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_USERS_2FA_SETUPKEY") + $": {secretDisplay}");
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
