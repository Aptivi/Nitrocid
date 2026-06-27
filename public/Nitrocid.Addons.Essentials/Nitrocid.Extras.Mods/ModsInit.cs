//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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
using Nitrocid.Extras.Mods.Settings;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Extensions;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using System.Collections.Generic;
using System.Linq;
using Nitrocid.Languages;

namespace Nitrocid.Extras.Mods
{
    internal class ModsInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("modman", LanguageTools.GetLocalized("NKS_MODS_COMMAND_MODMAN_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "start/stop/info/reload/install/uninstall", new()
                        {
                            ExactWording = ["start", "stop", "info", "reload", "install", "uninstall"],
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_MODS_COMMAND_MODMAN_ARGUMENT_STARTSTOP_DESC")
                        }),
                        new CommandArgumentPart(true, "modfilename", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_MODS_COMMAND_MODMAN_ARGUMENT_MODFILENAME_DESC")
                        }),
                    ]),
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "list/reloadall/stopall/startall/tui", new()
                        {
                            ExactWording = ["list", "reloadall", "stopall", "startall", "tui"],
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_MODS_COMMAND_MODMAN_ARGUMENT_LISTRELOAD_DESC")
                        }),
                    ]),
                ], new ModManCommand()),

            new CommandInfo("modmanual", LanguageTools.GetLocalized("NKS_MODS_COMMAND_MODMANUAL_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "modname", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_MODS_COMMAND_MODMANUAL_ARGUMENT_MODNAME_DESC")
                        }),
                    ])
                ], new ModManualCommand()),
        ];

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasMods);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasMods);

        internal static ModsConfig ModsConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(ModsConfig)) ? (ModsConfig)Config.baseConfigurations[nameof(ModsConfig)] : Config.GetFallbackKernelConfig<ModsConfig>();

        public void FinalizeAddon()
        { }

        public void StartAddon()
        {
            LanguageTools.AddCustomAction(AddonName, new("Nitrocid.Extras.Mods.Resources.Languages.Output.Localizations", typeof(ModsInit).Assembly));
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
