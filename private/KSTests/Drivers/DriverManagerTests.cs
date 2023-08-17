
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

using KS.Drivers;
using KS.Drivers.Console;
using KS.Drivers.Encryption;
using KS.Drivers.Filesystem;
using KS.Drivers.Network;
using KS.Drivers.Regexp;
using KS.Drivers.RNG;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using KSTests.Drivers.DriverData;
using KS.Drivers.DebugLogger;

namespace KSTests.Drivers
{
    [TestFixture]
    public class DriverManagerTests
    {

        private static IEnumerable<TestCaseData> ExpectedDriverNames
        {
            get
            {
                return new[] {
                    //               ---------- Actual ----------                       ---------- Expected ----------
                    new TestCaseData(DriverHandler.CurrentConsoleDriver,                "Default"),
                    new TestCaseData(DriverHandler.CurrentConsoleDriverLocal,           "Default"),
                    new TestCaseData(DriverHandler.CurrentEncryptionDriver,             "Default"),
                    new TestCaseData(DriverHandler.CurrentEncryptionDriverLocal,        "Default"),
                    new TestCaseData(DriverHandler.CurrentFilesystemDriver,             "Default"),
                    new TestCaseData(DriverHandler.CurrentFilesystemDriverLocal,        "Default"),
                    new TestCaseData(DriverHandler.CurrentNetworkDriver,                "Default"),
                    new TestCaseData(DriverHandler.CurrentNetworkDriverLocal,           "Default"),
                    new TestCaseData(DriverHandler.CurrentRandomDriver,                 "Default"),
                    new TestCaseData(DriverHandler.CurrentRandomDriverLocal,            "Default"),
                    new TestCaseData(DriverHandler.CurrentRegexpDriver,                 "Default"),
                    new TestCaseData(DriverHandler.CurrentRegexpDriverLocal,            "Default"),
                    new TestCaseData(DriverHandler.CurrentDebugLoggerDriver,            "Default"),
                    new TestCaseData(DriverHandler.CurrentDebugLoggerDriverLocal,       "Default"),
                };
            }
        }

        private static IEnumerable<TestCaseData> RegisteredConsoleDriver
        {
            get
            {
                return new[] {
                    //               ---------- Provided ----------
                    new TestCaseData(DriverTypes.Console, new MyCustomConsoleDriver()),
                };
            }
        }

        private static IEnumerable<TestCaseData> RegisteredEncryptionDriver
        {
            get
            {
                return new[] {
                    //               ---------- Provided ----------
                    new TestCaseData(DriverTypes.Encryption, new MyCustomEncryptionDriver()),
                };
            }
        }

        private static IEnumerable<TestCaseData> RegisteredFilesystemDriver
        {
            get
            {
                return new[] {
                    //               ---------- Provided ----------
                    new TestCaseData(DriverTypes.Filesystem, new MyCustomFilesystemDriver()),
                };
            }
        }

        private static IEnumerable<TestCaseData> RegisteredNetworkDriver
        {
            get
            {
                return new[] {
                    //               ---------- Provided ----------
                    new TestCaseData(DriverTypes.Network, new MyCustomNetworkDriver()),
                };
            }
        }

        private static IEnumerable<TestCaseData> RegisteredRNGDriver
        {
            get
            {
                return new[] {
                    //               ---------- Provided ----------
                    new TestCaseData(DriverTypes.RNG, new MyCustomRNGDriver()),
                };
            }
        }

        private static IEnumerable<TestCaseData> RegisteredRegexpDriver
        {
            get
            {
                return new[] {
                    //               ---------- Provided ----------
                    new TestCaseData(DriverTypes.Regexp, new MyCustomRegexpDriver()),
                };
            }
        }

        private static IEnumerable<TestCaseData> RegisteredDebugLoggerDriver
        {
            get
            {
                return new[] {
                    //               ---------- Provided ----------
                    new TestCaseData(DriverTypes.DebugLogger, new MyCustomDebugLoggerDriver()),
                };
            }
        }

