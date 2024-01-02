//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Interactive;
using KS.ConsoleBase.Writers;
using KS.Kernel.Configuration;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Network.Base.Connections;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Switches;
using Nettify.Rss.Instance;
using Nitrocid.Extras.RssShell.RSS.Interactive;
using System;

namespace Nitrocid.Extras.RssShell
{
    internal class RssCommandExec : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (SwitchManager.ContainsSwitch(parameters.SwitchesList, "-tui"))
            {
                if (parameters.ArgumentsList.Length > 0)
                {
                    RssReaderCli.rssConnection = EstablishRssConnection(parameters.ArgumentsList[0]);
                    ((RSSFeed)RssReaderCli.rssConnection.ConnectionInstance).Refresh();
                    InteractiveTuiTools.OpenInteractiveTui(new RssReaderCli());
                }
                else
                {
                    string address = Input.ReadLine(Translate.DoTranslation("Enter the RSS feed URL") + ": ", Config.MainConfig.RssHeadlineUrl);
                    if (string.IsNullOrEmpty(address) || !Uri.TryCreate(address, UriKind.Absolute, out Uri uri))
                    {
                        TextWriters.Write(Translate.DoTranslation("Error trying to parse the address. Make sure that you've written the address correctly."), KernelColorType.Error);
                        return 10000 + (int)KernelExceptionType.RSSNetwork;
                    }
                    RssReaderCli.rssConnection = EstablishRssConnection(address);
                    ((RSSFeed)RssReaderCli.rssConnection.ConnectionInstance).Refresh();
                    InteractiveTuiTools.OpenInteractiveTui(new RssReaderCli());
                }
            }
            else
                NetworkConnectionTools.OpenConnectionForShell("RSSShell", EstablishRssConnection, (_, connection) =>
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
