
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

using KS.Misc.Text;
using NUnit.Framework;
using Shouldly;

namespace KSTests.Misc.Text
{

    [TestFixture]
    public class CharManagerTests
    {

        /// <summary>
        /// Tests getting all letters and numbers
        /// </summary>
        [Test]
        [Description("Querying")]
        public static void TestGetAllLettersAndNumbers()
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
        [Test]
        [Description("Querying")]
        public static void TestGetAllLetters()
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
        [Test]
        [Description("Querying")]
        public static void TestGetAllNumbers()
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
        [Test]
        [Description("Querying")]
        public static void TestGetEsc()
        {
            var esc = CharManager.GetEsc();
            esc.ShouldBe('\x1b');
        }

        /// <summary>
        /// Tests checking to see if the letter is a real control character
        /// </summary>
        [Test]
        [TestCase('\x00', ExpectedResult = false)]
        [TestCase('\x07', ExpectedResult = true)]
        [TestCase('\x08', ExpectedResult = false)]
        [TestCase('\x0a', ExpectedResult = false)]
        [TestCase('\x0d', ExpectedResult = false)]
        [TestCase('\x20', ExpectedResult = false)]
        [TestCase('a', ExpectedResult = false)]
        [TestCase('1', ExpectedResult = false)]
        [Description("Querying")]
        public static bool TestIsControlChar(char possiblyControlChar) => 
            CharManager.IsControlChar(possiblyControlChar);

        /// <summary>
        /// Tests checking to see if the letter is a real control character
        /// </summary>
        [Test]
        [Description("Querying")]
        public static void TestIsControlCharDynamic()
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
