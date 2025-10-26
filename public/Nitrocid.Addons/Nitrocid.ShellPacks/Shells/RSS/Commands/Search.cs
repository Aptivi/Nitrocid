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

using Nitrocid.ShellPacks.Tools;
using Nettify.Rss.Instance;
using Terminaux.Shell.Commands;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors.Themes.Colors;
using Textify.General;
using Terminaux.Base.Extensions;

namespace Nitrocid.ShellPacks.Shells.RSS.Commands
{
    /// <summary>
    /// Searhces the articles
    /// </summary>
    /// <remarks>
    /// If you want to search the articles for a phrase, you can use this command. You can also control searching for title, description, and case sensitivity.
    /// <list type="table">
    /// <listheader>
    /// <term>Switches</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>-t</term>
    /// <description>Search for title</description>
    /// </item>
    /// <item>
    /// <term>-d</term>
    /// <description>Search for description</description>
    /// </item>
    /// <item>
    /// <term>-a</term>
    /// <description>Search for title and description</description>
    /// </item>
    /// <item>
    /// <term>-cs</term>
    /// <description>Case sensitive search</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// </remarks>
    class SearchCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            bool findTitle = parameters.ContainsSwitch("-t");
            bool findDescription = parameters.ContainsSwitch("-d");
            bool findAll = parameters.ContainsSwitch("-a");
            bool caseSensitive = parameters.ContainsSwitch("-cs");

            if (findAll)
                findTitle = findDescription = true;

            var foundArticles = RSSShellTools.SearchArticles(parameters.ArgumentsList[0], findTitle, findDescription, caseSensitive);
            foreach (RSSArticle Article in foundArticles)
            {
                TextWriterColor.Write("- {0}: ", false, ThemeColorType.ListEntry, Article.ArticleTitle);
                TextWriterColor.Write(Article.ArticleLink, true, ThemeColorType.ListValue);
                TextWriterColor.Write("    {0}", Article.ArticleDescription.SplitNewLines()[0].Truncate(200));
            }
            return 0;
        }

    }
}
