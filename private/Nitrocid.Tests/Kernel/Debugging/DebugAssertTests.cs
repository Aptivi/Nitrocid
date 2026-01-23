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

using Nitrocid.Drivers;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Tests.Drivers.DriverData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using Nitrocid.Drivers.Encoding;

namespace Nitrocid.Tests.Kernel.Debugging
{

    [TestClass]
    public class DebugAssertTests
    {

        /// <summary>
        /// Tests assertion...
        /// </summary>
        [TestMethod]
        [Description("Misc")]
        public void TestAssertNormal() =>
            Should.NotThrow(() => DebugCheck.Assert(true, ""));

        /// <summary>
        /// Tests assertion...
        /// </summary>
        [TestMethod]
        [Description("Misc")]
        public void TestAssertNormalFailing()
        {
            DriverHandler.RegisterDriver(DriverTypes.Encoding, new MyCustomEncodingDriver());
            DriverHandler.SetDriver<IEncodingDriver>("MyCustom");
            Should.Throw(() => DebugCheck.Assert(false, ""), typeof(KernelException));
        }

        /// <summary>
        /// Tests assertion...
        /// </summary>
        [TestMethod]
        [Description("Misc")]
        public void TestAssertNormalMessage() =>
            Should.NotThrow(() => DebugCheck.Assert(true, "Always true"));

        /// <summary>
        /// Tests assertion...
        /// </summary>
        [TestMethod]
        [Description("Misc")]
        public void TestAssertNormalMessageFailing()
        {
            DriverHandler.RegisterDriver(DriverTypes.Encoding, new MyCustomEncodingDriver());
            DriverHandler.SetDriver<IEncodingDriver>("MyCustom");
            Should.Throw(() => DebugCheck.Assert(false, "Always false"), typeof(KernelException));
        }

        /// <summary>
        /// Tests assertion...
        /// </summary>
        [TestMethod]
        [Description("Misc")]
        public void TestAssertNotNormal() =>
            Should.NotThrow(() => DebugCheck.AssertNot(false, ""));

        /// <summary>
        /// Tests assertion...
        /// </summary>
        [TestMethod]
        [Description("Misc")]
        public void TestAssertNotNormalFailing()
        {
            DriverHandler.RegisterDriver(DriverTypes.Encoding, new MyCustomEncodingDriver());
            DriverHandler.SetDriver<IEncodingDriver>("MyCustom");
            Should.Throw(() => DebugCheck.AssertNot(true, ""), typeof(KernelException));
        }

        /// <summary>
        /// Tests assertion...
        /// </summary>
        [TestMethod]
        [Description("Misc")]
        public void TestAssertNotNormalMessage() =>
            Should.NotThrow(() => DebugCheck.AssertNot(false, "Always false"));

        /// <summary>
        /// Tests assertion...
        /// </summary>
        [TestMethod]
        [Description("Misc")]
        public void TestAssertNotNormalMessageFailing()
        {
            DriverHandler.RegisterDriver(DriverTypes.Encoding, new MyCustomEncodingDriver());
            DriverHandler.SetDriver<IEncodingDriver>("MyCustom");
            Should.Throw(() => DebugCheck.AssertNot(true, "Always true"), typeof(KernelException));
        }

        /// <summary>
        /// Tests assertion...
        /// </summary>
        [TestMethod]
        [Description("Misc")]
        public void TestAssertNull() =>
            Should.NotThrow(() => DebugCheck.AssertNull(Array.Empty<string>(), ""));

        /// <summary>
        /// Tests assertion...
        /// </summary>
        [TestMethod]
        [Description("Misc")]
        public void TestAssertNullFailing()
        {
            DriverHandler.RegisterDriver(DriverTypes.Encoding, new MyCustomEncodingDriver());
            DriverHandler.SetDriver<IEncodingDriver>("MyCustom");
            Should.Throw(() => DebugCheck.AssertNull<string[]?>(null, ""), typeof(KernelException));
        }

        /// <summary>
        /// Tests assertion...
        /// </summary>
        [TestMethod]
        [Description("Misc")]
        public void TestAssertNullMessage() =>
            Should.NotThrow(() => DebugCheck.AssertNull(Array.Empty<string>(), "Always true"));

        /// <summary>
        /// Tests assertion...
        /// </summary>
        [TestMethod]
        [Description("Misc")]
        public void TestAssertNullMessageFailing()
        {
            DriverHandler.RegisterDriver(DriverTypes.Encoding, new MyCustomEncodingDriver());
            DriverHandler.SetDriver<IEncodingDriver>("MyCustom");
            Should.Throw(() => DebugCheck.AssertNull<string[]?>(null, "Always false"), typeof(KernelException));
        }

        /// <summary>
        /// Tests assertion...
        /// </summary>
        [TestMethod]
        [Description("Misc")]
        public void TestAssertNotNull() =>
            Should.NotThrow(() => DebugCheck.AssertNotNull<string[]?>(null, ""));

        /// <summary>
        /// Tests assertion...
        /// </summary>
        [TestMethod]
        [Description("Misc")]
        public void TestAssertNotNullFailing()
        {
            DriverHandler.RegisterDriver(DriverTypes.Encoding, new MyCustomEncodingDriver());
            DriverHandler.SetDriver<IEncodingDriver>("MyCustom");
            Should.Throw(() => DebugCheck.AssertNotNull(Array.Empty<string>(), ""), typeof(KernelException));
        }

        /// <summary>
        /// Tests assertion...
        /// </summary>
        [TestMethod]
        [Description("Misc")]
        public void TestAssertNotNullMessage() =>
            Should.NotThrow(() => DebugCheck.AssertNotNull<string[]?>(null, "Always true"));

        /// <summary>
        /// Tests assertion...
        /// </summary>
        [TestMethod]
        [Description("Misc")]
        public void TestAssertNotNullMessageFailing()
        {
            DriverHandler.RegisterDriver(DriverTypes.Encoding, new MyCustomEncodingDriver());
            DriverHandler.SetDriver<IEncodingDriver>("MyCustom");
            Should.Throw(() => DebugCheck.AssertNotNull(Array.Empty<string>(), "Always false"), typeof(KernelException));
        }

        /// <summary>
        /// Tests assertion...
        /// </summary>
        [TestMethod]
        [Description("Misc")]
        public void TestAssertForceFail()
        {
            DriverHandler.RegisterDriver(DriverTypes.Encoding, new MyCustomEncodingDriver());
            DriverHandler.SetDriver<IEncodingDriver>("MyCustom");
            Should.Throw(() => DebugCheck.AssertFail("Always false"), typeof(KernelException));
        }

        [ClassCleanup]
        public static void RestoreSettings()
        {
            DriverHandler.SetDriver<IEncodingDriver>("Default");
            DriverHandler.UnregisterDriver(DriverTypes.Encoding, "MyCustom");
        }

    }
}
