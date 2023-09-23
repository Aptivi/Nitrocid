
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
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers.Encryption;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Screensaver;
using KS.Misc.Text.Probers.Placeholder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KS.Users.Login.Handlers
{
    /// <summary>
    /// Abstract class of the base login handler
    /// </summary>
    public abstract class BaseLoginHandler : ILoginHandler
    {
        /// <inheritdoc/>
        public virtual void LoginScreen()
        {
            // Generate user list
            if (KernelFlags.ShowAvailableUsers)
            {
                var UsersList = UserManagement.ListAllUsers();
                TextWriterColor.Write(Translate.DoTranslation("You can log in to these accounts:"));
                ListWriterColor.WriteList(UsersList);
            }
        }

        /// <inheritdoc/>
        public virtual bool PasswordHandler(string user)
        {
            // Prompts user to enter a user's password
            while (!(KernelFlags.RebootRequested | KernelFlags.KernelShutdown))
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
                        TextWriterColor.Write(PlaceParse.ProbePlaces(Login.PasswordPrompt), false, KernelColorType.Input);
                    else
                        TextWriterColor.Write(Translate.DoTranslation("{0}'s password: "), false, KernelColorType.Input, user);

                    // Get input
                    string answerpass = Input.ReadLineNoInputUnsafe();

                    if (UserManagement.ValidatePassword(user, answerpass))
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

        /// <inheritdoc/>
        public virtual string UserSelector()
        {
            // Prompt user to login
            if (!string.IsNullOrWhiteSpace(Login.UsernamePrompt))
                TextWriterColor.Write(PlaceParse.ProbePlaces(Login.UsernamePrompt), false, KernelColorType.Input);
            else
                TextWriterColor.Write(Translate.DoTranslation("Username: "), false, KernelColorType.Input);
            return Input.ReadLine();
        }
    }
}
