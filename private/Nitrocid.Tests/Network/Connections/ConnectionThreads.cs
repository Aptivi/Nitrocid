//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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
using System;
using System.Threading;

namespace Nitrocid.Tests.Network.Connections
{
    internal static class ConnectionThreads
    {
        internal static KernelThread ftpThread = new("FTP thread", true, HandleConnection);
        internal static KernelThread httpThread = new("HTTP thread", true, HandleConnection);
        internal static KernelThread mailThread = new("Mail thread", true, HandleConnection);
        internal static KernelThread rssThread = new("RSS thread", true, HandleConnection);
        internal static KernelThread sftpThread = new("SFTP thread", true, HandleConnection);
        internal static KernelThread sshThread = new("SSH thread", true, HandleConnection);
        internal static KernelThread restThread = new("REST thread", true, HandleConnection);

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
