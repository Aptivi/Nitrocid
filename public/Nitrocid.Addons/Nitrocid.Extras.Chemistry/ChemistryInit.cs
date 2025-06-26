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

using Nitrocid.Extras.Chemistry.Commands;
using Nitrocid.Extras.Chemistry.Screensavers;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Misc.Screensaver;
using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Extras.Chemistry
{
    internal class ChemistryInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("element", /* Localizable */ "Prints information about a chemical substance to the console.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "name/symbol/atomicNumber", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Either a name, a symbol, or an atomic number of a substance"
                        }),
                    ])
                ], new ElementCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("elements", /* Localizable */ "Prints information about all chemical substances to the console.", new ElementsCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported)
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasChemistry);

        void IAddon.StartAddon()
        {
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);
            ScreensaverManager.AddonSavers.Add("periodicpreview", new PeriodicPreviewDisplay());
        }

        void IAddon.StopAddon()
        {
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);
            ScreensaverManager.AddonSavers.Remove("periodicpreview");
        }

        void IAddon.FinalizeAddon()
        { }
    }
}
