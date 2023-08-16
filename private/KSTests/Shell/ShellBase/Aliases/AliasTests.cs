
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

using KS.Shell.ShellBase.Aliases;
using KS.Shell.ShellBase.Shells;
using NUnit.Framework;
using Org.BouncyCastle.Asn1.X509;
using Shouldly;

namespace KSTests.Shell.ShellBase.Aliases
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
        [TestCase(ShellType.ArchiveShell, "cdir", "currentdir")]
        [TestCase(ShellType.DebugShell, "threadsbt", "tbt")]
        [TestCase(ShellType.FTPShell, "get", "download")]
        [TestCase(ShellType.HexShell, "clear", "wipe")]
        [TestCase(ShellType.HTTPShell, "delete", "wipe")]
        [TestCase(ShellType.JsonShell, "clear", "wipe")]
        [TestCase(ShellType.MailShell, "rm", "wipe")]
        [TestCase(ShellType.RSSShell, "search", "find")]
        [TestCase(ShellType.SFTPShell, "put", "upload")]
        [TestCase(ShellType.TextShell, "clear", "wipe")]
        [Description("Action")]
        public void TestAddAlias(ShellType type, string source, string target)
        {
            AliasManager.AddAlias(source, target, type).ShouldBeTrue();
            AliasManager.DoesAliasExistLocal(target, type).ShouldBeTrue();
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
        [TestCase("ArchiveShell", "chdir", "cd")]
        [TestCase("DebugShell", "currentbt", "cbt")]
        [TestCase("FTPShell", "cat", "read")]
        [TestCase("HexShell", "print", "pr")]
        [TestCase("HTTPShell", "post", "pf")]
        [TestCase("JsonShell", "print", "pr")]
        [TestCase("MailShell", "list", "ls")]
        [TestCase("RSSShell", "list", "ls")]
        [TestCase("SFTPShell", "cat", "read")]
        [TestCase("TextShell", "save", "s")]
        [Description("Action")]
        public void TestAddAlias(string type, string source, string target)
        {
            AliasManager.AddAlias(source, target, type).ShouldBeTrue();
            AliasManager.DoesAliasExistLocal(target, type).ShouldBeTrue();
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
        [Description("Action")]
        public void TestAddAliasForUnifiedCommand(ShellType type)
        {
            AliasManager.AddAlias("exit", "quit", type).ShouldBeTrue();
            AliasManager.DoesAliasExistLocal("quit", type).ShouldBeTrue();
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
        [Description("Action")]
        public void TestAddAliasForUnifiedCommand(string type)
        {
            AliasManager.AddAlias("presets", "p", type).ShouldBeTrue();
            AliasManager.DoesAliasExistLocal("p", type).ShouldBeTrue();
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
        [TestCase(ShellType.ArchiveShell, "currentdir")]
        [TestCase(ShellType.DebugShell, "tbt")]
        [TestCase(ShellType.FTPShell, "download")]
        [TestCase(ShellType.HexShell, "wipe")]
        [TestCase(ShellType.HTTPShell, "wipe")]
        [TestCase(ShellType.JsonShell, "wipe")]
        [TestCase(ShellType.MailShell, "wipe")]
        [TestCase(ShellType.RSSShell, "find")]
        [TestCase(ShellType.SFTPShell, "upload")]
        [TestCase(ShellType.TextShell, "wipe")]
        [Description("Action")]
        public void TestRemoveAlias(ShellType type, string target)
        {
            AliasManager.InitAliases();
            AliasManager.RemoveAlias(target, type).ShouldBeTrue();
            AliasManager.DoesAliasExistLocal(target, type).ShouldBeFalse();
            AliasManager.PurgeAliases();
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
        [TestCase("ArchiveShell", "cd")]
        [TestCase("DebugShell", "cbt")]
        [TestCase("FTPShell", "read")]
        [TestCase("HexShell", "pr")]
        [TestCase("HTTPShell", "pf")]
        [TestCase("JsonShell", "pr")]
        [TestCase("MailShell", "ls")]
        [TestCase("RSSShell", "ls")]
        [TestCase("SFTPShell", "read")]
        [TestCase("TextShell", "s")]
        [Description("Action")]
        public void TestRemoveAlias(string type, string target)
        {
            AliasManager.InitAliases();
            AliasManager.RemoveAlias(target, type).ShouldBeTrue();
            AliasManager.DoesAliasExistLocal(target, type).ShouldBeFalse();
            AliasManager.PurgeAliases();
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
        [Description("Action")]
        public void TestRemoveAliasForUnifiedCommand(ShellType type)
        {
            AliasManager.InitAliases();
            AliasManager.RemoveAlias("quit", type).ShouldBeTrue();
            AliasManager.DoesAliasExistLocal("quit", type).ShouldBeFalse();
            AliasManager.PurgeAliases();
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
        [Description("Action")]
        public void TestRemoveAliasForUnifiedCommand(string type)
        {
            AliasManager.InitAliases();
            AliasManager.RemoveAlias("p", type).ShouldBeTrue();
            AliasManager.DoesAliasExistLocal("p", type).ShouldBeFalse();
            AliasManager.PurgeAliases();
            AliasManager.SaveAliases();
            AliasManager.DoesAliasExist("p", type).ShouldBeFalse();
        }

    }
}
