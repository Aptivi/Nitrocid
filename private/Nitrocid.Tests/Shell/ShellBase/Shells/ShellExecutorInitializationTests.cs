//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.Shell.ShellBase.Shells;
using NUnit.Framework;
using Shouldly;
using System;

namespace Nitrocid.Tests.Shell.ShellBase.Shells
{

    [TestFixture]
    public class ShellExecutorInitializationTests
    {

        private static BaseShell ShellInstance;

        /// <summary>
        /// Tests initializing the shell instance from base
        /// </summary>
        [Test]
        [Description("Initialization")]
        [SetUp]
        public void TestInitializeShellExecutorFromBase()
        {
            // Create instance
            ShellInstance = new ShellTest();

            // Check for null
            ShellInstance.ShouldNotBeNull();
        }

        /// <summary>
        /// Tests initializing the shell instance from base
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializedShellExecution()
        {
            Should.NotThrow(new Action(() => ShellInstance.InitializeShell()));
            ShellInstance.Bail.ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing the shell instance from base
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializedShellExecutionWithArguments()
        {
            Should.NotThrow(new Action(() => ShellInstance.InitializeShell("Hello", "World")));
            ShellInstance.Bail.ShouldBeTrue();
        }

        /// <summary>
        /// Tests registering the shell instance from base
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestRegisteredShellExecution()
        {
            var instanceInfo = new ShellInfoTest();

            // Register the shell and get its info
            ShellManager.RegisterShell("Basic debug shell", instanceInfo);
            var shellInfo = ShellManager.GetShellInfo("Basic debug shell");

            // Verify correctness
            ShellManager.AvailableShells.ShouldContainKey("Basic debug shell");
            shellInfo.ShouldNotBeNull();
            shellInfo.ShellType.ShouldBe("Basic debug shell");

            // Start the shell
            Should.NotThrow(new Action(() => ShellManager.StartShellForced("Basic debug shell")));

            // Make sure that the shell stack is empty due to manual Bail.
            ShellManager.ShellStack.ShouldBeEmpty();

            // Unregister the shell
            ShellManager.UnregisterShell("Basic debug shell");

            // Check to see if we no longer have this shell
            ShellManager.AvailableShells.ShouldNotContainKey("Basic debug shell");
        }

        /// <summary>
        /// Tests registering the shell instance from base
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestRegisteredShellExecutionWithArguments()
        {
            var instanceInfo = new ShellInfoTest();

            // Register the shell and get its info
            ShellManager.RegisterShell("Basic debug shell", instanceInfo);
            var shellInfo = ShellManager.GetShellInfo("Basic debug shell");

            // Verify correctness
            ShellManager.AvailableShells.ShouldContainKey("Basic debug shell");
            shellInfo.ShouldNotBeNull();
            shellInfo.ShellType.ShouldBe("Basic debug shell");

            // Start the shell
            Should.NotThrow(new Action(() => ShellManager.StartShellForced("Basic debug shell", "Hello", "World")));

            // Make sure that the shell stack is empty due to manual Bail.
            ShellManager.ShellStack.ShouldBeEmpty();

            // Unregister the shell
            ShellManager.UnregisterShell("Basic debug shell");

            // Check to see if we no longer have this shell
            ShellManager.AvailableShells.ShouldNotContainKey("Basic debug shell");
        }

        /// <summary>
        /// Tests checking to see if the shell is built in
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestIsShellBuiltin()
        {
            ShellManager.IsShellBuiltin("Shell").ShouldBeTrue();
            ShellManager.IsShellBuiltin("NoShell").ShouldBeFalse();
        }

        /// <summary>
        /// Tests getting shell info
        /// </summary>
        [Test]
        [TestCase(ShellType.Shell)]
        [TestCase(ShellType.AdminShell)]
        [TestCase(ShellType.DebugShell)]
        [TestCase(ShellType.HexShell)]
        [TestCase(ShellType.TextShell)]
        [Description("Initialization")]
        public void TestGetShellInfo(ShellType type)
        {
            var shellInfo = ShellManager.GetShellInfo(type);
            shellInfo.ShouldNotBeNull();
            shellInfo.ShellType.ShouldBe(type.ToString());
        }

        /// <summary>
        /// Tests getting shell info
        /// </summary>
        [Test]
        [TestCase("Shell")]
        [TestCase("AdminShell")]
        [TestCase("DebugShell")]
        [TestCase("HexShell")]
        [TestCase("TextShell")]
        [Description("Initialization")]
        public void TestGetShellInfo(string type)
        {
            var shellInfo = ShellManager.GetShellInfo(type);
            shellInfo.ShouldNotBeNull();
            shellInfo.ShellType.ShouldBe(type);
        }

    }
}
