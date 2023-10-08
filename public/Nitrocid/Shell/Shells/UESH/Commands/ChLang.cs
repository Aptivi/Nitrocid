
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
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Switches;
using KS.Users;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Changes your kernel language
    /// </summary>
    /// <remarks>
    /// If you want to change your kernel language without having to go to Settings (for scripting purposes), you can use this command.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the command.
    /// </remarks>
    class ChLangCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            bool inferSysLang = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-usesyslang");
            bool useUser = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-user");
            string language = inferSysLang ? "eng" : parameters.ArgumentsList[0];
            if (!LanguageManager.ListAllLanguages().ContainsKey(language))
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Invalid language") + $" {language}", true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.NoSuchLanguage;
            }

            // Change the language
            if (inferSysLang)
                language = LanguageManager.InferLanguageFromSystem();
            if (useUser)
            {
                UserManagement.CurrentUser.PreferredLanguage = language;
                UserManagement.SaveUsers();
            }
            else
                LanguageManager.SetLang(language);
            TextWriterColor.Write(Translate.DoTranslation("You may need to log out and log back in for changes to take effect."));
            return 0;
        }

    }
}
