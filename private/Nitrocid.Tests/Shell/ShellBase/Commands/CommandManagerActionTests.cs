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

using KS.Kernel.Exceptions;
using KS.Shell.ShellBase.Arguments;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using Nitrocid.Tests.Shell.ShellBase.Commands.TestCommands;
using NUnit.Framework;
using Shouldly;

namespace Nitrocid.Tests.Shell.ShellBase.Commands
{

    [TestFixture]
    public class CommandManagerActionTests
    {

        /// <summary>
        /// Tests seeing if the command is found in specific shell (test case: lsr command)
        /// </summary>
        [Test]
        [TestCase(ShellType.Shell, ExpectedResult = false)]
        [TestCase(ShellType.AdminShell, ExpectedResult = false)]
        [TestCase(ShellType.DebugShell, ExpectedResult = false)]
        [TestCase(ShellType.HexShell, ExpectedResult = false)]
        [TestCase(ShellType.TextShell, ExpectedResult = false)]
        [Description("Action")]
        public bool TestIsCommandFoundInSpecificShell(ShellType type) =>
            CommandManager.IsCommandFound("lsr", type);

        /// <summary>
        /// Tests seeing if the command is found in specific shell (test case: lsr command)
        /// </summary>
        [Test]
        [TestCase("Shell", ExpectedResult = false)]
        [TestCase("AdminShell", ExpectedResult = false)]
        [TestCase("DebugShell", ExpectedResult = false)]
        [TestCase("HexShell", ExpectedResult = false)]
        [TestCase("TextShell", ExpectedResult = false)]
        [Description("Action")]
        public bool TestIsCommandFoundInSpecificShell(string type) =>
            CommandManager.IsCommandFound("lsr", type);

        /// <summary>
        /// Tests seeing if the command is found in all the shells (test case: detach command)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestIsCommandFoundInAllTheShells() =>
            CommandManager.IsCommandFound("save").ShouldBeTrue();

        /// <summary>
        /// Tests registering the command
        /// </summary>
        [Test]
        [TestCase(ShellType.Shell)]
        [TestCase(ShellType.AdminShell)]
        [TestCase(ShellType.DebugShell)]
        [TestCase(ShellType.HexShell)]
        [TestCase(ShellType.TextShell)]
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
        [Test]
        [TestCase("Shell")]
        [TestCase("AdminShell")]
        [TestCase("DebugShell")]
        [TestCase("HexShell")]
        [TestCase("TextShell")]
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
        [Test]
        [TestCase(ShellType.Shell)]
        [TestCase(ShellType.AdminShell)]
        [TestCase(ShellType.DebugShell)]
        [TestCase(ShellType.HexShell)]
        [TestCase(ShellType.TextShell)]
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
        [Test]
        [TestCase("Shell")]
        [TestCase("AdminShell")]
        [TestCase("DebugShell")]
        [TestCase("HexShell")]
        [TestCase("TextShell")]
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
        [Test]
        [TestCase(ShellType.Shell)]
        [TestCase(ShellType.AdminShell)]
        [TestCase(ShellType.DebugShell)]
        [TestCase(ShellType.HexShell)]
        [TestCase(ShellType.TextShell)]
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
        [Test]
        [TestCase("Shell")]
        [TestCase("AdminShell")]
        [TestCase("DebugShell")]
        [TestCase("HexShell")]
        [TestCase("TextShell")]
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
        [Test]
        [TestCase(ShellType.Shell)]
        [TestCase(ShellType.AdminShell)]
        [TestCase(ShellType.DebugShell)]
        [TestCase(ShellType.HexShell)]
        [TestCase(ShellType.TextShell)]
        [Description("Action")]
        public void TestRegisterNullCommand(ShellType type) =>
            Should.Throw(() => CommandManager.RegisterCustomCommand(type, null), typeof(KernelException));

        /// <summary>
        /// Tests registering the command (Counterexample)
        /// </summary>
        [Test]
        [TestCase("Shell")]
        [TestCase("AdminShell")]
        [TestCase("DebugShell")]
        [TestCase("HexShell")]
        [TestCase("TextShell")]
        [Description("Action")]
        public void TestRegisterNullCommand(string type) =>
            Should.Throw(() => CommandManager.RegisterCustomCommand(type, null), typeof(KernelException));

        /// <summary>
        /// Tests unregistering the command
        /// </summary>
        [Test]
        [TestCase(ShellType.Shell)]
        [TestCase(ShellType.AdminShell)]
        [TestCase(ShellType.DebugShell)]
        [TestCase(ShellType.HexShell)]
        [TestCase(ShellType.TextShell)]
        [Description("Action")]
        public void TestUnregisterCommand(ShellType type)
        {
            Should.NotThrow(() => CommandManager.UnregisterCustomCommand(type, "mycmd"));
            CommandManager.IsCommandFound("mycmd", type).ShouldBeFalse();
        }

        /// <summary>
        /// Tests unregistering the command
        /// </summary>
        [Test]
        [TestCase("Shell")]
        [TestCase("AdminShell")]
        [TestCase("DebugShell")]
        [TestCase("HexShell")]
        [TestCase("TextShell")]
        [Description("Action")]
        public void TestUnregisterCommand(string type)
        {
            Should.NotThrow(() => CommandManager.UnregisterCustomCommand(type, "mycmd2"));
            CommandManager.IsCommandFound("mycmd2", type).ShouldBeFalse();
        }

