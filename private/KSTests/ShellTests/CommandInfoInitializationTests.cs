
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

namespace KSTests.ShellTests
{

    [TestFixture]
    public class CommandInfoInitializationTests
    {

        /// <summary>
        /// Tests initializing CommandInfo instance from a command line Command
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeCommandInfoInstanceFromCommandLineArg()
        {
            // Create instance
            var CommandInstance = new CommandInfo("help", ShellType.Shell, "Help page", new CommandArgumentInfo(new[] { "" }, false, 0), null);

            // Check for null
            CommandInstance.ShouldNotBeNull();
            CommandInstance.Command.ShouldNotBeNullOrEmpty();
            CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty();
            CommandInstance.CommandArgumentInfo.HelpUsages.ShouldNotBeNull();

            // Check for property correctness
            CommandInstance.Command.ShouldBe("help");
            CommandInstance.HelpDefinition.ShouldBe("Help page");
            CommandInstance.CommandArgumentInfo.HelpUsages.ShouldNotBeEmpty();
            CommandInstance.Type.ShouldBe("Shell");
            CommandInstance.Flags.HasFlag(CommandFlags.Strict).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.Obsolete).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.NoMaintenance).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.SettingVariable).ShouldBeFalse();
            CommandInstance.Flags.HasFlag(CommandFlags.RedirectionSupported).ShouldBeFalse();
        }

    }
}
