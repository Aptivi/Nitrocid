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

using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Tests.Shell.ShellBase.Commands.TestCommands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using Nitrocid.Shell.ShellBase.Shells;

namespace Nitrocid.Tests.Shell.ShellBase.Commands
{

    [TestClass]
    public class CommandManagerActionTests
    {

        /// <summary>
        /// Tests seeing if the command is found in specific shell (test case: lsr command)
        /// </summary>
        [TestMethod]
        [DataRow(ShellType.Shell, false)]
        [DataRow(ShellType.AdminShell, false)]
        [DataRow(ShellType.DebugShell, false)]
        [DataRow(ShellType.HexShell, false)]
        [DataRow(ShellType.TextShell, false)]
        [Description("Action")]
        public void TestIsCommandFoundInSpecificShell(ShellType type, bool expected)
        {
            bool actual = CommandManager.IsCommandFound("lsr", type);
            actual.ShouldBe(expected);
        }

        /// <summary>
        /// Tests seeing if the command is found in specific shell (test case: lsr command)
        /// </summary>
        [TestMethod]
        [DataRow("Shell", false)]
        [DataRow("AdminShell", false)]
        [DataRow("DebugShell", false)]
        [DataRow("HexShell", false)]
        [DataRow("TextShell", false)]
        [Description("Action")]
        public void TestIsCommandFoundInSpecificShell(string type, bool expected)
        {
            bool actual = CommandManager.IsCommandFound("lsr", type);
            actual.ShouldBe(expected);
        }

        /// <summary>
        /// Tests seeing if the command is found in all the shells (test case: detach command)
        /// </summary>
        [TestMethod]
        [Description("Action")]
        public void TestIsCommandFoundInAllTheShells() =>
            CommandManager.IsCommandFound("save").ShouldBeTrue();

        /// <summary>
        /// Tests registering the command
        /// </summary>
        [TestMethod]
        [DataRow(ShellType.Shell)]
        [DataRow(ShellType.AdminShell)]
        [DataRow(ShellType.DebugShell)]
        [DataRow(ShellType.HexShell)]
        [DataRow(ShellType.TextShell)]
        [Description("Action")]
        public void TestRegisterCommand(ShellType type)
        {
            Should.NotThrow(() => CommandManager.RegisterCustomCommand(type,
                new CommandInfo("mycmd", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand())
            ));
            CommandManager.IsCommandFound("mycmd", type).ShouldBeTrue();
        }

        /// <summary>
        /// Tests registering the command
        /// </summary>
        [TestMethod]
        [DataRow("Shell")]
        [DataRow("AdminShell")]
        [DataRow("DebugShell")]
        [DataRow("HexShell")]
        [DataRow("TextShell")]
        [Description("Action")]
        public void TestRegisterCommand(string type)
        {
            Should.NotThrow(() => CommandManager.RegisterCustomCommand(type,
                new CommandInfo("mycmd2", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand())
            ));
            CommandManager.IsCommandFound("mycmd2", type).ShouldBeTrue();
        }

        /// <summary>
        /// Tests registering the command (Counterexample)
        /// </summary>
        [TestMethod]
        [DataRow(ShellType.Shell)]
        [DataRow(ShellType.AdminShell)]
        [DataRow(ShellType.DebugShell)]
        [DataRow(ShellType.HexShell)]
        [DataRow(ShellType.TextShell)]
        [Description("Action")]
        public void TestRegisterEmptyCommandName(ShellType type)
        {
            Should.Throw(() => CommandManager.RegisterCustomCommand(type,
                new CommandInfo("", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand())
            ), typeof(KernelException));
        }

        /// <summary>
        /// Tests registering the command (Counterexample)
        /// </summary>
        [TestMethod]
        [DataRow("Shell")]
        [DataRow("AdminShell")]
        [DataRow("DebugShell")]
        [DataRow("HexShell")]
        [DataRow("TextShell")]
        [Description("Action")]
        public void TestRegisterEmptyCommandName(string type)
        {
            Should.Throw(() => CommandManager.RegisterCustomCommand(type,
                new CommandInfo("", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand())
            ), typeof(KernelException));
        }

