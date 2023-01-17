
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using Extensification.StringExts;
using KS.Languages;
using System;
using System.Collections.Generic;
using System.Text;

namespace KS.Kernel.Exceptions
{
    internal static class KernelExceptionMessages
    {
        internal static Dictionary<KernelExceptionType, string> messages
        {
            get
            {
                return new Dictionary<KernelExceptionType, string>()
                {
                    { KernelExceptionType.Unknown,                          Translate.DoTranslation("Unknown kernel error.") },
                    { KernelExceptionType.AliasAlreadyExists,               Translate.DoTranslation("The specified alias already exists. Make sure that you select a different alias.") },
                    { KernelExceptionType.AliasInvalidOperation,            Translate.DoTranslation("The alias system encountered an invalid operation trying to process your request.") },
                    { KernelExceptionType.AliasNoSuchAlias,                 Translate.DoTranslation("The specified alias isn't found.") },
                    { KernelExceptionType.AliasNoSuchCommand,               Translate.DoTranslation("The specified command isn't found to alias to.") },
                    { KernelExceptionType.AliasNoSuchType,                  Translate.DoTranslation("The specified alias type isn't found.") },
                    { KernelExceptionType.Color,                            Translate.DoTranslation("An error occurred while trying to process the color request. Common mistakes include specifying the wrong color specifier.") },
                    { KernelExceptionType.Config,                           Translate.DoTranslation("A kernel configuration error occurred.") },
                    { KernelExceptionType.ConsoleReadTimeout,               Translate.DoTranslation("User didn't provide any input in a timely fashion.") },
                    { KernelExceptionType.Filesystem,                       Translate.DoTranslation("A general filesystem error occurred. Check to make sure that the path specified is correct, and that the file or directory actually exists.") },
                    { KernelExceptionType.FTPFilesystem,                    Translate.DoTranslation("A general FTP filesystem error occurred. Check to make sure that the path specified is correct, and that the file or directory actually exists.") },
                    { KernelExceptionType.FTPNetwork,                       Translate.DoTranslation("A general FTP network error occurred. Check to make sure that your internet connection is working.") },
                    { KernelExceptionType.FTPShell,                         Translate.DoTranslation("A general FTP shell error occurred.") },
                    { KernelExceptionType.Hostname,                         Translate.DoTranslation("Hostname error.") },
                    { KernelExceptionType.HTTPShell,                        Translate.DoTranslation("A general HTTP shell error occurred.") },
                    { KernelExceptionType.InsaneConsoleDetected,            Translate.DoTranslation("Insane console has been detected. Kernel Simulator can't continue running, because your console is incompatible with our requirements.") },
                    { KernelExceptionType.InvalidFeed,                      Translate.DoTranslation("Invalid RSS feed.") },
                    { KernelExceptionType.InvalidFeedLink,                  Translate.DoTranslation("Invalid RSS feed link.") },
                    { KernelExceptionType.InvalidFeedType,                  Translate.DoTranslation("Invalid RSS feed type.") },
                    { KernelExceptionType.InvalidHashAlgorithm,             Translate.DoTranslation("Invalid hash algorithm. If you're using a custom hash sum driver, check to make sure that the driver is loaded.") },
                    { KernelExceptionType.InvalidHash,                      Translate.DoTranslation("Invalid hash sum. Check to make sure that you copied and pasted the hash sum correctly. If you're using a custom hash sum driver, check to make sure that the driver is loaded and that it works as expected.") },
                    { KernelExceptionType.InvalidKernelPath,                Translate.DoTranslation("Invalid kernel path.") },
                    { KernelExceptionType.InvalidManpage,                   Translate.DoTranslation("Invalid manual page. Check to make sure that the mod containing the valid manual pages is loaded.") },
                    { KernelExceptionType.InvalidMod,                       Translate.DoTranslation("Invalid mod. If the mod is a .NET Framework mod, make sure that you're using the .NET Framework version of Kernel Simulator. The same applies to .NET CoreCLR version. If your mod is a valid .NET assembly, check to make sure that it actually implements the interface that is necessary to start the mod.") },
                    { KernelExceptionType.InvalidPath,                      Translate.DoTranslation("Invalid filesystem path. Check to make sure that it's written correctly.") },
                    { KernelExceptionType.InvalidPlaceholder,               Translate.DoTranslation("Invalid placeholder. Consult the kernel documentation for more information.") },
                    { KernelExceptionType.LanguageInstall,                  Translate.DoTranslation("The custom language failed to install. Check to make sure that the language file is correctly formed. Usually, the locale generator does a good job in making the localization information for your custom language.") },
                    { KernelExceptionType.LanguageParse,                    Translate.DoTranslation("The custom language failed to load. Check to make sure that the language file is correctly formed. Usually, the locale generator does a good job in making the localization information for your custom language.") },
                    { KernelExceptionType.LanguageUninstall,                Translate.DoTranslation("The custom language failed to uninstall.") },
                    { KernelExceptionType.Mail,                             Translate.DoTranslation("The mail system encountered an error.") },
                    { KernelExceptionType.ModInstall,                       Translate.DoTranslation("The mod failed to install. Check to make sure that it's a valid mod file. If the mod is a .NET Framework mod, make sure that you're using the .NET Framework version of Kernel Simulator. The same applies to .NET CoreCLR version. If your mod is a valid .NET assembly, check to make sure that it actually implements the interface that is necessary to start the mod.") },
                    { KernelExceptionType.ModNoParts,                       Translate.DoTranslation("The mod contains no parts. Consult with the mod vendor for a new copy.") },
                    { KernelExceptionType.ModUninstall,                     Translate.DoTranslation("The mod uninstallation has failed.") },
                    { KernelExceptionType.MOTD,                             Translate.DoTranslation("An error occurred trying to read or set the message of the day.") },
                    { KernelExceptionType.NoSuchEvent,                      Translate.DoTranslation("There is no event by this name.") },
                    { KernelExceptionType.NoSuchLanguage,                   Translate.DoTranslation("There is no language by this name.") },
                    { KernelExceptionType.NoSuchMailDirectory,              Translate.DoTranslation("There is no mail directory by this name.") },
                    { KernelExceptionType.NoSuchMod,                        Translate.DoTranslation("There is no mod by this name.") },
                    { KernelExceptionType.NoSuchReflectionProperty,         Translate.DoTranslation("There is no reflection property by this name.") },
                    { KernelExceptionType.NoSuchReflectionVariable,         Translate.DoTranslation("There is no reflection variable by this name.") },
                    { KernelExceptionType.NoSuchScreensaver,                Translate.DoTranslation("There is no screensaver by this name.") },
                    { KernelExceptionType.NoSuchShellPreset,                Translate.DoTranslation("There is no shell preset by this name.") },
                    { KernelExceptionType.NoSuchTheme,                      Translate.DoTranslation("There is no theme by this name.") },
                    { KernelExceptionType.NullUsers,                        Translate.DoTranslation("There are zero users in the entire kernel. We've reached an extremely rare situation. The kernel can't continue.") },
                    { KernelExceptionType.PermissionManagement,             Translate.DoTranslation("An error occurred trying to manage permissions") },
                    { KernelExceptionType.RemoteDebugDeviceAlreadyExists,   Translate.DoTranslation("The remote debug device already exists.") },
                    { KernelExceptionType.RemoteDebugDeviceNotFound,        Translate.DoTranslation("The remote debug device is not found. Check to make sure that you've written an IP address of the device right.") },
                    { KernelExceptionType.RemoteDebugDeviceOperation,       Translate.DoTranslation("The remote debug device operation failed.") },
                    { KernelExceptionType.RSSNetwork,                       Translate.DoTranslation("The RSS network error occurred. Check to make sure that your internet connection is working.") },
                    { KernelExceptionType.RSSShell,                         Translate.DoTranslation("The RSS shell error occurred.") },
                    { KernelExceptionType.ScreensaverManagement,            Translate.DoTranslation("The screensaver management error occurred during the operation.") },
                    { KernelExceptionType.SFTPFilesystem,                   Translate.DoTranslation("A general SFTP filesystem error occurred. Check to make sure that the path specified is correct, and that the file or directory actually exists.") },
                    { KernelExceptionType.SFTPNetwork,                      Translate.DoTranslation("A general SFTP network error occurred. Check to make sure that your internet connection is working.") },
                    { KernelExceptionType.SFTPShell,                        Translate.DoTranslation("A general SFTP shell error occurred.") },
                    { KernelExceptionType.UESHConditionParse,               Translate.DoTranslation("Condition parsing failed. Please check your syntax to meet the operator's expected design.") },
                    { KernelExceptionType.UESHScript,                       Translate.DoTranslation("An error occurred while trying to parse the UESH script file. Check your script file for errors and correct them.") },
                    { KernelExceptionType.UserCreation,                     Translate.DoTranslation("A user creation error occurred.") },
                    { KernelExceptionType.UserManagement,                   Translate.DoTranslation("A user management error occurred.") },
                    { KernelExceptionType.Network,                          Translate.DoTranslation("A general network error occurred. Should you be trying to connect to a remote end, make sure that you're connected to the Internet or to your local network.") },
                    { KernelExceptionType.UnsupportedConsole,               Translate.DoTranslation("An unsupported console has been detected.") },
                    { KernelExceptionType.AssertionFailure,                 Translate.DoTranslation("An assertion failure has been detected in the kernel! This is most likely a bug in the kernel module that should be fixed on our end.") },
                    { KernelExceptionType.NetworkOffline,                   Translate.DoTranslation("Your network needs to be online before being able to perform operations related to networking. Connect your network adapter and try again.") },
                    { KernelExceptionType.PermissionDenied,                 Translate.DoTranslation("Permission denied trying to perform an operation.") },
                    { KernelExceptionType.NoSuchUser,                       Translate.DoTranslation("User doesn't exist. Check to make sure that you've written the user correctly.") },
                    { KernelExceptionType.NoSuchDriver,                     Translate.DoTranslation("Driver doesn't exist. Check to make sure that you've written the driver name correctly, and that the driver is registered properly.") },
                };
            }
        }

