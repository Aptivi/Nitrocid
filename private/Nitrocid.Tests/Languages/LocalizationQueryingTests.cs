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

using Nitrocid.Base.Languages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Nitrocid.Tests.Languages
{

    [TestClass]
    public class LocalizationQueryingTests
    {

        /// <summary>
        /// Tests getting cultures
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetCultures()
        {
            var cultures = CultureManager.GetCultures();
            cultures.ShouldNotBeNull();
            cultures.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests getting languages
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestListLanguages()
        {
            var langs = LanguageManager.ListLanguages("arb");
            langs.ShouldNotBeNull();
            langs.ShouldNotBeEmpty();
            langs.Count.ShouldBe(2);
        }

        /// <summary>
        /// Tests getting all languages
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestListAllLanguages()
        {
            var langs = LanguageManager.ListAllLanguages();
            langs.ShouldNotBeNull();
            langs.ShouldNotBeEmpty();
        }

    }
}
