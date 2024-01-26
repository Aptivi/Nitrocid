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

namespace Nitrocid.Tests.Misc.Editors
{

    [TestClass]
    public class TextToolsActionTests
    {

        /// <summary>
        /// Tests checking to see if the string is numeric
        /// </summary>
        [TestMethod]
        [DataRow("64", true)]
        [DataRow("64.5", true)]
        [DataRow("64-5", false)]
        [DataRow("Alsalaam 3lekom", false)]
        [DataRow("Nitrocid", false)]
        [Description("Action")]
        public void TestIsStringNumeric(string Expression, bool expected)
        {
            bool actual = TextTools.IsStringNumeric(Expression);
            actual.ShouldBe(expected);
        }

        /// <summary>
        /// Tests formatting the string
        /// </summary>
        [TestMethod]
        [DataRow("Hello, Alex!", "Hello, {0}!", "Alex")]
        [DataRow("We have 0x0F faults!", "We have 0x{0:X2} faults!", 15)]
        [DataRow("Destroy {0 ships!", "Destroy {0 ships!", 3)]
        [Description("Action")]
        public void TestFormatString(string expected, string Expression, params object[] Vars)
        {
            string actual = TextTools.FormatString(Expression, Vars);
            actual.ShouldBe(expected);
        }
    }
}
