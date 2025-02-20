﻿//
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

using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.ConsoleBase.Colors;
using Textify.General;
using Terminaux.Base.Extensions;
using Nettify.Rss.Searcher;

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

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var foundFeeds = SearcherTools.GetRssFeeds(parameters.ArgumentsList[0]);
            foreach (var feed in foundFeeds)
            {
                TextWriters.Write("- {0}: ", false, KernelColorType.ListEntry, feed.Title);
                TextWriters.Write(feed.FeedId, true, KernelColorType.ListValue);
                TextWriterColor.Write("    {0}", feed.Description.SplitNewLines()[0].Truncate(200));
            }
            return 0;
        }

    }
}
