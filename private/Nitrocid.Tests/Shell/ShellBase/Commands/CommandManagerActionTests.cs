
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
        [TestCase(ShellType.ArchiveShell, ExpectedResult = false)]
        [TestCase(ShellType.DebugShell, ExpectedResult = false)]
        [TestCase(ShellType.FTPShell, ExpectedResult = true)]
        [TestCase(ShellType.HexShell, ExpectedResult = false)]
        [TestCase(ShellType.HTTPShell, ExpectedResult = false)]
        [TestCase(ShellType.JsonShell, ExpectedResult = false)]
        [TestCase(ShellType.MailShell, ExpectedResult = false)]
        [TestCase(ShellType.RSSShell, ExpectedResult = false)]
        [TestCase(ShellType.SFTPShell, ExpectedResult = true)]
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
        [TestCase("ArchiveShell", ExpectedResult = false)]
        [TestCase("DebugShell", ExpectedResult = false)]
        [TestCase("FTPShell", ExpectedResult = true)]
        [TestCase("HexShell", ExpectedResult = false)]
        [TestCase("HTTPShell", ExpectedResult = false)]
        [TestCase("JsonShell", ExpectedResult = false)]
        [TestCase("MailShell", ExpectedResult = false)]
        [TestCase("RSSShell", ExpectedResult = false)]
        [TestCase("SFTPShell", ExpectedResult = true)]
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
            CommandManager.IsCommandFound("detach").ShouldBeTrue();

    }
}
