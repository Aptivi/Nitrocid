
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

using System.Diagnostics;
using KS.Network.SpeedDial;
using NUnit.Framework;
using Shouldly;

namespace KSTests.NetworkTests
{

    [TestFixture]
    public class SpeedDialManagementTests
    {

        /// <summary>
        /// Tests adding FTP speed dial entry
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestAddEntryToFTPSpeedDial() => SpeedDialTools.TryAddEntryToSpeedDial("ftp.riken.jp", 21, SpeedDialType.FTP, false, "anonymous", FluentFTP.FtpEncryptionMode.None).ShouldBeTrue();

        /// <summary>
        /// Tests adding SFTP speed dial entry
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestAddEntryToSFTPSpeedDial() => SpeedDialTools.TryAddEntryToSpeedDial("test.rebex.net", 22, SpeedDialType.SFTP, false, "demo", FluentFTP.FtpEncryptionMode.None).ShouldBeTrue();

        /// <summary>
        /// Tests listing FTP speed dial entries
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestListFTPSpeedDialEntries()
        {
            SpeedDialTools.ListSpeedDialEntries(SpeedDialType.FTP).ShouldNotBeEmpty();
            Debug.WriteLine(string.Join(" | ", SpeedDialTools.ListSpeedDialEntries(SpeedDialType.FTP).Keys));
        }

        /// <summary>
        /// Tests listing SFTP speed dial entries
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestListSFTPSpeedDialEntries()
        {
            SpeedDialTools.ListSpeedDialEntries(SpeedDialType.SFTP).ShouldNotBeEmpty();
            Debug.WriteLine(string.Join(" | ", SpeedDialTools.ListSpeedDialEntries(SpeedDialType.SFTP).Keys));
        }

    }
}
