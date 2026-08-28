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
using Nitrocid.Base.Network.Types.RSS;
using Nitrocid.ShellPacks.Shells.RSS.Interactive;
using Nitrocid.ShellPacks.Tools;
using Terminaux.Inputs.Interactive;
using Terminaux.Reader;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.ShellPacks.Commands
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
                var tui = new RssReaderCli();
                tui.Bindings.Add(new InteractiveTuiBinding<RSSArticle>(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_KEYBINDING_INFO"), ConsoleKey.F1, (article, _, _, _) => tui.ShowArticleInfo(article)));
                tui.Bindings.Add(new InteractiveTuiBinding<RSSArticle>(LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_TUI_KEYBINDING_READMORE"), ConsoleKey.F2, (article, _, _, _) => tui.OpenArticleLink(article)));
                tui.Bindings.Add(new InteractiveTuiBinding<RSSArticle>(LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_TUI_KEYBINDING_REFRESH"), ConsoleKey.F3, (article, _, _, _) => tui.RefreshFeed()));
                if (parameters.ArgumentsList.Length > 0)
                    tui.rssConnection = EstablishRssConnection(parameters.ArgumentsList[0]);
                else
                {
                    string address = TermReader.Read(LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_FEEDURLPROMPT") + ": ", Config.MainConfig.RssHeadlineUrl);
                    if (string.IsNullOrEmpty(address) || !Uri.TryCreate(address, UriKind.Absolute, out Uri? uri))
                    {
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_ADDRESSUNPARSABLE"), ThemeColorType.Error);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.RSSNetwork);
                    }
                    tui.rssConnection = EstablishRssConnection(address);
                }
                var client = (RSSFeed?)tui.rssConnection?.ConnectionInstance ??
                    throw new KernelException(KernelExceptionType.RSSShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_NOTCONNECTED_1"));
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
                address = TermReader.Read(LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_SERVERADDRESSPROMPT") + " ");
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
