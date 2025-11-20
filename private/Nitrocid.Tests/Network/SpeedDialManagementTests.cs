//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using Nitrocid.Base.Network.Connections;
using Nitrocid.Base.Network.SpeedDial;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Nitrocid.Tests.Network
{

    [TestClass]
    public class SpeedDialManagementTests
    {

        /// <summary>
        /// Tests adding speed dial entry
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestAddEntryToSpeedDial() =>
            SpeedDialTools.TryAddEntryToSpeedDial("ftp.riken.jp", 21, NetworkConnectionType.FTP, "anonymous", "", false).ShouldBeTrue();

        /// <summary>
        /// Tests listing speed dial entries
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestListSpeedDialEntries() =>
            SpeedDialTools.ListSpeedDialEntries().ShouldNotBeEmpty();

    }
}
