
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
using System;

namespace KSTests.Shell.ShellBase.Commands
{

    [TestFixture]
    public class CommandInfoInitializationTests
    {

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
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
        public void TestInitializeCommandInfoInstanceFromCommandLineArgSimple(ShellType type)
        {
            // Create instance
            var CommandInstance = new CommandInfo("help", type, "Help page", new CommandArgumentInfo(Array.Empty<string>(), Array.Empty<SwitchInfo>(), false, 0), null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo.Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo.Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo.Arguments.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo.Switches.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo.ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo.MinimumArguments.ShouldBe(0);
            CommandInstance.CommandArgumentInfo.AutoCompleter.ShouldBeNull();
            CommandInstance.Type.ShouldBe(type.ToString());
            CommandInstance.Flags.HasFlag(CommandFlags.Strict).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.NoMaintenance).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.SettingVariable).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
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
        public void TestInitializeCommandInfoInstanceFromCommandLineArg(ShellType type)
        {
            // Create instance
            var CommandInstance = new CommandInfo("help", type, "Help page", new CommandArgumentInfo(new[] { "testarg" }, Array.Empty<SwitchInfo>(), false, 0), null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo.Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo.Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo.Arguments.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo.Switches.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo.ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo.MinimumArguments.ShouldBe(0);
            CommandInstance.CommandArgumentInfo.AutoCompleter.ShouldBeNull();
            CommandInstance.Type.ShouldBe(type.ToString());
            CommandInstance.Flags.HasFlag(CommandFlags.Strict).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.NoMaintenance).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.SettingVariable).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
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
        public void TestInitializeCommandInfoInstanceFromCommandLineArgWithSwitch(ShellType type)
        {
            // Create instance
            var CommandInstance = new CommandInfo("help", type, "Help page", new CommandArgumentInfo(Array.Empty<string>(), new[] { new SwitchInfo("s", "Simple help") }, false, 0), null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo.Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo.Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo.Arguments.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo.Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo.ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo.MinimumArguments.ShouldBe(0);
            CommandInstance.CommandArgumentInfo.AutoCompleter.ShouldBeNull();
            CommandInstance.Type.ShouldBe(type.ToString());
            CommandInstance.Flags.HasFlag(CommandFlags.Strict).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.NoMaintenance).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.SettingVariable).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();
            
            // Check for switch info correctness
            var @switch = CommandInstance.CommandArgumentInfo.Switches[0];
            @switch.SwitchName.ShouldBe("s");
            @switch.HelpDefinition.ShouldBe("Simple help");
            @switch.IsRequired.ShouldBeFalse();
            @switch.ArgumentsRequired.ShouldBeFalse();
            @switch.ConflictsWith.ShouldBeEmpty();
            @switch.OptionalizeLastRequiredArguments.ShouldBe(0);
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
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
        public void TestInitializeCommandInfoInstanceFromCommandLineArgWithSwitchAndArg(ShellType type)
        {
            // Create instance
            var CommandInstance = new CommandInfo("help", type, "Help page", new CommandArgumentInfo(new[] { "testarg" }, new[] { new SwitchInfo("s", "Simple help") }, false, 0), null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo.Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo.Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo.Arguments.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo.Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo.ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo.MinimumArguments.ShouldBe(0);
            CommandInstance.CommandArgumentInfo.AutoCompleter.ShouldBeNull();
            CommandInstance.Type.ShouldBe(type.ToString());
            CommandInstance.Flags.HasFlag(CommandFlags.Strict).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.NoMaintenance).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.SettingVariable).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();

