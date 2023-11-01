//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Reflection;
using KS.Kernel.Configuration;
using KS.Kernel.Configuration.Instances;
using KS.Kernel.Updates;
using KS.Misc.Reflection;
using NUnit.Framework;
using Shouldly;

namespace Nitrocid.Tests.Misc.Reflection
{

    [TestFixture]
    public class FieldManagementTests
    {

        public static string TryToChangeIt = "No.";

        /// <summary>
        /// Tests checking field
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestCheckField() =>
            FieldManager.CheckField(nameof(KernelSaverConfig.MatrixDelay)).ShouldBeFalse();

        /// <summary>
        /// Tests getting value
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestGetFieldValue()
        {
            string Value = Convert.ToString(FieldManager.GetFieldValue(nameof(TryToChangeIt), typeof(FieldManagementTests)));
            Value.ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests setting value
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestSetFieldValue()
        {
            FieldManager.SetFieldValue(nameof(TryToChangeIt), "Yes!", typeof(FieldManagementTests));
            string Value = Convert.ToString(FieldManager.GetFieldValue(nameof(TryToChangeIt), typeof(FieldManagementTests)));
            Value.ShouldBe("Yes!");
        }

        /// <summary>
        /// Tests getting fields
        /// </summary>
        [Test]
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
        [Test]
        [Description("Management")]
        public void TestGetFieldsNoEvaluation()
        {
            var Fields = FieldManager.GetFields(typeof(KernelMainConfig));
            Fields.ShouldNotBeNull();
            Fields.ShouldBeEmpty();
        }

    }
}
