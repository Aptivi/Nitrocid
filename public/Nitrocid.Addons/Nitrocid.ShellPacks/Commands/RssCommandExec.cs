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
using Terminaux.Colors.Themes.Colors;
using Terminaux.Inputs.Interactive;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.ShellPacks.Shells.RSS.Interactive;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Switches;
using System;
using Nitrocid.ConsoleBase.Inputs;
using Nitrocid.Network.Connections;

namespace Nitrocid.ShellPacks.Commands
{
    internal class RssCommandExec : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (SwitchManager.ContainsSwitch(parameters.SwitchesList, "-tui"))
            {
                var tui = new RssReaderCli();
                tui.Bindings.Add(new InteractiveTuiBinding<RSSArticle>(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_KEYBINDING_INFO"), ConsoleKey.F1, (article, _, _, _) => tui.ShowArticleInfo(article)));
                tui.Bindings.Add(new InteractiveTuiBinding<RSSArticle>(LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_TUI_KEYBINDING_READMORE"), ConsoleKey.F2, (article, _, _, _) => tui.OpenArticleLink(article)));
                tui.Bindings.Add(new InteractiveTuiBinding<RSSArticle>(LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_TUI_KEYBINDING_REFRESH"), ConsoleKey.F3, (article, _, _, _) => tui.RefreshFeed()));
                var client = (RSSFeed?)tui.rssConnection?.ConnectionInstance ??
                    throw new KernelException(KernelExceptionType.RSSShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_NOTCONNECTED_1"));
                if (parameters.ArgumentsList.Length > 0)
                    tui.rssConnection = EstablishRssConnection(parameters.ArgumentsList[0]);
                else
                {
                    string address = InputTools.ReadLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_FEEDURLPROMPT") + ": ", Config.MainConfig.RssHeadlineUrl);
                    if (string.IsNullOrEmpty(address) || !Uri.TryCreate(address, UriKind.Absolute, out Uri? uri))
                    {
                        TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_ADDRESSUNPARSABLE"), ThemeColorType.Error);
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
                address = InputTools.ReadLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_SERVERADDRESSPROMPT") + " ");
            return NetworkConnectionTools.EstablishConnection("RSS connection", address, NetworkConnectionType.RSS, new RSSFeed(address, RSSFeedType.Infer));
        }

    }
}
