
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.ConsoleBase.Inputs;
using KS.Languages;
using KS.Network.Base.Connections;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using System.Net.Http;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// You can interact with the Hyper Text Transfer Protocol (HTTP) using this shell.
    /// </summary>
    class HttpCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, ref string variableValue)
        {
            NetworkConnectionTools.OpenConnectionForShell(ShellType.HTTPShell, EstablishHttpConnection, (_, connection) => EstablishHttpConnection(connection["Address"].ToString()), StringArgs);
            return 0;
        }

        private NetworkConnection EstablishHttpConnection(string address)
        {
            if (string.IsNullOrEmpty(address))
                address = Input.ReadLine(Translate.DoTranslation("Enter the server address:") + " ");
            return NetworkConnectionTools.EstablishConnection("HTTP connection", address, NetworkConnectionType.HTTP, new HttpClient());
        }

    }
}
