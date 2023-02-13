
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

using System.Collections.Generic;
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
using KS.Misc.Writers.ConsoleWriters;
using KS.Network.RSS;
using KS.Shell.ShellBase.Shells;
using KS.Kernel.Events;
using KS.Misc.Writers.MiscWriters;
using System;
using KS.Misc.Probers.Placeholder;

namespace KS.Users.Login
{
    /// <summary>
    /// Login module
    /// </summary>
    public static class Login
    {

        /// <summary>
        /// List of usernames
        /// </summary>
        internal static Dictionary<string, UserInfo> Users = new();
        /// <summary>
        /// Current username
        /// </summary>
        private static UserInfo CurrentUserInfo = new("root", Encryption.GetEncryptedString("", "SHA256"), Array.Empty<string>(), "System Account", "");

        /// <summary>
        /// Current username
        /// </summary>
        public static UserInfo CurrentUser => CurrentUserInfo;
        /// <summary>
        /// Username prompt
        /// </summary>
        public static string UsernamePrompt { get; set; } = "";
        /// <summary>
        /// Password prompt
        /// </summary>
        public static string PasswordPrompt { get; set; } = "";

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
                if (Users.Count == 0)
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
                        ShowPasswordPrompt(answeruser);
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
        public static void ShowPasswordPrompt(string usernamerequested)
        {
            // Prompts user to enter a user's password
            while (!(Flags.RebootRequested | Flags.KernelShutdown))
            {
                // Get the password from dictionary
                string UserPassword = Users[usernamerequested].Password;

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
                    {
                        SignIn(usernamerequested);
                        return;
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Wrong password."), true, KernelColorType.Error);
                        if (!Flags.Maintenance)
                            if (!Screensaver.LockMode)
                                return;
                    }
                }
                else
                {
                    // Log-in instantly
                    DebugWriter.WriteDebug(DebugLevel.I, "Password is empty");
                    SignIn(usernamerequested);
                    return;
                }
            }

            Flags.RebootRequested = false;
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

            // Sign in to user.
            CurrentUserInfo = Users[signedInUser];

            // Set preferred language
            if (!string.IsNullOrWhiteSpace(Login.CurrentUser.PreferredLanguage))
                LanguageManager.currentUserLanguage = LanguageManager.Languages[CurrentUserInfo.PreferredLanguage];
            else
                LanguageManager.currentUserLanguage = LanguageManager.currentLanguage;

            // Fire event PostLogin
            EventsManager.FireEvent(EventType.PostLogin, CurrentUser.Username);

            // Show license information
            WelcomeMessage.WriteLicense();

            // Show MOTD
            Flags.ShowMOTDOnceFlag = true;
            if (Flags.ShowMAL)
                TextWriterColor.Write(PlaceParse.ProbePlaces(MalParse.MAL), true, KernelColorType.Banner);
            if (!Flags.ModernLogon)
                RSSTools.ShowHeadlineLogin();

            // Initialize shell
            DebugWriter.WriteDebug(DebugLevel.I, "Shell is being initialized...");
            ShellStart.StartShellForced(ShellType.Shell);
            ShellStart.PurgeShells();
        }

    }
}
