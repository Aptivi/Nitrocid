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

using Terminaux.Inputs.Styles.Selection;
using System.Linq;
using Nitrocid.Base.Languages;

namespace Nitrocid.Base.Kernel.Debugging.Testing.Facades
{
    internal class ChangeLanguage : TestFacade
    {
        public override string TestName => LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTFACADES_CHANGELANGUAGE_DESC");
        public override TestSection TestSection => TestSection.Languages;
        public override void Run()
        {
            var langChoices = LanguageManager.Languages.Select(kvp => (kvp.Key, kvp.Value.EnglishName)).ToArray();
            int langIndex = SelectionStyle.PromptSelection(LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTFACADES_CHANGELANGUAGE_CHOOSE"), langChoices);
            if (langIndex < 0)
                return;
            string lang = LanguageManager.Languages.Keys.ElementAt(langIndex);
            LanguageManager.SetLangDry(lang);
        }
    }
}
