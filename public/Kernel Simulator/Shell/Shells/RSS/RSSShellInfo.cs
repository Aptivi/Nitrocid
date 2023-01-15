
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.Collections.Generic;
using KS.Shell.Prompts.Presets.RSS;
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.Shells.RSS.Commands;

namespace KS.Shell.Shells.RSS
{
    /// <summary>
    /// Common RSS shell class
    /// </summary>
    internal class RSSShellInfo : BaseShellInfo, IShellInfo
    {

        /// <summary>
        /// RSS commands
        /// </summary>
        public override Dictionary<string, CommandInfo> Commands => new()
        {
            { "articleinfo", new CommandInfo("articleinfo", ShellType, "Gets the article info", new CommandArgumentInfo(new[] { "<feednum>" }, true, 1), new RSS_ArticleInfoCommand()) },
            { "bookmark", new CommandInfo("bookmark", ShellType, "Bookmarks the feed", new CommandArgumentInfo(), new RSS_BookmarkCommand()) },
            { "chfeed", new CommandInfo("chfeed", ShellType, "Changes the feed link", new CommandArgumentInfo(new[] { "[-bookmark] <feedurl/bookmarknumber>" }, true, 1), new RSS_ChFeedCommand()) },
            { "feedinfo", new CommandInfo("feedinfo", ShellType, "Gets the feed info", new CommandArgumentInfo(), new RSS_FeedInfoCommand()) },
            { "list", new CommandInfo("list", ShellType, "Lists all feeds", new CommandArgumentInfo(), new RSS_ListCommand()) },
            { "listbookmark", new CommandInfo("listbookmark", ShellType, "Lists all bookmarked feeds", new CommandArgumentInfo(), new RSS_ListBookmarkCommand()) },
            { "read", new CommandInfo("read", ShellType, "Reads a feed in a web browser", new CommandArgumentInfo(new[] { "<feednum>" }, true, 1), new RSS_ReadCommand()) },
            { "search", new CommandInfo("search", ShellType, "Searches the feed for a phrase in title and/or description", new CommandArgumentInfo(new[] { "[-t|-d|-a|-cs] <phrase>" }, true, 1), new RSS_SearchCommand()) },
            { "selfeed", new CommandInfo("selfeed", ShellType, "Selects the feed from the existing feed list from online sources", new CommandArgumentInfo(), new RSS_SelFeedCommand()) },
            { "unbookmark", new CommandInfo("unbookmark", ShellType, "Removes the feed bookmark", new CommandArgumentInfo(), new RSS_UnbookmarkCommand()) }
        };

        public override Dictionary<string, PromptPresetBase> ShellPresets => new()
        {
            { "Default", new RSSDefaultPreset() },
            { "PowerLine1", new RSSPowerLine1Preset() },
            { "PowerLine2", new RSSPowerLine2Preset() },
            { "PowerLine3", new RSSPowerLine3Preset() },
            { "PowerLineBG1", new RSSPowerLineBG1Preset() },
            { "PowerLineBG2", new RSSPowerLineBG2Preset() },
            { "PowerLineBG3", new RSSPowerLineBG3Preset() }
        };

        public override BaseShell ShellBase => new RSSShell();

        public override PromptPresetBase CurrentPreset => PromptPresetManager.CurrentPresets["RSSShell"];

    }
}
