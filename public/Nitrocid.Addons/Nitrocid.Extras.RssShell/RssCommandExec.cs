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

using Nettify.Rss.Instance;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Inputs.Interactive;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Extras.RssShell.RSS.Interactive;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Switches;
using System;
using Nitrocid.ConsoleBase.Inputs;
using Nitrocid.Network.Connections;

namespace Nitrocid.Extras.RssShell
{
    internal class RssCommandExec : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (SwitchManager.ContainsSwitch(parameters.SwitchesList, "-tui"))
            {
                var tui = new RssReaderCli();
                tui.Bindings.Add(new InteractiveTuiBinding<RSSArticle>(Translate.DoTranslation("Info"), ConsoleKey.F1, (article, _, _, _) => tui.ShowArticleInfo(article)));
                tui.Bindings.Add(new InteractiveTuiBinding<RSSArticle>(Translate.DoTranslation("Read More"), ConsoleKey.F2, (article, _, _, _) => tui.OpenArticleLink(article)));
                tui.Bindings.Add(new InteractiveTuiBinding<RSSArticle>(Translate.DoTranslation("Refresh"), ConsoleKey.F3, (article, _, _, _) => tui.RefreshFeed()));
                var client = (RSSFeed?)tui.rssConnection?.ConnectionInstance ??
                    throw new KernelException(KernelExceptionType.RSSShell, Translate.DoTranslation("Client is not connected yet."));
                if (parameters.ArgumentsList.Length > 0)
                    tui.rssConnection = EstablishRssConnection(parameters.ArgumentsList[0]);
                else
                {
                    string address = InputTools.ReadLine(Translate.DoTranslation("Enter the RSS feed URL") + ": ", Config.MainConfig.RssHeadlineUrl);
                    if (string.IsNullOrEmpty(address) || !Uri.TryCreate(address, UriKind.Absolute, out Uri? uri))
                    {
                        TextWriters.Write(Translate.DoTranslation("Error trying to parse the address. Make sure that you've written the address correctly."), KernelColorType.Error);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.RSSNetwork);
                    }
                    tui.rssConnection = EstablishRssConnection(address);
                }
                client.Refresh();
                InteractiveTuiTools.OpenInteractiveTui(tui);
            }
            else
                NetworkConnectionTools.OpenConnectionForShell("RSSShell", EstablishRssConnection, (_, connection) =>
                    EstablishRssConnection(connection.Address), parameters.ArgumentsText);
            return 0;
        }

        private NetworkConnection EstablishRssConnection(string address)
        {
            if (string.IsNullOrEmpty(address))
                address = InputTools.ReadLine(Translate.DoTranslation("Enter the server address:") + " ");
            return NetworkConnectionTools.EstablishConnection("RSS connection", address, NetworkConnectionType.RSS, new RSSFeed(address, RSSFeedType.Infer));
        }

    }
}
