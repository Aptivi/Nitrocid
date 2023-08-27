
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
using KS.Misc.Probers.Motd;
using KS.Misc.Probers.Placeholder;
using NUnit.Framework;
using Shouldly;

namespace Nitrocid.Tests.Misc.Probers
{

    [TestFixture]
    [Order(1)]
    public class MOTDSettingTests
    {

        /// <summary>
        /// Tests setting MOTD
        /// </summary>
        [Test]
        [Description("Setting")]
        public void TestSetMOTD()
        {
            MotdParse.SetMotd(PlaceParse.ProbePlaces("Hello, I am on <system>"));
            var MOTDFile = new StreamReader(Paths.GetKernelPath(KernelPathType.MOTD));
            MOTDFile.ReadLine().ShouldBe(PlaceParse.ProbePlaces("Hello, I am on <system>"));
            MOTDFile.Close();
        }

        /// <summary>
        /// Tests setting MAL
        /// </summary>
        [Test]
        [Description("Setting")]
        public void TestSetMAL()
        {
            MalParse.SetMal(PlaceParse.ProbePlaces("Hello, I am on <system>"));
            var MALFile = new StreamReader(Paths.GetKernelPath(KernelPathType.MAL));
            MALFile.ReadLine().ShouldBe(PlaceParse.ProbePlaces("Hello, I am on <system>"));
            MALFile.Close();
        }

    }
}
