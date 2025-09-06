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

using System;
using Nitrocid.Arguments;
using Terminaux.Shell.Arguments;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Nitrocid.Tests.Arguments
{
    [TestClass]
    public class ArgumentInfoTests
    {
        private static ArgumentExecutor? ArgumentInstance;

        /// <summary>
        /// Tests initializing ArgumentInfo instance from a command line argument
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeArgumentInfoInstanceFromCommandLineArg()
        {
            // Create instance
            var ArgumentInstance = new ArgumentInfo("help", "Help page", [new CommandArgumentInfo()], null);

            // Check for null
            ArgumentInstance.ShouldNotBeNull();
            ArgumentInstance.Argument.ShouldNotBeNullOrEmpty();
            ArgumentInstance.HelpDefinition.ShouldNotBeNullOrEmpty();

            // Check for property correctness
            ArgumentInstance.Argument.ShouldBe("help");
            ArgumentInstance.ArgArgumentInfo[0].ArgumentsRequired.ShouldBeFalse();
            ArgumentInstance.HelpDefinition.ShouldBe("Help page");
            ArgumentInstance.ArgArgumentInfo[0].MinimumArguments.ShouldBe(0);
            ArgumentInstance.Obsolete.ShouldBeFalse();
        }

        /// <summary>
        /// Tests initializing the argument instance from base
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializeArgumentFromBase()
        {
            // Create instance
            ArgumentInstance = new ArgumentTest();

            // Check for null
            ArgumentInstance.ShouldNotBeNull();
        }

        /// <summary>
        /// Tests initializing the argument instance from base
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializedArgumentExecution()
        {
            var parameters = new ArgumentParameters("", [], "", [], [], "test");
            Should.NotThrow(new Action(() => ArgumentInstance?.Execute(parameters)));
        }

        /// <summary>
        /// Tests initializing the argument instance from base
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializedArgumentExecutionWithArguments()
        {
            var parameters = new ArgumentParameters("Hello World", ["Hello", "World"], "Hello World", ["Hello", "World"], [], "test");
            Should.NotThrow(new Action(() => ArgumentInstance?.Execute(parameters)));
        }

        /// <summary>
        /// Tests initializing the argument instance from base
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializedArgumentExecutionWithSwitches()
        {
            var parameters = new ArgumentParameters("", [], "-s", [], ["-s"], "test");
            Should.NotThrow(new Action(() => ArgumentInstance?.Execute(parameters)));
        }

        /// <summary>
        /// Tests initializing the argument instance from base
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestInitializedArgumentExecutionWithArgumentsAndSwitches()
        {
            var parameters = new ArgumentParameters("Hello!", ["Hello!"], "-s Hello!", ["Hello!"], ["-s"], "test");
            Should.NotThrow(new Action(() => ArgumentInstance?.Execute(parameters)));
        }
    }
}
