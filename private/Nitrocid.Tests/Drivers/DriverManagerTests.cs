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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Collections.Generic;
using Nitrocid.Tests.Drivers.DriverData;
using Nitrocid.Base.Drivers;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Drivers.Filesystem;
using Nitrocid.Base.Drivers.Encoding;
using Nitrocid.Base.Drivers.HardwareProber;
using Nitrocid.Base.Drivers.Network;
using Nitrocid.Base.Drivers.Sorting;
using Nitrocid.Base.Drivers.Regexp;
using Nitrocid.Base.Drivers.Input;
using Nitrocid.Base.Drivers.Encryption;
using Nitrocid.Base.Drivers.DebugLogger;

namespace Nitrocid.Tests.Drivers
{
    [TestClass]
    public class DriverManagerTests
    {

        public static IEnumerable<object[]> ExpectedDriverNames =>
            [
                // ---------- Actual ----------                                                         ---------- Expected ----------
                [DriverTypes.Encryption, DriverHandler.CurrentEncryptionDriver,                         "Default"],
                [DriverTypes.Encryption, DriverHandler.CurrentEncryptionDriverLocal,                    "Default"],
                [DriverTypes.Filesystem, DriverHandler.CurrentFilesystemDriver,                         "Default"],
                [DriverTypes.Filesystem, DriverHandler.CurrentFilesystemDriverLocal,                    "Default"],
                [DriverTypes.Network, DriverHandler.CurrentNetworkDriver,                               "Default"],
                [DriverTypes.Network, DriverHandler.CurrentNetworkDriverLocal,                          "Default"],
                [DriverTypes.RNG, DriverHandler.CurrentRandomDriver,                                    "Default"],
                [DriverTypes.RNG, DriverHandler.CurrentRandomDriverLocal,                               "Default"],
                [DriverTypes.Regexp, DriverHandler.CurrentRegexpDriver,                                 "Default"],
                [DriverTypes.Regexp, DriverHandler.CurrentRegexpDriverLocal,                            "Default"],
                [DriverTypes.DebugLogger, DriverHandler.CurrentDebugLoggerDriver,                       "Default"],
                [DriverTypes.DebugLogger, DriverHandler.CurrentDebugLoggerDriverLocal,                  "Default"],
                [DriverTypes.Encoding, DriverHandler.CurrentEncodingDriver,                             "Default"],
                [DriverTypes.Encoding, DriverHandler.CurrentEncodingDriverLocal,                        "Default"],
                [DriverTypes.HardwareProber, DriverHandler.CurrentHardwareProberDriver,                 "Default"],
                [DriverTypes.HardwareProber, DriverHandler.CurrentHardwareProberDriverLocal,            "Default"],
                [DriverTypes.Sorting, DriverHandler.CurrentSortingDriver,                               "Default"],
                [DriverTypes.Sorting, DriverHandler.CurrentSortingDriverLocal,                          "Default"],
                [DriverTypes.Input, DriverHandler.CurrentInputDriver,                                   "Default"],
                [DriverTypes.Input, DriverHandler.CurrentInputDriverLocal,                              "Default"],
                [DriverTypes.EncodingAsymmetric, DriverHandler.CurrentEncodingAsymmetricDriver,         "Default"],
                [DriverTypes.EncodingAsymmetric, DriverHandler.CurrentEncodingAsymmetricDriverLocal,    "Default"],
            ];

        public static IEnumerable<object[]> RegisteredEncryptionDriver =>
            [
                //                       ---------- Provided ----------
                [DriverTypes.Encryption, new MyCustomEncryptionDriver()],
            ];

        public static IEnumerable<object[]> RegisteredFilesystemDriver =>
            [
                //                        ---------- Provided ----------
                [DriverTypes.Filesystem, new MyCustomFilesystemDriver()],
            ];

        public static IEnumerable<object[]> RegisteredNetworkDriver =>
            [
                //                     ---------- Provided ----------
                [DriverTypes.Network, new MyCustomNetworkDriver()],
            ];

        public static IEnumerable<object[]> RegisteredRNGDriver =>
            [
                //                 ---------- Provided ----------
                [DriverTypes.RNG, new MyCustomRNGDriver()],
            ];

        public static IEnumerable<object[]> RegisteredRegexpDriver =>
            [
                //                    ---------- Provided ----------
                [DriverTypes.Regexp, new MyCustomRegexpDriver()],
            ];

