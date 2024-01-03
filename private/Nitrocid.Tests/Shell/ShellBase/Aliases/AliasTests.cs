//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using Nitrocid.Shell.ShellBase.Aliases;
using Nitrocid.Shell.ShellBase.Shells;
using NUnit.Framework;
using Shouldly;

namespace Nitrocid.Tests.Shell.ShellBase.Aliases
{

    [TestFixture]
    public class AliasTests
    {

        /// <summary>
        /// Tests adding alias
        /// </summary>
        [Test]
        [Order(1)]
        [TestCase(ShellType.Shell, "shutdown", "poweroff")]
        [TestCase(ShellType.AdminShell, "journal", "j")]
        [TestCase(ShellType.DebugShell, "keyinfo", "key")]
        [TestCase(ShellType.HexShell, "clear", "wipe")]
        [TestCase(ShellType.TextShell, "clear", "wipe")]
        [Description("Action")]
        public void TestAddAlias(ShellType type, string source, string target)
        {
            AliasManager.AddAlias(source, target, type).ShouldBeTrue();
            AliasManager.SaveAliases();
            AliasManager.DoesAliasExist(target, type).ShouldBeTrue();
        }

        /// <summary>
        /// Tests adding alias
        /// </summary>
        [Test]
        [Order(2)]
        [TestCase("Shell", "cls", "clear")]
        [TestCase("AdminShell", "lsevents", "lse")]
        [TestCase("DebugShell", "currentbt", "cbt")]
        [TestCase("HexShell", "print", "pr")]
        [TestCase("TextShell", "save", "s")]
        [Description("Action")]
        public void TestAddAlias(string type, string source, string target)
        {
            AliasManager.AddAlias(source, target, type).ShouldBeTrue();
            AliasManager.SaveAliases();
            AliasManager.DoesAliasExist(target, type).ShouldBeTrue();
        }

        /// <summary>
        /// Tests adding alias
        /// </summary>
        [Test]
        [Order(1)]
        [TestCase(ShellType.Shell)]
        [TestCase(ShellType.AdminShell)]
        [TestCase(ShellType.DebugShell)]
        [TestCase(ShellType.HexShell)]
        [TestCase(ShellType.TextShell)]
        [Description("Action")]
        public void TestAddAliasForUnifiedCommand(ShellType type)
        {
            AliasManager.AddAlias("exit", "quit", type).ShouldBeTrue();
            AliasManager.SaveAliases();
            AliasManager.DoesAliasExist("quit", type).ShouldBeTrue();
        }

        /// <summary>
        /// Tests adding alias
        /// </summary>
        [Test]
        [Order(2)]
        [TestCase("Shell")]
        [TestCase("AdminShell")]
        [TestCase("DebugShell")]
        [TestCase("HexShell")]
        [TestCase("TextShell")]
        [Description("Action")]
        public void TestAddAliasForUnifiedCommand(string type)
        {
            AliasManager.AddAlias("presets", "p", type).ShouldBeTrue();
            AliasManager.SaveAliases();
            AliasManager.DoesAliasExist("p", type).ShouldBeTrue();
        }

        /// <summary>
        /// Tests removing alias
        /// </summary>
        [Test]
        [Order(3)]
        [TestCase(ShellType.Shell, "poweroff")]
        [TestCase(ShellType.AdminShell, "j")]
        [TestCase(ShellType.DebugShell, "key")]
        [TestCase(ShellType.HexShell, "wipe")]
        [TestCase(ShellType.TextShell, "wipe")]
        [Description("Action")]
        public void TestRemoveAlias(ShellType type, string target)
        {
            AliasManager.InitAliases();
            AliasManager.RemoveAlias(target, type).ShouldBeTrue();
            AliasManager.SaveAliases();
            AliasManager.DoesAliasExist(target, type).ShouldBeFalse();
        }

        /// <summary>
        /// Tests removing alias
        /// </summary>
        [Test]
        [Order(4)]
        [TestCase("Shell", "clear")]
        [TestCase("AdminShell", "lse")]
        [TestCase("DebugShell", "cbt")]
        [TestCase("HexShell", "pr")]
        [TestCase("TextShell", "s")]
        [Description("Action")]
        public void TestRemoveAlias(string type, string target)
        {
            AliasManager.InitAliases();
            AliasManager.RemoveAlias(target, type).ShouldBeTrue();
            AliasManager.SaveAliases();
            AliasManager.DoesAliasExist(target, type).ShouldBeFalse();
        }

        /// <summary>
        /// Tests removing alias
        /// </summary>
        [Test]
        [Order(3)]
        [TestCase(ShellType.Shell)]
        [TestCase(ShellType.AdminShell)]
        [TestCase(ShellType.DebugShell)]
        [TestCase(ShellType.HexShell)]
        [TestCase(ShellType.TextShell)]
        [Description("Action")]
        public void TestRemoveAliasForUnifiedCommand(ShellType type)
        {
            AliasManager.InitAliases();
            AliasManager.RemoveAlias("quit", type).ShouldBeTrue();
            AliasManager.SaveAliases();
            AliasManager.DoesAliasExist("quit", type).ShouldBeFalse();
        }

        /// <summary>
        /// Tests removing alias
        /// </summary>
        [Test]
        [Order(4)]
        [TestCase("Shell")]
        [TestCase("AdminShell")]
        [TestCase("DebugShell")]
        [TestCase("HexShell")]
        [TestCase("TextShell")]
        [Description("Action")]
        public void TestRemoveAliasForUnifiedCommand(string type)
        {
            AliasManager.InitAliases();
            AliasManager.RemoveAlias("p", type).ShouldBeTrue();
            AliasManager.SaveAliases();
            AliasManager.DoesAliasExist("p", type).ShouldBeFalse();
        }

    }
}
