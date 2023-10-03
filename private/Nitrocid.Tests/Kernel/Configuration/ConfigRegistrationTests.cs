
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
using Nitrocid.Tests.Kernel.Configuration.CustomConfigs;
using NUnit.Framework;
using Shouldly;

namespace Nitrocid.Tests.Kernel.Configuration
{
    [TestFixture]
    public class ConfigRegistrationTests
    {
        private readonly string name = nameof(KernelCustomSettings);

        /// <summary>
        /// Tests registering a custom config
        /// </summary>
        [OneTimeSetUp]
        [Description("Management")]
        public void TestRegisterConfig()
        {
            var customSettings = new KernelCustomSettings();
            ConfigTools.RegisterCustomSetting(customSettings);
        }

        /// <summary>
        /// Tests checking the built-in settings
        /// </summary>
        [Test]
        [TestCase(nameof(KernelMainConfig))]
        [TestCase(nameof(KernelSaverConfig))]
        [TestCase(nameof(KernelSplashConfig))]
        [Description("Management")]
        public void TestCheckBuiltinConfig(string name)
        {
            ConfigTools.IsCustomSettingRegistered(name).ShouldBeTrue();
            ConfigTools.IsCustomSettingBuiltin(name).ShouldBeTrue();
        }

        /// <summary>
        /// Tests checking the custom settings
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestCheckCustomConfig()
        {
            ConfigTools.IsCustomSettingRegistered(name).ShouldBeTrue();
            ConfigTools.IsCustomSettingBuiltin(name).ShouldBeFalse();
        }

        /// <summary>
        /// Tests checking built-in settings resources (shallow)
        /// </summary>
        [Test]
        [TestCase(nameof(KernelMainConfig))]
        [TestCase(nameof(KernelSaverConfig))]
        [TestCase(nameof(KernelSplashConfig))]
        [Description("Management")]
        public void TestCheckBuiltinSettingsResourcesShallow(string name)
        {
            // Shallow
            var config = Config.GetKernelConfig(name);
            config.ShouldNotBeNull();
            var res = config.SettingsEntries;
            res.ShouldNotBeNull();
            res.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests checking built-in settings resources (deep)
        /// </summary>
        [Test]
        [TestCase(nameof(KernelMainConfig))]
        [TestCase(nameof(KernelSaverConfig))]
        [TestCase(nameof(KernelSplashConfig))]
        [Description("Management")]
        public void TestCheckBuiltinSettingsResourcesDeep(string name)
        {
            // Shallow
            var config = Config.GetKernelConfig(name);
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
        /// Tests checking built-in settings resources (deep with evaluation)
        /// </summary>
        [Test]
        [TestCase(nameof(KernelMainConfig))]
        [TestCase(nameof(KernelSaverConfig))]
        [TestCase(nameof(KernelSplashConfig))]
        [Description("Management")]
        public void TestCheckBuiltinSettingsResourcesDeepEval(string name)
        {
            // Shallow
            var config = Config.GetKernelConfig(name);
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
        /// Tests checking custom settings resources (shallow)
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestCheckCustomSettingsResourcesShallow()
        {
            // Shallow
            var config = Config.GetKernelConfig(name);
            config.ShouldNotBeNull();
            var res = config.SettingsEntries;
            res.ShouldNotBeNull();
            res.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests checking custom settings resources (deep)
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestCheckCustomSettingsResourcesDeep()
        {
            // Shallow
            var config = Config.GetKernelConfig(name);
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
        /// Tests checking custom settings resources (deep with evaluation)
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestCheckCustomSettingsResourcesDeepEval()
        {
            // Shallow
            var config = Config.GetKernelConfig(name);
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
                    if (value is string state)
                        state.ShouldNotBe("Unknown");
                }
            }
        }

        /// <summary>
        /// Tests unregistering a custom config
        /// </summary>
        [OneTimeTearDown]
        [Description("Management")]
        public void TestUnregisterConfig()
        {
            ConfigTools.UnregisterCustomSetting(name);
            ConfigTools.IsCustomSettingRegistered(name).ShouldBeFalse();
        }
    }
}
