
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

using KS.Kernel.Threading;
using KS.Network.Base;
using KS.Network.Base.Connections;
using KSTests.Network.Connections;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using System.Linq;

namespace KSTests.Network
{

    [TestFixture]
    public class NetworkActionTests
    {

        private static IEnumerable<TestCaseData> Connections
        {
            get
            {
                return new[] {
                    //               ---------- Provided ----------
                    new TestCaseData("FTP client", "ftp.fabrikam.com", NetworkConnectionType.FTP, ConnectionThreads.ftpThread),
                    new TestCaseData("HTTP client", "api.fabrikam.com", NetworkConnectionType.HTTP, ConnectionThreads.httpThread),
                    new TestCaseData("Mail client", "mail@fabrikam.com", NetworkConnectionType.Mail, ConnectionThreads.mailThread),
                    new TestCaseData("RSS client", "feed.fabrikam.com/atom", NetworkConnectionType.RSS, ConnectionThreads.rssThread),
                    new TestCaseData("SFTP client", "sftp.fabrikam.com", NetworkConnectionType.SFTP, ConnectionThreads.sftpThread),
                    new TestCaseData("SSH client", "freeshell.fabrikam.com", NetworkConnectionType.SSH, ConnectionThreads.sshThread),
                };
            }
        }

        /// <summary>
        /// Tests establishing network connection (instances)
        /// </summary>
        [Test]
        [TestCase("FTP client", "ftp.fabrikam.com", NetworkConnectionType.FTP, null)]
        [TestCase("HTTP client", "api.fabrikam.com", NetworkConnectionType.HTTP, null)]
        [TestCase("Mail client", "mail@fabrikam.com", NetworkConnectionType.Mail, null)]
        [TestCase("RSS client", "feed.fabrikam.com/atom", NetworkConnectionType.RSS, null)]
        [TestCase("SFTP client", "sftp.fabrikam.com", NetworkConnectionType.SFTP, null)]
        [TestCase("SSH client", "freeshell.fabrikam.com", NetworkConnectionType.SSH, null)]
        [Description("Action")]
        public void TestEstablishConnectionInstance(string name, string url, NetworkConnectionType type, object connectionClient)
        {
            var connection = NetworkConnectionTools.EstablishConnection(name, url, type, connectionClient);
            connection.ShouldNotBeNull();
            connection.ConnectionType.ShouldBe(type);
            connection.ConnectionName.ShouldContain(type.ToString());
            connection.ConnectionOriginalUrl.ShouldBe(url);
            connection.ConnectionUri.OriginalString.ShouldBe(url);
            connection.ConnectionIsInstance.ShouldBeTrue();
            connection.ConnectionInstance.ShouldBeNull();
            int index = NetworkConnectionTools.GetConnectionIndex(connection);
            NetworkConnectionTools.GetNetworkConnections(type)[index].ShouldBe(connection);
        }

        /// <summary>
        /// Tests establishing network connection (threads)
        /// </summary>
        [Test]
        [TestCaseSource(nameof(Connections))]
        [Description("Action")]
        public void TestEstablishConnectionInstance(string name, string url, NetworkConnectionType type, KernelThread connectionClient)
        {
            var connection = NetworkConnectionTools.EstablishConnection(name, url, type, connectionClient);
            connection.ShouldNotBeNull();
            connection.ConnectionType.ShouldBe(type);
            connection.ConnectionName.ShouldContain(type.ToString());
            connection.ConnectionOriginalUrl.ShouldBe(url);
            connection.ConnectionUri.OriginalString.ShouldBe(url);
            connection.ConnectionIsInstance.ShouldBeFalse();
            connection.ConnectionInstance.ShouldBeNull();
            int index = NetworkConnectionTools.GetConnectionIndex(connection);
            NetworkConnectionTools.GetNetworkConnections(type)[index].ShouldBe(connection);
        }

        /// <summary>
        /// Tests getting network connections
        /// </summary>
        [Test]
        [TestCase(NetworkConnectionType.FTP)]
        [TestCase(NetworkConnectionType.HTTP)]
        [TestCase(NetworkConnectionType.Mail)]
        [TestCase(NetworkConnectionType.RSS)]
        [TestCase(NetworkConnectionType.SFTP)]
        [TestCase(NetworkConnectionType.SSH)]
        [Description("Action")]
        public void TestGetNetworkConnections(NetworkConnectionType type)
        {
            var connections = NetworkConnectionTools.GetNetworkConnections(type);
            connections.ShouldNotBeNull();
            connections.ShouldNotBeEmpty();
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
