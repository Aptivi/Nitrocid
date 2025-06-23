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
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using Textify.General;
using Terminaux.Base.Extensions;

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

        public override int Execute(CommandParameters parameters, ref string variableValue)
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