        public static IEnumerable<object[]> RegisteredDebugLoggerDriver =>
            [
                //                         ---------- Provided ----------
                [DriverTypes.DebugLogger, new MyCustomDebugLoggerDriver()],
            ];

        public static IEnumerable<object[]> RegisteredEncodingDriver =>
            [
                //                      ---------- Provided ----------
                [DriverTypes.Encoding, new MyCustomEncodingDriver()],
            ];

        public static IEnumerable<object[]> RegisteredHardwareProberDriver =>
            [
                //                            ---------- Provided ----------
                [DriverTypes.HardwareProber, new MyCustomHardwareProberDriver()],
            ];

        public static IEnumerable<object[]> RegisteredSortingDriver =>
            [
                //                     ---------- Provided ----------
                [DriverTypes.Sorting, new MyCustomSortingDriver()],
            ];

        public static IEnumerable<object[]> RegisteredInputDriver =>
            [
                //                   ---------- Provided ----------
                [DriverTypes.Input, new MyCustomInputDriver()],
            ];

        public static IEnumerable<object[]> RegisteredEncodingAsymmetricDriver =>
            [
                //                               ---------- Provided ----------
                [DriverTypes.EncodingAsymmetric, new MyCustomEncodingAsymmetricDriver()],
            ];

        // NOTE: Ordering of the functions below is important for new MSTest as it fails when we keep it as it is. The
        //       new MSTest .exe way of running tests runs tests in the source code ordered way (TestAddDriver before
        //       TestGetDriver before TestGetDriverName before ...), so code accordingly.

        [TestMethod]
        [DynamicData(nameof(RegisteredEncryptionDriver), DynamicDataDisplayNameDeclaringType = typeof(DriverManagerTests))]
        [DynamicData(nameof(RegisteredFilesystemDriver), DynamicDataDisplayNameDeclaringType = typeof(DriverManagerTests))]
        [DynamicData(nameof(RegisteredNetworkDriver), DynamicDataDisplayNameDeclaringType = typeof(DriverManagerTests))]
        [DynamicData(nameof(RegisteredRNGDriver), DynamicDataDisplayNameDeclaringType = typeof(DriverManagerTests))]
        [DynamicData(nameof(RegisteredRegexpDriver), DynamicDataDisplayNameDeclaringType = typeof(DriverManagerTests))]
        [DynamicData(nameof(RegisteredDebugLoggerDriver), DynamicDataDisplayNameDeclaringType = typeof(DriverManagerTests))]
        [DynamicData(nameof(RegisteredEncodingDriver), DynamicDataDisplayNameDeclaringType = typeof(DriverManagerTests))]
        [DynamicData(nameof(RegisteredHardwareProberDriver), DynamicDataDisplayNameDeclaringType = typeof(DriverManagerTests))]
        [DynamicData(nameof(RegisteredSortingDriver), DynamicDataDisplayNameDeclaringType = typeof(DriverManagerTests))]
        [DynamicData(nameof(RegisteredInputDriver), DynamicDataDisplayNameDeclaringType = typeof(DriverManagerTests))]
        [DynamicData(nameof(RegisteredEncodingAsymmetricDriver), DynamicDataDisplayNameDeclaringType = typeof(DriverManagerTests))]
        [Description("Management")]
        public void TestAddDriver(DriverTypes type, IDriver driver)
        {
            Should.NotThrow(() => DriverHandler.RegisterDriver(type, driver));
            DriverHandler.IsRegistered(type, driver).ShouldBeTrue();
        }

        [TestMethod]
        [DataRow("SHA512", DriverTypes.Encryption)]
        [DataRow("Default", DriverTypes.Filesystem)]
        [DataRow("Default", DriverTypes.Network)]
        [DataRow("Standard", DriverTypes.RNG)]
        [DataRow("Default", DriverTypes.Regexp)]
        [DataRow("Default", DriverTypes.DebugLogger)]
        [DataRow("Default", DriverTypes.Encoding)]
        [DataRow("Default", DriverTypes.HardwareProber)]
        [DataRow("Default", DriverTypes.Sorting)]
        [DataRow("Default", DriverTypes.Input)]
        [DataRow("RSA", DriverTypes.EncodingAsymmetric)]
        [Description("Management")]
        public void TestGetDriver(string driverName, DriverTypes expectedType)
        {
            var driver = DriverHandler.GetDriver(expectedType, driverName);
            driver.DriverName.ShouldBe(driverName);
            driver.DriverType.ShouldBe(expectedType);
        }

