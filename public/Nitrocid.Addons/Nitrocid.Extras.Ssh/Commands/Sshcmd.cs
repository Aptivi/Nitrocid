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

using Nitrocid.Languages;
using Nitrocid.Extras.Ssh.SSH;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;

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
        public override string Command =>
            "sshcmd";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_SSH_COMMAND_SSHCMD_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "address:port", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SSH_COMMAND_ARGUMENT_ADDRESS_DESC"
                    }),
                    new CommandArgumentPart(true, "username", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SSH_COMMAND_ARGUMENT_USERNAME_DESC"
                    }),
                    new CommandArgumentPart(true, "command", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SSH_COMMAND_SSHCMD_ARGUMENT_COMMAND_DESC"
                    }),
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string fullAddress = parameters.ArgumentsList[0];
            string username = parameters.ArgumentsList[1];
            string command = parameters.ArgumentsList[2];
            var splitAddress = fullAddress.Split(':');
            string address = splitAddress[0];
            int port = splitAddress.Length > 1 ? int.Parse(splitAddress[1]) : 22;
            SSHTools.InitializeSSH(address, port, username, SSHTools.ConnectionType.Command, command);
            return 0;
        }

    }
}
