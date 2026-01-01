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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nitrocid.Shell.ShellBase.Aliases;
using Nitrocid.Shell.ShellBase.Shells;
using Shouldly;

namespace Nitrocid.Tests.Shell.ShellBase.Aliases
{

    [TestClass]
    public class AliasTests
    {

        /// <summary>
        /// Tests adding alias
        /// </summary>
        [TestMethod]
        [DataRow(ShellType.Shell, "shutdown", "poweroff")]
        [DataRow(ShellType.AdminShell, "journal", "j")]
        [DataRow(ShellType.DebugShell, "keyinfo", "key")]
        [DataRow(ShellType.HexShell, "clear", "wipe")]
        [DataRow(ShellType.TextShell, "clear", "wipe")]
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
        [TestMethod]
        [DataRow("Shell", "cls", "clear")]
        [DataRow("AdminShell", "lsevents", "lse")]
        [DataRow("DebugShell", "currentbt", "cbt")]
        [DataRow("HexShell", "print", "pr")]
        [DataRow("TextShell", "save", "s")]
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
        [TestMethod]
        [DataRow(ShellType.Shell)]
        [DataRow(ShellType.AdminShell)]
        [DataRow(ShellType.DebugShell)]
        [DataRow(ShellType.HexShell)]
        [DataRow(ShellType.TextShell)]
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
        [TestMethod]
        [DataRow("Shell")]
        [DataRow("AdminShell")]
        [DataRow("DebugShell")]
        [DataRow("HexShell")]
        [DataRow("TextShell")]
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
        [TestMethod]
        [DataRow(ShellType.Shell, "poweroff")]
        [DataRow(ShellType.AdminShell, "j")]
        [DataRow(ShellType.DebugShell, "key")]
        [DataRow(ShellType.HexShell, "wipe")]
        [DataRow(ShellType.TextShell, "wipe")]
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
        [TestMethod]
        [DataRow("Shell", "clear")]
        [DataRow("AdminShell", "lse")]
        [DataRow("DebugShell", "cbt")]
        [DataRow("HexShell", "pr")]
        [DataRow("TextShell", "s")]
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
        [TestMethod]
        [DataRow(ShellType.Shell)]
        [DataRow(ShellType.AdminShell)]
        [DataRow(ShellType.DebugShell)]
        [DataRow(ShellType.HexShell)]
        [DataRow(ShellType.TextShell)]
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
        [TestMethod]
        [DataRow("Shell")]
        [DataRow("AdminShell")]
        [DataRow("DebugShell")]
        [DataRow("HexShell")]
        [DataRow("TextShell")]
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
