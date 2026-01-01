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

using System;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Nitrocid.Tests.Shell.ShellBase.Commands
{

    [TestClass]
    public class CommandManagerQueryingTests
    {

        /// <summary>
        /// Tests getting list of commands from specific shell type
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetCommandListFromSpecificShell()
        {
            var Commands = CommandManager.GetCommandNames(ShellType.Shell);
            Console.WriteLine(format: "Commands from Shell: {0} commands", Commands.Length);
            Console.WriteLine(format: string.Join(", ", Commands));
            Commands.ShouldNotBeNull();
            Commands.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests getting list of commands from all shells
        /// </summary>
        [TestMethod]
        [DataRow(ShellType.Shell)]
        [DataRow(ShellType.AdminShell)]
        [DataRow(ShellType.DebugShell)]
        [DataRow(ShellType.HexShell)]
        [DataRow(ShellType.TextShell)]
        [Description("Querying")]
        public void TestGetCommandListFromAllShells(ShellType type)
        {
            var Commands = CommandManager.GetCommandNames(type);
            Console.WriteLine(format: "Commands from {0}: {1} commands", type.ToString(), Commands.Length);
            Console.WriteLine(format: string.Join(", ", Commands));
            Commands.ShouldNotBeNull();
            Commands.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests getting list of commands from all shells
        /// </summary>
        [TestMethod]
        [DataRow("Shell")]
        [DataRow("AdminShell")]
        [DataRow("DebugShell")]
        [DataRow("HexShell")]
        [DataRow("TextShell")]
        [Description("Querying")]
        public void TestGetCommandListFromAllShells(string type)
        {
            var Commands = CommandManager.GetCommandNames(type);
            Console.WriteLine(format: "Commands from {0}: {1} commands", type, Commands.Length);
            Console.WriteLine(format: string.Join(", ", Commands));
            Commands.ShouldNotBeNull();
            Commands.ShouldNotBeEmpty();
        }

    }
}
