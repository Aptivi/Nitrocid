
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

using System;
using System.Reflection;
using Figgle;
using KS.Kernel.Configuration;
using KS.Misc.Reflection;
using NUnit.Framework;
using Shouldly;

namespace KSTests.Misc
{

    [TestFixture]
    public class PropertyManagementTests
    {

        /// <summary>
        /// Tests checking property
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestCheckProperty() => PropertyManager.CheckProperty("PersonLookupDelay").ShouldBeTrue();

        /// <summary>
        /// Tests getting value
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestGetPropertyValueInstance()
        {
            string Value = Convert.ToString(PropertyManager.GetPropertyValueInstance(Config.SaverConfig, "PersonLookupDelay"));
            Value.ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests setting value
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestSetPropertyValueInstance()
        {
            PropertyManager.SetPropertyValueInstance(Config.SaverConfig, "PersonLookupDelay", 100);
            int Value = Convert.ToInt32(PropertyManager.GetPropertyValueInstance(Config.SaverConfig, "PersonLookupDelay"));
            Value.ShouldBe(100);
        }

        /// <summary>
        /// Tests getting variable
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestGetConfigProperty()
        {
            var PropertyInfo = PropertyManager.GetProperty("PersonLookupDelay");
            PropertyInfo.Name.ShouldBe("PersonLookupDelay");
        }

        /// <summary>
        /// Tests getting properties
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestGetProperties()
        {
            var Properties = PropertyManager.GetProperties(typeof(FiggleFonts));
            Properties.ShouldNotBeNull();
            Properties.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests getting properties
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestGetPropertiesNoEvaluation()
        {
            var Properties = PropertyManager.GetPropertiesNoEvaluation(typeof(FiggleFonts));
            Properties.ShouldNotBeNull();
            Properties.ShouldNotBeEmpty();
        }

    }
}
