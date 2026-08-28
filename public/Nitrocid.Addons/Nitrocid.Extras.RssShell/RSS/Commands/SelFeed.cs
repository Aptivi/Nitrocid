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

using Nettify.Rss.Searcher;
using Nitrocid.Languages;
using Terminaux.Base.Extensions;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;

namespace Nitrocid.Extras.RssShell.RSS.Commands
{
    /// <summary>
    /// Searches the feeds
    /// </summary>
    /// <remarks>
    /// If you want to search the feed library for a feed, you can use this command.
    /// </remarks>
    class SelFeedCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "selfeed";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_COMMAND_SELFEED_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "phrase", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_RSS_COMMAND_SEARCH_ARGUMENT_PHRASE_DESC"
                    })
                ])
            ];

        public override CommandFlags Flags =>
            CommandFlags.Wrappable;

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            var foundFeeds = SearcherTools.GetRssFeeds(parameters.ArgumentsList[0]);
            foreach (var feed in foundFeeds)
            {
                TextWriterColor.Write("- {0}: ", false, ThemeColorType.ListEntry, feed.Title);
                TextWriterColor.Write(feed.FeedId, true, ThemeColorType.ListValue);
                TextWriterColor.Write("    {0}", feed.Description.SplitNewLines()[0].Truncate(200));
            }
            return 0;
        }

    }
}