        [TestMethod]
        [DynamicData(nameof(ExpectedDriverNames), DynamicDataDisplayNameDeclaringType = typeof(DriverManagerTests))]
        [Description("Management")]
        public void TestGetDriverName(DriverTypes type, IDriver driver, string expected)
        {
            string driverName = DriverHandler.GetDriverName(type, driver);
            driverName.ShouldBe(expected);
        }

        [TestMethod]
        [DataRow(DriverTypes.Encryption)]
        [DataRow(DriverTypes.Filesystem)]
        [DataRow(DriverTypes.Network)]
        [DataRow(DriverTypes.RNG)]
        [DataRow(DriverTypes.Regexp)]
        [DataRow(DriverTypes.DebugLogger)]
        [DataRow(DriverTypes.Encoding)]
        [DataRow(DriverTypes.HardwareProber)]
        [DataRow(DriverTypes.Sorting)]
        [DataRow(DriverTypes.Input)]
        [DataRow(DriverTypes.EncodingAsymmetric)]
        [Description("Management")]
        public void TestGetDrivers(DriverTypes type)
        {
            var driver = DriverHandler.GetDrivers(type);
            driver.ShouldNotBeEmpty();
        }

        [TestMethod]
        [DataRow(DriverTypes.Encryption)]
        [DataRow(DriverTypes.Filesystem)]
        [DataRow(DriverTypes.Network)]
        [DataRow(DriverTypes.RNG)]
        [DataRow(DriverTypes.Regexp)]
        [DataRow(DriverTypes.DebugLogger)]
        [DataRow(DriverTypes.Encoding)]
        [DataRow(DriverTypes.HardwareProber)]
        [DataRow(DriverTypes.Sorting)]
        [DataRow(DriverTypes.Input)]
        [DataRow(DriverTypes.EncodingAsymmetric)]
        [Description("Management")]
        public void TestGetDriverNames(DriverTypes type)
        {
            string[] driverNames = DriverHandler.GetDriverNames(type);
            driverNames.ShouldNotBeEmpty();
        }

        [TestMethod]
        [DataRow(DriverTypes.Encryption, "Default")]
        [DataRow(DriverTypes.Filesystem, "Default")]
        [DataRow(DriverTypes.Network, "Default")]
        [DataRow(DriverTypes.RNG, "Default")]
        [DataRow(DriverTypes.Regexp, "Default")]
        [DataRow(DriverTypes.DebugLogger, "Default")]
        [DataRow(DriverTypes.Encoding, "Default")]
        [DataRow(DriverTypes.HardwareProber, "Default")]
        [DataRow(DriverTypes.Sorting, "Default")]
        [DataRow(DriverTypes.Input, "Default")]
        [DataRow(DriverTypes.EncodingAsymmetric, "Default")]
        [Description("Management")]
        public void TestGetFallbackDriver(DriverTypes type, string driverName)
        {
            var driver = DriverHandler.GetFallbackDriver(type);
            driver.ShouldNotBeNull();
            driver.DriverName.ShouldBe(driverName);
        }

        [TestMethod]
        [DataRow(DriverTypes.Encryption, "Default")]
        [DataRow(DriverTypes.Filesystem, "Default")]
        [DataRow(DriverTypes.Network, "Default")]
        [DataRow(DriverTypes.RNG, "Default")]
        [DataRow(DriverTypes.Regexp, "Default")]
        [DataRow(DriverTypes.DebugLogger, "Default")]
        [DataRow(DriverTypes.Encoding, "Default")]
        [DataRow(DriverTypes.HardwareProber, "Default")]
        [DataRow(DriverTypes.Sorting, "Default")]
        [DataRow(DriverTypes.Input, "Default")]
        [DataRow(DriverTypes.EncodingAsymmetric, "Default")]
        [Description("Management")]
        public void TestGetFallbackDriverName(DriverTypes type, string expectedName)
        {
            string driverName = DriverHandler.GetFallbackDriverName(type);
            driverName.ShouldNotBeNull();
            driverName.ShouldNotBeEmpty();
            driverName.ShouldBe(expectedName);
        }

