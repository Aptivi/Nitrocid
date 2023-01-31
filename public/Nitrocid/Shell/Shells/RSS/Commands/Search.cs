
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.Misc.Writers.ConsoleWriters;
using KS.Network.RSS;
using KS.Network.RSS.Instance;
using KS.Shell.ShellBase.Commands;
using System.Linq;

namespace KS.Shell.Shells.RSS.Commands
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
    class RSS_SearchCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            bool findTitle       = ListSwitchesOnly.Contains("-t");
            bool findDescription = ListSwitchesOnly.Contains("-d");
            bool findAll         = ListSwitchesOnly.Contains("-a");
            bool caseSensitive   = ListSwitchesOnly.Contains("-cs");

            if (findAll)
                findTitle = findDescription = true;

            var foundArticles = RSSTools.SearchArticles(ListArgsOnly[0], findTitle, findDescription, caseSensitive);
            foreach (RSSArticle Article in foundArticles)
            {
                TextWriterColor.Write("- {0}: ", false, KernelColorType.ListEntry, Article.ArticleTitle);
                TextWriterColor.Write(Article.ArticleLink, true, KernelColorType.ListValue);
                TextWriterColor.Write("    {0}", Article.ArticleDescription.SplitNewLines()[0].Truncate(200));
            }
        }

    }
}
