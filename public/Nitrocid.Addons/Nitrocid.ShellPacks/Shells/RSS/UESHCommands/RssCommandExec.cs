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

using System;
using Nettify.Rss.Instance;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Network.Connections;
using Nitrocid.ShellPacks.Shells.RSS.Tools;
using Terminaux.Reader;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.ShellPacks.Shells.RSS.UESHCommands
{
    internal class RssCommandExec : BaseCommand, ICommand
    {
        public override string Command =>
            "rss";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_COMMAND_RSS_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(false, "feedlink", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_COMMON_COMMAND_RSS_ARGUMENT_FEEDLINK_DESC"
                    }),
                ],
                [
                    new SwitchInfo("tui", /* Localizable */ "NKS_SHELLPACKS_COMMON_COMMAND_RSS_SWITCH_TUI_DESC"),
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
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
                    string address = TermReader.Read(LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_FEEDURLPROMPT") + ": ", Config.MainConfig.RssHeadlineUrl);
                    if (string.IsNullOrEmpty(address) || !Uri.TryCreate(address, UriKind.Absolute, out Uri? uri))
                    {
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_ADDRESSUNPARSABLE"), ThemeColorType.Error);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.RSSNetwork);
                    }
                    var connection = EstablishRssConnection(address);
                    feed = GetFeed(connection);
                }
                RSSTools.OpenFeedTui(feed);
            }
            else
                NetworkConnectionTools.OpenConnectionForShell("RSSShell", EstablishRssConnection, (_, connection) =>
                    EstablishRssConnection(connection.Address), parameters.ArgumentsText);
            return 0;
        }

        private NetworkInstanceConnection<RSSFeed> EstablishRssConnection(string address)
        {
            if (string.IsNullOrEmpty(address))
                address = TermReader.Read(LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_SERVERADDRESSPROMPT") + " ");
            return NetworkConnectionTools.EstablishConnection("RSS connection", address, NetworkConnectionType.RSS, new RSSFeed(address, RSSFeedType.Infer));
        }

        private RSSFeed GetFeed(NetworkInstanceConnection<RSSFeed> connection)
        {
            if (connection is null || connection.ConnectionInstance is not RSSFeed feed)
                throw new KernelException(KernelExceptionType.RSSNetwork, LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_READERCLI_EXCEPTION_INVALIDINSTANCE"));
            return feed;
        }
    }
}
