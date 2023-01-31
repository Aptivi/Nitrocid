
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

using KS.Kernel.Configuration;
using NUnit.Framework;
using Shouldly;

namespace KSTests.KernelTests
{
    [TestFixture]
    public class ConfigManagementTests
    {
        /// <summary>
        /// Tests creating a new JSON object containing the kernel settings of all kinds
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestGetNewConfigObject() => Config.GetNewConfigObject().ShouldNotBeNull();

        /// <summary>
        /// Tests config repair (Actually, it checks to see if any of the config entries is missing. If any one of them is missing, unit test fails.)
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestRepairConfig() => ConfigTools.RepairConfig().ShouldBeFalse();

        /// <summary>
        /// Tests getting a config category
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestGetConfigCategoryStandard() => ConfigTools.GetConfigCategory(ConfigCategory.General).ShouldNotBeNull();

        /// <summary>
        /// Tests getting a config category with a sub-category
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestGetConfigCategoryWithSubcategory() => ConfigTools.GetConfigCategory(ConfigCategory.Screensaver, "Matrix").ShouldNotBeNull();

        /// <summary>
        /// Tests setting the value of an entry in a category
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestSetConfigValueAndWriteStandard()
        {
            var Token = ConfigTools.GetConfigCategory(ConfigCategory.General);
            ConfigTools.SetConfigValue(ConfigCategory.General, Token, "Check for Updates on Startup", false);
            Token["Check for Updates on Startup"].ToObject<bool>().ShouldBeFalse();
        }

        /// <summary>
        /// Tests setting the value of an entry in a category with the sub-category
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestSetConfigValueAndWriteWithSubcategory()
        {
            var Token = ConfigTools.GetConfigCategory(ConfigCategory.Screensaver, "Matrix");
            ConfigTools.SetConfigValue(ConfigCategory.Screensaver, Token, "Delay in Milliseconds", 2);
            Token["Delay in Milliseconds"].ToObject<int>().ShouldBe(2);
        }

        /// <summary>
        /// Tests checking the settings variables
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestCheckConfigVariables()
        {
            var SettingsVariables = ConfigTools.CheckConfigVariables();
            SettingsVariables.ShouldNotBeNull();
            SettingsVariables.ShouldNotBeEmpty();
            SettingsVariables.Values.ShouldNotContain(false);
        }
    }
}
