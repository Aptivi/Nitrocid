
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

namespace KSTests.Shell
{

    [TestFixture]
    public class CommandManagerActionTests
    {

        /// <summary>
        /// Tests seeing if the command is found in specific shell (test case: Normal shell, help command)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestIsCommandFoundInSpecificShell() => CommandManager.IsCommandFound("help", ShellType.Shell).ShouldBeTrue();

        /// <summary>
        /// Tests seeing if the command is found in all the shells (test case: detach command)
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestIsCommandFoundInAllTheShells() => CommandManager.IsCommandFound("detach").ShouldBeTrue();

    }
}
