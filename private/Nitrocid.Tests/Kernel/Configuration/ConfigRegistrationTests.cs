//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Configuration.Instances;
using Nitrocid.Tests.Kernel.Configuration.CustomConfigs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Nitrocid.Tests.Kernel.Configuration
{
    [TestClass]
    public class ConfigRegistrationTests
    {
        private readonly static string name = nameof(KernelCustomSettings);

        /// <summary>
        /// Tests registering a custom config
        /// </summary>
        [ClassInitialize]
        public static void InitRegisterConfig(TestContext tc)
        {
            tc.WriteLine("Entering config registration tests...");
            var customSettings = new KernelCustomSettings();
            ConfigTools.RegisterCustomSetting(customSettings);
        }

        /// <summary>
        /// Tests checking the built-in settings
        /// </summary>
        [TestMethod]
        [DataRow(nameof(KernelDriverConfig))]
        [DataRow(nameof(KernelMainConfig))]
        [DataRow(nameof(KernelSaverConfig))]
        [DataRow(nameof(KernelSplashConfig))]
        [DataRow(nameof(KernelWidgetsConfig))]
        [Description("Management")]
        public void TestCheckBuiltinConfig(string name)
        {
            ConfigTools.IsCustomSettingRegistered(name).ShouldBeTrue();
            ConfigTools.IsCustomSettingBuiltin(name).ShouldBeTrue();
        }

        /// <summary>
        /// Tests checking the custom settings
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestCheckCustomConfig()
        {
            ConfigTools.IsCustomSettingRegistered(name).ShouldBeTrue();
            ConfigTools.IsCustomSettingBuiltin(name).ShouldBeFalse();
        }

        /// <summary>
        /// Tests checking built-in settings resources (shallow)
        /// </summary>
        [TestMethod]
        [DataRow(nameof(KernelDriverConfig))]
        [DataRow(nameof(KernelMainConfig))]
        [DataRow(nameof(KernelSaverConfig))]
        [DataRow(nameof(KernelSplashConfig))]
        [DataRow(nameof(KernelWidgetsConfig))]
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
        [TestMethod]
        [DataRow(nameof(KernelDriverConfig))]
        [DataRow(nameof(KernelMainConfig))]
        [DataRow(nameof(KernelSaverConfig))]
        [DataRow(nameof(KernelSplashConfig))]
        [DataRow(nameof(KernelWidgetsConfig))]
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
        [TestMethod]
        [DataRow(nameof(KernelDriverConfig))]
        [DataRow(nameof(KernelMainConfig))]
        [DataRow(nameof(KernelSaverConfig))]
        [DataRow(nameof(KernelSplashConfig))]
        [DataRow(nameof(KernelWidgetsConfig))]
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
        [TestMethod]
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
        [TestMethod]
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
        [TestMethod]
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
        [ClassCleanup]
        public static void UninitUnregisterConfig()
        {
            ConfigTools.UnregisterCustomSetting(name);
            ConfigTools.IsCustomSettingRegistered(name).ShouldBeFalse();
        }
    }
}
