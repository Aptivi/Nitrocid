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
using Nitrocid.Extras.Calculators.Commands;
using Terminaux.Shell.Commands;
using System.Collections.Generic;
using Nitrocid.Kernel.Extensions;
using Terminaux.Shell.Shells;
using System.Linq;

namespace Nitrocid.Extras.Calculators
{
    internal class CalculatorsInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("calc", /* Localizable */ "Calculator to calculate expressions.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "expression", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Mathematical expression"
                        }),
                    ], true)
                ], new CalcCommand()),

            new CommandInfo("imaginary", /* Localizable */ "Show information about the imaginary number formula specified by a specified real and imaginary number",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "real", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "Real number"
                        }),
                        new CommandArgumentPart(true, "imaginary", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "Imaginary number"
                        }),
                    ])
                ], new ImaginaryCommand()),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasCalculators);

        void IAddon.StartAddon() =>
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);

        void IAddon.StopAddon() =>
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);

        void IAddon.FinalizeAddon()
        { }
    }
}
