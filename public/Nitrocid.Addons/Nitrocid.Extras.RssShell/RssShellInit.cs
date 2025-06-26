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

using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Shell.ShellBase.Switches;
using Nitrocid.Extras.RssShell.RSS;
using Nitrocid.Extras.RssShell.Settings;
using Nitrocid.Extras.RssShell.Tools;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Shell.ShellBase.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Modifications;
using System.Linq;

namespace Nitrocid.Extras.RssShell
{
    internal class RssShellInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("rss", /* Localizable */ "Opens an RSS shell to read the feeds",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "feedlink", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "RSS feed link"
                        }),
                    ],
                    [
                        new SwitchInfo("tui", /* Localizable */ "Opens an interactive RSS feed reader TUI"),
                    ])
                ], new RssCommandExec())
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasRssShell);

        internal static RssConfig RssConfig =>
            (RssConfig)Config.baseConfigurations[nameof(RssConfig)];

        ReadOnlyDictionary<string, Delegate>? IAddon.PubliclyAvailableFunctions =>
            new(new Dictionary<string, Delegate>()
            {
                { nameof(RSSShellTools.GetFirstArticle), RSSShellTools.GetFirstArticle },
                { nameof(RSSShellTools.GetArticles), RSSShellTools.GetArticles },
                { nameof(RSSShellTools.SearchArticles), RSSShellTools.SearchArticles },
                { nameof(RSSBookmarkManager.AddRSSFeedToBookmark), new Action(RSSBookmarkManager.AddRSSFeedToBookmark) },
                { nameof(RSSBookmarkManager.AddRSSFeedToBookmark) + "2", new Action<string>(RSSBookmarkManager.AddRSSFeedToBookmark) },
                { nameof(RSSBookmarkManager.RemoveRSSFeedFromBookmark), new Action(RSSBookmarkManager.RemoveRSSFeedFromBookmark) },
                { nameof(RSSBookmarkManager.RemoveRSSFeedFromBookmark) + "2", new Action<string>(RSSBookmarkManager.RemoveRSSFeedFromBookmark) },
                { nameof(RSSBookmarkManager.GetBookmarks), new Func<List<string>>(RSSBookmarkManager.GetBookmarks) },
                { nameof(RSSBookmarkManager.GetBookmark), new Func<int, string>(RSSBookmarkManager.GetBookmark) },
            });

        ReadOnlyDictionary<string, PropertyInfo>? IAddon.PubliclyAvailableProperties => null;

        ReadOnlyDictionary<string, FieldInfo>? IAddon.PubliclyAvailableFields => null;

        void IAddon.FinalizeAddon()
        {
            var config = new RssConfig();
            ConfigTools.RegisterBaseSetting(config);
            ShellManager.RegisterAddonShell("RSSShell", new RSSShellInfo());
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);
        }

        void IAddon.StartAddon()
        { }

        void IAddon.StopAddon()
        {
            ShellManager.UnregisterAddonShell("RSSShell");
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(RssConfig));
        }
    }
}