        /// <summary>
        /// Tests registering the command (Counterexample)
        /// </summary>
        [TestMethod]
        [DataRow(ShellType.Shell)]
        [DataRow(ShellType.AdminShell)]
        [DataRow(ShellType.DebugShell)]
        [DataRow(ShellType.HexShell)]
        [DataRow(ShellType.TextShell)]
        [Description("Action")]
        public void TestRegisterCommandConflicting(ShellType type)
        {
            Should.Throw(() => CommandManager.RegisterCustomCommand(type,
                new CommandInfo("exit", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand())
            ), typeof(KernelException));
        }

        /// <summary>
        /// Tests registering the command (Counterexample)
        /// </summary>
        [TestMethod]
        [DataRow("Shell")]
        [DataRow("AdminShell")]
        [DataRow("DebugShell")]
        [DataRow("HexShell")]
        [DataRow("TextShell")]
        [Description("Action")]
        public void TestRegisterCommandConflicting(string type)
        {
            Should.Throw(() => CommandManager.RegisterCustomCommand(type,
                new CommandInfo("exit", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand())
            ), typeof(KernelException));
        }

        /// <summary>
        /// Tests registering the command (Counterexample)
        /// </summary>
        [TestMethod]
        [DataRow(ShellType.Shell)]
        [DataRow(ShellType.AdminShell)]
        [DataRow(ShellType.DebugShell)]
        [DataRow(ShellType.HexShell)]
        [DataRow(ShellType.TextShell)]
        [Description("Action")]
        public void TestRegisterNullCommand(ShellType type) =>
            Should.Throw(() => CommandManager.RegisterCustomCommand(type, null), typeof(KernelException));

        /// <summary>
        /// Tests registering the command (Counterexample)
        /// </summary>
        [TestMethod]
        [DataRow("Shell")]
        [DataRow("AdminShell")]
        [DataRow("DebugShell")]
        [DataRow("HexShell")]
        [DataRow("TextShell")]
        [Description("Action")]
        public void TestRegisterNullCommand(string type) =>
            Should.Throw(() => CommandManager.RegisterCustomCommand(type, null), typeof(KernelException));

        /// <summary>
        /// Tests unregistering the command
        /// </summary>
        [TestMethod]
        [DataRow(ShellType.Shell)]
        [DataRow(ShellType.AdminShell)]
        [DataRow(ShellType.DebugShell)]
        [DataRow(ShellType.HexShell)]
        [DataRow(ShellType.TextShell)]
        [Description("Action")]
        public void TestUnregisterCommand(ShellType type)
        {
            Should.NotThrow(() => CommandManager.UnregisterCustomCommand(type, "mycmd"));
            CommandManager.IsCommandFound("mycmd", type).ShouldBeFalse();
        }

        /// <summary>
        /// Tests unregistering the command
        /// </summary>
        [TestMethod]
        [DataRow("Shell")]
        [DataRow("AdminShell")]
        [DataRow("DebugShell")]
        [DataRow("HexShell")]
        [DataRow("TextShell")]
        [Description("Action")]
        public void TestUnregisterCommand(string type)
        {
            Should.NotThrow(() => CommandManager.UnregisterCustomCommand(type, "mycmd2"));
            CommandManager.IsCommandFound("mycmd2", type).ShouldBeFalse();
        }

        /// <summary>
        /// Tests unregistering the command (Counterexample)
        /// </summary>
        [TestMethod]
        [DataRow(ShellType.Shell)]
        [DataRow(ShellType.AdminShell)]
        [DataRow(ShellType.DebugShell)]
        [DataRow(ShellType.HexShell)]
        [DataRow(ShellType.TextShell)]
        [Description("Action")]
        public void TestUnregisterNonexistentCommand(ShellType type) =>
            Should.Throw(() => CommandManager.UnregisterCustomCommand(type, "mycmd3"), typeof(KernelException));

