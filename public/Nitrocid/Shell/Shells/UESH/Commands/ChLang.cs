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
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Switches;
using Nitrocid.Users;
using Terminaux.Inputs.Styles.Choice;
using System.Linq;
using Terminaux.Inputs.Styles;

namespace Nitrocid.Shell.Shells.UESH.Commands
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
            bool useUser = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-user");
            bool useCountry = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-country");
            string language = "eng";
            if (useCountry)
            {
                // Country selection is entirely different, because a country might contain more than one language
                var countries = LanguageManager.ListAllCountries();
                string country = parameters.ArgumentsList[0];
                if (!countries.TryGetValue(country, out LanguageInfo[]? countryLanguages))
                {
                    TextWriters.Write(Translate.DoTranslation("Invalid country") + $" {country}", true, KernelColorType.Error);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.LanguageManagement);
                }

                if (countryLanguages.Length > 1)
                {
                    var langChoices = countryLanguages.Select((li, idx) => new InputChoiceInfo($"{idx + 1}", $"{li.FullLanguageName} [{li.ThreeLetterLanguageName}]")).ToArray();
                    string choice = ChoiceStyle.PromptChoice(Translate.DoTranslation("Choose a language for country") + $" {country}", langChoices, new()
                    {
                        OutputType = ChoiceOutputType.Modern,
                        PressEnter = true,
                    });
                    if (!int.TryParse(choice, out int langNum) || langNum < 1 || langNum >= langChoices.Length)
                    {
                        TextWriters.Write(Translate.DoTranslation("Invalid language selection for") + $" {country}", true, KernelColorType.Error);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.LanguageManagement);
                    }
                    language = countryLanguages[langNum - 1].ThreeLetterLanguageName;
                }
                else
                    language = countryLanguages[0].ThreeLetterLanguageName;
            }
            else
            {
                // Language selection takes only one language
                language = parameters.ArgumentsList[0];
                var languages = LanguageManager.ListAllLanguages();
                if (!languages.ContainsKey(language))
                {
                    TextWriters.Write(Translate.DoTranslation("Invalid language") + $" {language}", true, KernelColorType.Error);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.NoSuchLanguage);
                }
            }

            // Change the language
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
