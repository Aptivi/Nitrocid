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

using Terminaux.Shell.Arguments;
using Nitrocid.Extras.Docking.Commands;
using Nitrocid.Extras.Docking.Dock;
using Nitrocid.Extras.Docking.Localized;
using Terminaux.Shell.Commands;
using System.Collections.Generic;
using Nitrocid.Kernel.Extensions;
using System.Linq;
using Nitrocid.Languages;

namespace Nitrocid.Extras.Docking
{
    internal class DockingInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("dock", /* Localizable */ "NKS_DOCKING_COMMAND_DOCK_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "dockName", new()
                        {
                            AutoCompleter = (_) => DockTools.GetDockScreenNames(),
                            ArgumentDescription = /* Localizable */ "NKS_DOCKING_COMMAND_DOCK_ARGUMENT_DOCKNAME_DESC"
                        }),
                    ])
                ], new DockCommand())
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasDocking);

        string IAddon.AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasDocking);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        void IAddon.FinalizeAddon()
        { }

        void IAddon.StartAddon()
        {
            LanguageTools.AddCustomAction("Nitrocid.Extras.Docking", new(() => LocalStrings.Languages, () => LocalStrings.Localizations, LocalStrings.Translate, LocalStrings.CheckCulture, LocalStrings.ListLanguagesCulture, LocalStrings.Exists));
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
        }

        void IAddon.StopAddon()
        {
            LanguageTools.RemoveCustomAction("Nitrocid.Extras.Docking");
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
        }
    }
}