        [TestMethod]
        [DataRow(DriverTypes.Encryption, "Default")]
        [DataRow(DriverTypes.Filesystem, "Default")]
        [DataRow(DriverTypes.Network, "Default")]
        [DataRow(DriverTypes.RNG, "Default")]
        [DataRow(DriverTypes.Regexp, "Default")]
        [DataRow(DriverTypes.DebugLogger, "Default")]
        [DataRow(DriverTypes.Encoding, "Default")]
        [DataRow(DriverTypes.HardwareProber, "Default")]
        [DataRow(DriverTypes.Sorting, "Default")]
        [DataRow(DriverTypes.Input, "Default")]
        [DataRow(DriverTypes.EncodingAsymmetric, "Default")]
        [Description("Management")]
        public void TestGetCurrentDriver(DriverTypes driverType, string expectedName)
        {
            var currentDriver = DriverHandler.GetCurrentDriver(driverType);
            currentDriver.DriverName.ShouldBe(expectedName);
        }

        [TestMethod]
        [DataRow(DriverTypes.Encryption, "Default")]
        [DataRow(DriverTypes.Filesystem, "Default")]
        [DataRow(DriverTypes.Network, "Default")]
        [DataRow(DriverTypes.RNG, "Default")]
        [DataRow(DriverTypes.Regexp, "Default")]
        [DataRow(DriverTypes.DebugLogger, "Default")]
        [DataRow(DriverTypes.Encoding, "Default")]
        [DataRow(DriverTypes.HardwareProber, "Default")]
        [DataRow(DriverTypes.Sorting, "Default")]
        [DataRow(DriverTypes.Input, "Default")]
        [DataRow(DriverTypes.EncodingAsymmetric, "Default")]
        [Description("Management")]
        public void TestGetCurrentDriverLocal(DriverTypes driverType, string expectedName)
        {
            var currentDriver = DriverHandler.GetCurrentDriverLocal(driverType);
            currentDriver.DriverName.ShouldBe(expectedName);
        }

        [TestMethod]
        [DataRow(DriverTypes.Encryption, "SHA512", "SHA512", "Default")]
        [DataRow(DriverTypes.Filesystem, "Default", "Default", "Default")]
        [DataRow(DriverTypes.Network, "Default", "Default", "Default")]
        [DataRow(DriverTypes.RNG, "Standard", "Standard", "Default")]
        [DataRow(DriverTypes.Regexp, "Default", "Default", "Default")]
        [DataRow(DriverTypes.DebugLogger, "UnitTest", "UnitTest", "Default")]
        [DataRow(DriverTypes.Encoding, "Default", "Default", "Default")]
        [DataRow(DriverTypes.HardwareProber, "Default", "Default", "Default")]
        [DataRow(DriverTypes.Sorting, "Default", "Default", "Default")]
        [DataRow(DriverTypes.Input, "Default", "Default", "Default")]
        [DataRow(DriverTypes.EncodingAsymmetric, "RSA", "RSA", "Default")]
        [Description("Management")]
        public void TestBeginLocalDriver(DriverTypes type, string name, string expectedName, string expectedNameAfterLocal)
        {
            Should.NotThrow(() => DriverHandler.BeginLocalDriver(type, name));
            DriverHandler.currentDrivers[type].DriverName.ShouldBe(expectedNameAfterLocal);
            DriverHandler.currentDriversLocal[type].DriverName.ShouldBe(expectedName);
            Should.NotThrow(() => DriverHandler.EndLocalDriver(type));
            DriverHandler.currentDrivers[type].DriverName.ShouldBe(expectedNameAfterLocal);
            DriverHandler.currentDriversLocal[type].DriverName.ShouldBe(expectedNameAfterLocal);
        }

