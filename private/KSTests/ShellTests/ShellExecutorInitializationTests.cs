
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

using KS.Shell.ShellBase.Shells;
using NUnit.Framework;
using Shouldly;
using System;

namespace KSTests.ShellTests
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

    }
}
