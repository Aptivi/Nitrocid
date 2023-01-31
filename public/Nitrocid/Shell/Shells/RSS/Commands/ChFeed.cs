
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

using System;
using System.Linq;
using KS.Network.RSS.Bookmarks;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.RSS.Commands
{
    /// <summary>
    /// Changes current feed
    /// </summary>
    /// <remarks>
    /// If you want to read another feed, you can use this command to provide a second feed URL to the shell so you can interact with it.
    /// </remarks>
    class RSS_ChFeedCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            bool UseBookmarkNum = ListSwitchesOnly.Contains("-bookmark");
            int BookmarkNum;
            if (UseBookmarkNum)
            {
                BookmarkNum = int.Parse(ListArgsOnly[0]);
                RSSShellCommon.RSSFeedLink = RSSBookmarkManager.GetBookmark(BookmarkNum);
            }
            else
            {
                RSSShellCommon.RSSFeedLink = ListArgsOnly[0];
            }
        }

    }
}