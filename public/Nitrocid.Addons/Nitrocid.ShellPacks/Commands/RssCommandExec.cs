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

using Nettify.Rss.Instance;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Commands;
using System;
using Nitrocid.Base.ConsoleBase.Inputs;
using Nitrocid.Base.Network.Connections;
using Nitrocid.ShellPacks.Tools;

namespace Nitrocid.ShellPacks.Commands
{
    internal class RssCommandExec : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (parameters.ContainsSwitch("-tui"))
            {
                RSSFeed? feed = null;
                if (parameters.ArgumentsList.Length > 0)
                {
                    var connection = EstablishRssConnection(parameters.ArgumentsList[0]);
                    feed = GetFeed(connection);
                }
                else
                {
                    string address = InputTools.ReadLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_FEEDURLPROMPT") + ": ", Config.MainConfig.RssHeadlineUrl);
                    if (string.IsNullOrEmpty(address) || !Uri.TryCreate(address, UriKind.Absolute, out Uri? uri))
                    {
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_ADDRESSUNPARSABLE"), ThemeColorType.Error);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.RSSNetwork);
                    }
                    var connection = EstablishRssConnection(address);
                    feed = GetFeed(connection);
                }
                RSSShellTools.OpenFeedTui(feed);
            }
            else
                NetworkConnectionTools.OpenConnectionForShell("RSSShell", EstablishRssConnection, (_, connection) =>
                    EstablishRssConnection(connection.Address), parameters.ArgumentsText);
            return 0;
        }

        private NetworkConnection EstablishRssConnection(string address)
        {
            if (string.IsNullOrEmpty(address))
                address = InputTools.ReadLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_SERVERADDRESSPROMPT") + " ");
            return NetworkConnectionTools.EstablishConnection("RSS connection", address, NetworkConnectionType.RSS, new RSSFeed(address, RSSFeedType.Infer));
        }

        private RSSFeed GetFeed(NetworkConnection connection)
        {
            if (connection is null || connection.ConnectionInstance is not RSSFeed feed)
                throw new KernelException(KernelExceptionType.RSSNetwork, LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_READERCLI_EXCEPTION_INVALIDINSTANCE"));
            return feed;
        }
    }
}
