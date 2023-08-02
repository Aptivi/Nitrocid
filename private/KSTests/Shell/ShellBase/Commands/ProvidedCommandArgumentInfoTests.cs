
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
using KS.Shell.ShellBase.Shells;
using NUnit.Framework;
using Shouldly;

namespace KSTests.Shell.ShellBase.Commands
{
    [TestFixture]
    public class ProvidedCommandArgumentInfoTests
    {
        /// <summary>
        /// Tests initializing <see cref="ProvidedCommandArgumentsInfo"/> instance from a command line argument
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
        [Description("Initialization")]
        public void TestInitializeProvidedCommandArgumentsInfoInstanceFromCommandLineArgNoArg(ShellType type)
        {
            // Create instance
            var cmdArginfo = new ProvidedCommandArgumentsInfo("help", type);

            // Test for null
            cmdArginfo.ShouldNotBeNull();
            cmdArginfo.Command.ShouldNotBeNullOrEmpty();
            cmdArginfo.ArgumentsList.ShouldBeEmpty();
            cmdArginfo.ArgumentsText.ShouldBeNullOrEmpty();
            cmdArginfo.SwitchesList.ShouldBeEmpty();

            // Test for correctness
            cmdArginfo.Command.ShouldBe("help");
            cmdArginfo.RequiredArgumentsProvided.ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing <see cref="ProvidedCommandArgumentsInfo"/> instance from a command line argument
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
        [Description("Initialization")]
        public void TestInitializeProvidedCommandArgumentsInfoInstanceFromCommandLineArgWithArg(ShellType type)
        {
            // Create instance
            var cmdArginfo = new ProvidedCommandArgumentsInfo("help list", type);

            // Test for null
            cmdArginfo.ShouldNotBeNull();
            cmdArginfo.Command.ShouldNotBeNullOrEmpty();
            cmdArginfo.ArgumentsList.ShouldNotBeEmpty();
            cmdArginfo.ArgumentsText.ShouldNotBeNullOrEmpty();
            cmdArginfo.SwitchesList.ShouldBeEmpty();

            // Test for correctness
            cmdArginfo.Command.ShouldBe("help");
            cmdArginfo.ArgumentsList.ShouldHaveSingleItem();
            cmdArginfo.ArgumentsList.ShouldContain("list");
            cmdArginfo.ArgumentsText.ShouldBe("list");
            cmdArginfo.RequiredArgumentsProvided.ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing <see cref="ProvidedCommandArgumentsInfo"/> instance from a command line argument
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
        [Description("Initialization")]
        public void TestInitializeProvidedCommandArgumentsInfoInstanceFromCommandLineArgWithSwitch(ShellType type)
        {
            // Create instance
            var cmdArginfo = new ProvidedCommandArgumentsInfo("help -switch", type);

            // Test for null
            cmdArginfo.ShouldNotBeNull();
            cmdArginfo.Command.ShouldNotBeNullOrEmpty();
            cmdArginfo.ArgumentsList.ShouldBeEmpty();
            cmdArginfo.ArgumentsText.ShouldBeNullOrEmpty();
            cmdArginfo.SwitchesList.ShouldNotBeEmpty();

            // Test for correctness
            cmdArginfo.Command.ShouldBe("help");
            cmdArginfo.RequiredArgumentsProvided.ShouldBeTrue();
            cmdArginfo.SwitchesList.ShouldHaveSingleItem();
            cmdArginfo.SwitchesList.ShouldContain("-switch");
        }

        /// <summary>
        /// Tests initializing <see cref="ProvidedCommandArgumentsInfo"/> instance from a command line argument
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
        [Description("Initialization")]
        public void TestInitializeProvidedCommandArgumentsInfoInstanceFromCommandLineArgFull(ShellType type)
        {
            // Create instance
            var cmdArginfo = new ProvidedCommandArgumentsInfo("help -switch list", type);

            // Test for null
            cmdArginfo.ShouldNotBeNull();
            cmdArginfo.Command.ShouldNotBeNullOrEmpty();
            cmdArginfo.ArgumentsList.ShouldNotBeEmpty();
            cmdArginfo.ArgumentsText.ShouldNotBeNullOrEmpty();
            cmdArginfo.SwitchesList.ShouldNotBeEmpty();

            // Test for correctness
            cmdArginfo.Command.ShouldBe("help");
            cmdArginfo.ArgumentsList.ShouldHaveSingleItem();
            cmdArginfo.ArgumentsList.ShouldContain("list");
            cmdArginfo.ArgumentsText.ShouldBe("list");
            cmdArginfo.RequiredArgumentsProvided.ShouldBeTrue();
            cmdArginfo.SwitchesList.ShouldHaveSingleItem();
            cmdArginfo.SwitchesList.ShouldContain("-switch");
        }

        /// <summary>
        /// Tests initializing <see cref="ProvidedCommandArgumentsInfo"/> instance from a command line argument
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
        [Description("Initialization")]
        public void TestInitializeProvidedCommandArgumentsInfoInstanceFromCommandLineArgNoArg(string type)
        {
            // Create instance
            var cmdArginfo = new ProvidedCommandArgumentsInfo("help", type);

            // Test for null
            cmdArginfo.ShouldNotBeNull();
            cmdArginfo.Command.ShouldNotBeNullOrEmpty();
            cmdArginfo.ArgumentsList.ShouldBeEmpty();
            cmdArginfo.ArgumentsText.ShouldBeNullOrEmpty();
            cmdArginfo.SwitchesList.ShouldBeEmpty();

            // Test for correctness
            cmdArginfo.Command.ShouldBe("help");
            cmdArginfo.RequiredArgumentsProvided.ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing <see cref="ProvidedCommandArgumentsInfo"/> instance from a command line argument
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
        [Description("Initialization")]
        public void TestInitializeProvidedCommandArgumentsInfoInstanceFromCommandLineArgWithArg(string type)
        {
            // Create instance
            var cmdArginfo = new ProvidedCommandArgumentsInfo("help list", type);

            // Test for null
            cmdArginfo.ShouldNotBeNull();
            cmdArginfo.Command.ShouldNotBeNullOrEmpty();
            cmdArginfo.ArgumentsList.ShouldNotBeEmpty();
            cmdArginfo.ArgumentsText.ShouldNotBeNullOrEmpty();
            cmdArginfo.SwitchesList.ShouldBeEmpty();

            // Test for correctness
            cmdArginfo.Command.ShouldBe("help");
            cmdArginfo.ArgumentsList.ShouldHaveSingleItem();
            cmdArginfo.ArgumentsList.ShouldContain("list");
            cmdArginfo.ArgumentsText.ShouldBe("list");
            cmdArginfo.RequiredArgumentsProvided.ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing <see cref="ProvidedCommandArgumentsInfo"/> instance from a command line argument
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
        [Description("Initialization")]
        public void TestInitializeProvidedCommandArgumentsInfoInstanceFromCommandLineArgWithSwitch(string type)
        {
            // Create instance
            var cmdArginfo = new ProvidedCommandArgumentsInfo("help -switch", type);

            // Test for null
            cmdArginfo.ShouldNotBeNull();
            cmdArginfo.Command.ShouldNotBeNullOrEmpty();
            cmdArginfo.ArgumentsList.ShouldBeEmpty();
            cmdArginfo.ArgumentsText.ShouldBeNullOrEmpty();
            cmdArginfo.SwitchesList.ShouldNotBeEmpty();

            // Test for correctness
            cmdArginfo.Command.ShouldBe("help");
            cmdArginfo.RequiredArgumentsProvided.ShouldBeTrue();
            cmdArginfo.SwitchesList.ShouldHaveSingleItem();
            cmdArginfo.SwitchesList.ShouldContain("-switch");
        }

        /// <summary>
        /// Tests initializing <see cref="ProvidedCommandArgumentsInfo"/> instance from a command line argument
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
        [Description("Initialization")]
        public void TestInitializeProvidedCommandArgumentsInfoInstanceFromCommandLineArgFull(string type)
        {
            // Create instance
            var cmdArginfo = new ProvidedCommandArgumentsInfo("help -switch list", type);

            // Test for null
            cmdArginfo.ShouldNotBeNull();
            cmdArginfo.Command.ShouldNotBeNullOrEmpty();
            cmdArginfo.ArgumentsList.ShouldNotBeEmpty();
            cmdArginfo.ArgumentsText.ShouldNotBeNullOrEmpty();
            cmdArginfo.SwitchesList.ShouldNotBeEmpty();

            // Test for correctness
            cmdArginfo.Command.ShouldBe("help");
            cmdArginfo.ArgumentsList.ShouldHaveSingleItem();
            cmdArginfo.ArgumentsList.ShouldContain("list");
            cmdArginfo.ArgumentsText.ShouldBe("list");
            cmdArginfo.RequiredArgumentsProvided.ShouldBeTrue();
            cmdArginfo.SwitchesList.ShouldHaveSingleItem();
            cmdArginfo.SwitchesList.ShouldContain("-switch");
        }
    }
}
