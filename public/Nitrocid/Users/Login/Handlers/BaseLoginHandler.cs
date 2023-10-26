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

using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers.Encryption;
using KS.Kernel;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Power;
using KS.Languages;
using KS.Misc.Screensaver;
using KS.Misc.Text;
using KS.Misc.Text.Probers.Motd;
using KS.Misc.Text.Probers.Placeholder;

namespace KS.Users.Login.Handlers
{
    /// <summary>
    /// Abstract class of the base login handler
    /// </summary>
    public abstract class BaseLoginHandler : ILoginHandler
    {

        internal static bool ShowMOTDOnceFlag = true;

        /// <summary>
        /// Clear Screen On Log-in
        /// </summary>
        public static bool ClearOnLogin =>
            Config.MainConfig.ClearOnLogin;
        /// <summary>
        /// Show MOTD on log-in
        /// </summary>
        public static bool ShowMOTD =>
            Config.MainConfig.ShowMOTD;
        /// <summary>
        /// Show MAL on log-in
        /// </summary>
        public static bool ShowMAL =>
            Config.MainConfig.ShowMAL;
        /// <summary>
        /// Whether or not to show available usernames on login
        /// </summary>
        public static bool ShowAvailableUsers =>
            Config.MainConfig.ShowAvailableUsers;

        /// <inheritdoc/>
        public virtual void LoginScreen()
        {
            // Clear console if ClearOnLogin is set to True (If a user has enabled Clear Screen on Login)
            if (ClearOnLogin)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Clearing screen...");
                ConsoleWrapper.Clear();
            }

            // Show MOTD once
            DebugWriter.WriteDebug(DebugLevel.I, "showMOTDOnceFlag = {0}, showMOTD = {1}", ShowMOTDOnceFlag, ShowMOTD);
            if (ShowMOTDOnceFlag && ShowMOTD)
            {
                // This is not going to happen when the modern logon is enabled.
                TextWriterColor.WriteKernelColor(CharManager.NewLine + PlaceParse.ProbePlaces(MotdParse.MotdMessage), true, KernelColorType.Banner);
                ShowMOTDOnceFlag = false;
            }

            // Generate user list
            if (ShowAvailableUsers)
            {
                var UsersList = UserManagement.ListAllUsers();
                TextWriterColor.Write(Translate.DoTranslation("You can log in to these accounts:"));
                ListWriterColor.WriteList(UsersList);
            }
        }

        /// <inheritdoc/>
        public virtual bool PasswordHandler(string user, ref string pass)
        {
            // Prompts user to enter a user's password
            while (!(PowerManager.RebootRequested | PowerManager.KernelShutdown))
            {
                // Get the password from dictionary
                int userIndex = UserManagement.GetUserIndex(user);
                string UserPassword = UserManagement.Users[userIndex].Password;

                // Check if there's a password
                if (UserPassword != Encryption.GetEmptyHash("SHA256"))
                {
                    // Wait for input
                    DebugWriter.WriteDebug(DebugLevel.I, "Password not empty");
                    if (!string.IsNullOrWhiteSpace(Login.PasswordPrompt))
                        TextWriterColor.WriteKernelColor(PlaceParse.ProbePlaces(Login.PasswordPrompt), false, KernelColorType.Input);
                    else
                        TextWriterColor.WriteKernelColor(Translate.DoTranslation("{0}'s password: "), false, KernelColorType.Input, user);

                    // Get input
                    string answerpass = Input.ReadLineNoInputUnsafe();
                    pass = answerpass;
                    if (UserManagement.ValidatePassword(user, answerpass))
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

        /// <inheritdoc/>
        public virtual string UserSelector()
        {
            // Prompt user to login
            if (!string.IsNullOrWhiteSpace(Login.UsernamePrompt))
                TextWriterColor.WriteKernelColor(PlaceParse.ProbePlaces(Login.UsernamePrompt), false, KernelColorType.Input);
            else
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Username: "), false, KernelColorType.Input);
            return Input.ReadLine();
        }
    }
}
