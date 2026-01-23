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

namespace Nitrocid.Extras.Mods
{
    internal class ModsInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("modman", /* Localizable */ "Manage your mods",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "start/stop/info/reload/install/uninstall", new()
                        {
                            ExactWording = ["start", "stop", "info", "reload", "install", "uninstall"],
                            ArgumentDescription = /* Localizable */ "Whether to start, stop, get info out of, reload, install, or uninstall a mod"
                        }),
                        new CommandArgumentPart(true, "modfilename", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Mod name or file name"
                        }),
                    ]),
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "list/reloadall/stopall/startall/tui", new()
                        {
                            ExactWording = ["list", "reloadall", "stopall", "startall", "tui"],
                            ArgumentDescription = /* Localizable */ "Whether to list, reload, stop, start, or interactively manage all mods"
                        }),
                    ]),
                ], new ModManCommand()),

            new CommandInfo("modmanual", /* Localizable */ "Mod manual",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "modname", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Mod name"
                        }),
                    ])
                ], new ModManualCommand()),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasMods);

        internal static ModsConfig ModsConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(ModsConfig)) ? (ModsConfig)Config.baseConfigurations[nameof(ModsConfig)] : Config.GetFallbackKernelConfig<ModsConfig>();

        void IAddon.FinalizeAddon()
        { }

        void IAddon.StartAddon()
        {
            var config = new ModsConfig();
            ConfigTools.RegisterBaseSetting(config);
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
        }

        void IAddon.StopAddon()
        {
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(ModsConfig));
        }
    }
}