        [TestMethod]
        [DataRow(DriverTypes.Encryption, "SHA512", "SHA512", "Default")]
        [DataRow(DriverTypes.Filesystem, "Default", "Default", "Default")]
        [DataRow(DriverTypes.Network, "Default", "Default", "Default")]
        [DataRow(DriverTypes.RNG, "Standard", "Standard", "Default")]
        [DataRow(DriverTypes.Regexp, "Default", "Default", "Default")]
        [DataRow(DriverTypes.DebugLogger, "UnitTest", "Default", "Default")]
        [DataRow(DriverTypes.Encoding, "Default", "Default", "Default")]
        [DataRow(DriverTypes.HardwareProber, "Default", "Default", "Default")]
        [DataRow(DriverTypes.Sorting, "Default", "Default", "Default")]
        [DataRow(DriverTypes.Input, "Default", "Default", "Default")]
        [DataRow(DriverTypes.EncodingAsymmetric, "RSA", "RSA", "Default")]
        [Description("Management")]
        public void TestBeginLocalDriverSafe(DriverTypes type, string name, string expectedName, string expectedNameAfterLocal)
        {
            Should.NotThrow(() => DriverHandler.BeginLocalDriverSafe(type, name));
            DriverHandler.currentDrivers[type].DriverName.ShouldBe(expectedNameAfterLocal);
            DriverHandler.currentDriversLocal[type].DriverName.ShouldBe(expectedName);
            Should.NotThrow(() => DriverHandler.EndLocalDriver(type));
            DriverHandler.currentDrivers[type].DriverName.ShouldBe(expectedNameAfterLocal);
            DriverHandler.currentDriversLocal[type].DriverName.ShouldBe(expectedNameAfterLocal);
        }

        [TestMethod]
        [Description("Management")]
        public void TestSetEncryptionDriver()
        {
            EncryptionDriverTools.SetEncryptionDriver("SHA512");
            DriverHandler.CurrentEncryptionDriver.DriverName.ShouldBe("SHA512");
            DriverHandler.CurrentEncryptionDriverLocal.DriverName.ShouldBe("SHA512");
        }

        [TestMethod]
        [Description("Management")]
        public void TestGetEncryptionDrivers()
        {
            var drivers = EncryptionDriverTools.GetEncryptionDrivers();
            drivers.ShouldNotBeEmpty();
        }

        [TestMethod]
        [Description("Management")]
        public void TestGetEncryptionDriverNames()
        {
            var drivers = EncryptionDriverTools.GetEncryptionDriverNames();
            drivers.ShouldNotBeEmpty();
        }

        [TestMethod]
        [Description("Management")]
        public void TestSetFilesystemDriver()
        {
            FilesystemDriverTools.SetFilesystemDriver("Default");
            DriverHandler.CurrentFilesystemDriver.DriverName.ShouldBe("Default");
            DriverHandler.CurrentFilesystemDriverLocal.DriverName.ShouldBe("Default");
        }

        [TestMethod]
        [Description("Management")]
        public void TestGetFilesystemDrivers()
        {
            var drivers = FilesystemDriverTools.GetFilesystemDrivers();
            drivers.ShouldNotBeEmpty();
        }

        [TestMethod]
        [Description("Management")]
        public void TestGetFilesystemDriverNames()
        {
            var drivers = FilesystemDriverTools.GetFilesystemDriverNames();
            drivers.ShouldNotBeEmpty();
        }

        [TestMethod]
        [Description("Management")]
        public void TestSetNetworkDriver()
        {
            NetworkDriverTools.SetNetworkDriver("Default");
            DriverHandler.CurrentNetworkDriver.DriverName.ShouldBe("Default");
            DriverHandler.CurrentNetworkDriverLocal.DriverName.ShouldBe("Default");
        }

        [TestMethod]
        [Description("Management")]
        public void TestGetNetworkDrivers()
        {
            var drivers = NetworkDriverTools.GetNetworkDrivers();
            drivers.ShouldNotBeEmpty();
        }

        [TestMethod]
        [Description("Management")]
        public void TestGetNetworkDriverNames()
        {
            var drivers = NetworkDriverTools.GetNetworkDriverNames();
            drivers.ShouldNotBeEmpty();
        }

        [TestMethod]
        [Description("Management")]
        public void TestSetRandomDriver()
        {
            RandomDriverTools.SetRandomDriver("Standard");
            DriverHandler.CurrentRandomDriver.DriverName.ShouldBe("Standard");
            DriverHandler.CurrentRandomDriverLocal.DriverName.ShouldBe("Standard");
        }

        [TestMethod]
        [Description("Management")]
        public void TestGetRandomDrivers()
        {
            var drivers = RandomDriverTools.GetRandomDrivers();
            drivers.ShouldNotBeEmpty();
        }

        [TestMethod]
        [Description("Management")]
        public void TestGetRandomDriverNames()
        {
            var drivers = RandomDriverTools.GetRandomDriverNames();
            drivers.ShouldNotBeEmpty();
        }