        [Test]
        [Description("Management")]
        public void TestSetConsoleDriver()
        {
            ConsoleDriverTools.SetConsoleDriver("Default");
            DriverHandler.CurrentConsoleDriver.DriverName.ShouldBe("Default");
            DriverHandler.CurrentConsoleDriverLocal.DriverName.ShouldBe("Default");
        }

        [Test]
        [Description("Management")]
        public void TestGetConsoleDrivers()
        {
            var drivers = ConsoleDriverTools.GetConsoleDrivers();
            drivers.ShouldNotBeEmpty();
        }

        [Test]
        [Description("Management")]
        public void TestGetConsoleDriverNames()
        {
            var drivers = ConsoleDriverTools.GetConsoleDriverNames();
            drivers.ShouldNotBeEmpty();
        }

        [Test]
        [Description("Management")]
        public void TestSetEncryptionDriver()
        {
            EncryptionDriverTools.SetEncryptionDriver("SHA384");
            DriverHandler.CurrentEncryptionDriver.DriverName.ShouldBe("SHA384");
            DriverHandler.CurrentEncryptionDriverLocal.DriverName.ShouldBe("SHA384");
        }

        [Test]
        [Description("Management")]
        public void TestGetEncryptionDrivers()
        {
            var drivers = EncryptionDriverTools.GetEncryptionDrivers();
            drivers.ShouldNotBeEmpty();
        }

        [Test]
        [Description("Management")]
        public void TestGetEncryptionDriverNames()
        {
            var drivers = EncryptionDriverTools.GetEncryptionDriverNames();
            drivers.ShouldNotBeEmpty();
        }

        [Test]
        [Description("Management")]
        public void TestSetFilesystemDriver()
        {
            FilesystemDriverTools.SetFilesystemDriver("Default");
            DriverHandler.CurrentFilesystemDriver.DriverName.ShouldBe("Default");
            DriverHandler.CurrentFilesystemDriverLocal.DriverName.ShouldBe("Default");
        }

        [Test]
        [Description("Management")]
        public void TestGetFilesystemDrivers()
        {
            var drivers = FilesystemDriverTools.GetFilesystemDrivers();
            drivers.ShouldNotBeEmpty();
        }

        [Test]
        [Description("Management")]
        public void TestGetFilesystemDriverNames()
        {
            var drivers = FilesystemDriverTools.GetFilesystemDriverNames();
            drivers.ShouldNotBeEmpty();
        }

        [Test]
        [Description("Management")]
        public void TestSetNetworkDriver()
        {
            NetworkDriverTools.SetNetworkDriver("Default");
            DriverHandler.CurrentNetworkDriver.DriverName.ShouldBe("Default");
            DriverHandler.CurrentNetworkDriverLocal.DriverName.ShouldBe("Default");
        }

        [Test]
        [Description("Management")]
        public void TestGetNetworkDrivers()
        {
            var drivers = NetworkDriverTools.GetNetworkDrivers();
            drivers.ShouldNotBeEmpty();
        }

        [Test]
        [Description("Management")]
        public void TestGetNetworkDriverNames()
        {
            var drivers = NetworkDriverTools.GetNetworkDriverNames();
            drivers.ShouldNotBeEmpty();
        }

        [Test]
        [Description("Management")]
        public void TestSetRandomDriver()
        {
            RandomDriverTools.SetRandomDriver("Standard");
            DriverHandler.CurrentRandomDriver.DriverName.ShouldBe("Standard");
            DriverHandler.CurrentRandomDriverLocal.DriverName.ShouldBe("Standard");
        }

        [Test]
        [Description("Management")]
        public void TestGetRandomDrivers()
        {
            var drivers = RandomDriverTools.GetRandomDrivers();
            drivers.ShouldNotBeEmpty();
        }

        [Test]
        [Description("Management")]
        public void TestGetRandomDriverNames()
        {
            var drivers = RandomDriverTools.GetRandomDriverNames();
            drivers.ShouldNotBeEmpty();
        }

