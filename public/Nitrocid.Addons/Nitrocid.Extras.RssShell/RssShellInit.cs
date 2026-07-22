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

using Terminaux.Shell.Arguments;
using Terminaux.Shell.Switches;
using Nitrocid.Extras.RssShell.RSS;
using Nitrocid.Extras.RssShell.Settings;
using Nitrocid.Kernel.Configuration;
using Terminaux.Shell.Commands;
using System.Collections.Generic;
using Nitrocid.Kernel.Extensions;
using Terminaux.Shell.Shells;
using System.Linq;
using Nitrocid.Languages;

namespace Nitrocid.Extras.RssShell
{
    internal class RssShellInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("rss", LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_COMMAND_RSS_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "feedlink", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_COMMAND_RSS_ARGUMENT_FEEDLINK_DESC")
                        }),
                    ],
                    [
                        new SwitchInfo("tui", LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_COMMAND_RSS_SWITCH_TUI_DESC")),
                    ])
                ], new RssCommandExec())
        ];

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasRssShell);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasRssShell);

        internal static RssConfig RssConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(RssConfig)) ? (RssConfig)Config.baseConfigurations[nameof(RssConfig)] : Config.GetFallbackKernelConfig<RssConfig>();

        public void FinalizeAddon()
        {
            LanguageTools.AddCustomAction(AddonName, new("Nitrocid.Extras.RssShell.Resources.Languages.Output.Localizations", typeof(RssShellInit).Assembly));
            var config = new RssConfig();
            ConfigTools.RegisterBaseSetting(config);
            ShellManager.RegisterShell("RSSShell", new RSSShellInfo());
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
        }

        public void StartAddon()
        { }

        public void StopAddon()
        {
            LanguageTools.RemoveCustomAction(AddonName);
            ShellManager.UnregisterShell("RSSShell");
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(RssConfig));
        }
    }
}
