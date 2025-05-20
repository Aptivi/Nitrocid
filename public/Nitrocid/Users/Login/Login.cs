﻿//
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

using System;
using Nitrocid.Kernel;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Screensaver;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Drivers.Encryption;
using Nitrocid.Users.Login.Handlers;
using Nitrocid.Kernel.Events;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Power;
using Nitrocid.Security.Permissions;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Infobox.Tools;

namespace Nitrocid.Users.Login
{
    /// <summary>
    /// Login module
    /// </summary>
    public static class Login
    {

        internal static bool LogoutRequested;
        internal static bool LoggedIn;

        /// <summary>
        /// Prompts user for login information
        /// </summary>
        public static void LoginPrompt()
        {
            // Fire event PreLogin
            EventsManager.FireEvent(EventType.PreLogin);

            // Check to see if there are any users
            if (UserManagement.Users.Count == 0)
            {
                // Extremely rare state reached
                DebugWriter.WriteDebug(DebugLevel.F, "Shell reached rare state, because userword count is 0.");
                throw new KernelException(KernelExceptionType.NullUsers, Translate.DoTranslation("There are no more users remaining in the list."));
            }

            // Get the handler!
            try
            {
                // Sanity check...
                string handlerName = LoginHandlerTools.CurrentHandlerName;
                var handler = LoginHandlerTools.GetHandler(handlerName) ??
                    throw new KernelException(KernelExceptionType.LoginHandler, Translate.DoTranslation("The login handler is not found!") + $" {handlerName}");

                // Login loop until either power action (in case login handler tries to shut the kernel down) or sign in action
                string user = "";
                ModernLogonScreen.screenNum = 1;
                while (!PowerManager.RebootRequested && !PowerManager.KernelShutdown)
                {
                    // First, set root account
                    var userInfo =
                        (UserManagement.UserExists("root") ?
                         UserManagement.GetUser("root") :
                         UserManagement.fallbackRootAccount) ??
                        throw new KernelException(KernelExceptionType.LoginHandler, Translate.DoTranslation("Can't set root account."));
                    UserManagement.CurrentUserInfo = userInfo;

                    // Now, show the Login screen
                    bool proceed = handler.LoginScreen();

                    // The login screen may provide an option to refresh itself.
                    if (!proceed && !PowerManager.RebootRequested && !PowerManager.KernelShutdown)
                        continue;

                    // Prompt for username
                    user = handler.UserSelector();

                    // Handlers may return an empty username. This may indicate that the user has exited. In this case, go to the beginning.
                    if (string.IsNullOrEmpty(user))
                    {
                        // Cancel shutdown and reboot attempts
                        PowerManager.RebootRequested = false;
                        PowerManager.KernelShutdown = false;
                        continue;
                    }

                    // OK. Here's where things get tricky. Some handlers may misleadingly give us a completely invalid username, so verify it
                    // the second time for these handlers to behave.
                    if (!UserManagement.ValidateUsername(user))
                    {
                        // Cancel shutdown and reboot attempts
                        PowerManager.RebootRequested = false;
                        PowerManager.KernelShutdown = false;
                        TextWriters.Write(Translate.DoTranslation("Wrong username or username not found."), true, KernelColorType.Error);
                        continue;
                    }

                    // Prompt for password, assuming that the username is valid.
                    string pass = "";
                    bool valid = handler.PasswordHandler(user, ref pass);
                    valid = UserManagement.ValidatePassword(user, pass);
                    if (!valid)
                    {
                        // Cancel shutdown and reboot attempts
                        PowerManager.RebootRequested = false;
                        PowerManager.KernelShutdown = false;
                        TextWriters.Write(Translate.DoTranslation("Wrong password."), true, KernelColorType.Error);
                    }
                    else if (!PermissionsTools.IsPermissionGranted(user, PermissionTypes.ManagePower) && (PowerManager.RebootRequested || PowerManager.KernelShutdown))
                    {
                        // Cancel shutdown and reboot attempts
                        PowerManager.RebootRequested = false;
                        PowerManager.KernelShutdown = false;
                        InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("You don't have permission to request a reboot or a shutdown."), new InfoBoxSettings()
                        {
                            ForegroundColor = KernelColorTools.GetColor(KernelColorType.Error)
                        });
                    }
                    else
                        break;
                }

                // Check for the state before the final login flow
                if (!PowerManager.RebootRequested && !PowerManager.KernelShutdown)
                    SignIn(user);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Handler is killed! {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Kernel panicking...");
                KernelPanic.KernelError(KernelErrorLevel.F, true, 10, Translate.DoTranslation("Login handler has crashed!") + $" {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Prompts user for password
        /// </summary>
        /// <param name="usernamerequested">A username that is about to be logged in</param>
        public static bool ShowPasswordPrompt(string usernamerequested)
        {
            // Prompts user to enter a user's password
            while (!PowerManager.RebootRequested && !PowerManager.KernelShutdown)
            {
                // Sanity check...
                string handlerName = LoginHandlerTools.CurrentHandlerName;
                var handler = LoginHandlerTools.GetHandler(handlerName) ??
                    throw new KernelException(KernelExceptionType.LoginHandler, Translate.DoTranslation("The login handler is not found!") + $" {handlerName}");

                // Get the password from dictionary
                int userIndex = UserManagement.GetUserIndex(usernamerequested);
                string UserPassword = UserManagement.Users[userIndex].Password;

                // Check if there's a password
                if (UserPassword != Encryption.GetEmptyHash("SHA256"))
                {
                    // Wait for input
                    DebugWriter.WriteDebug(DebugLevel.I, "Password not empty");
                    string answerpass = "";
                    handler.PasswordHandler(usernamerequested, ref answerpass);
                    if (UserManagement.ValidatePassword(usernamerequested, answerpass))
                        return true;
                    else
                    {
                        TextWriters.Write(Translate.DoTranslation("Wrong password."), true, KernelColorType.Error);
                        if (!KernelEntry.Maintenance)
                        {
                            if (!ScreensaverManager.LockMode)
                                return false;
                        }
                        else
                            return false;
                    }
                }
                else
                {
                    // Log-in instantly
                    DebugWriter.WriteDebug(DebugLevel.I, "Password is empty");
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Signs in to the username
        /// </summary>
        /// <param name="signedInUser">A specified username</param>
        internal static void SignIn(string signedInUser)
        {
            // Release lock
            if (ScreensaverManager.LockMode)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Releasing lock and getting back to shell...");
                ScreensaverManager.LockMode = false;
                EventsManager.FireEvent(EventType.PostUnlock, ScreensaverManager.DefaultSaverName);
                return;
            }

            // Notifies the kernel that the user has signed in
            LoggedIn = true;
            DebugWriter.WriteDebug(DebugLevel.I, "Logged in to {0}!", vars: [signedInUser]);

            // Sign in to user.
            UserManagement.CurrentUserInfo = UserManagement.GetUser(signedInUser) ??
                throw new KernelException(KernelExceptionType.LoginHandler, Translate.DoTranslation("Can't get user info for") + $" {signedInUser}");

            // Set preferred language
            string preferredLanguage = UserManagement.CurrentUser.PreferredLanguage ?? "";
            DebugWriter.WriteDebug(DebugLevel.I, "Preferred language {0}. Trying to set dryly...", vars: [preferredLanguage]);
            if (!string.IsNullOrWhiteSpace(preferredLanguage))
                LanguageManager.currentUserLanguage = LanguageManager.Languages[preferredLanguage];
            else
                LanguageManager.currentUserLanguage = LanguageManager.currentLanguage;

            // Set preferred culture
            string preferredCulture = UserManagement.CurrentUser.PreferredCulture ?? "";
            DebugWriter.WriteDebug(DebugLevel.I, "Preferred culture {0}. Trying to set dryly...", vars: [preferredCulture]);
            if (!string.IsNullOrWhiteSpace(preferredCulture))
                CultureManager.currentUserCulture = CultureManager.GetCulturesDictionary()[preferredCulture];
            else
                CultureManager.currentUserCulture = CultureManager.currentCulture;

            // Fire event PostLogin
            EventsManager.FireEvent(EventType.PostLogin, UserManagement.CurrentUser.Username);
            DebugWriter.WriteDebug(DebugLevel.I, "Out of login flow.");
        }

        internal static void PromptMaintenanceLogin()
        {
            if (Config.MainConfig.EnableSplash)
                KernelColorTools.LoadBackground();
            TextWriterColor.Write(Translate.DoTranslation("Enter the admin password for maintenance."));
            string user = "root";
            if (UserManagement.UserExists(user))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Root account found. Prompting for password...");
                for (int tries = 0; tries < 3 && !PowerManager.RebootRequested; tries++)
                {
                    if (ShowPasswordPrompt(user))
                        SignIn(user);
                    else
                    {
                        KernelColorTools.LoadBackground();
                        TextWriterColor.Write(Translate.DoTranslation("Incorrect admin password. You have {0} tries."), 3 - (tries + 1), true, KernelColorType.Error);
                        if (tries == 2)
                        {
                            TextWriters.Write(Translate.DoTranslation("Out of chances. Rebooting..."), true, KernelColorType.Error);
                            PowerManager.PowerManage(PowerMode.Reboot);
                        }
                    }
                }
            }
            else
            {
                // Some malicious mod removed the root account, or rare situation happened and it was gone.
                DebugWriter.WriteDebug(DebugLevel.F, "Root account not found for maintenance.");
                throw new KernelException(KernelExceptionType.NoSuchUser, Translate.DoTranslation("Some malicious mod removed the root account, or rare situation happened and it was gone."));
            }
        }

    }
}
