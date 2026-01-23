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

using Newtonsoft.Json.Linq;
using System.Linq;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Misc.Reflection.Internal;
using Nitrocid.Kernel.Exceptions;

namespace Nitrocid.Kernel.Debugging.Testing.Facades
{
    internal class CheckLocalizationLines : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Checks all the localization text line numbers to see if they're all equal");
        public override TestSection TestSection => TestSection.Languages;
        public override void Run()
        {
            var EnglishJson = JToken.Parse(ResourcesManager.ConvertToString(ResourcesManager.GetData("eng.json", ResourcesType.Languages) ??
                throw new KernelException(KernelExceptionType.LanguageManagement, Translate.DoTranslation("Can't open the English localization resource"))));
            JToken LanguageJson;
            foreach (string LanguageName in LanguageManager.Languages.Keys)
            {
                LanguageJson = JToken.Parse(ResourcesManager.ConvertToString(ResourcesManager.GetData($"{LanguageName}.json", ResourcesType.Languages) ??
                throw new KernelException(KernelExceptionType.LanguageManagement, Translate.DoTranslation("Can't open the localization resource for") + $" {LanguageName}")));
                if (LanguageJson.Count() != EnglishJson.Count())
                    TextWriters.Write(Translate.DoTranslation("Line mismatch in") + " {0}: {1} <> {2}", true, KernelColorType.Warning, LanguageName, LanguageJson.Count(), EnglishJson.Count());
            }
        }
    }
}
