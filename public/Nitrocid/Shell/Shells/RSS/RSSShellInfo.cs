
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

using System.Collections.Generic;
using KS.Shell.Prompts.Presets.RSS;
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.Shells.RSS.Commands;
using System;

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
            { "articleinfo", new CommandInfo("articleinfo", ShellType, /* Localizable */ "Gets the article info",
                new CommandArgumentInfo(new[] { "feednum" }, Array.Empty<SwitchInfo>(), true, 1), new RSS_ArticleInfoCommand()) },
            { "bookmark", new CommandInfo("bookmark", ShellType, /* Localizable */ "Bookmarks the feed",
                new CommandArgumentInfo(), new RSS_BookmarkCommand()) },
            { "detach", new CommandInfo("detach", ShellType, /* Localizable */ "Exits the shell without disconnecting",
                new CommandArgumentInfo(), new RSS_DetachCommand()) },
            { "feedinfo", new CommandInfo("feedinfo", ShellType, /* Localizable */ "Gets the feed info",
                new CommandArgumentInfo(), new RSS_FeedInfoCommand()) },
            { "list", new CommandInfo("list", ShellType, /* Localizable */ "Lists all feeds",
                new CommandArgumentInfo(), new RSS_ListCommand()) },
            { "listbookmark", new CommandInfo("listbookmark", ShellType, /* Localizable */ "Lists all bookmarked feeds",
                new CommandArgumentInfo(), new RSS_ListBookmarkCommand()) },
            { "read", new CommandInfo("read", ShellType, /* Localizable */ "Reads a feed in a web browser",
                new CommandArgumentInfo(new[] { "feednum" }, Array.Empty<SwitchInfo>(), true, 1), new RSS_ReadCommand()) },
            { "search", new CommandInfo("search", ShellType, /* Localizable */ "Searches the feed for a phrase in title and/or description",
                new CommandArgumentInfo(new[] { "phrase" }, new[] { new SwitchInfo("t", /* Localizable */ "Search for title"), new SwitchInfo("d", /* Localizable */ "Search for description"), new SwitchInfo("a", /* Localizable */ "Search for title and description"), new SwitchInfo("cs", /* Localizable */ "Case sensitive search") }, true, 1), new RSS_SearchCommand()) },
            { "unbookmark", new CommandInfo("unbookmark", ShellType, /* Localizable */ "Removes the feed bookmark",
                new CommandArgumentInfo(), new RSS_UnbookmarkCommand()) }
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

        public override bool AcceptsNetworkConnection => true;

    }
}
