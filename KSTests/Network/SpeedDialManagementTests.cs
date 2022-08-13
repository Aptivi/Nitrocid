using System.Diagnostics;

// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Network;
using Microsoft.VisualBasic.CompilerServices;
using NUnit.Framework;
using Shouldly;

namespace KSTests
{

    [TestFixture]
    public class SpeedDialManagementTests
    {

        /// <summary>
    /// Tests adding FTP speed dial entry
    /// </summary>
        [Test]
        [Description("Management")]
        public void TestAddEntryToFTPSpeedDial()
        {
            NetworkTools.TryAddEntryToSpeedDial("ftp.riken.jp", Conversions.ToInteger("21"), "anonymous", NetworkTools.SpeedDialType.FTP, FluentFTP.FtpEncryptionMode.None, false).ShouldBeTrue();
        }

        /// <summary>
    /// Tests adding SFTP speed dial entry
    /// </summary>
        [Test]
        [Description("Management")]
        public void TestAddEntryToSFTPSpeedDial()
        {
            NetworkTools.TryAddEntryToSpeedDial("test.rebex.net", Conversions.ToInteger("22"), "demo", NetworkTools.SpeedDialType.SFTP, FluentFTP.FtpEncryptionMode.None, false).ShouldBeTrue();
        }

        /// <summary>
    /// Tests listing FTP speed dial entries
    /// </summary>
        [Test]
        [Description("Management")]
        public void TestListFTPSpeedDialEntries()
        {
            NetworkTools.ListSpeedDialEntries(NetworkTools.SpeedDialType.FTP).ShouldNotBeEmpty();
            Debug.WriteLine(string.Join(" | ", NetworkTools.ListSpeedDialEntries(NetworkTools.SpeedDialType.FTP).Keys));
        }

        /// <summary>
    /// Tests listing SFTP speed dial entries
    /// </summary>
        [Test]
        [Description("Management")]
        public void TestListSFTPSpeedDialEntries()
        {
            NetworkTools.ListSpeedDialEntries(NetworkTools.SpeedDialType.SFTP).ShouldNotBeEmpty();
            Debug.WriteLine(string.Join(" | ", NetworkTools.ListSpeedDialEntries(NetworkTools.SpeedDialType.SFTP).Keys));
        }

    }
}