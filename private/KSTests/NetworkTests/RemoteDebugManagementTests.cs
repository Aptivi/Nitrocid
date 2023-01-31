
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

using KS.Kernel.Debugging.RemoteDebug;
using NUnit.Framework;
using Shouldly;
using System;

namespace KSTests.NetworkTests
{

    [TestFixture]
    public class RemoteDebugManagementTests
    {

        /// <summary>
        /// Tests adding device to json
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestAddDeviceToJson() => RemoteDebugTools.TryAddDeviceToJson("123.123.123.123").ShouldBeTrue();

        /// <summary>
        /// Tests setting device property
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestDeviceSetProperty() => RemoteDebugTools.TrySetDeviceProperty("123.123.123.123", RemoteDebugTools.DeviceProperty.Name, "TestUser").ShouldBeTrue();

        /// <summary>
        /// Tests getting device property
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestGetDeviceProperty() => Convert.ToString(RemoteDebugTools.GetDeviceProperty("123.123.123.123", RemoteDebugTools.DeviceProperty.Name)).ShouldBe("TestUser");

        /// <summary>
        /// Removes a test device created by <see cref="TestAddDeviceToJson()"/>
        /// </summary>
        [OneTimeTearDown]
        public static void TestRemoveTestDevice() => RemoteDebugTools.TryRemoveDeviceFromJson("123.123.123.123").ShouldBeTrue();

    }
}
