
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

using KS.Languages;
using NUnit.Framework;
using Shouldly;

namespace KSTests.Languages
{

    [TestFixture]
    public class LocalizationSettingTests
    {

        /// <summary>
        /// Tests updating the culture
        /// </summary>
        [Test]
        [Description("Setting")]
        public void TestUpdateCulture()
        {
            LanguageManager.currentLanguage = LanguageManager.Languages["spa"];
            string ExpectedCulture = "Spanish";
            CultureManager.UpdateCulture();
            CultureManager.CurrentCult.EnglishName.ShouldContain(ExpectedCulture);
        }

        /// <summary>
        /// Tests updating the culture
        /// </summary>
        [Test]
        [Description("Setting")]
        public void TestUpdateCultureDry()
        {
            LanguageManager.currentLanguage = LanguageManager.Languages["spa"];
            string ExpectedCulture = "Spanish";
            CultureManager.UpdateCultureDry();
            CultureManager.CurrentCult.EnglishName.ShouldContain(ExpectedCulture);
        }

        /// <summary>
        /// Tests updating the culture using custom culture
        /// </summary>
        [Test]
        [Description("Setting")]
        public void TestUpdateCultureCustom()
        {
            LanguageManager.currentLanguage = LanguageManager.Languages["spa"];
            string ExpectedCulture = "Spanish";
            CultureManager.UpdateCulture(ExpectedCulture);
            CultureManager.CurrentCult.EnglishName.ShouldContain(ExpectedCulture);
        }

        /// <summary>
        /// Tests language setting
        /// </summary>
        [Test]
        [Description("Setting")]
        public void TestSetLang()
        {
            LanguageManager.SetLang("spa").ShouldBeTrue();
            LanguageManager.CurrentLanguage.ShouldBe("spa");

            // Check for null
            var InfoInstance = LanguageManager.CurrentLanguageInfo;
            InfoInstance.ShouldNotBeNull();
            InfoInstance.Strings.ShouldNotBeNull();
            InfoInstance.Cultures.ShouldNotBeNull();

            // Check for property correctness
            InfoInstance.Transliterable.ShouldBeFalse();
            InfoInstance.Custom.ShouldBeFalse();
            InfoInstance.FullLanguageName.ShouldBe("Spanish");
            InfoInstance.ThreeLetterLanguageName.ShouldBe("spa");
            InfoInstance.Strings.ShouldNotBeEmpty();
            InfoInstance.Cultures.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests language setting
        /// </summary>
        [Test]
        [Description("Setting")]
        public void TestSetLangDry()
        {
            LanguageManager.SetLangDry("arb").ShouldBeTrue();
            LanguageManager.CurrentLanguage.ShouldBe("arb");

            // Check for null
            var InfoInstance = LanguageManager.CurrentLanguageInfo;
            InfoInstance.ShouldNotBeNull();
            InfoInstance.Strings.ShouldNotBeNull();
            InfoInstance.Cultures.ShouldNotBeNull();

            // Check for property correctness
            InfoInstance.Transliterable.ShouldBeTrue();
            InfoInstance.Custom.ShouldBeFalse();
            InfoInstance.FullLanguageName.ShouldBe("Arabic");
            InfoInstance.ThreeLetterLanguageName.ShouldBe("arb");
            InfoInstance.Strings.ShouldNotBeEmpty();
            InfoInstance.Cultures.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Restores the language
        /// </summary>
        [TearDown]
        public void RestoreLanguage() =>
            LanguageManager.SetLang("eng");

    }
}