        /// <summary>
        /// Tests unregistering the command (Counterexample)
        /// </summary>
        [TestMethod]
        [DataRow("Shell")]
        [DataRow("AdminShell")]
        [DataRow("DebugShell")]
        [DataRow("HexShell")]
        [DataRow("TextShell")]
        [Description("Action")]
        public void TestUnregisterNonexistentCommand(string type) =>
            Should.Throw(() => CommandManager.UnregisterCustomCommand(type, "mycmd4"), typeof(KernelException));

        /// <summary>
        /// Tests unregistering the command (Counterexample)
        /// </summary>
        [TestMethod]
        [DataRow(ShellType.Shell)]
        [DataRow(ShellType.AdminShell)]
        [DataRow(ShellType.DebugShell)]
        [DataRow(ShellType.HexShell)]
        [DataRow(ShellType.TextShell)]
        [Description("Action")]
        public void TestUnregisterNullCommand(ShellType type) =>
            Should.Throw(() => CommandManager.UnregisterCustomCommand(type, null), typeof(KernelException));

        /// <summary>
        /// Tests unregistering the command (Counterexample)
        /// </summary>
        [TestMethod]
        [DataRow("Shell")]
        [DataRow("AdminShell")]
        [DataRow("DebugShell")]
        [DataRow("HexShell")]
        [DataRow("TextShell")]
        [Description("Action")]
        public void TestUnregisterNullCommand(string type) =>
            Should.Throw(() => CommandManager.UnregisterCustomCommand(type, null), typeof(KernelException));

        /// <summary>
        /// Tests registering the commands
        /// </summary>
        [TestMethod]
        [DataRow(ShellType.Shell)]
        [DataRow(ShellType.AdminShell)]
        [DataRow(ShellType.DebugShell)]
        [DataRow(ShellType.HexShell)]
        [DataRow(ShellType.TextShell)]
        [Description("Action")]
        public void TestRegisterCommands(ShellType type)
        {
            var commandInfos = new CommandInfo[]
            {
                new("cmdgroup", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand()),

                new("cmdgroup1", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand()),

                new("cmdgroup2", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand()),
            };
            Should.NotThrow(() => CommandManager.RegisterCustomCommands(type, commandInfos));
            CommandManager.IsCommandFound("cmdgroup", type).ShouldBeTrue();
            CommandManager.IsCommandFound("cmdgroup1", type).ShouldBeTrue();
            CommandManager.IsCommandFound("cmdgroup2", type).ShouldBeTrue();
        }

        /// <summary>
        /// Tests registering the commands
        /// </summary>
        [TestMethod]
        [DataRow("Shell")]
        [DataRow("AdminShell")]
        [DataRow("DebugShell")]
        [DataRow("HexShell")]
        [DataRow("TextShell")]
        [Description("Action")]
        public void TestRegisterCommands(string type)
        {
            var commandInfos = new CommandInfo[]
            {
                new("cmdgroup3", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand()),

                new("cmdgroup4", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand()),

                new("cmdgroup5", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand()),
            };
            Should.NotThrow(() => CommandManager.RegisterCustomCommands(type, commandInfos));
            CommandManager.IsCommandFound("cmdgroup3", type).ShouldBeTrue();
            CommandManager.IsCommandFound("cmdgroup4", type).ShouldBeTrue();
            CommandManager.IsCommandFound("cmdgroup5", type).ShouldBeTrue();
        }

        /// <summary>
        /// Tests registering the commands (Counterexample)
        /// </summary>
        [TestMethod]
        [DataRow(ShellType.Shell)]
        [DataRow(ShellType.AdminShell)]
        [DataRow(ShellType.DebugShell)]
        [DataRow(ShellType.HexShell)]
        [DataRow(ShellType.TextShell)]
        [Description("Action")]
        public void TestRegisterCommandsWithErrors(ShellType type)
        {
            var commandInfos = new CommandInfo[]
            {
                new("command", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand()),

                new("exit", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand()),

                new("", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand()),
            };
            Should.Throw(() => CommandManager.RegisterCustomCommands(type, commandInfos), typeof(KernelException));
            CommandManager.IsCommandFound("command", type).ShouldBeTrue();
        }

