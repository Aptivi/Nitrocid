
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

using System.Diagnostics;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using NUnit.Framework;
using Shouldly;

namespace KSTests.Shell.ShellBase.Commands
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
        [TestCase(ShellType.Shell)]
        [TestCase(ShellType.AdminShell)]
        [TestCase(ShellType.ArchiveShell)]
        [TestCase(ShellType.DebugShell)]
        [TestCase(ShellType.FTPShell)]
        [TestCase(ShellType.HexShell)]
        [TestCase(ShellType.HTTPShell)]
        [TestCase(ShellType.JsonShell)]
        [TestCase(ShellType.MailShell)]
        [TestCase(ShellType.RSSShell)]
        [TestCase(ShellType.SFTPShell)]
        [TestCase(ShellType.TextShell)]
        [Description("Querying")]
        public void TestGetCommandListFromAllShells(ShellType type)
        {
            var Commands = CommandManager.GetCommands(type);
            Debug.WriteLine(format: "Commands from {0}: {1} commands", type.ToString(), Commands.Count);
            Debug.WriteLine(format: string.Join(", ", Commands));
            Commands.ShouldNotBeNull();
            Commands.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests getting list of commands from all shells
        /// </summary>
        [Test]
        [TestCase("Shell")]
        [TestCase("AdminShell")]
        [TestCase("ArchiveShell")]
        [TestCase("DebugShell")]
        [TestCase("FTPShell")]
        [TestCase("HexShell")]
        [TestCase("HTTPShell")]
        [TestCase("JsonShell")]
        [TestCase("MailShell")]
        [TestCase("RSSShell")]
        [TestCase("SFTPShell")]
        [TestCase("TextShell")]
        [Description("Querying")]
        public void TestGetCommandListFromAllShells(string type)
        {
            var Commands = CommandManager.GetCommands(type);
            Debug.WriteLine(format: "Commands from {0}: {1} commands", type, Commands.Count);
            Debug.WriteLine(format: string.Join(", ", Commands));
            Commands.ShouldNotBeNull();
            Commands.ShouldNotBeEmpty();
        }

    }
}