        /// <summary>
        /// Tests unregistering the command (Counterexample)
        /// </summary>
        [Test]
        [TestCase(ShellType.Shell)]
        [TestCase(ShellType.AdminShell)]
        [TestCase(ShellType.DebugShell)]
        [TestCase(ShellType.HexShell)]
        [TestCase(ShellType.TextShell)]
        [Description("Action")]
        public void TestUnregisterNonexistentCommand(ShellType type) =>
            Should.Throw(() => CommandManager.UnregisterCustomCommand(type, "mycmd3"), typeof(KernelException));

        /// <summary>
        /// Tests unregistering the command (Counterexample)
        /// </summary>
        [Test]
        [TestCase("Shell")]
        [TestCase("AdminShell")]
        [TestCase("DebugShell")]
        [TestCase("HexShell")]
        [TestCase("TextShell")]
        [Description("Action")]
        public void TestUnregisterNonexistentCommand(string type) =>
            Should.Throw(() => CommandManager.UnregisterCustomCommand(type, "mycmd4"), typeof(KernelException));

        /// <summary>
        /// Tests unregistering the command (Counterexample)
        /// </summary>
        [Test]
        [TestCase(ShellType.Shell)]
        [TestCase(ShellType.AdminShell)]
        [TestCase(ShellType.DebugShell)]
        [TestCase(ShellType.HexShell)]
        [TestCase(ShellType.TextShell)]
        [Description("Action")]
        public void TestUnregisterNullCommand(ShellType type) =>
            Should.Throw(() => CommandManager.UnregisterCustomCommand(type, null), typeof(KernelException));

        /// <summary>
        /// Tests unregistering the command (Counterexample)
        /// </summary>
        [Test]
        [TestCase("Shell")]
        [TestCase("AdminShell")]
        [TestCase("DebugShell")]
        [TestCase("HexShell")]
        [TestCase("TextShell")]
        [Description("Action")]
        public void TestUnregisterNullCommand(string type) =>
            Should.Throw(() => CommandManager.UnregisterCustomCommand(type, null), typeof(KernelException));

        /// <summary>
        /// Tests registering the commands
        /// </summary>
        [Test]
        [TestCase(ShellType.Shell)]
        [TestCase(ShellType.AdminShell)]
        [TestCase(ShellType.DebugShell)]
        [TestCase(ShellType.HexShell)]
        [TestCase(ShellType.TextShell)]
        [Description("Action")]
        public void TestRegisterCommands(ShellType type)
        {
            var commandInfos = new CommandInfo[]
            {
                new CommandInfo("cmdgroup", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand()),

                new CommandInfo("cmdgroup1", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand()),

                new CommandInfo("cmdgroup2", $"My command help definition for type {type}...",
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
        [Test]
        [TestCase("Shell")]
        [TestCase("AdminShell")]
        [TestCase("DebugShell")]
        [TestCase("HexShell")]
        [TestCase("TextShell")]
        [Description("Action")]
        public void TestRegisterCommands(string type)
        {
            var commandInfos = new CommandInfo[]
            {
                new CommandInfo("cmdgroup3", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand()),

                new CommandInfo("cmdgroup4", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand()),

                new CommandInfo("cmdgroup5", $"My command help definition for type {type}...",
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
        [Test]
        [TestCase(ShellType.Shell)]
        [TestCase(ShellType.AdminShell)]
        [TestCase(ShellType.DebugShell)]
        [TestCase(ShellType.HexShell)]
        [TestCase(ShellType.TextShell)]
        [Description("Action")]
        public void TestRegisterCommandsWithErrors(ShellType type)
        {
            var commandInfos = new CommandInfo[]
            {
                new CommandInfo("command", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand()),

                new CommandInfo("exit", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand()),

                new CommandInfo("", $"My command help definition for type {type}...",
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
        [Test]
        [TestCase("Shell")]
        [TestCase("AdminShell")]
        [TestCase("DebugShell")]
        [TestCase("HexShell")]
        [TestCase("TextShell")]
        [Description("Action")]
        public void TestRegisterCommandsWithErrors(string type)
        {
            var commandInfos = new CommandInfo[]
            {
                new CommandInfo("command2", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand()),

                new CommandInfo("exit", $"My command help definition for type {type}...",
                    [
                        new CommandArgumentInfo()
                    ], new CustomCommand()),

                new CommandInfo("", $"My command help definition for type {type}...",
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
        [Test]
        [TestCase(ShellType.Shell)]
        [TestCase(ShellType.AdminShell)]
        [TestCase(ShellType.DebugShell)]
        [TestCase(ShellType.HexShell)]
        [TestCase(ShellType.TextShell)]
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
        [Test]
        [TestCase("Shell")]
        [TestCase("AdminShell")]
        [TestCase("DebugShell")]
        [TestCase("HexShell")]
        [TestCase("TextShell")]
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
        [Test]
        [TestCase(ShellType.Shell)]
        [TestCase(ShellType.AdminShell)]
        [TestCase(ShellType.DebugShell)]
        [TestCase(ShellType.HexShell)]
        [TestCase(ShellType.TextShell)]
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
        [Test]
        [TestCase("Shell")]
        [TestCase("AdminShell")]
        [TestCase("DebugShell")]
        [TestCase("HexShell")]
        [TestCase("TextShell")]
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