        [Test]
        [Description("Management")]
        public void TestSetRegexpDriver()
        {
            RegexpDriverTools.SetRegexpDriver("Default");
            DriverHandler.CurrentRegexpDriver.DriverName.ShouldBe("Default");
            DriverHandler.CurrentRegexpDriverLocal.DriverName.ShouldBe("Default");
        }

        [Test]
        [Description("Management")]
        public void TestGetRegexpDrivers()
        {
            var drivers = RegexpDriverTools.GetRegexpDrivers();
            drivers.ShouldNotBeEmpty();
        }

        [Test]
        [Description("Management")]
        public void TestGetRegexpDriverNames()
        {
            var drivers = RegexpDriverTools.GetRegexpDriverNames();
            drivers.ShouldNotBeEmpty();
        }

        [Test]
        [Description("Management")]
        public void TestSetDebugLoggerDriver()
        {
            DebugLoggerDriverTools.SetDebugLoggerDriver("Default");
            DriverHandler.CurrentDebugLoggerDriver.DriverName.ShouldBe("Default");
            DriverHandler.CurrentDebugLoggerDriverLocal.DriverName.ShouldBe("Default");
        }

        [Test]
        [Description("Management")]
        public void TestGetDebugLoggerDrivers()
        {
            var drivers = DebugLoggerDriverTools.GetDebugLoggerDrivers();
            drivers.ShouldNotBeEmpty();
        }

        [Test]
        [Description("Management")]
        public void TestGetDebugLoggerDriverNames()
        {
            var drivers = DebugLoggerDriverTools.GetDebugLoggerDriverNames();
            drivers.ShouldNotBeEmpty();
        }

        [Test]
        [TestCase<IConsoleDriver>("Null", DriverTypes.Console)]
        [TestCase<IEncryptionDriver>("SHA384", DriverTypes.Encryption)]
        [TestCase<IFilesystemDriver>("Default", DriverTypes.Filesystem)]
        [TestCase<INetworkDriver>("Default", DriverTypes.Network)]
        [TestCase<IRandomDriver>("Standard", DriverTypes.RNG)]
        [TestCase<IRegexpDriver>("Default", DriverTypes.Regexp)]
        [TestCase<IDebugLoggerDriver>("Default", DriverTypes.DebugLogger)]
        [Description("Management")]
        public void TestGetDriver<T>(string driverName, DriverTypes expectedType)
        {
            var driver = DriverHandler.GetDriver<T>(driverName);
            ((IDriver)driver).DriverName.ShouldBe(driverName);
            ((IDriver)driver).DriverType.ShouldBe(expectedType);
        }

        [Test]
        [TestCaseSource<IConsoleDriver>(nameof(ExpectedDriverNames))]
        [TestCaseSource<IEncryptionDriver>(nameof(ExpectedDriverNames))]
        [TestCaseSource<IFilesystemDriver>(nameof(ExpectedDriverNames))]
        [TestCaseSource<INetworkDriver>(nameof(ExpectedDriverNames))]
        [TestCaseSource<IRandomDriver>(nameof(ExpectedDriverNames))]
        [TestCaseSource<IRegexpDriver>(nameof(ExpectedDriverNames))]
        [TestCaseSource<IDebugLoggerDriver>(nameof(ExpectedDriverNames))]
        [Description("Management")]
        public void TestGetDriverName<T>(IDriver driver, string expectedName)
        {
            string driverName = DriverHandler.GetDriverName<T>(driver);
            driverName.ShouldBe(expectedName);
        }

        [Test]
        [TestCase<IConsoleDriver>]
        [TestCase<IEncryptionDriver>]
        [TestCase<IFilesystemDriver>]
        [TestCase<INetworkDriver>]
        [TestCase<IRandomDriver>]
        [TestCase<IRegexpDriver>]
        [TestCase<IDebugLoggerDriver>]
        [Description("Management")]
        public void TestGetDrivers<T>()
        {
            var driver = DriverHandler.GetDrivers<T>();
            driver.ShouldNotBeEmpty();
        }

