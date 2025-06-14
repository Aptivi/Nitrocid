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
using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Extras.Mods
{
    internal class ModsInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("modman", LanguageTools.GetLocalized("NKS_MODS_COMMAND_MODMAN_DESC", "Nitrocid.Extras.Mods"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "start/stop/info/reload/install/uninstall", new()
                        {
                            ExactWording = ["start", "stop", "info", "reload", "install", "uninstall"],
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_MODS_COMMAND_MODMAN_ARGUMENT_STARTSTOP_DESC", "Nitrocid.Extras.Mods")
                        }),
                        new CommandArgumentPart(true, "modfilename", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_MODS_COMMAND_MODMAN_ARGUMENT_MODFILENAME_DESC", "Nitrocid.Extras.Mods")
                        }),
                    ]),
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "list/reloadall/stopall/startall/tui", new()
                        {
                            ExactWording = ["list", "reloadall", "stopall", "startall", "tui"],
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_MODS_COMMAND_MODMAN_ARGUMENT_LISTRELOAD_DESC", "Nitrocid.Extras.Mods")
                        }),
                    ]),
                ], new ModManCommand(), CommandFlags.Strict),

            new CommandInfo("modmanual", LanguageTools.GetLocalized("NKS_MODS_COMMAND_MODMANUAL_DESC", "Nitrocid.Extras.Mods"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "modname", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_MODS_COMMAND_MODMANUAL_ARGUMENT_MODNAME_DESC", "Nitrocid.Extras.Mods")
                        }),
                    ])
                ], new ModManualCommand()),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasMods);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Important;

        internal static ModsConfig ModsConfig =>
            (ModsConfig)Config.baseConfigurations[nameof(ModsConfig)];

        void IAddon.FinalizeAddon()
        { }

        void IAddon.StartAddon()
        {
            LanguageTools.AddCustomAction("Nitrocid.Extras.Mods", new(() => LocalStrings.Languages, () => LocalStrings.Localizations, LocalStrings.Translate, LocalStrings.CheckCulture, LocalStrings.ListLanguagesCulture, LocalStrings.Exists));
            var config = new ModsConfig();
            ConfigTools.RegisterBaseSetting(config);
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);
        }

        void IAddon.StopAddon()
        {
            LanguageTools.RemoveCustomAction("Nitrocid.Extras.Mods");
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(ModsConfig));
        }
    }
}
