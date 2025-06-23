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

using Nitrocid.Extras.Mods.Commands;
using Nitrocid.Extras.Mods.Localized;
using Nitrocid.Extras.Mods.Settings;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Extras.Mods
{
    internal class ModsInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("modman", /* Localizable */ "NKS_MODS_COMMAND_MODMAN_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "start/stop/info/reload/install/uninstall", new()
                        {
                            ExactWording = ["start", "stop", "info", "reload", "install", "uninstall"],
                            ArgumentDescription = /* Localizable */ "NKS_MODS_COMMAND_MODMAN_ARGUMENT_STARTSTOP_DESC"
                        }),
                        new CommandArgumentPart(true, "modfilename", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_MODS_COMMAND_MODMAN_ARGUMENT_MODFILENAME_DESC"
                        }),
                    ]),
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "list/reloadall/stopall/startall/tui", new()
                        {
                            ExactWording = ["list", "reloadall", "stopall", "startall", "tui"],
                            ArgumentDescription = /* Localizable */ "NKS_MODS_COMMAND_MODMAN_ARGUMENT_LISTRELOAD_DESC"
                        }),
                    ]),
                ], new ModManCommand()),

            new CommandInfo("modmanual", /* Localizable */ "NKS_MODS_COMMAND_MODMANUAL_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "modname", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_MODS_COMMAND_MODMANUAL_ARGUMENT_MODNAME_DESC"
                        }),
                    ])
                ], new ModManualCommand()),
        ];

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasMods);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasMods);

        public ModLoadPriority AddonType => ModLoadPriority.Important;

        internal static ModsConfig ModsConfig =>
            (ModsConfig)Config.baseConfigurations[nameof(ModsConfig)];

        public void FinalizeAddon()
        { }

        public void StartAddon()
        {
            LanguageTools.AddCustomAction(AddonName, new(() => LocalStrings.Languages, () => LocalStrings.Localizations, LocalStrings.Translate, LocalStrings.CheckCulture, LocalStrings.ListLanguagesCulture, LocalStrings.Exists));
            var config = new ModsConfig();
            ConfigTools.RegisterBaseSetting(config);
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
        }

        public void StopAddon()
        {
            LanguageTools.RemoveCustomAction(AddonName);
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(ModsConfig));
        }
    }
}
