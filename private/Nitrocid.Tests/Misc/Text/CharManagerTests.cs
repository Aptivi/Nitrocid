//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using Textify.General;

namespace Nitrocid.Tests.Misc.Text
{

    [TestClass]
    public class CharManagerTests
    {

        /// <summary>
        /// Tests getting all letters and numbers
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetAllLettersAndNumbers()
        {
            var chars = CharManager.GetAllLettersAndNumbers();
            chars.ShouldNotBeNull();
            chars.ShouldNotBeEmpty();
            chars.ShouldContain('a');
            chars.ShouldContain('1');
        }

        /// <summary>
        /// Tests getting all letters
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetAllLetters()
        {
            var chars = CharManager.GetAllLetters();
            chars.ShouldNotBeNull();
            chars.ShouldNotBeEmpty();
            chars.ShouldContain('a');
            chars.ShouldNotContain('1');
        }

        /// <summary>
        /// Tests getting all numbers
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetAllNumbers()
        {
            var chars = CharManager.GetAllNumbers();
            chars.ShouldNotBeNull();
            chars.ShouldNotBeEmpty();
            chars.ShouldNotContain('a');
            chars.ShouldContain('1');
        }

        /// <summary>
        /// Tests getting escape character
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetEsc()
        {
            var esc = CharManager.GetEsc();
            esc.ShouldBe('\x1b');
        }

        /// <summary>
        /// Tests checking to see if the letter is a real control character
        /// </summary>
        [TestMethod]
        [DataRow('\x00', false)]
        [DataRow('\x07', true)]
        [DataRow('\x08', false)]
        [DataRow('\x0a', false)]
        [DataRow('\x0d', false)]
        [DataRow('\x20', false)]
        [DataRow('a', false)]
        [DataRow('1', false)]
        [Description("Querying")]
        public void TestIsControlChar(char possiblyControlChar, bool expected)
        {
            bool actual = CharManager.IsControlChar(possiblyControlChar);
            actual.ShouldBe(expected);
        }

        /// <summary>
        /// Tests checking to see if the letter is a real control character
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestIsControlCharDynamic()
        {
            for (char ch = (char)0; ch < (char)30; ch++)
            {
                bool expected = (ch > (char)0 && ch < (char)8) || (ch > (char)13 && ch < (char)26);
                bool actual = CharManager.IsControlChar(ch);
                actual.ShouldBe(expected);
            }
        }
    }

}
