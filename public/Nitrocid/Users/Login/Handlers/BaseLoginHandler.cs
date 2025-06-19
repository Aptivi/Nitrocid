//
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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Inputs;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Drivers.Encryption;
using Nitrocid.Kernel;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Power;
using Nitrocid.Languages;
using Nitrocid.Misc.Screensaver;
using Textify.Tools.Placeholder;
using Terminaux.Base;
using Nitrocid.Users.Login.Motd;

namespace Nitrocid.Users.Login.Handlers
{
    /// <summary>
    /// Abstract class of the base login handler
    /// </summary>
    public abstract class BaseLoginHandler : ILoginHandler
    {

        internal static bool ShowMOTDOnceFlag = true;

        /// <inheritdoc/>
        public virtual bool LoginScreen()
        {
            // Clear console if ClearOnLogin is set to True (If a user has enabled Clear Screen on Login)
            if (Config.MainConfig.ClearOnLogin)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Clearing screen...");
                ConsoleWrapper.Clear();
            }

            // Show MOTD once
            DebugWriter.WriteDebug(DebugLevel.I, "showMOTDOnceFlag = {0}, showMOTD = {1}", vars: [ShowMOTDOnceFlag, Config.MainConfig.ShowMOTD]);
            if (ShowMOTDOnceFlag && Config.MainConfig.ShowMOTD)
            {
                // This is not going to happen when the modern logon is enabled.
                TextWriters.Write(PlaceParse.ProbePlaces(MotdParse.MotdMessage), true, KernelColorType.Banner);
                MotdParse.ProcessDynamicMotd();
                ShowMOTDOnceFlag = false;
            }

            // Generate user list
            if (Config.MainConfig.ShowAvailableUsers)
            {
                var UsersList = UserManagement.ListAllUsers();
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_USERS_LOGIN_ACCOUNTLISTING"));
                TextWriters.WriteList(UsersList);
            }
            return true;
        }

        /// <inheritdoc/>
        public virtual bool PasswordHandler(string user, ref string pass)
        {
            // Prompts user to enter a user's password
            while (!PowerManager.RebootRequested && !PowerManager.KernelShutdown)
            {
                // Get the password from dictionary
                int userIndex = UserManagement.GetUserIndex(user);
                string UserPassword = UserManagement.Users[userIndex].Password;

                // Check if there's a password
                if (UserPassword != Encryption.GetEmptyHash("SHA256"))
                {
                    // Wait for input
                    DebugWriter.WriteDebug(DebugLevel.I, "Password not empty");
                    if (!string.IsNullOrWhiteSpace(Config.MainConfig.PasswordPrompt))
                        TextWriters.Write(PlaceParse.ProbePlaces(Config.MainConfig.PasswordPrompt), false, KernelColorType.Input);
                    else
                        TextWriters.Write(LanguageTools.GetLocalized("NKS_USERS_LOGIN_PASSWORDPROMPT"), false, KernelColorType.Input, user);

                    // Get input
                    string answerpass = InputTools.ReadLineNoInputUnsafe();
                    pass = answerpass;
                    if (UserManagement.ValidatePassword(user, answerpass))
                        return true;
                    else
                    {
                        TextWriters.Write(LanguageTools.GetLocalized("NKS_USERS_LOGIN_WRONGPASSWORD"), true, KernelColorType.Error);
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
            if (!string.IsNullOrWhiteSpace(Config.MainConfig.UsernamePrompt))
                TextWriters.Write(PlaceParse.ProbePlaces(Config.MainConfig.UsernamePrompt), false, KernelColorType.Input);
            else
                TextWriters.Write(LanguageTools.GetLocalized("NKS_USERS_LOGIN_USERNAMEPROMPT"), false, KernelColorType.Input);
            return InputTools.ReadLine();
        }
    }
}
