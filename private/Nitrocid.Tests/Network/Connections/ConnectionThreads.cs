//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Threading;
using Threadify.Manager;

namespace Nitrocid.Tests.Network.Connections
{
    internal static class ConnectionThreads
    {
        internal static ThreadInstance ftpThread = new("FTP thread", true, HandleConnection);
        internal static ThreadInstance httpThread = new("HTTP thread", true, HandleConnection);
        internal static ThreadInstance mailThread = new("Mail thread", true, HandleConnection);
        internal static ThreadInstance rssThread = new("RSS thread", true, HandleConnection);
        internal static ThreadInstance sftpThread = new("SFTP thread", true, HandleConnection);
        internal static ThreadInstance sshThread = new("SSH thread", true, HandleConnection);
        internal static ThreadInstance restThread = new("REST thread", true, HandleConnection);

        internal static void HandleConnection()
        {
            Thread.Sleep(500);
            Console.WriteLine("Connecting...");
            Thread.Sleep(1000);
            Console.WriteLine("Disconnecting...");
            Thread.Sleep(1000);
        }
    }
}