            // Check for switch info correctness
            var @switch = CommandInstance.CommandArgumentInfo.Switches[0];
            @switch.SwitchName.ShouldBe("s");
            @switch.HelpDefinition.ShouldBe("Simple help");
            @switch.IsRequired.ShouldBeFalse();
            @switch.ArgumentsRequired.ShouldBeFalse();
            @switch.ConflictsWith.ShouldBeEmpty();
            @switch.OptionalizeLastRequiredArguments.ShouldBe(0);
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
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
        public void TestInitializeCommandInfoInstanceFromCommandLineArgSimple(string type)
        {
            // Create instance
            var CommandInstance = new CommandInfo("help", type, "Help page", new CommandArgumentInfo(Array.Empty<string>(), Array.Empty<SwitchInfo>(), false, 0), null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo.Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo.Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo.Arguments.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo.Switches.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo.ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo.MinimumArguments.ShouldBe(0);
            CommandInstance.CommandArgumentInfo.AutoCompleter.ShouldBeNull();
            CommandInstance.Type.ShouldBe(type);
            CommandInstance.Flags.HasFlag(CommandFlags.Strict).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.NoMaintenance).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.SettingVariable).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
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
        public void TestInitializeCommandInfoInstanceFromCommandLineArg(string type)
        {
            // Create instance
            var CommandInstance = new CommandInfo("help", type, "Help page", new CommandArgumentInfo(new[] { "testarg" }, Array.Empty<SwitchInfo>(), false, 0), null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo.Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo.Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo.Arguments.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo.Switches.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo.ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo.MinimumArguments.ShouldBe(0);
            CommandInstance.CommandArgumentInfo.AutoCompleter.ShouldBeNull();
            CommandInstance.Type.ShouldBe(type);
            CommandInstance.Flags.HasFlag(CommandFlags.Strict).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.NoMaintenance).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.SettingVariable).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
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
        public void TestInitializeCommandInfoInstanceFromCommandLineArgWithSwitch(string type)
        {
            // Create instance
            var CommandInstance = new CommandInfo("help", type, "Help page", new CommandArgumentInfo(Array.Empty<string>(), new[] { new SwitchInfo("s", "Simple help") }, false, 0), null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo.Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo.Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo.Arguments.ShouldBeEmpty();
            CommandInstance.CommandArgumentInfo.Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo.ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo.MinimumArguments.ShouldBe(0);
            CommandInstance.CommandArgumentInfo.AutoCompleter.ShouldBeNull();
            CommandInstance.Type.ShouldBe(type);
            CommandInstance.Flags.HasFlag(CommandFlags.Strict).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.NoMaintenance).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.SettingVariable).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();
            
            // Check for switch info correctness
            var @switch = CommandInstance.CommandArgumentInfo.Switches[0];
            @switch.SwitchName.ShouldBe("s");
            @switch.HelpDefinition.ShouldBe("Simple help");
            @switch.IsRequired.ShouldBeFalse();
            @switch.ArgumentsRequired.ShouldBeFalse();
            @switch.ConflictsWith.ShouldBeEmpty();
            @switch.OptionalizeLastRequiredArguments.ShouldBe(0);
        }

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
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
        public void TestInitializeCommandInfoInstanceFromCommandLineArgWithSwitchAndArg(string type)
        {
            // Create instance
            var CommandInstance = new CommandInfo("help", type, "Help page", new CommandArgumentInfo(new[] { "testarg" }, new[] { new SwitchInfo("s", "Simple help") }, false, 0), null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo.Arguments.ShouldNotBeNull();
            CommandInstance.CommandArgumentInfo.Switches.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo.Arguments.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo.Switches.ShouldNotBeEmpty();
            CommandInstance.CommandArgumentInfo.ArgumentsRequired.ShouldBeFalse();
            CommandInstance.CommandArgumentInfo.MinimumArguments.ShouldBe(0);
            CommandInstance.CommandArgumentInfo.AutoCompleter.ShouldBeNull();
            CommandInstance.Type.ShouldBe(type);
            CommandInstance.Flags.HasFlag(CommandFlags.Strict).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.NoMaintenance).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.SettingVariable).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();

            // Check for switch info correctness
            var @switch = CommandInstance.CommandArgumentInfo.Switches[0];
            @switch.SwitchName.ShouldBe("s");
            @switch.HelpDefinition.ShouldBe("Simple help");
            @switch.IsRequired.ShouldBeFalse();
            @switch.ArgumentsRequired.ShouldBeFalse();
            @switch.ConflictsWith.ShouldBeEmpty();
            @switch.OptionalizeLastRequiredArguments.ShouldBe(0);
        }

    }
}
