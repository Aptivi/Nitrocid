
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

using System.IO;
using KS.Files;
using KS.Misc.Text.Probers.Motd;
using NUnit.Framework;
using Shouldly;

namespace Nitrocid.Tests.Misc.Probers
{

    [TestFixture]
    [Order(2)]
    public class MOTDManagementTests
    {

        /// <summary>
        /// Tests reading MOTD from file
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestReadMOTDFromFile()
        {
            MotdParse.ReadMotd();
            string MOTDLine = File.ReadAllText(Paths.GetKernelPath(KernelPathType.MOTD));
            MOTDLine.ShouldBe(MotdParse.MOTDMessage);
        }

        /// <summary>
        /// Tests reading MAL from file
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestReadMALFromFile()
        {
            MalParse.ReadMal();
            string MALLine = File.ReadAllText(Paths.GetKernelPath(KernelPathType.MAL));
            MALLine.ShouldBe(MalParse.MAL);
        }

    }
}
