//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.ConsoleBase.Inputs;
using KS.Languages;
using KS.Network.Base.Connections;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using Syndian.Instance;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Opens an RSS shell
    /// </summary>
    /// <remarks>
    /// You can interact with the RSS shell to connect to a feed server and interact with them.
    /// </remarks>
    class RssCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            NetworkConnectionTools.OpenConnectionForShell(ShellType.RSSShell, EstablishRssConnection, (_, connection) =>
            EstablishRssConnection(connection.Address), parameters.ArgumentsText);
            return 0;
        }

        private NetworkConnection EstablishRssConnection(string address)
        {
            if (string.IsNullOrEmpty(address))
                address = Input.ReadLine(Translate.DoTranslation("Enter the server address:") + " ");
            return NetworkConnectionTools.EstablishConnection("RSS connection", address, NetworkConnectionType.RSS, new RSSFeed(address, RSSFeedType.Infer));
        }

    }
}
