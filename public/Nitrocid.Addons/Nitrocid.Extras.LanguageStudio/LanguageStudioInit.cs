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
using Nitrocid.Extras.LanguageStudio.Commands;
using Nitrocid.Shell.ShellBase.Commands;
using System.Collections.Generic;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Shell.ShellBase.Shells;
using System.Linq;
using Nitrocid.Shell.ShellBase.Switches;

namespace Nitrocid.Extras.LanguageStudio
{
    internal class LanguageStudioInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("mklang", /* Localizable */ "Makes a new language",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "pathToTranslations", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to translations directory"
                        }),
                    ],
                    [
                        new SwitchInfo("tui", /* Localizable */ "Makes a new language in an interactive TUI")
                    ])
                ], new MkLangCommand())
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasLanguageStudio);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        void IAddon.StartAddon() =>
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);

        void IAddon.StopAddon() =>
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);

        void IAddon.FinalizeAddon()
        { }
    }
}
