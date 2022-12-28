
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.Diagnostics;
using KS.Shell;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using NUnit.Framework;
using Shouldly;

namespace KSTests.ShellTests
{

    [TestFixture]
    public class CommandManagerQueryingTests
    {

        /// <summary>
        /// Tests getting list of commands from specific shell type
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestGetCommandListFromSpecificShell()
        {
            var Commands = CommandManager.GetCommands(ShellType.Shell);
            Debug.WriteLine(format: "Commands from Shell: {0} commands", Commands.Count);
            Debug.WriteLine(format: string.Join(", ", Commands));
            Commands.ShouldNotBeNull();
            Commands.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests getting list of commands from all shells
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestGetCommandListFromAllShells()
        {
            foreach (string ShellTypeName in Shell.AvailableShells.Keys)
            {
                var Commands = CommandManager.GetCommands(ShellTypeName);
                Debug.WriteLine(format: "Commands from {0}: {1} commands", ShellTypeName, Commands.Count);
                Debug.WriteLine(format: string.Join(", ", Commands));
                Commands.ShouldNotBeNull();
                Commands.ShouldNotBeEmpty();
            }
        }

    }
}
