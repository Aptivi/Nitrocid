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

using Nitrocid.Base.Languages;
using Nitrocid.Base.Network.Connections;
using Nitrocid.Base.Network.SpeedDial;
using Nitrocid.ShellPacks.Shells.SFTP.Tools;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;

namespace Nitrocid.ShellPacks.Shells.SFTP.UESHCommands
{
    internal class SftpCommandExec : BaseCommand, ICommand
    {
        public override string Command =>
            "sftp";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_COMMAND_SFTP_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(false, "server", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_COMMON_COMMAND_SFTP_ARGUMENT_SERVER_DESC"
                    }),
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            NetworkConnectionTools.OpenConnectionForShell("SFTPShell", SFTPTools.SFTPTryToConnect, EstablishSftpConnection, parameters.ArgumentsText);
            return 0;
        }

        private NetworkConnection EstablishSftpConnection(string address, SpeedDialEntry connection)
        {
            var info = SFTPTools.GetConnectionInfo(address, connection.Port, connection.Username);
            return SFTPTools.ConnectSFTP(info);
        }

    }
}
