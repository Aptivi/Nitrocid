
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
using KS.Kernel.Configuration.Instances;
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
        [TestCase(ConfigType.Kernel, nameof(KernelMainConfig))]
        [TestCase(ConfigType.Screensaver, nameof(KernelSaverConfig))]
        [TestCase(ConfigType.Splash, nameof(KernelSplashConfig))]
        [Description("Management")]
        public void TestCheckSettingsInstances(ConfigType type, string expectedType)
        {
            var instance = Config.GetKernelConfig(type);
            instance.ShouldNotBeNull();
            instance.GetType().Name.ShouldBe(expectedType);
        }

        /// <summary>
        /// Tests checking settings resources (shallow)
        /// </summary>
        [Test]
        [TestCase(ConfigType.Kernel)]
        [TestCase(ConfigType.Screensaver)]
        [TestCase(ConfigType.Splash)]
        [Description("Management")]
        public void TestCheckSettingsResourcesShallow(ConfigType type)
        {
            // Shallow
            var res = ConfigTools.OpenSettingsResource(type);
            res.ShouldNotBeNull();
            res.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests checking settings resources (deep)
        /// </summary>
        [Test]
        [TestCase(ConfigType.Kernel)]
        [TestCase(ConfigType.Screensaver)]
        [TestCase(ConfigType.Splash)]
        [Description("Management")]
        public void TestCheckSettingsResourcesDeep(ConfigType type)
        {
            // Shallow
            var res = ConfigTools.OpenSettingsResource(type);
            res.ShouldNotBeNull();
            res.ShouldNotBeEmpty();

            // Deep
            foreach (var entry in res)
            {
                entry.ShouldNotBeNull();
                entry.Keys.ShouldNotBeNull();
                entry.Keys.ShouldNotBeEmpty();
                foreach (var key in entry.Keys)
                    key.ShouldNotBeNull();
            }
        }

        /// <summary>
        /// Tests checking settings resources (shallow)
        /// </summary>
        [Test]
        [TestCase(ConfigType.Kernel)]
        [TestCase(ConfigType.Screensaver)]
        [TestCase(ConfigType.Splash)]
        [Description("Management")]
        public void TestCheckSettingsResourcesShallowGet(ConfigType type)
        {
            // Shallow
            var config = Config.GetKernelConfig(type);
            config.ShouldNotBeNull();
            var res = config.SettingsEntries;
            res.ShouldNotBeNull();
            res.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests checking settings resources (deep)
        /// </summary>
        [Test]
        [TestCase(ConfigType.Kernel)]
        [TestCase(ConfigType.Screensaver)]
        [TestCase(ConfigType.Splash)]
        [Description("Management")]
        public void TestCheckSettingsResourcesDeepGet(ConfigType type)
        {
            // Shallow
            var config = Config.GetKernelConfig(type);
            config.ShouldNotBeNull();
            var res = config.SettingsEntries;
            res.ShouldNotBeNull();
            res.ShouldNotBeEmpty();

            // Deep
            foreach (var entry in res)
            {
                entry.ShouldNotBeNull();
                entry.Keys.ShouldNotBeNull();
                entry.Keys.ShouldNotBeEmpty();
                foreach (var key in entry.Keys)
                    key.ShouldNotBeNull();
            }
        }

        /// <summary>
        /// Tests checking settings resources (deep with evaluation)
        /// </summary>
        [Test]
        [TestCase(ConfigType.Kernel)]
        [TestCase(ConfigType.Screensaver)]
        [TestCase(ConfigType.Splash)]
        [Description("Management")]
        public void TestCheckSettingsResourcesDeepEvalGet(ConfigType type)
        {
            // Shallow
            var config = Config.GetKernelConfig(type);
            config.ShouldNotBeNull();
            var res = config.SettingsEntries;
            res.ShouldNotBeNull();
            res.ShouldNotBeEmpty();

            // Deep
            foreach (var entry in res)
            {
                entry.ShouldNotBeNull();
                entry.Keys.ShouldNotBeNull();
                entry.Keys.ShouldNotBeEmpty();
                foreach (var key in entry.Keys)
                {
                    key.ShouldNotBeNull();
                    var value = ConfigTools.GetValueFromEntry(key, config);
                    value.ShouldNotBeNull();
                    value.ShouldNotBe("Unknown");
                }
            }
        }

        /// <summary>
        /// Tests translating the built-in config types
        /// </summary>
        [Test]
        [TestCase(ConfigType.Kernel, nameof(KernelMainConfig))]
        [TestCase(ConfigType.Screensaver, nameof(KernelSaverConfig))]
        [TestCase(ConfigType.Splash, nameof(KernelSplashConfig))]
        [Description("Management")]
        public void TestTranslateBuiltinConfigType(ConfigType type, string expectedType) =>
            ConfigTools.TranslateBuiltinConfigType(type).ShouldBe(expectedType);

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
