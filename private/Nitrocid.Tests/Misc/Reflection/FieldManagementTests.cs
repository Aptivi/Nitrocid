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

using System;
using Nitrocid.Base.Kernel.Configuration.Instances;
using Nitrocid.Base.Kernel.Updates;
using Nitrocid.Base.Misc.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Nitrocid.Tests.Misc.Reflection
{

    [TestClass]
    public class FieldManagementTests
    {

        public static string TryToChangeIt = "No.";

        /// <summary>
        /// Tests checking field
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestCheckField() =>
            FieldManager.CheckField(nameof(KernelSaverConfig.MatrixBleedDelay)).ShouldBeFalse();

        /// <summary>
        /// Tests getting value
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestGetFieldValue()
        {
            var Value = Convert.ToString(FieldManager.GetFieldValue(nameof(TryToChangeIt), typeof(FieldManagementTests)));
            if (Value is string value)
                value.ShouldNotBeNullOrEmpty();
            else
                Assert.Fail("Can't get field value");
        }

        /// <summary>
        /// Tests setting value
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestSetFieldValue()
        {
            FieldManager.SetFieldValue(nameof(TryToChangeIt), "Yes!", typeof(FieldManagementTests));
            var Value = Convert.ToString(FieldManager.GetFieldValue(nameof(TryToChangeIt), typeof(FieldManagementTests)));
            if (Value is string value)
                value.ShouldBe("Yes!");
            else
                Assert.Fail("Can't set field value");
        }

        /// <summary>
        /// Tests getting fields
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestGetFields()
        {
            var Fields = FieldManager.GetFields(typeof(UpdateManager));
            Fields.ShouldNotBeNull();
            Fields.ShouldBeEmpty();
        }

        /// <summary>
        /// Tests getting fields
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestGetFieldsNoEvaluation()
        {
            var Fields = FieldManager.GetFields(typeof(KernelMainConfig));
            Fields.ShouldNotBeNull();
            Fields.ShouldBeEmpty();
        }

    }
}
