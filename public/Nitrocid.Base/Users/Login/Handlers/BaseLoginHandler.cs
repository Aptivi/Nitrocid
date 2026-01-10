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

using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Textify.Tools.Placeholder;
using Terminaux.Base;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Screensaver;
using Nitrocid.Base.Users.Login.Motd;
using Nitrocid.Base.ConsoleBase.Inputs;
using Nitrocid.Base.Kernel.Power;
using Nitrocid.Base.Drivers.Encryption;

namespace Nitrocid.Base.Users.Login.Handlers
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
                TextWriterColor.Write(PlaceParse.ProbePlaces(MotdParse.MotdMessage), true, ThemeColorType.Banner);
                MotdParse.ProcessDynamicMotd();
                ShowMOTDOnceFlag = false;
            }

            // Generate user list
            if (Config.MainConfig.ShowAvailableUsers)
            {
                var UsersList = UserManagement.ListAllUsers();
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_USERS_LOGIN_ACCOUNTLISTING"));
                ListWriterColor.WriteList(UsersList);
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
                        TextWriterColor.Write(PlaceParse.ProbePlaces(Config.MainConfig.PasswordPrompt), false, ThemeColorType.Input);
                    else
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_USERS_LOGIN_PASSWORDPROMPT"), false, ThemeColorType.Input, user);

                    // Get input
                    string answerpass = InputTools.ReadLineNoInputUnsafe();
                    pass = answerpass;
                    if (UserManagement.ValidatePassword(user, answerpass))
                        return true;
                    else
                    {
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_USERS_LOGIN_WRONGPASSWORD"), true, ThemeColorType.Error);
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
                TextWriterColor.Write(PlaceParse.ProbePlaces(Config.MainConfig.UsernamePrompt), false, ThemeColorType.Input);
            else
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_USERS_LOGIN_USERNAMEPROMPT"), false, ThemeColorType.Input);
            return InputTools.ReadLine();
        }
    }
}
