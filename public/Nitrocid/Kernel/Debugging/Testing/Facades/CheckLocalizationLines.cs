
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
using KS.Languages;
using Newtonsoft.Json.Linq;
using KS.Resources;
using System.Linq;

namespace KS.Kernel.Debugging.Testing.Facades
{
    internal class CheckLocalizationLines : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Checks all the localization text line numbers to see if they're all equal");
        public override TestSection TestSection => TestSection.Languages;
        public override void Run(params string[] args)
        {
            var EnglishJson = JToken.Parse(LanguageResources.eng);
            JToken LanguageJson;
            foreach (string LanguageName in LanguageManager.Languages.Keys)
            {
                LanguageJson = JToken.Parse(LanguageResources.ResourceManager.GetString(LanguageName.Replace("-", "_")));
                if (LanguageJson.Count() != EnglishJson.Count())
                {
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("Line mismatch in") + " {0}: {1} <> {2}", true, KernelColorType.Warning, LanguageName, LanguageJson.Count(), EnglishJson.Count());
                }
            }
        }
    }
}
