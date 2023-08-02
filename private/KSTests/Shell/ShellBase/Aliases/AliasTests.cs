
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
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using NUnit.Framework;
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
        [Ignore("The program '[15212] testhost.exe' has exited with code 3221225477 (0xc0000005) 'Access violation'. " +
            "According to Event Viewer, P9: System.StackOverflowException")]
        [Description("Action")]
        public void TestAddAlias(ShellType type)
        {
            AliasManager.AddAlias("exit", "quit", type).ShouldBeTrue();
            var aliasList = AliasManager.GetAliasesListFromType(type);
            aliasList.ShouldNotBeNull();
            aliasList.ShouldNotBeEmpty();
            aliasList.ShouldContainKeyAndValue("exit", "quit");
            AliasManager.DoesAliasExist("quit", type).ShouldBeTrue();
        }

        /// <summary>
        /// Tests adding alias
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
        [Ignore("The program '[15212] testhost.exe' has exited with code 3221225477 (0xc0000005) 'Access violation'. " +
            "According to Event Viewer, P9: System.StackOverflowException")]
        [Description("Action")]
        public void TestAddAlias(string type)
        {
            AliasManager.AddAlias("exit", "quit", type).ShouldBeTrue();
            var aliasList = AliasManager.GetAliasesListFromType(type);
            aliasList.ShouldNotBeNull();
            aliasList.ShouldNotBeEmpty();
            aliasList.ShouldContainKeyAndValue("exit", "quit");
            AliasManager.DoesAliasExist("quit", type).ShouldBeTrue();
        }

        /// <summary>
        /// Tests removing alias
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
        [Ignore("The program '[15212] testhost.exe' has exited with code 3221225477 (0xc0000005) 'Access violation'. " +
            "According to Event Viewer, P9: System.StackOverflowException")]
        [Description("Action")]
        public void TestRemoveAlias(ShellType type)
        {
            AliasManager.RemoveAlias("quit", type).ShouldBeTrue();
            var aliasList = AliasManager.GetAliasesListFromType(type);
            aliasList.ShouldNotBeNull();
            aliasList.ShouldNotBeEmpty();
            aliasList.ShouldNotContainKey("exit");
            AliasManager.DoesAliasExist("quit", type).ShouldBeFalse();
        }

        /// <summary>
        /// Tests removing alias
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
        [Ignore("The program '[15212] testhost.exe' has exited with code 3221225477 (0xc0000005) 'Access violation'. " +
            "According to Event Viewer, P9: System.StackOverflowException")]
        [Description("Action")]
        public void TestRemoveAlias(string type)
        {
            AliasManager.RemoveAlias("quit", type).ShouldBeTrue();
            var aliasList = AliasManager.GetAliasesListFromType(type);
            aliasList.ShouldNotBeNull();
            aliasList.ShouldNotBeEmpty();
            aliasList.ShouldNotContainKey("exit");
            AliasManager.DoesAliasExist("quit", type).ShouldBeFalse();
        }

    }
}
