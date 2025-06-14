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

using Nitrocid.ConsoleBase.Inputs;
using Nitrocid.Languages;
using Nitrocid.Network.Connections;
using Nitrocid.Shell.ShellBase.Commands;
using System.Net.Http;

namespace Nitrocid.ShellPacks.Commands
{
    internal class HttpCommandExec : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            NetworkConnectionTools.OpenConnectionForShell("HTTPShell", EstablishHttpConnection, (_, connection) =>
            EstablishHttpConnection(connection.Address), parameters.ArgumentsText);
            return 0;
        }

        private NetworkConnection EstablishHttpConnection(string address)
        {
            if (string.IsNullOrEmpty(address))
                address = InputTools.ReadLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_SERVERADDRESSPROMPT") + " ", "Nitrocid.ShellPacks");
            return NetworkConnectionTools.EstablishConnection("HTTP connection", address, NetworkConnectionType.HTTP, new HttpClient());
        }

    }
}