        [TestMethod]
        [Description("Management")]
        public void TestSetRegexpDriver()
        {
            RegexpDriverTools.SetRegexpDriver("Default");
            DriverHandler.CurrentRegexpDriver.DriverName.ShouldBe("Default");
            DriverHandler.CurrentRegexpDriverLocal.DriverName.ShouldBe("Default");
        }

        [TestMethod]
        [Description("Management")]
        public void TestGetRegexpDrivers()
        {
            var drivers = RegexpDriverTools.GetRegexpDrivers();
            drivers.ShouldNotBeEmpty();
        }

        [TestMethod]
        [Description("Management")]
        public void TestGetRegexpDriverNames()
        {
            var drivers = RegexpDriverTools.GetRegexpDriverNames();
            drivers.ShouldNotBeEmpty();
        }

        [TestMethod]
        [Description("Management")]
        public void TestSetDebugLoggerDriver()
        {
            DebugLoggerDriverTools.SetDebugLoggerDriver("Default");
            DriverHandler.CurrentDebugLoggerDriver.DriverName.ShouldBe("Default");
            DriverHandler.CurrentDebugLoggerDriverLocal.DriverName.ShouldBe("Default");
        }

        [TestMethod]
        [Description("Management")]
        public void TestGetDebugLoggerDrivers()
        {
            var drivers = DebugLoggerDriverTools.GetDebugLoggerDrivers();
            drivers.ShouldNotBeEmpty();
        }

        [TestMethod]
        [Description("Management")]
        public void TestGetDebugLoggerDriverNames()
        {
            var drivers = DebugLoggerDriverTools.GetDebugLoggerDriverNames();
            drivers.ShouldNotBeEmpty();
        }

        [TestMethod]
        [Description("Management")]
        public void TestSetEncodingDriver()
        {
            EncodingDriverTools.SetEncodingDriver("AES");
            DriverHandler.CurrentEncodingDriver.DriverName.ShouldBe("Default");
            DriverHandler.CurrentEncodingDriverLocal.DriverName.ShouldBe("Default");
        }

        [TestMethod]
        [Description("Management")]
        public void TestGetEncodingDrivers()
        {
            var drivers = EncodingDriverTools.GetEncodingDrivers();
            drivers.ShouldNotBeEmpty();
        }

        [TestMethod]
        [Description("Management")]
        public void TestGetEncodingDriverNames()
        {
            var drivers = EncodingDriverTools.GetEncodingDriverNames();
            drivers.ShouldNotBeEmpty();
        }

        [TestMethod]
        [Description("Management")]
        public void TestSetHardwareProberDriver()
        {
            HardwareProberDriverTools.SetHardwareProberDriver("Default");
            DriverHandler.CurrentHardwareProberDriver.DriverName.ShouldBe("Default");
            DriverHandler.CurrentHardwareProberDriverLocal.DriverName.ShouldBe("Default");
        }

        [TestMethod]
        [Description("Management")]
        public void TestGetHardwareProberDrivers()
        {
            var drivers = HardwareProberDriverTools.GetHardwareProberDrivers();
            drivers.ShouldNotBeEmpty();
        }

        [TestMethod]
        [Description("Management")]
        public void TestGetHardwareProberDriverNames()
        {
            var drivers = HardwareProberDriverTools.GetHardwareProberDriverNames();
            drivers.ShouldNotBeEmpty();
        }

        [TestMethod]
        [Description("Management")]
        public void TestSetSortingDriver()
        {
            SortingDriverTools.SetSortingDriver("Default");
            DriverHandler.CurrentSortingDriver.DriverName.ShouldBe("Default");
            DriverHandler.CurrentSortingDriverLocal.DriverName.ShouldBe("Default");
        }

        [TestMethod]
        [Description("Management")]
        public void TestGetSortingDrivers()
        {
            var drivers = SortingDriverTools.GetSortingDrivers();
            drivers.ShouldNotBeEmpty();
        }

        [TestMethod]
        [Description("Management")]
        public void TestGetSortingDriverNames()
        {
            var drivers = SortingDriverTools.GetSortingDriverNames();
            drivers.ShouldNotBeEmpty();
        }

