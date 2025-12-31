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

using Nitrocid.Base.Network.Connections;
using Nitrocid.Base.Network.SpeedDial;
using Terminaux.Shell.Commands;
using Nitrocid.ShellPacks.Tools;

namespace Nitrocid.ShellPacks.Commands
{
    internal class SftpCommandExec : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
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
