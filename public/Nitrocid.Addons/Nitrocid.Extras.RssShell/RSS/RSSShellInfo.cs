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

using System.Collections.Generic;
using Nitrocid.Shell.ShellBase.Switches;
using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Extras.RssShell.RSS.Presets;
using Nitrocid.Extras.RssShell.RSS.Commands;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Shell.Prompts;

namespace Nitrocid.Extras.RssShell.RSS
{
    /// <summary>
    /// Common RSS shell class
    /// </summary>
    internal class RSSShellInfo : BaseShellInfo<RSSShell>, IShellInfo
    {
        /// <summary>
        /// RSS commands
        /// </summary>
        public override List<CommandInfo> Commands =>
        [
            new CommandInfo("articleinfo", /* Localizable */ "Gets the article info",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "feednum", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "Feed number"
                        })
                    ])
                ], new ArticleInfoCommand()),

            new CommandInfo("bookmark", /* Localizable */ "Bookmarks the feed", new BookmarkCommand()),

            new CommandInfo("detach", /* Localizable */ "Exits the shell without disconnecting", new DetachCommand()),

            new CommandInfo("feedinfo", /* Localizable */ "Gets the feed info", new FeedInfoCommand()),

            new CommandInfo("list", /* Localizable */ "Lists all feeds", new ListCommand(), CommandFlags.Wrappable),

            new CommandInfo("listbookmark", /* Localizable */ "Lists all bookmarked feeds", new ListBookmarkCommand(), CommandFlags.Wrappable),

            new CommandInfo("read", /* Localizable */ "Reads a feed in a web browser",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "feednum", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "Feed number"
                        })
                    ])
                ], new ReadCommand()),

            new CommandInfo("search", /* Localizable */ "Searches the feed for a phrase in title and/or description",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "phrase", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Phrase to search"
                        })
                    ],
                    [
                        new SwitchInfo("t", /* Localizable */ "Search for title", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("d", /* Localizable */ "Search for description", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("a", /* Localizable */ "Search for title and description", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("cs", /* Localizable */ "Case sensitive search", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new SearchCommand(), CommandFlags.Wrappable),

            new CommandInfo("selfeed", /* Localizable */ "Searches the feed library for a feed",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "phrase", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Phrase to search"
                        })
                    ])
                ], new SelFeedCommand(), CommandFlags.Wrappable),

            new CommandInfo("unbookmark", /* Localizable */ "Removes the feed bookmark", new UnbookmarkCommand()),
        ];

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

        public override bool AcceptsNetworkConnection => true;

        public override string NetworkConnectionType => "RSS";
    }
}
