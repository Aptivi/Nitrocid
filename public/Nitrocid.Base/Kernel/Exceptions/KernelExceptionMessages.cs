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

using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using System;
using System.Collections.Generic;
using System.Text;
using Textify.General;

namespace Nitrocid.Base.Kernel.Exceptions
{
    internal static class KernelExceptionMessages
    {
        internal static Dictionary<KernelExceptionType, string> Messages =>
            new()
            {
                { KernelExceptionType.Unknown,                          LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_UNKNOWN") },
                { KernelExceptionType.AliasAlreadyExists,               LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_ALIASALREADYEXISTS") },
                { KernelExceptionType.AliasInvalidOperation,            LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_ALIASINVALIDOPERATION") },
                { KernelExceptionType.AliasNoSuchAlias,                 LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_ALIASNOSUCHALIAS") },
                { KernelExceptionType.AliasNoSuchCommand,               LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_ALIASNOSUCHCOMMAND") },
                { KernelExceptionType.AliasNoSuchType,                  LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_ALIASNOSUCHTYPE") },
                { KernelExceptionType.Color,                            LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_COLOR") },
                { KernelExceptionType.Config,                           LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_CONFIG") },
                { KernelExceptionType.ConsoleReadTimeout,               LanguageTools.GetLocalized("NKS_DRIVERS_INPUT_BASE_EXCEPTION_INPUTTIMEOUT") },
                { KernelExceptionType.Filesystem,                       LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_FILESYSTEM") },
                { KernelExceptionType.FTPFilesystem,                    LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_FTPFILESYSTEM") },
                { KernelExceptionType.FTPNetwork,                       LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_FTPNETWORK") },
                { KernelExceptionType.FTPShell,                         LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_FTPSHELL") },
                { KernelExceptionType.GroupManagement,                  LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_GROUPMANAGEMENT") },
                { KernelExceptionType.Hostname,                         LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_HOSTNAME") },
                { KernelExceptionType.HTTPShell,                        LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_HTTPSHEL") },
                { KernelExceptionType.InvalidFeed,                      LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_INVALIDFEED") },
                { KernelExceptionType.InvalidFeedLink,                  LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_INVALIDFEEDLINK") },
                { KernelExceptionType.InvalidFeedType,                  LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_INVALIDFEEDTYPE") },
                { KernelExceptionType.InvalidHashAlgorithm,             LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_INVALIDHASHALGORITHM") },
                { KernelExceptionType.InvalidHash,                      LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_INVALIDHASHSUM") },
                { KernelExceptionType.InvalidKernelPath,                LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_INVALIDKERNELPATH") },
                { KernelExceptionType.InvalidManpage,                   LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_INVALIDMANPAGE") },
                { KernelExceptionType.InvalidMod,                       LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_INVALIDMOD") },
                { KernelExceptionType.InvalidPath,                      LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_INVALIDPATH") },
                { KernelExceptionType.InvalidPlaceholder,               LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_INVALIDPLACEHOLDER") },
                { KernelExceptionType.LanguageInstall,                  LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_LANGUAGEINSTALL") },
                { KernelExceptionType.LanguageParse,                    LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_LANGUAGELOAD") },
                { KernelExceptionType.LanguageUninstall,                LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_LANGUAGEUNINSTALL") },
                { KernelExceptionType.Mail,                             LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_MAIL") },
                { KernelExceptionType.ModInstall,                       LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_MODINSTALL") },
                { KernelExceptionType.ModWithoutMod,                    LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_MODWITHOUTMOD") },
                { KernelExceptionType.ModUninstall,                     LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_MODUNINSTALL") },
                { KernelExceptionType.MOTD,                             LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_MOTD") },
                { KernelExceptionType.NoSuchEvent,                      LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_NOSUCHEVENT") },
                { KernelExceptionType.NoSuchLanguage,                   LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_NOSUCHLANGUAGE") },
                { KernelExceptionType.NoSuchMailDirectory,              LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_NOSUCHMODDIRECTORY") },
                { KernelExceptionType.NoSuchMod,                        LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_NOSUCHMOD") },
                { KernelExceptionType.NoSuchReflectionProperty,         LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_NOSUCHREFLECTIONPROPERTY") },
                { KernelExceptionType.NoSuchReflectionVariable,         LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_NOSUCHREFLECTIONVARIABLE") },
                { KernelExceptionType.NoSuchScreensaver,                LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_NOSUCHSCREENSAVER") },
                { KernelExceptionType.NoSuchShellPreset,                LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_NOSUCHSHELLPRESET") },
                { KernelExceptionType.NullUsers,                        LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_NULLUSERS") },
                { KernelExceptionType.PermissionManagement,             LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_PERMISSIONMANAGEMENT") },
                { KernelExceptionType.RemoteDebugDeviceAlreadyExists,   LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_REMOTEDEBUGDEVICEALREADYEXISTS") },
                { KernelExceptionType.RemoteDebugDeviceNotFound,        LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_REMOTEDEBUGDEVICENOTFOUND") },
                { KernelExceptionType.RemoteDebugDeviceOperation,       LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_REMOTEDEBUGDEVICEOPERATION") },
                { KernelExceptionType.RSSNetwork,                       LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_RSSNETWORK") },
                { KernelExceptionType.RSSShell,                         LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_RSSSHELL") },
                { KernelExceptionType.ScreensaverManagement,            LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_SCREENSAVERMANAGEMENT") },
                { KernelExceptionType.SFTPFilesystem,                   LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_SFTPFILESYSTEM") },
                { KernelExceptionType.SFTPNetwork,                      LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_SFTPNETWORK") },
                { KernelExceptionType.SFTPShell,                        LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_SFTPSHELL") },
                { KernelExceptionType.UserCreation,                     LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_USERCREATION") },
                { KernelExceptionType.UserManagement,                   LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_USERMANAGEMENT") },
                { KernelExceptionType.Network,                          LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_NETWORK") },
                { KernelExceptionType.AssertionFailure,                 LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_ASSERTIONFAILURE") },
                { KernelExceptionType.NetworkOffline,                   LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_NETWORKOFFLINE") },
                { KernelExceptionType.PermissionDenied,                 LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_PERMISSIONDENIED") },
                { KernelExceptionType.NoSuchUser,                       LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_NOSUCHUSER") },
                { KernelExceptionType.NoSuchDriver,                     LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_NOSUCHDRIVER") },
                { KernelExceptionType.ThreadNotReadyYet,                LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_THREADNOTREADYYET") },
                { KernelExceptionType.ThreadOperation,                  LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_THREADOPERATION") },
                { KernelExceptionType.ShellOperation,                   LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_SHELLOPERATION") },
                { KernelExceptionType.NotImplementedYet,                LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_NOTIMPLEMENTEDYET") },
                { KernelExceptionType.RemoteProcedure,                  LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_REMOTEPROCEDURE") },
                { KernelExceptionType.Encryption,                       LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_ENCRYPTION") },
                { KernelExceptionType.Debug,                            LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_DEBUG") },
                { KernelExceptionType.Archive,                          LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_ARCHIVE") },
                { KernelExceptionType.HexEditor,                        LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_HEXEDITOR") },
                { KernelExceptionType.JsonEditor,                       LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_JSONEDITOR") },
                { KernelExceptionType.TextEditor,                       LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_TEXTEDITOR") },
                { KernelExceptionType.OldModDetected,                   LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_OLDMODDETECTED") },
                { KernelExceptionType.RegularExpression,                LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_REGULAREXPRESSION") },
                { KernelExceptionType.Contacts,                         LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_CONTACTS") },
                { KernelExceptionType.SqlEditor,                        LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_SQLEDITOR") },
                { KernelExceptionType.NoSuchGroup,                      LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_NOSUCHGROUP") },
                { KernelExceptionType.CustomSettings,                   LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_CUSTOMSETTINGS") },
                { KernelExceptionType.NetworkConnection,                LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_NETWORKCONNECTION") },
                { KernelExceptionType.HTTPNetwork,                      LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_HTTPNETWORK") },
                { KernelExceptionType.CommandManager,                   LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_COMMANDMANAGER") },
                { KernelExceptionType.LocaleGen,                        LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_LOCALEGEN") },
                { KernelExceptionType.TimeDate,                         LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_TIMEDATE") },
                { KernelExceptionType.ModManual,                        LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_MODMANUAL") },
                { KernelExceptionType.Calendar,                         LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_CALENDAR") },
                { KernelExceptionType.NotificationManagement,           LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_NOTIFICATIONMANAGEMENT") },
                { KernelExceptionType.LanguageManagement,               LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_LANGUAGEMANAGEMENT") },
                { KernelExceptionType.ModManagement,                    LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_MODMANAGEMENT") },
                { KernelExceptionType.Reflection,                       LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_REFLECTION") },
                { KernelExceptionType.EventManagement,                  LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_EVENTMANAGEMENT") },
                { KernelExceptionType.AddonManagement,                  LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_ADDONMANAGEMENT") },
                { KernelExceptionType.NoteManagement,                   LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_NOTEMANAGEMENT") },
                { KernelExceptionType.Hardware,                         LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_HARDWARE") },
                { KernelExceptionType.LoginHandler,                     LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_LOGINHANDLER") },
                { KernelExceptionType.Encoding,                         LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_ENCODING") },
                { KernelExceptionType.PrivacyConsent,                   LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_PRIVACYCONSENT") },
                { KernelExceptionType.Splash,                           LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_SPLASH") },
                { KernelExceptionType.Text,                             LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_TEXT") },
                { KernelExceptionType.InvalidPlaceholderAction,         LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_INVALIDPLACEHOLDERACTION") },
                { KernelExceptionType.DriverHandler,                    LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_DRIVERHANDLER") },
                { KernelExceptionType.ProgressHandler,                  LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_PROGRESSHANDLER") },
                { KernelExceptionType.Console,                          LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_CONSOLE") },
                { KernelExceptionType.Journaling,                       LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_JOURNALING") },
                { KernelExceptionType.Docking,                          LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_DOCKING") },
                { KernelExceptionType.Security,                         LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_SECURITY") },
                { KernelExceptionType.DriverManagement,                 LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_DRIVERMANAGEMENT") },
                { KernelExceptionType.Environment,                      LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_ENVIRONMENT") },
                { KernelExceptionType.Bootloader,                       LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_BOOTLOADER") },
                { KernelExceptionType.Alarm,                            LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_ALARM") },
                { KernelExceptionType.Widget,                           LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_WIDGET") },
                { KernelExceptionType.Homepage,                         LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_HOMEPAGE") },
                { KernelExceptionType.NoSuchCulture,                    LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_NOSUCHCULTURE") },
                { KernelExceptionType.AudioCue,                         LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_AUDIOCUE") },
                { KernelExceptionType.ChatbotAI,                        LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_MESSAGE_CHATBOTAI") },
            };

        internal static string GetFinalExceptionMessage(KernelExceptionType exceptionType, string message, Exception? e, params object[] vars)
        {
            StringBuilder builder = new();

            // Display introduction
            DebugWriter.WriteDebug(DebugLevel.I, "Not a nested KernelException.");
            builder.AppendLine(LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_FINALEXCEPTION_INTRO"));
            builder.AppendLine();

            // Display error type
            builder.AppendLine("--- " + LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_FINALEXCEPTION_EXCEPTIONINFO") + " ---");
            builder.AppendLine("- " + LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_FINALEXCEPTION_ERRORTYPE") + $": {exceptionType} [{Convert.ToInt32(exceptionType)}]");
            builder.AppendLine("  " + LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_FINALEXCEPTION_ERRORMESSAGE") + $": {GetMessageFromType(exceptionType)}");
            builder.AppendLine();

            // Display error message
            builder.AppendLine("--- " + LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_FINALEXCEPTION_ADDIITONALINFO") + " ---");
            DebugWriter.WriteDebug(DebugLevel.I, "Error message \"{0}\"", vars: [message]);
            if (!string.IsNullOrWhiteSpace(message))
            {
                builder.AppendLine("- " + LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_FINALEXCEPTION_HASINFO"));
                builder.AppendLine("  " + TextTools.FormatString(message, vars));
            }
            else
                builder.AppendLine("- " + LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_FINALEXCEPTION_HASNOINFO"));
            builder.AppendLine();

            // Display exception
            builder.AppendLine("--- " + LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_FINALEXCEPTION_EXCEPTIONDETAILS") + " ---");
            DebugWriter.WriteDebug(DebugLevel.I, "Exception is not null: {0}", vars: [e is not null]);
            if (e is not null)
            {
                builder.AppendLine("- " + LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_FINALEXCEPTION_ADDIITONALINFO1"));
                builder.AppendLine("  " + $"{e.GetType().Name}: {(e is KernelException kex ? kex.OriginalExceptionMessage : e.Message)}");
            }
            else
                builder.AppendLine("- " + LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_FINALEXCEPTION_HASNOEXCEPTIONINFO"));
            builder.AppendLine();

            // Display inner exceptions
            builder.AppendLine("--- " + LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_FINALEXCEPTION_INNEREXCEPTION") + " ---");
            int exceptionIndex = 1;
            if (e is not null)
                e = e.InnerException;
            if (e is not null)
                builder.AppendLine("- " + LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_FINALEXCEPTION_ADDIITONALERRORS"));
            while (e is not null)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Inner exception {0} is not null: {1}", vars: [exceptionIndex, e is not null]);
                builder.AppendLine("  " + $"[{exceptionIndex}] {e?.GetType().Name}: {(e is KernelException kex ? kex.OriginalExceptionMessage : e?.Message)}");
                e = e?.InnerException;
                exceptionIndex++;
            }
            if (e is null)
                builder.AppendLine("- " + LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_FINALEXCEPTION_ADDITIONALERRORS"));

            builder.AppendLine();
            builder.Append(LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_FINALEXCEPTION_OUTRO"));
            return builder.ToString();
        }

        internal static string GetMessageFromType(KernelExceptionType exceptionType) =>
            Messages.TryGetValue(exceptionType, out string? type) ?
            type :
            LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_FINALEXCEPTION_INVALIDTYPE");
    }
}
