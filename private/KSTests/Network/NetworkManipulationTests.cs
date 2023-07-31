
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

using KS.Kernel.Configuration;
using KS.Network.Base;
using NUnit.Framework;
using Shouldly;

namespace KSTests.Network
{

    // Warning: Don't implement the unit tests related to downloading or uploading files. This causes AppVeyor to choke.
    [TestFixture]
    public class NetworkManipulationTests
    {

        /// <summary>
        /// Tests hostname change
        /// </summary>
        [Test]
        [Description("Manipulation")]
        public void TestChangeHostname()
        {
            NetworkTools.TryChangeHostname("NewHost").ShouldBeTrue();
            NetworkTools.HostName.ShouldBe("NewHost");
            Config.MainConfig.HostName.ShouldBe("NewHost");
        }

    }
}
