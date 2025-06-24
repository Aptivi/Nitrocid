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

using Nitrocid.Base.Kernel.Threading;
using Nitrocid.Base.Network;
using Nitrocid.Base.Network.Connections;
using Nitrocid.Tests.Network.Connections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Collections.Generic;
using System.Threading;

namespace Nitrocid.Tests.Network
{

    [TestClass]
    public class NetworkActionTests
    {

        public static IEnumerable<(string, string, NetworkConnectionType, object)> Connections =>
            [
                // ---------- Provided ----------
                ("FTP client", "ftp.fabrikam.com", NetworkConnectionType.FTP, ConnectionThreads.ftpThread),
                ("HTTP client", "api.fabrikam.com", NetworkConnectionType.HTTP, ConnectionThreads.httpThread),
                ("Mail client", "mail@fabrikam.com", NetworkConnectionType.Mail, ConnectionThreads.mailThread),
                ("RSS client", "feed.fabrikam.com/atom", NetworkConnectionType.RSS, ConnectionThreads.rssThread),
                ("SFTP client", "sftp.fabrikam.com", NetworkConnectionType.SFTP, ConnectionThreads.sftpThread),
                ("SSH client", "freeshell.fabrikam.com", NetworkConnectionType.SSH, ConnectionThreads.sshThread),
            ];

        /// <summary>
        /// Tests establishing network connection (instances)
        /// </summary>
        [TestMethod]
        [DataRow("FTP client", "ftp.fabrikam.com", NetworkConnectionType.FTP, null)]
        [DataRow("HTTP client", "api.fabrikam.com", NetworkConnectionType.HTTP, null)]
        [DataRow("Mail client", "mail@fabrikam.com", NetworkConnectionType.Mail, null)]
        [DataRow("RSS client", "feed.fabrikam.com/atom", NetworkConnectionType.RSS, null)]
        [DataRow("SFTP client", "sftp.fabrikam.com", NetworkConnectionType.SFTP, null)]
        [DataRow("SSH client", "freeshell.fabrikam.com", NetworkConnectionType.SSH, null)]
        [Description("Action")]
        public void TestEstablishConnectionInstance(string name, string url, NetworkConnectionType type, object connectionClient)
        {
            var connection = NetworkConnectionTools.EstablishConnection(name, url, type, connectionClient);
            connection.ShouldNotBeNull();
            connection.ConnectionType.ShouldBe(type.ToString());
            connection.ConnectionName.ShouldContain(type.ToString());
            connection.ConnectionOriginalUrl.ShouldBe(url);
            connection.ConnectionUri.OriginalString.ShouldBe(url);
            connection.ConnectionIsInstance.ShouldBeTrue();
            connection.ConnectionInstance.ShouldBeNull();
            NetworkConnectionTools.GetNetworkConnections(type).ShouldContain(connection);
        }

        /// <summary>
        /// Tests establishing network connection (threads)
        /// </summary>
        [TestMethod]
        [DynamicData(nameof(Connections), DynamicDataDisplayNameDeclaringType = typeof(NetworkActionTests))]
        [Description("Action")]
        public void TestEstablishConnectionInstance(string name, string url, NetworkConnectionType type, KernelThread connectionClient)
        {
            var connection = NetworkConnectionTools.EstablishConnection(name, url, type, connectionClient);
            connection.ShouldNotBeNull();
            connection.ConnectionType.ShouldBe(type.ToString());
            connection.ConnectionName.ShouldContain(type.ToString());
            connection.ConnectionOriginalUrl.ShouldBe(url);
            connection.ConnectionUri.OriginalString.ShouldBe(url);
            connection.ConnectionIsInstance.ShouldBeFalse();
            connection.ConnectionInstance.ShouldBeNull();
            NetworkConnectionTools.GetNetworkConnections(type).ShouldContain(connection);
        }