        [Test]
        [TestCase<IConsoleDriver>]
        [TestCase<IEncryptionDriver>]
        [TestCase<IFilesystemDriver>]
        [TestCase<INetworkDriver>]
        [TestCase<IRandomDriver>]
        [TestCase<IRegexpDriver>]
        [TestCase<IDebugLoggerDriver>]
        [Description("Management")]
        public void TestGetDriverNames<T>()
        {
            string[] driverNames = DriverHandler.GetDriverNames<T>();
            driverNames.ShouldNotBeEmpty();
        }

        [Test]
        [TestCaseSource<IConsoleDriver>(nameof(RegisteredConsoleDriver))]
        [TestCaseSource<IEncryptionDriver>(nameof(RegisteredEncryptionDriver))]
        [TestCaseSource<IFilesystemDriver>(nameof(RegisteredFilesystemDriver))]
        [TestCaseSource<INetworkDriver>(nameof(RegisteredNetworkDriver))]
        [TestCaseSource<IRandomDriver>(nameof(RegisteredRNGDriver))]
        [TestCaseSource<IRegexpDriver>(nameof(RegisteredRegexpDriver))]
        [TestCaseSource<IDebugLoggerDriver>(nameof(RegisteredDebugLoggerDriver))]
        [Description("Management")]
        public void TestRegisterDriver<T>(DriverTypes type, IDriver driver)
        {
            Should.NotThrow(() => DriverHandler.RegisterDriver<T>(type, driver));
            DriverHandler.IsRegistered(type, driver).ShouldBeTrue();
        }

        [Test]
        [TestCase<IConsoleDriver>(DriverTypes.Console, "MyCustom")]
        [TestCase<IEncryptionDriver>(DriverTypes.Encryption, "MyCustom")]
        [TestCase<IFilesystemDriver>(DriverTypes.Filesystem, "MyCustom")]
        [TestCase<INetworkDriver>(DriverTypes.Network, "MyCustom")]
        [TestCase<IRandomDriver>(DriverTypes.RNG, "MyCustom")]
        [TestCase<IRegexpDriver>(DriverTypes.Regexp, "MyCustom")]
        [TestCase<IDebugLoggerDriver>(DriverTypes.DebugLogger, "MyCustom")]
        [Description("Management")]
        public void TestUnregisterDriver(DriverTypes type, string name)
        {
            Should.NotThrow(() => DriverHandler.UnregisterDriver(type, name));
            DriverHandler.IsRegistered(type, name).ShouldBeFalse();
        }

        [Test]
        [TestCase<IConsoleDriver>(DriverTypes.Console, "File", "File")]
        [TestCase<IEncryptionDriver>(DriverTypes.Encryption, "SHA384", "SHA384")]
        [TestCase<IFilesystemDriver>(DriverTypes.Filesystem, "Default", "Default")]
        [TestCase<INetworkDriver>(DriverTypes.Network, "Default", "Default")]
        [TestCase<IRandomDriver>(DriverTypes.RNG, "Standard", "Standard")]
        [TestCase<IRegexpDriver>(DriverTypes.Regexp, "Default", "Default")]
        [TestCase<IDebugLoggerDriver>(DriverTypes.DebugLogger, "Default", "Default")]
        [Description("Management")]
        public void TestSetDriver<T>(DriverTypes type, string name, string expectedName)
        {
            Should.NotThrow(() => DriverHandler.SetDriver<T>(name));
            DriverHandler.currentDrivers[type].DriverName.ShouldBe(expectedName);
            DriverHandler.currentDriversLocal[type].DriverName.ShouldBe(expectedName);
        }

