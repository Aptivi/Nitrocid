
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
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Probers.Motd;
using KS.Misc.Screensaver;
using KS.Misc.Text;
using KS.Network.RSS;
using KS.Shell.ShellBase.Shells;
using KS.Kernel.Events;
using KS.Misc.Probers.Placeholder;
using KS.Kernel.Configuration;
using KS.Kernel.Time.Renderers;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.ConsoleBase.Writers.MiscWriters;
using System;

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
            while (!(Flags.RebootRequested | Flags.KernelShutdown))
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
                if (Flags.ClearOnLogin == true)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Clearing screen...");
                    ConsoleBase.ConsoleWrapper.Clear();
                }

                // Show MOTD once
                DebugWriter.WriteDebug(DebugLevel.I, "showMOTDOnceFlag = {0}, showMOTD = {1}", Flags.ShowMOTDOnceFlag, Flags.ShowMOTD);
                if (Flags.ShowMOTDOnceFlag == true & Flags.ShowMOTD == true)
                {
                    TextWriterColor.Write(CharManager.NewLine + PlaceParse.ProbePlaces(MotdParse.MOTDMessage), true, KernelColorType.Banner);
                    Flags.ShowMOTDOnceFlag = false;
                }

                // How do we prompt user to login?
                var UsersList = UserManagement.ListAllUsers();
                if (Flags.ModernLogon)
                {
                    // Invoke the modern logon handler
                    ModernLogonScreen.ShowLogon();
                }
                else
                {
                    // Generate user list
                    if (Flags.ShowAvailableUsers)
                    {
                        TextWriterColor.Write(Translate.DoTranslation("You can log in to these accounts:"));
                        ListWriterColor.WriteList(UsersList);
                    }

                    // Prompt user to login
                    if (!string.IsNullOrWhiteSpace(UsernamePrompt))
                        TextWriterColor.Write(PlaceParse.ProbePlaces(UsernamePrompt), false, KernelColorType.Input);
                    else
                        TextWriterColor.Write(Translate.DoTranslation("Username: "), false, KernelColorType.Input);
                    string answeruser = Input.ReadLine();

                    // Parse input
                    if (UserManagement.ValidateUsername(answeruser))
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Validation complete for user {0}", answeruser);
                        if (ShowPasswordPrompt(answeruser))
                            SignIn(answeruser);
                    }
                    else
                        TextWriterColor.Write(Translate.DoTranslation("Wrong username or username not found."), true, KernelColorType.Error);
                }
            }

            Flags.RebootRequested = false;
        }

        /// <summary>
        /// Prompts user for password
        /// </summary>
        /// <param name="usernamerequested">A username that is about to be logged in</param>
        public static bool ShowPasswordPrompt(string usernamerequested)
        {
            // Prompts user to enter a user's password
            while (!(Flags.RebootRequested | Flags.KernelShutdown))
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
                        if (!Flags.Maintenance)
                            if (!Screensaver.LockMode)
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

            Flags.RebootRequested = false;
            return false;
        }

        /// <summary>
        /// Signs in to the username
        /// </summary>
        /// <param name="signedInUser">A specified username</param>
        internal static void SignIn(string signedInUser)
        {
            // Release lock
            if (Screensaver.LockMode)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Releasing lock and getting back to shell...");
                Screensaver.LockMode = false;
                EventsManager.FireEvent(EventType.PostUnlock, Screensaver.DefaultSaverName);
                return;
            }

            // Notifies the kernel that the user has signed in
            Flags.LoggedIn = true;
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
            if (Flags.ShowCurrentTimeBeforeLogin)
                TimeDateMiscRenderers.ShowCurrentTimes();
            TextWriterColor.Write();

            // Show license information
            WelcomeMessage.WriteLicense();

            // Show development disclaimer
            WelcomeMessage.ShowDevelopmentDisclaimer();

            // Show MOTD
            Flags.ShowMOTDOnceFlag = true;
            if (Flags.ShowMAL)
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

    }
}
