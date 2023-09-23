
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Drivers.Encryption;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Screensaver;
using KS.Misc.Text;
using KS.Network.RSS;
using KS.Shell.ShellBase.Shells;
using KS.Kernel.Events;
using KS.Kernel.Configuration;
using KS.Kernel.Time.Renderers;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.ConsoleBase.Writers.MiscWriters;
using KS.Users.Login.Handlers;
using System;
using KS.Misc.Text.Probers.Motd;
using KS.Misc.Text.Probers.Placeholder;

namespace KS.Users.Login
{
    /// <summary>
    /// Login module
    /// </summary>
    public static class Login
    {

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
            while (!(KernelFlags.RebootRequested | KernelFlags.KernelShutdown))
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

                // Clear console if ClearOnLogin is set to True (If a user has enabled Clear Screen on Login)
                if (KernelFlags.ClearOnLogin == true)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Clearing screen...");
                    ConsoleBase.ConsoleWrapper.Clear();
                }

                // Show MOTD once
                DebugWriter.WriteDebug(DebugLevel.I, "showMOTDOnceFlag = {0}, showMOTD = {1}", KernelFlags.ShowMOTDOnceFlag, KernelFlags.ShowMOTD);
                if (KernelFlags.ShowMOTDOnceFlag == true & KernelFlags.ShowMOTD == true)
                {
                    TextWriterColor.Write(CharManager.NewLine + PlaceParse.ProbePlaces(MotdParse.MOTDMessage), true, KernelColorType.Banner);
                    KernelFlags.ShowMOTDOnceFlag = false;
                }

                // How do we prompt user to login?
                string handlerName = "classic";
                if (KernelFlags.ModernLogon)
                {
                    // Invoke the modern logon handler
                    handlerName = "modern";
                }

                // Get the handler!
                var handler = LoginHandlerTools.GetHandler(handlerName);

                try
                {
                    // Sanity check...
                    if (handler is null)
                        throw new KernelException(KernelExceptionType.LoginHandler, Translate.DoTranslation("The login handler is not found!") + $" {handlerName}");

                    // Now, show the Login screen
                    handler.LoginScreen();

                    // Prompt for username
                    string user = handler.UserSelector();

                    // OK. Here's where things get tricky. Some handlers may misleadingly give us a completely invalid username, so verify it
                    // the second time for these handlers to behave.
                    if (!UserManagement.ValidateUsername(user))
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Wrong username or username not found."), true, KernelColorType.Error);
                        continue;
                    }

                    // Prompt for password, assuming that the username is valid.
                    bool valid = handler.PasswordHandler(user);
                    if (valid)
                    {
                        // TODO: Handle invalid statuses given by some trickster.
                        SignIn(user);
                    }
                    else
                        TextWriterColor.Write(Translate.DoTranslation("Wrong password."), true, KernelColorType.Error);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Handler is killed! {0}", ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, "Kernel panicking...");
                    KernelPanic.KernelError(KernelErrorLevel.F, true, 10, Translate.DoTranslation("Login handler has crashed!") + $" {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Prompts user for password
        /// </summary>
        /// <param name="usernamerequested">A username that is about to be logged in</param>
        public static bool ShowPasswordPrompt(string usernamerequested)
        {
            // Prompts user to enter a user's password
            while (!(KernelFlags.RebootRequested | KernelFlags.KernelShutdown))
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
                        TextWriterColor.Write(PlaceParse.ProbePlaces(PasswordPrompt), false, KernelColorType.Input);
                    else
                        TextWriterColor.Write(Translate.DoTranslation("{0}'s password: "), false, KernelColorType.Input, usernamerequested);

                    // Get input
                    string answerpass = Input.ReadLineNoInputUnsafe();

                    if (UserManagement.ValidatePassword(usernamerequested, answerpass))
                        return true;
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Wrong password."), true, KernelColorType.Error);
                        if (!KernelFlags.Maintenance)
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
            KernelFlags.LoggedIn = true;
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

            // Show current time
            SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Welcome!"), true, KernelColorType.Stage);
            if (KernelFlags.ShowCurrentTimeBeforeLogin)
                TimeDateMiscRenderers.ShowCurrentTimes();
            TextWriterColor.Write();

            // Show license information
            WelcomeMessage.WriteLicense();

            // Show development disclaimer
            WelcomeMessage.ShowDevelopmentDisclaimer();

            // TODO: Remove this when .NET 8.0 releases on November and Nitrocid KS gets re-targeted to that version on December.
#if NET7_0
            TextWriterColor.Write("* You're running a .NET 7.0 version of Nitrocid KS. This is going to be used as a testing ground to ensure that we can have smooth upgrade experience to .NET 8.0. Meanwhile, you can evaluate this version until .NET 8.0 gets released on November.");
#endif

            // Show the tip
            if (KernelFlags.ShowTip)
                WelcomeMessage.ShowTip();

            // Show MOTD
            KernelFlags.ShowMOTDOnceFlag = true;
            if (KernelFlags.ShowMAL)
                TextWriterColor.Write(PlaceParse.ProbePlaces(MalParse.MAL), true, KernelColorType.Banner);
            DebugWriter.WriteDebug(DebugLevel.I, "Loaded MAL.");

            // Show headline
            RSSTools.ShowHeadlineLogin();
            DebugWriter.WriteDebug(DebugLevel.I, "Loaded headline.");

            // Initialize shell
            DebugWriter.WriteDebug(DebugLevel.I, "Shell is being initialized. Login username {0}...", signedInUser);
            ShellStart.StartShellForced(ShellType.Shell);
            ShellStart.PurgeShells();
            DebugWriter.WriteDebug(DebugLevel.I, "Out of login flow.");
        }

        internal static void PromptMaintenanceLogin()
        {
            TextWriterColor.Write(Translate.DoTranslation("Enter the admin password for maintenance."));
            string user = "root";
            if (UserManagement.UserExists(user))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Root account found. Prompting for password...");
                for (int tries = 0; tries < 3 && !KernelFlags.RebootRequested; tries++)
                {
                    if (ShowPasswordPrompt(user))
                        SignIn(user);
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Incorrect admin password. You have {0} tries."), 3 - (tries + 1), true, KernelColorType.Error);
                        if (tries == 2)
                            TextWriterColor.Write(Translate.DoTranslation("Out of chances. Rebooting..."), true, KernelColorType.Error);
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
