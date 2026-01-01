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

using Nitrocid.Extras.Ssh.SSH;
using Nitrocid.Shell.ShellBase.Commands;
using System;

namespace Nitrocid.Extras.Ssh.Commands
{
    /// <summary>
    /// You can interact with the Secure SHell server (SSH) to remotely execute commands on the host of another PC.
    /// </summary>
    /// <remarks>
    /// Secure SHell server (SSH) is a type of server which lets another computer connect to it to run commands in it. In the recent iterations, it is bound to support X11 forwarding. Our implementation is pretty basic, and uses the SSH.NET library by Renci.
    /// <br></br>
    /// This command lets you connect to another computer to remotely execute commands.
    /// </remarks>
    class SshcmdCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var AddressDelimiter = parameters.ArgumentsList[0].Split(':');
            string Address = AddressDelimiter[0];
            if (AddressDelimiter.Length > 1)
            {
                int Port = Convert.ToInt32(AddressDelimiter[1]);
                SSHTools.InitializeSSH(Address, Port, parameters.ArgumentsList[1], SSHTools.ConnectionType.Command, parameters.ArgumentsList[2]);
            }
            else
            {
                SSHTools.InitializeSSH(Address, 22, parameters.ArgumentsList[1], SSHTools.ConnectionType.Command, parameters.ArgumentsList[2]);
            }
            return 0;
        }

    }
}
