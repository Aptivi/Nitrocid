
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

using KS.Shell.ShellBase.Commands;
using NUnit.Framework;
using Shouldly;
using System;

namespace Nitrocid.Tests.Shell.ShellBase.Commands
{

    [TestFixture]
    public class CommandExecutorInitializationTests
    {

        private static BaseCommand CommandInstance;

        /// <summary>
        /// Tests initializing the command instance from base
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeCommandExecutorFromBase()
        {
            // Create instance
            CommandInstance = new CommandTest();

            // Check for null
            CommandInstance.ShouldNotBeNull();
        }

        /// <summary>
        /// Tests initializing the command instance from base
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializedCommandExecution()
        {
            string dummy = "";
            Should.NotThrow(new Action(() => CommandInstance.Execute("", Array.Empty<string>(), "", Array.Empty<string>(), Array.Empty<string>(), ref dummy)));
        }

        /// <summary>
        /// Tests initializing the command instance from base
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializedCommandExecutionWithArguments()
        {
            string dummy = "";
            Should.NotThrow(new Action(() => CommandInstance.Execute("Hello World", new[] { "Hello", "World" }, "Hello World", new[] { "Hello", "World" }, Array.Empty<string>(), ref dummy)));
        }

        /// <summary>
        /// Tests initializing the command instance from base
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializedCommandExecutionWithSwitches()
        {
            string dummy = "";
            Should.NotThrow(new Action(() => CommandInstance.Execute("-s", Array.Empty<string>(), "-s", Array.Empty<string>(), new[] { "-s" }, ref dummy)));
        }

        /// <summary>
        /// Tests initializing the command instance from base
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializedCommandExecutionWithArgumentsAndSwitches()
        {
            string dummy = "";
            Should.NotThrow(new Action(() => CommandInstance.Execute("-s Hello!", new[] { "Hello!" }, "-s Hello!", new[] { "Hello!" }, new[] { "-s" }, ref dummy)));
        }

    }
}