        /// <summary>
        /// Tests getting network connections
        /// </summary>
        [TestMethod]
        [DataRow(NetworkConnectionType.FTP)]
        [DataRow(NetworkConnectionType.HTTP)]
        [DataRow(NetworkConnectionType.Mail)]
        [DataRow(NetworkConnectionType.RSS)]
        [DataRow(NetworkConnectionType.SFTP)]
        [DataRow(NetworkConnectionType.SSH)]
        [Description("Action")]
        public void TestGetNetworkConnections(NetworkConnectionType type)
        {
            var connections = NetworkConnectionTools.GetNetworkConnections(type);
            connections.ShouldNotBeNull();
            connections.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests getting network connections from a specific type
        /// </summary>
        [TestMethod]
        [DataRow(NetworkConnectionType.FTP)]
        [DataRow(NetworkConnectionType.HTTP)]
        [DataRow(NetworkConnectionType.Mail)]
        [DataRow(NetworkConnectionType.RSS)]
        [DataRow(NetworkConnectionType.SFTP)]
        [DataRow(NetworkConnectionType.SSH)]
        [Description("Action")]
        public void TestGetConnectionIndexSpecific(NetworkConnectionType type)
        {
            var connections = NetworkConnectionTools.GetNetworkConnections(type);
            connections.ShouldNotBeNull();
            connections.ShouldNotBeEmpty();
            var connection = connections[0];
            int index = NetworkConnectionTools.GetConnectionIndexSpecific(connection, type);
            index.ShouldBe(0);
            connections[index].ShouldBe(connection);
        }

        /// <summary>
        /// Tests getting network connections from a specific type
        /// </summary>
        [TestMethod]
        [DataRow(NetworkConnectionType.FTP)]
        [DataRow(NetworkConnectionType.HTTP)]
        [DataRow(NetworkConnectionType.Mail)]
        [DataRow(NetworkConnectionType.RSS)]
        [DataRow(NetworkConnectionType.SFTP)]
        [DataRow(NetworkConnectionType.SSH)]
        [Description("Action")]
        public void TestGetConnectionFromIndexSpecific(NetworkConnectionType type)
        {
            var connections = NetworkConnectionTools.GetNetworkConnections(type);
            connections.ShouldNotBeNull();
            connections.ShouldNotBeEmpty();
            var connection = NetworkConnectionTools.GetConnectionFromIndexSpecific(0, type);
            connections.ShouldContain(connection);
            connections[0].ShouldBe(connection);
        }

        /// <summary>
        /// Tests pinging with custom timeout and buffer
        /// </summary>
        [TestMethod]
        [Description("Action")]
        public void TestGetFilenameFromUrl()
        {
            string Url = "https://www.fabrikam.com/downloads/file.bin?apikey=FAAD64328FE82D";
            string FileNameFromUrl = NetworkTools.GetFilenameFromUrl(Url);
            FileNameFromUrl.ShouldNotBeNullOrEmpty();
            FileNameFromUrl.ShouldBe("file.bin");
        }

        /// <summary>
        /// Tests registering, establishing, closing, and unregistering a custom connection type
        /// </summary>
        [TestMethod]
        [Description("Action")]
        public void TestCustomConnectionType()
        {
            string typeName = "REST API";
            NetworkConnectionTools.RegisterCustomConnectionType(typeName);
            NetworkConnectionTools.ConnectionTypeExists(typeName).ShouldBeTrue();
            var connection = NetworkConnectionTools.EstablishConnection("MyConnection", "rest.fabrikam.com", typeName, ConnectionThreads.restThread);
            connection.ShouldNotBeNull();
            connection.ConnectionType.ShouldBe(typeName);
            connection.ConnectionName.ShouldContain("MyConnection");
            connection.ConnectionOriginalUrl.ShouldBe("rest.fabrikam.com");
            connection.ConnectionUri.OriginalString.ShouldBe("rest.fabrikam.com");
            connection.ConnectionIsInstance.ShouldBeFalse();
            connection.ConnectionInstance.ShouldBeNull();
            NetworkConnectionTools.GetNetworkConnections(typeName).ShouldContain(connection);
            int index = NetworkConnectionTools.GetConnectionIndex(connection);
            Thread.Sleep(3000);
            NetworkConnectionTools.CloseConnection(index);
            NetworkConnectionTools.UnregisterCustomConnectionType(typeName);
            NetworkConnectionTools.ConnectionTypeExists(typeName).ShouldBeFalse();
        }

    }
}
