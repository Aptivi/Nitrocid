
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
using System.Linq;

namespace Nitrocid.Tests.Kernel.Configuration
{
    [TestFixture]
    public class ConfigManagementTests
    {
        /// <summary>
        /// Tests checking the three settings instance variables
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestCheckSettingsInstances()
        {
            var instance = Config.MainConfig;
            instance.ShouldNotBeNull();
            var instanceSaver = Config.SaverConfig;
            instanceSaver.ShouldNotBeNull();
            var instanceSplash = Config.SplashConfig;
            instanceSplash.ShouldNotBeNull();
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
            SettingsVariables.Where((kvp) => !kvp.Value).ShouldBeEmpty();
        }
    }
}
