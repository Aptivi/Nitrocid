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

using FluentFTP;
using Nitrocid.Base.Network.Connections;
using Nitrocid.Base.Network.SpeedDial;
using Terminaux.Shell.Commands;
using Nitrocid.ShellPacks.Tools;
using System;

namespace Nitrocid.ShellPacks.Commands
{
    internal class FtpCommandExec : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            NetworkConnectionTools.OpenConnectionForShell("FTPShell", FTPTools.TryToConnect, EstablishFtpConnection, parameters.ArgumentsText);
            return 0;
        }

        private NetworkConnection? EstablishFtpConnection(string address, SpeedDialEntry connection)
        {
            var options = connection.Options;
            var encMode = options.Length > 1 ? Enum.Parse<FtpEncryptionMode>(options[1].ToString() ?? "") : FtpEncryptionMode.Auto;
            return FTPTools.PromptForPassword(null, options[0].ToString() ?? "", address, connection.Port, options.Length > 1 ? encMode : FtpEncryptionMode.None);
        }

    }
}