        /// <summary>
        /// Tests registering the commands (Counterexample)
        /// </summary>
        [TestMethod]
        [DataRow("Shell")]
        [DataRow("AdminShell")]
        [DataRow("DebugShell")]
        [DataRow("HexShell")]
        [DataRow("TextShell")]
        [Description("Action")]
        public void TestRegisterCommandsWithErrors(string type)
        {
            var commandInfos = new CommandInfo[]
            {
                new("command2", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand()),

                new("exit", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand()),

                new("", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand()),
            };
            Should.Throw(() => CommandManager.RegisterCustomCommands(type, commandInfos), typeof(KernelException));
            CommandManager.IsCommandFound("command2", type).ShouldBeTrue();
        }

        /// <summary>
        /// Tests unregistering the commands
        /// </summary>
        [TestMethod]
        [DataRow(ShellType.Shell)]
        [DataRow(ShellType.AdminShell)]
        [DataRow(ShellType.DebugShell)]
        [DataRow(ShellType.HexShell)]
        [DataRow(ShellType.TextShell)]
        [Description("Action")]
        public void TestUnregisterCommands(ShellType type)
        {
            var commandInfos = new string[]
            {
                "cmdgroup",
                "cmdgroup1",
                "cmdgroup2"
            };
            Should.NotThrow(() => CommandManager.UnregisterCustomCommands(type, commandInfos));
            CommandManager.IsCommandFound("cmdgroup", type).ShouldBeFalse();
            CommandManager.IsCommandFound("cmdgroup1", type).ShouldBeFalse();
            CommandManager.IsCommandFound("cmdgroup2", type).ShouldBeFalse();
        }

        /// <summary>
        /// Tests unregistering the commands
        /// </summary>
        [TestMethod]
        [DataRow("Shell")]
        [DataRow("AdminShell")]
        [DataRow("DebugShell")]
        [DataRow("HexShell")]
        [DataRow("TextShell")]
        [Description("Action")]
        public void TestUnregisterCommands(string type)
        {
            var commandInfos = new string[]
            {
                "cmdgroup3",
                "cmdgroup4",
                "cmdgroup5"
            };
            Should.NotThrow(() => CommandManager.UnregisterCustomCommands(type, commandInfos));
            CommandManager.IsCommandFound("cmdgroup3", type).ShouldBeFalse();
            CommandManager.IsCommandFound("cmdgroup4", type).ShouldBeFalse();
            CommandManager.IsCommandFound("cmdgroup5", type).ShouldBeFalse();
        }

        /// <summary>
        /// Tests unregistering the commands (Counterexample)
        /// </summary>
        [TestMethod]
        [DataRow(ShellType.Shell)]
        [DataRow(ShellType.AdminShell)]
        [DataRow(ShellType.DebugShell)]
        [DataRow(ShellType.HexShell)]
        [DataRow(ShellType.TextShell)]
        [Description("Action")]
        public void TestUnregisterCommandsWithErrors(ShellType type)
        {
            var commandInfos = new string[]
            {
                "command",
                "exit",
                ""
            };
            Should.Throw(() => CommandManager.UnregisterCustomCommands(type, commandInfos), typeof(KernelException));
            CommandManager.IsCommandFound("command", type).ShouldBeFalse();
        }

        /// <summary>
        /// Tests unregistering the commands (Counterexample)
        /// </summary>
        [TestMethod]
        [DataRow("Shell")]
        [DataRow("AdminShell")]
        [DataRow("DebugShell")]
        [DataRow("HexShell")]
        [DataRow("TextShell")]
        [Description("Action")]
        public void TestUnregisterCommandsWithErrors(string type)
        {
            var commandInfos = new string[]
            {
                "command2",
                "exit",
                ""
            };
            Should.Throw(() => CommandManager.UnregisterCustomCommands(type, commandInfos), typeof(KernelException));
            CommandManager.IsCommandFound("command2", type).ShouldBeFalse();
        }

    }
}
