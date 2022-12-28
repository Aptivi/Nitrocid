
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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

using KS.Misc.Reflection;
using NUnit.Framework;

namespace KSTests.MiscTests
{

    [TestFixture]
    public class ReflectionActionTests
    {

        /// <summary>
        /// Tests checking to see if the string is numeric
        /// </summary>
        [TestCase("64", ExpectedResult = true)]
        [TestCase("64.5", ExpectedResult = true)]
        [TestCase("64-5", ExpectedResult = false)]
        [TestCase("Alsalaam 3lekom", ExpectedResult = false)]
        [TestCase("", ExpectedResult = false)]
        [Description("Action")]
        public bool TestIsStringNumeric(string Expression) => StringQuery.IsStringNumeric(Expression);

        /// <summary>
        /// Tests formatting the string
        /// </summary>
        [TestCase("Hello, {0}!", "Alex", ExpectedResult = "Hello, Alex!")]
        [TestCase("We have 0x{0:X2} faults!", 15, ExpectedResult = "We have 0x0F faults!")]
        [TestCase("Destroy {0 ships!", 3, ExpectedResult = "Destroy {0 ships!")]
        [Description("Action")]
        public string TestFormatString(string Expression, params object[] Vars) => StringManipulate.FormatString(Expression, Vars);

    }
}