        [Test]
        [TestCase<IConsoleDriver>(DriverTypes.Console, "File", "Default")]
        [TestCase<IEncryptionDriver>(DriverTypes.Encryption, "SHA384", "SHA384")]
        [TestCase<IFilesystemDriver>(DriverTypes.Filesystem, "Default", "Default")]
        [TestCase<INetworkDriver>(DriverTypes.Network, "Default", "Default")]
        [TestCase<IRandomDriver>(DriverTypes.RNG, "Standard", "Standard")]
        [TestCase<IRegexpDriver>(DriverTypes.Regexp, "Default", "Default")]
        [TestCase<IDebugLoggerDriver>(DriverTypes.DebugLogger, "Default", "Default")]
        [Description("Management")]
        public void TestSetDriverSafe<T>(DriverTypes type, string name, string expectedName)
        {
            Should.NotThrow(() => DriverHandler.SetDriverSafe<T>(name));
            DriverHandler.currentDrivers[type].DriverName.ShouldBe(expectedName);
            DriverHandler.currentDriversLocal[type].DriverName.ShouldBe(expectedName);
        }

        [Test]
        [TestCase<IConsoleDriver>(DriverTypes.Console, "File", "File", "Default")]
        [TestCase<IEncryptionDriver>(DriverTypes.Encryption, "SHA384", "SHA384", "SHA256")]
        [TestCase<IFilesystemDriver>(DriverTypes.Filesystem, "Default", "Default", "Default")]
        [TestCase<INetworkDriver>(DriverTypes.Network, "Default", "Default", "Default")]
        [TestCase<IRandomDriver>(DriverTypes.RNG, "Standard", "Standard", "Default")]
        [TestCase<IRegexpDriver>(DriverTypes.Regexp, "Default", "Default", "Default")]
        [TestCase<IDebugLoggerDriver>(DriverTypes.DebugLogger, "Default", "Default", "UnitTest")]
        [Description("Management")]
        public void TestBeginLocalDriver<T>(DriverTypes type, string name, string expectedName, string expectedNameAfterLocal)
        {
            Should.NotThrow(() => DriverHandler.BeginLocalDriver<T>(name));
            DriverHandler.currentDrivers[type].DriverName.ShouldBe(expectedNameAfterLocal);
            DriverHandler.currentDriversLocal[type].DriverName.ShouldBe(expectedName);
            Should.NotThrow(DriverHandler.EndLocalDriver<T>);
            DriverHandler.currentDrivers[type].DriverName.ShouldBe(expectedNameAfterLocal);
            DriverHandler.currentDriversLocal[type].DriverName.ShouldBe(expectedNameAfterLocal);
        }

        [Test]
        [TestCase<IConsoleDriver>(DriverTypes.Console, "File", "Default", "Default")]
        [TestCase<IEncryptionDriver>(DriverTypes.Encryption, "SHA384", "SHA384", "SHA256")]
        [TestCase<IFilesystemDriver>(DriverTypes.Filesystem, "Default", "Default", "Default")]
        [TestCase<INetworkDriver>(DriverTypes.Network, "Default", "Default", "Default")]
        [TestCase<IRandomDriver>(DriverTypes.RNG, "Standard", "Standard", "Default")]
        [TestCase<IRegexpDriver>(DriverTypes.Regexp, "Default", "Default", "Default")]
        [TestCase<IDebugLoggerDriver>(DriverTypes.DebugLogger, "Default", "Default", "UnitTest")]
        [Description("Management")]
        public void TestBeginLocalDriverSafe<T>(DriverTypes type, string name, string expectedName, string expectedNameAfterLocal)
        {
            Should.NotThrow(() => DriverHandler.BeginLocalDriverSafe<T>(name));
            DriverHandler.currentDrivers[type].DriverName.ShouldBe(expectedNameAfterLocal);
            DriverHandler.currentDriversLocal[type].DriverName.ShouldBe(expectedName);
            Should.NotThrow(DriverHandler.EndLocalDriver<T>);
            DriverHandler.currentDrivers[type].DriverName.ShouldBe(expectedNameAfterLocal);
            DriverHandler.currentDriversLocal[type].DriverName.ShouldBe(expectedNameAfterLocal);
        }

    }
}
