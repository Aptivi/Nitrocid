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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;

namespace Nitrocid.ShellPacks.Shells.RSS.Commands
{
    /// <summary>
    /// Gets feed information
    /// </summary>
    /// <remarks>
    /// If you want to know more about the current RSS feed, you can use this command to get some information about it. It currently provides you the title, the link, the description, the feed type, and the number of articles.
    /// </remarks>
    class FeedInfoCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var feed = RSSShellCommon.RSSFeedInstance ??
                throw new KernelException(KernelExceptionType.RSSShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_FEEDINFO_NOFEED", "Nitrocid.ShellPacks"));
            TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_ARTICLEINFO_TITLE", "Nitrocid.ShellPacks") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write(feed.FeedTitle, true, KernelColorType.ListValue);
            TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_ARTICLEINFO_LINK", "Nitrocid.ShellPacks") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write(feed.FeedUrl, true, KernelColorType.ListValue);
            TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_ARTICLEINFO_DESC", "Nitrocid.ShellPacks") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write(feed.FeedDescription, true, KernelColorType.ListValue);
            TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_ARTICLEINFO_FEEDTYPE", "Nitrocid.ShellPacks") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write(feed.FeedType.ToString(), true, KernelColorType.ListValue);
            TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_ARTICLEINFO_ARTICLECOUNT", "Nitrocid.ShellPacks") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write(feed.FeedArticles.Length.ToString(), true, KernelColorType.ListValue);
            return 0;
        }

    }
}