        [TestMethod]
        [Description("Management")]
        public void TestSetInputDriver()
        {
            InputDriverTools.SetInputDriver("Default");
            DriverHandler.CurrentInputDriver.DriverName.ShouldBe("Default");
            DriverHandler.CurrentInputDriverLocal.DriverName.ShouldBe("Default");
        }

        [TestMethod]
        [Description("Management")]
        public void TestGetInputDrivers()
        {
            var drivers = InputDriverTools.GetInputDrivers();
            drivers.ShouldNotBeEmpty();
        }

        [TestMethod]
        [Description("Management")]
        public void TestGetInputDriverNames()
        {
            var drivers = InputDriverTools.GetInputDriverNames();
            drivers.ShouldNotBeEmpty();
        }


        [TestMethod]
        [DataRow(DriverTypes.Encryption, "MyCustom")]
        [DataRow(DriverTypes.Filesystem, "MyCustom")]
        [DataRow(DriverTypes.Network, "MyCustom")]
        [DataRow(DriverTypes.RNG, "MyCustom")]
        [DataRow(DriverTypes.Regexp, "MyCustom")]
        [DataRow(DriverTypes.DebugLogger, "MyCustom")]
        [DataRow(DriverTypes.Encoding, "MyCustom")]
        [DataRow(DriverTypes.HardwareProber, "MyCustom")]
        [DataRow(DriverTypes.Sorting, "MyCustom")]
        [DataRow(DriverTypes.Input, "MyCustom")]
        [DataRow(DriverTypes.EncodingAsymmetric, "MyCustom")]
        [Description("Management")]
        public void TestUnregisterDriver(DriverTypes type, string name)
        {
            Should.NotThrow(() => DriverHandler.UnregisterDriver(type, name));
            DriverHandler.IsRegistered(type, name).ShouldBeFalse();
        }

        [TestMethod]
        [DataRow(DriverTypes.Encryption, "SHA512", "SHA512")]
        [DataRow(DriverTypes.Filesystem, "Default", "Default")]
        [DataRow(DriverTypes.Network, "Default", "Default")]
        [DataRow(DriverTypes.RNG, "Standard", "Standard")]
        [DataRow(DriverTypes.Regexp, "Default", "Default")]
        [DataRow(DriverTypes.DebugLogger, "Default", "Default")]
        [DataRow(DriverTypes.Encoding, "Default", "Default")]
        [DataRow(DriverTypes.HardwareProber, "Default", "Default")]
        [DataRow(DriverTypes.Sorting, "Default", "Default")]
        [DataRow(DriverTypes.Input, "Default", "Default")]
        [DataRow(DriverTypes.EncodingAsymmetric, "RSA", "RSA")]
        [Description("Management")]
        public void TestSetDriver(DriverTypes type, string name, string expectedName)
        {
            Should.NotThrow(() => DriverHandler.SetDriver(type, name));
            DriverHandler.currentDrivers[type].DriverName.ShouldBe(expectedName);
            DriverHandler.currentDriversLocal[type].DriverName.ShouldBe(expectedName);
        }

        [TestMethod]
        [DataRow(DriverTypes.Encryption, "SHA512", "SHA512")]
        [DataRow(DriverTypes.Filesystem, "Default", "Default")]
        [DataRow(DriverTypes.Network, "Default", "Default")]
        [DataRow(DriverTypes.RNG, "Standard", "Standard")]
        [DataRow(DriverTypes.Regexp, "Default", "Default")]
        [DataRow(DriverTypes.DebugLogger, "Default", "Default")]
        [DataRow(DriverTypes.Encoding, "Default", "Default")]
        [DataRow(DriverTypes.HardwareProber, "Default", "Default")]
        [DataRow(DriverTypes.Sorting, "Default", "Default")]
        [DataRow(DriverTypes.Input, "Default", "Default")]
        [DataRow(DriverTypes.EncodingAsymmetric, "RSA", "RSA")]
        [Description("Management")]
        public void TestSetDriverSafe(DriverTypes type, string name, string expectedName)
        {
            Should.NotThrow(() => DriverHandler.SetDriverSafe(type, name));
            DriverHandler.currentDrivers[type].DriverName.ShouldBe(expectedName);
            DriverHandler.currentDriversLocal[type].DriverName.ShouldBe(expectedName);
        }

        [ClassCleanup]
        public static void UnsetUnitTestDebug() =>
            DriverHandler.SetDriver<IDebugLoggerDriver>("Default");
    }
}
