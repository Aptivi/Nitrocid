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

using Nitrocid.Shell.ShellBase.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;

namespace Nitrocid.Tests.Shell.ShellBase.Commands
{

    [TestClass]
    public class CommandExecutorInitializationTests
    {

        private static BaseCommand? CommandInstance;

        /// <summary>
        /// Tests initializing the command instance from base
        /// </summary>
        [TestMethod]
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
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializedCommandExecution()
        {
            CommandInstance.ShouldNotBeNull();
            string dummy = "";
            var parameters = new CommandParameters("", [], "", [], [], "say");
            Should.NotThrow(new Action(() => CommandInstance.Execute(parameters, ref dummy)));
        }

        /// <summary>
        /// Tests initializing the command instance from base
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializedCommandExecutionWithArguments()
        {
            CommandInstance.ShouldNotBeNull();
            string dummy = "";
            var parameters = new CommandParameters("Hello World", ["Hello", "World"], "Hello World", ["Hello", "World"], [], "say");
            Should.NotThrow(new Action(() => CommandInstance.Execute(parameters, ref dummy)));
        }

        /// <summary>
        /// Tests initializing the command instance from base
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializedCommandExecutionWithSwitches()
        {
            CommandInstance.ShouldNotBeNull();
            string dummy = "";
            var parameters = new CommandParameters("-s", [], "-s", [], ["-s"], "say");
            Should.NotThrow(new Action(() => CommandInstance.Execute(parameters, ref dummy)));
        }

        /// <summary>
        /// Tests initializing the command instance from base
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializedCommandExecutionWithArgumentsAndSwitches()
        {
            CommandInstance.ShouldNotBeNull();
            string dummy = "";
            var parameters = new CommandParameters("-s Hello!", ["Hello!"], "-s Hello!", ["Hello!"], ["-s"], "say");
            Should.NotThrow(new Action(() => CommandInstance.Execute(parameters, ref dummy)));
        }

    }
}
