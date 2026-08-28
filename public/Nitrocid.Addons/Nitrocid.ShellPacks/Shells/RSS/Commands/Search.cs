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
using Nitrocid.ShellPacks.Tools;
using Terminaux.Base.Extensions;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;

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
        public override string Command => 
            "search";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_COMMAND_SEARCH_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "phrase", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_RSS_COMMAND_SEARCH_ARGUMENT_PHRASE_DESC"
                    })
                ],
                [
                    new SwitchInfo("t", /* Localizable */ "NKS_SHELLPACKS_RSS_COMMAND_SEARCH_SWITCH_T_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    }),
                    new SwitchInfo("d", /* Localizable */ "NKS_SHELLPACKS_RSS_COMMAND_SEARCH_SWITCH_D_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    }),
                    new SwitchInfo("a", /* Localizable */ "NKS_SHELLPACKS_RSS_COMMAND_SEARCH_SWITCH_A_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    }),
                    new SwitchInfo("cs", /* Localizable */ "NKS_SHELLPACKS_RSS_COMMAND_SEARCH_SWITCH_CS_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    })
                ])
            ];

        public override CommandFlags Flags =>
            CommandFlags.Wrappable;

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            var rssShell = (RSSShell?)shell ??
                throw new KernelException(KernelExceptionType.RSSShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_LASTSHELLTYPEMISMATCH"));
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
