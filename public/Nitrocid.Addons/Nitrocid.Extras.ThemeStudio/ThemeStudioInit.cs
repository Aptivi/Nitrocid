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

using Terminaux.Shell.Arguments;
using Nitrocid.Extras.ThemeStudio.Commands;
using Terminaux.Shell.Commands;
using System.Collections.Generic;
using Nitrocid.Kernel.Extensions;
using Terminaux.Shell.Shells;
using System.Linq;
using Terminaux.Shell.Switches;

namespace Nitrocid.Extras.ThemeStudio
{
    internal class ThemeStudioInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("mktheme", /* Localizable */ "Makes a new theme",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "themeName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Theme name to create"
                        }),
                    ],
                    [
                        new SwitchInfo("tui", /* Localizable */ "Makes a new theme in an interactive TUI")
                    ])
                ], new MkThemeCommand()),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasThemeStudio);

        void IAddon.StartAddon() =>
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);

        void IAddon.StopAddon() =>
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);

        void IAddon.FinalizeAddon()
        { }
    }
}
