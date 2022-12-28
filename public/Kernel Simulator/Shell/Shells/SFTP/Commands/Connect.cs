
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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

using KS.Network.SFTP;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.SFTP.Commands
{
    /// <summary>
    /// Connects your SFTP client to any SFTP server that is valid
    /// </summary>
    /// <remarks>
    /// This command must be executed before running any interactive SFTP server commands, like get, put, cdl, cdr, etc.
    /// <br></br>
    /// This command opens a new session to connect your SFTP client to any SFTP server that is open to the public, and valid. It then asks for your credentials. Try with anonymous first, then usernames.
    /// </remarks>
    class SFTP_ConnectCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (ListArgsOnly[0].StartsWith("sftp://"))
            {
                SFTPTools.SFTPTryToConnect(ListArgsOnly[0]);
            }
            else
            {
                SFTPTools.SFTPTryToConnect($"sftp://{ListArgsOnly[0]}");
            }
        }

    }
}