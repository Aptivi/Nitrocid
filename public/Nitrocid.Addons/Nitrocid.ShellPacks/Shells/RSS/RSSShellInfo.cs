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
using Nitrocid.ShellPacks.Shells.RSS.Presets;
using Nitrocid.ShellPacks.Shells.RSS.Commands;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Shell.Prompts;
using Nitrocid.Languages;

namespace Nitrocid.ShellPacks.Shells.RSS
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
            new CommandInfo("articleinfo", /* Localizable */ "NKS_SHELLPACKS_RSS_COMMAND_ARTICLEINFO_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "feednum", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_RSS_COMMAND_ARGUMENT_FEEDNUM_DESC"
                        })
                    ])
                ], new ArticleInfoCommand()),

            new CommandInfo("bookmark", /* Localizable */ "NKS_SHELLPACKS_RSS_COMMAND_BOOKMARK_DESC", new BookmarkCommand()),

            new CommandInfo("detach", /* Localizable */ "NKS_SHELLPACKS_FTPSFTP_COMMAND_DETACH_DESC", new DetachCommand()),

            new CommandInfo("feedinfo", /* Localizable */ "NKS_SHELLPACKS_RSS_COMMAND_FEEDINFO_DESC", new FeedInfoCommand()),

            new CommandInfo("list", /* Localizable */ "NKS_SHELLPACKS_RSS_COMMAND_LIST_DESC", new ListCommand(), CommandFlags.Wrappable),

            new CommandInfo("listbookmark", /* Localizable */ "NKS_SHELLPACKS_RSS_COMMAND_LISTBOOKMARK_DESC", new ListBookmarkCommand(), CommandFlags.Wrappable),

            new CommandInfo("read", /* Localizable */ "NKS_SHELLPACKS_RSS_COMMAND_READ_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "feednum", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_RSS_COMMAND_ARGUMENT_FEEDNUM_DESC"
                        })
                    ])
                ], new ReadCommand()),

            new CommandInfo("search", /* Localizable */ "NKS_SHELLPACKS_RSS_COMMAND_SEARCH_DESC",
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
                ], new SearchCommand(), CommandFlags.Wrappable),

            new CommandInfo("selfeed", /* Localizable */ "NKS_SHELLPACKS_RSS_COMMAND_SELFEED_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "phrase", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_RSS_COMMAND_SEARCH_ARGUMENT_PHRASE_DESC"
                        })
                    ])
                ], new SelFeedCommand(), CommandFlags.Wrappable),

            new CommandInfo("unbookmark", /* Localizable */ "NKS_SHELLPACKS_RSS_COMMAND_UNBOOKMARK_DESC", new UnbookmarkCommand()),
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
