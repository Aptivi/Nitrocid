
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.Linq;
using KS.Languages;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Shouldly;

namespace KSTests
{

    [TestFixture]
    public class LocalizationInitializationTests
    {

        /// <summary>
        /// Tests creating the new instance of the language information
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestCreateNewLanguageInfoInstance()
        {
            var InfoInstance = new LanguageInfo("arb", "Arabic", true);

            // Check for null
            InfoInstance.ShouldNotBeNull();
            InfoInstance.LanguageResource.ShouldNotBeNull();
            InfoInstance.Cultures.ShouldNotBeNull();

            // Check for property correctness
            InfoInstance.Transliterable.ShouldBeTrue();
            InfoInstance.Custom.ShouldBeFalse();
            InfoInstance.FullLanguageName.ShouldBe("Arabic");
            InfoInstance.ThreeLetterLanguageName.ShouldBe("arb");
            InfoInstance.Cultures.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests translation dictionary preparation for a language
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestPrepareDictForOneLanguage()
        {
            int ExpectedLength = JObject.Parse(KS.My.Resources.Resources.spa).SelectToken("Localizations").Count();
            int ActualLength = Translate.PrepareDict("spa").Values.Count;
            ActualLength.ShouldBe(ExpectedLength);
        }

        /// <summary>
        /// Tests translation dictionary preparation for all languages
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestPrepareDictForAllLanguages()
        {
            foreach (string Lang in LanguageManager.Languages.Keys)
            {
                int ExpectedLength = JObject.Parse(KS.My.Resources.Resources.ResourceManager.GetString(Lang.Replace("-", "_"))).SelectToken("Localizations").Count();
                int ActualLength = Translate.PrepareDict(Lang).Values.Count;
                ActualLength.ShouldBe(ExpectedLength, $"Lang: {Lang}");
            }
        }

    }
}
