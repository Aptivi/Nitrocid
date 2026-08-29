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

using System.Collections.Generic;
using Terminaux.Shell.Switches;
using Terminaux.Shell.Arguments;
using Nitrocid.ShellPacks.Shells.RSS.Presets;
using Nitrocid.ShellPacks.Shells.RSS.Commands;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Prompts;

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
        public override BaseCommand[] Commands =>
        [
            new ArticleInfoCommand(),
            new BookmarkCommand(),
            new DetachCommand(),
            new FeedInfoCommand(),
            new ListCommand(),
            new ListBookmarkCommand(),
            new ReadCommand(),
            new SearchCommand(),
            new SelFeedCommand(),
            new UnbookmarkCommand(),
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
    }
}
