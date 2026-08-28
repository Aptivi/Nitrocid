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

using Nettify.Rss.Instance;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Terminaux.Base.Extensions;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;

namespace Nitrocid.ShellPacks.Shells.RSS.Commands
{
    /// <summary>
    /// Lists articles
    /// </summary>
    /// <remarks>
    /// If you want to get articles found in the current RSS feed, you can use this command.
    /// </remarks>
    class ListCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "list";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_COMMAND_LIST_DESC");

        public override CommandFlags Flags =>
            CommandFlags.Wrappable;

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            foreach (RSSArticle Article in RSSShellCommon.RSSFeedInstance?.FeedArticles ?? [])
            {
                TextWriterColor.Write("- {0}: ", false, ThemeColorType.ListEntry, Article.ArticleTitle);
                TextWriterColor.Write(Article.ArticleLink, true, ThemeColorType.ListValue);
                TextWriterColor.Write("    {0}", Article.ArticleDescription.SplitNewLines()[0].Truncate(200));
            }
            return 0;
        }

    }
}
