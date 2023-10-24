
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

using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Inputs.Styles;
using KS.Languages;
using System.Linq;

namespace KS.Kernel.Debugging.Testing.Facades
{
    internal class ChangeLanguage : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Tests changing the language to your preferred language");
        public override TestSection TestSection => TestSection.Languages;
        public override void Run(params string[] args)
        {
            var langCodes = InputChoiceTools.GetInputChoices(string.Join("/", LanguageManager.Languages.Keys), LanguageManager.Languages.Values.Select((lang) => lang.FullLanguageName).ToArray());
            int langIndex = SelectionStyle.PromptSelection(Translate.DoTranslation("Choose your language"), langCodes) - 1;
            if (langIndex < 0)
                return;
            string lang = LanguageManager.Languages.Keys.ElementAt(langIndex);
            LanguageManager.SetLangDry(lang);
        }
    }
}
