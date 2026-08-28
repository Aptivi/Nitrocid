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

using Nitrocid.Extras.Mods.Commands;
using Nitrocid.Extras.Mods.Settings;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Extensions;
using Nitrocid.Core.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Extras.Mods
{
    internal class ModsInit : IAddon
    {
        private readonly BaseCommand[] addonCommands =
        [
            new ModManCommand(),
            new ModManualCommand(),
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
