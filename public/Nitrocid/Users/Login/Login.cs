//
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Drivers.Encryption;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Screensaver;
using KS.Kernel.Events;
using KS.Kernel.Configuration;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Users.Login.Handlers;
using System;
using KS.Misc.Text.Probers.Placeholder;
using KS.Kernel.Power;
using KS.Kernel;

namespace KS.Users.Login
{
    /// <summary>
    /// Login module
    /// </summary>
    public static class Login
    {

        internal static bool LogoutRequested;
        internal static bool LoggedIn;

        /// <summary>
        /// Username prompt
        /// </summary>
        public static string UsernamePrompt =>
            Config.MainConfig.UsernamePrompt;
        /// <summary>
        /// Password prompt
        /// </summary>
        public static string PasswordPrompt =>
            Config.MainConfig.PasswordPrompt;

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
                while (!PowerManager.RebootRequested && !PowerManager.KernelShutdown)
                {
                    // First, set root account
                    UserManagement.CurrentUserInfo =
                        UserManagement.UserExists("root") ?
                        UserManagement.GetUser("root") :
                        UserManagement.fallbackRootAccount;

                    // Now, show the Login screen
                    handler.LoginScreen();

                    // Prompt for username
                    user = handler.UserSelector();

                    // OK. Here's where things get tricky. Some handlers may misleadingly give us a completely invalid username, so verify it
                    // the second time for these handlers to behave.
                    if (!UserManagement.ValidateUsername(user))
                    {
                        TextWriterColor.WriteKernelColor(Translate.DoTranslation("Wrong username or username not found."), true, KernelColorType.Error);
                        continue;
                    }

                    // Prompt for password, assuming that the username is valid.
                    string pass = "";
                    bool valid = handler.PasswordHandler(user, ref pass);
                    valid = UserManagement.ValidatePassword(user, pass);
                    if (!valid)
                        TextWriterColor.WriteKernelColor(Translate.DoTranslation("Wrong password."), true, KernelColorType.Error);
                    else
                        break;
                }

                // Check for the state before the final login flow
                if (!PowerManager.RebootRequested && !PowerManager.KernelShutdown)
                    SignIn(user);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Handler is killed! {0}", ex.Message);
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
                // Get the password from dictionary
                int userIndex = UserManagement.GetUserIndex(usernamerequested);
                string UserPassword = UserManagement.Users[userIndex].Password;

                // Check if there's a password
                if (UserPassword != Encryption.GetEmptyHash("SHA256"))
                {
                    // Wait for input
                    DebugWriter.WriteDebug(DebugLevel.I, "Password not empty");
                    if (!string.IsNullOrWhiteSpace(PasswordPrompt))
                        TextWriterColor.WriteKernelColor(PlaceParse.ProbePlaces(PasswordPrompt), false, KernelColorType.Input);
                    else
                        TextWriterColor.WriteKernelColor(Translate.DoTranslation("{0}'s password: "), false, KernelColorType.Input, usernamerequested);

                    // Get input
                    string answerpass = Input.ReadLineNoInputUnsafe();

                    if (UserManagement.ValidatePassword(usernamerequested, answerpass))
                        return true;
                    else
                    {
                        TextWriterColor.WriteKernelColor(Translate.DoTranslation("Wrong password."), true, KernelColorType.Error);
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
            DebugWriter.WriteDebug(DebugLevel.I, "Logged in to {0}!", signedInUser);

            // Sign in to user.
            UserManagement.CurrentUserInfo = UserManagement.GetUser(signedInUser);

            // Set preferred language
            string preferredLanguage = UserManagement.CurrentUser.PreferredLanguage;
            DebugWriter.WriteDebug(DebugLevel.I, "Preferred language {0}. Trying to set dryly...", preferredLanguage);
            if (!string.IsNullOrWhiteSpace(preferredLanguage))
                LanguageManager.currentUserLanguage = LanguageManager.Languages[preferredLanguage];
            else
                LanguageManager.currentUserLanguage = LanguageManager.currentLanguage;

            // Fire event PostLogin
            EventsManager.FireEvent(EventType.PostLogin, UserManagement.CurrentUser.Username);
            DebugWriter.WriteDebug(DebugLevel.I, "Out of login flow.");
        }

        internal static void PromptMaintenanceLogin()
        {
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
                        TextWriterColor.Write(Translate.DoTranslation("Incorrect admin password. You have {0} tries."), 3 - (tries + 1), true, KernelColorType.Error);
                        if (tries == 2)
                            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Out of chances. Rebooting..."), true, KernelColorType.Error);
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
