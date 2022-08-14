
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

using System.Net.NetworkInformation;
using KS.Network;
using NUnit.Framework;
using Shouldly;

namespace KSTests
{

    // Warning: Don't implement the unit tests related to downloading or uploading files. This causes AppVeyor to choke.
    [TestFixture]
    public class NetworkActionTests
    {

        /// <summary>
        /// Tests pinging
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestPingAddress()
        {
            NetworkTools.PingAddress("www.google.com").Status.ShouldBe(IPStatus.Success);
        }

        /// <summary>
        /// Tests pinging with custom timeout
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestPingAddressCustomTimeout()
        {
            NetworkTools.PingAddress("www.google.com", 60000).Status.ShouldBe(IPStatus.Success);
        }

        /// <summary>
        /// Tests pinging with custom timeout and buffer
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestPingAddressCustomTimeoutAndBuffer()
        {
            NetworkTools.PingAddress("www.google.com", 60000, System.Text.Encoding.Default.GetBytes("KS")).Status.ShouldBe(IPStatus.Success);
        }

        /// <summary>
        /// Tests pinging with custom timeout and buffer
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestGetFilenameFromUrl()
        {
            string Url = "https://www.fabrikam.com/downloads/file.bin?apikey=FAAD64328FE82D";
            string FileNameFromUrl = NetworkTools.GetFilenameFromUrl(Url);
            FileNameFromUrl.ShouldNotBeNullOrEmpty();
            FileNameFromUrl.ShouldBe("file.bin");
        }

    }
}
