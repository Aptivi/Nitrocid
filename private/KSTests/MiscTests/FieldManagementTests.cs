
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
using Shouldly;
using System;

namespace KSTests.MiscTests
{

    [TestFixture]
    public class FieldManagementTests
    {

        /// <summary>
        /// Tests checking field
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestCheckField() => FieldManager.CheckField("HiddenFiles").ShouldBeTrue();

        /// <summary>
        /// Tests getting value
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestGetValue()
        {
            string Value = Convert.ToString(FieldManager.GetValue("HiddenFiles"));
            Value.ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests setting value
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestSetValue()
        {
            FieldManager.SetValue("HiddenFiles", false);
            string Value = Convert.ToString(FieldManager.GetValue("HiddenFiles"));
            Value.ShouldBe("False");
        }

        /// <summary>
        /// Tests getting variable
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestGetField()
        {
            var Field = FieldManager.GetField("HiddenFiles");
            Field.Name.ShouldBe("HiddenFiles");
        }

    }
}