        internal static string GetFinalExceptionMessage(KernelExceptionType exceptionType, string message, Exception e, params object[] vars)
        {
            StringBuilder builder = new();

            // Display error type
            builder.AppendLine(Translate.DoTranslation("There is an error in the kernel or one of the kernel components. The below information may help you figure out why.") + "\n");
            builder.AppendLine(Translate.DoTranslation("The error type is") + $" {exceptionType} [{Convert.ToInt32(exceptionType)}]");
            builder.AppendLine((messages.ContainsKey(exceptionType) ? 
                                messages[exceptionType] : 
                                Translate.DoTranslation("Unfortunately, an invalid message type was given, so we don't exactly know what is the problem. Try turning on the debugger and reproducing the problem."))
                               + "\n");

            // Display error message
            if (!string.IsNullOrWhiteSpace(message))
                builder.AppendLine(Translate.DoTranslation("The module that caused the fault provided this additional information that may help you further") + $": {message.FormatString(vars)}\n");
            else
                builder.AppendLine(Translate.DoTranslation("The module that caused the fault didn't provide additional information.") + "\n");

            // Display exception
            if (e != null)
                builder.AppendLine(Translate.DoTranslation("Additionally, the faulty module provided this exception information") + $": {e.GetType().Name}: {e.Message}\n");
            else
                builder.AppendLine(Translate.DoTranslation("Also, the module didn't provide the exception information, so it's usually an indicator that something is wrong.") + "\n");

            builder.Append(Translate.DoTranslation("If the module tried to process your input, ensure that you've written all the parameters correctly."));
            return builder.ToString();
        }
    }
}
