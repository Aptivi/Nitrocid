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

using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Configuration.Instances;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Linq;

namespace Nitrocid.Tests.Kernel.Configuration
{
    [TestClass]
    public class ConfigManagementTests
    {
        /// <summary>
        /// Tests checking the three settings instance variables
        /// </summary>
        [TestMethod]
        [DataRow(nameof(KernelDriverConfig), nameof(KernelDriverConfig))]
        [DataRow(nameof(KernelMainConfig), nameof(KernelMainConfig))]
        [DataRow(nameof(KernelSaverConfig), nameof(KernelSaverConfig))]
        [DataRow(nameof(KernelSplashConfig), nameof(KernelSplashConfig))]
        [DataRow(nameof(KernelWidgetsConfig), nameof(KernelWidgetsConfig))]
        [Description("Management")]
        public void TestCheckSettingsInstances(string type, string expectedType)
        {
            var instance = Config.GetKernelConfig(type);
            instance.ShouldNotBeNull();
            instance.GetType().Name.ShouldBe(expectedType);
        }

        /// <summary>
        /// Tests checking settings resources (shallow)
        /// </summary>
        [TestMethod]
        [DataRow(nameof(KernelDriverConfig))]
        [DataRow(nameof(KernelMainConfig))]
        [DataRow(nameof(KernelSaverConfig))]
        [DataRow(nameof(KernelSplashConfig))]
        [DataRow(nameof(KernelWidgetsConfig))]
        [Description("Management")]
        public void TestCheckSettingsResourcesShallowGet(string type)
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
        [TestMethod]
        [DataRow(nameof(KernelDriverConfig))]
        [DataRow(nameof(KernelMainConfig))]
        [DataRow(nameof(KernelSaverConfig))]
        [DataRow(nameof(KernelSplashConfig))]
        [DataRow(nameof(KernelWidgetsConfig))]
        [Description("Management")]
        public void TestCheckSettingsResourcesDeepGet(string type)
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
        [TestMethod]
        [DataRow(nameof(KernelDriverConfig))]
        [DataRow(nameof(KernelMainConfig))]
        [DataRow(nameof(KernelSaverConfig))]
        [DataRow(nameof(KernelSplashConfig))]
        [DataRow(nameof(KernelWidgetsConfig))]
        [Description("Management")]
        public void TestCheckSettingsResourcesDeepEvalGet(string type)
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
        /// Tests checking the settings variables
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestCheckConfigVariables()
        {
            var SettingsVariables = ConfigTools.CheckConfigVariables();
            SettingsVariables.ShouldNotBeNull();
            SettingsVariables.ShouldNotBeEmpty();
            SettingsVariables.Where((result) => !result.Value).ShouldBeEmpty();
        }

        /// <summary>
        /// Tests getting the fallback kernel settings instance
        /// </summary>
        [TestMethod]
        [DataRow(nameof(KernelDriverConfig))]
        [DataRow(nameof(KernelMainConfig))]
        [DataRow(nameof(KernelSaverConfig))]
        [DataRow(nameof(KernelSplashConfig))]
        [DataRow(nameof(KernelWidgetsConfig))]
        [Description("Management")]
        public void TestGetFallbackKernelSettingsWithName(string name)
        {
            var SettingsVariables = Config.GetFallbackKernelConfig(name);
            SettingsVariables.ShouldNotBeNull();
        }

        /// <summary>
        /// Tests getting the fallback kernel settings instance
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestGetFallbackKernelSettingsWithGenericArg()
        {
            TestGetFallbackKernelSettingsWithGenericArgHelper<KernelDriverConfig>();
            TestGetFallbackKernelSettingsWithGenericArgHelper<KernelMainConfig>();
            TestGetFallbackKernelSettingsWithGenericArgHelper<KernelSaverConfig>();
            TestGetFallbackKernelSettingsWithGenericArgHelper<KernelSplashConfig>();
            TestGetFallbackKernelSettingsWithGenericArgHelper<KernelWidgetsConfig>();
        }

        private void TestGetFallbackKernelSettingsWithGenericArgHelper<TConfig>()
            where TConfig : BaseKernelConfig
        {
            var SettingsVariables = Config.GetFallbackKernelConfig<TConfig>();
            SettingsVariables.ShouldNotBeNull();
        }
    }
}
