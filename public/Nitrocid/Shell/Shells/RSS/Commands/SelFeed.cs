
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Inputs.Styles;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Network.RSS.Bookmarks;
using KS.Shell.ShellBase.Commands;
using System.Collections.Generic;

namespace KS.Shell.Shells.RSS.Commands
{
    /// <summary>
    /// Lets you select a feed
    /// </summary>
    /// <remarks>
    /// If you want to quickly select your favorite feed that you've bookmarked, you can use this command.
    /// </remarks>
    class RSS_SelFeedCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            var bookmarks = RSSBookmarkManager.GetBookmarks();
            if (bookmarks.Count == 0)
            {
                TextWriterColor.Write(Translate.DoTranslation("No bookmarks."), true, KernelColorType.Warning);
                return;
            }
            
            // Since the built-in InputChoiceInfo generator couldn't deal with URLs properly, we need to manually add each item
            // to the choice info list so that PromptSelection() can later present the choices to us.
            var bookmarksChoiceList = new List<InputChoiceInfo>();
            foreach (string bookmarkUrl in bookmarks)
                bookmarksChoiceList.Add(new InputChoiceInfo(bookmarkUrl, ""));
            int bookmarkNum = SelectionStyle.PromptSelection(Translate.DoTranslation("Select your favorite feed."), bookmarksChoiceList);

            // Get the feed from the list and connect the RSS shell to it
            string feed = RSSBookmarkManager.GetBookmark(bookmarkNum);
            RSSShellCommon.RSSFeedLink = feed;
        }

    }
}
