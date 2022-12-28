
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

using System.Collections.Generic;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Drivers.Encryption;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Probers;
using KS.Misc.Probers.Motd;
using KS.Misc.Screensaver;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.Network.RSS;
using KS.Shell.ShellBase.Shells;
using KS.Users.Groups;
using KS.Kernel.Events;

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
        public static string UsernamePrompt;
        /// <summary>
        /// Password prompt
        /// </summary>
        public static string PasswordPrompt;
        /// <summary>
        /// List of usernames and passwords
        /// </summary>
        internal static Dictionary<string, string> Users = new();
        /// <summary>
        /// Current username
        /// </summary>
        private static UserInfo CurrentUserInfo;

        /// <summary>
        /// Current username
        /// </summary>
        public static UserInfo CurrentUser => CurrentUserInfo;

        /// <summary>
        /// Prompts user for login information
        /// </summary>
        public static void LoginPrompt()
        {
            while (true)
            {
                // Check to see if the reboot is requested
                if (Flags.RebootRequested | Flags.KernelShutdown)
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Reboot has been requested. Exiting...");
                    Flags.RebootRequested = false;
                    return;
                }

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

                // Read MOTD and MAL
                MotdParse.ReadMotd();
                MalParse.ReadMal();

                // Show MOTD once
                DebugWriter.WriteDebug(DebugLevel.I, "showMOTDOnceFlag = {0}, showMOTD = {1}", Flags.ShowMOTDOnceFlag, Flags.ShowMOTD);
                if (Flags.ShowMOTDOnceFlag == true & Flags.ShowMOTD == true)
                {
                    TextWriterColor.Write(CharManager.NewLine + PlaceParse.ProbePlaces(MotdParse.MOTDMessage), true, KernelColorType.Banner);
                }
                Flags.ShowMOTDOnceFlag = false;

                // How do we prompt user to login?
                var UsersList = UserManagement.ListAllUsers();
                if (Flags.ChooseUser)
                {
                    // Generate user list
                    ListWriterColor.WriteList(UsersList);
                    int AnswerUserInt = 0;

                    // Prompt user to choose a user
                    while (AnswerUserInt == 0)
                    {
                        TextWriterColor.Write(">> ", false, KernelColorType.Input);
                        string AnswerUserString = Input.ReadLine();

                        // Parse input
                        if (!string.IsNullOrWhiteSpace(AnswerUserString))
                        {
                            if (int.TryParse(AnswerUserString, out AnswerUserInt))
                            {
                                string SelectedUser = UserManagement.SelectUser(AnswerUserInt);
                                DebugWriter.WriteDebug(DebugLevel.I, "Username correct. Finding if the user is disabled...");
                                if (!GroupManagement.HasGroup(SelectedUser, GroupManagement.GroupType.Disabled))
                                {
                                    DebugWriter.WriteDebug(DebugLevel.I, "User can log in. (User is not in disabled list)");
                                    ShowPasswordPrompt(SelectedUser);
                                }
                                else
                                {
                                    DebugWriter.WriteDebug(DebugLevel.W, "User can't log in. (User is in disabled list)");
                                    TextWriterColor.Write(Translate.DoTranslation("User is disabled."), true, KernelColorType.Error);
                                    EventsManager.FireEvent(EventType.LoginError, SelectedUser, LoginErrorReasons.Disabled);
                                }
                            }
                            else
                            {
                                TextWriterColor.Write(Translate.DoTranslation("The answer must be numeric."), true, KernelColorType.Error);
                            }
                        }
                        else
                        {
                            TextWriterColor.Write(Translate.DoTranslation("Please enter a user number."), true, KernelColorType.Error);
                        }
                    }
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
                    {
                        TextWriterColor.Write(PlaceParse.ProbePlaces(UsernamePrompt), false, KernelColorType.Input);
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Username: "), false, KernelColorType.Input);
                    }
                    string answeruser = Input.ReadLine();

                    // Parse input
                    if (answeruser.Contains(" "))
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Spaces found in username.");
                        TextWriterColor.Write(Translate.DoTranslation("Spaces are not allowed."), true, KernelColorType.Error);
                        EventsManager.FireEvent(EventType.LoginError, answeruser, LoginErrorReasons.Spaces);
                    }
                    else if (answeruser.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray()) != -1)
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Unknown characters found in username.");
                        TextWriterColor.Write(Translate.DoTranslation("Special characters are not allowed."), true, KernelColorType.Error);
                        EventsManager.FireEvent(EventType.LoginError, answeruser, LoginErrorReasons.SpecialCharacters);
                    }
                    else if (Users.ContainsKey(answeruser))
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Username correct. Finding if the user is disabled...");
                        if (!GroupManagement.HasGroup(answeruser, GroupManagement.GroupType.Disabled))
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "User can log in. (User is not in disabled list)");
                            ShowPasswordPrompt(answeruser);
                        }
                        else
                        {
                            DebugWriter.WriteDebug(DebugLevel.W, "User can't log in. (User is in disabled list)");
                            TextWriterColor.Write(Translate.DoTranslation("User is disabled."), true, KernelColorType.Error);
                            EventsManager.FireEvent(EventType.LoginError, answeruser, LoginErrorReasons.Disabled);
                        }
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Username not found.");
                        TextWriterColor.Write(Translate.DoTranslation("Wrong username."), true, KernelColorType.Error);
                        EventsManager.FireEvent(EventType.LoginError, answeruser, LoginErrorReasons.NotFound);
                    }
                }
            }
        }

        /// <summary>
        /// Prompts user for password
        /// </summary>
        /// <param name="usernamerequested">A username that is about to be logged in</param>
        public static void ShowPasswordPrompt(string usernamerequested)
        {
            // Prompts user to enter a user's password
            while (true)
            {
                // Check to see if reboot is requested
                if (Flags.RebootRequested | Flags.KernelShutdown)
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Reboot has been requested. Exiting...");
                    Flags.RebootRequested = false;
                    return;
                }

                // Get the password from dictionary
                string UserPassword = Users[usernamerequested];

                // Check if there's a password
                if (!(UserPassword == Encryption.GetEmptyHash("SHA256"))) // No password
                {
                    // Wait for input
                    DebugWriter.WriteDebug(DebugLevel.I, "Password not empty");
                    if (!string.IsNullOrWhiteSpace(PasswordPrompt))
                    {
                        TextWriterColor.Write(PlaceParse.ProbePlaces(PasswordPrompt), false, KernelColorType.Input);
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("{0}'s password: "), false, KernelColorType.Input, usernamerequested);
                    }

                    // Get input
                    string answerpass = Input.ReadLineNoInputUnsafe();

                    // Compute password hash
                    DebugWriter.WriteDebug(DebugLevel.I, "Computing written password hash...");
                    answerpass = Encryption.GetEncryptedString(answerpass, "SHA256");

                    // Parse password input
                    if (Users.TryGetValue(usernamerequested, out UserPassword) && UserPassword == answerpass)
                    {
                        // Log-in instantly
                        DebugWriter.WriteDebug(DebugLevel.I, "Password written correctly. Entering shell...");
                        SignIn(usernamerequested);
                        return;
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Passowrd written wrong...");
                        TextWriterColor.Write(Translate.DoTranslation("Wrong password."), true, KernelColorType.Error);
                        EventsManager.FireEvent(EventType.LoginError, usernamerequested, LoginErrorReasons.WrongPassword);
                        if (!Flags.Maintenance)
                        {
                            if (!Screensaver.LockMode)
                            {
                                return;
                            }
                        }
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
                EventsManager.FireEvent(EventType.PostUnlock, Screensaver.DefSaverName);
                return;
            }

            // Notifies the kernel that the user has signed in
            Flags.LoggedIn = true;

            // Sign in to user.
            CurrentUserInfo = new UserInfo(signedInUser);
            Flags.ShowMOTDOnceFlag = true;
            if (Flags.ShowMAL)
                TextWriterColor.Write(PlaceParse.ProbePlaces(MalParse.MAL), true, KernelColorType.Banner);
            RSSTools.ShowHeadlineLogin();

            // Fire event PostLogin
            EventsManager.FireEvent(EventType.PostLogin, CurrentUser.Username);

            // Initialize shell
            DebugWriter.WriteDebug(DebugLevel.I, "Shell is being initialized...");
            ShellStart.StartShellForced(ShellType.Shell);
            ShellStart.PurgeShells();
        }

    }
}
