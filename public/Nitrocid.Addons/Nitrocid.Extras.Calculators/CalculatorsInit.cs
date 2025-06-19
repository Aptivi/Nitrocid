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
using Nitrocid.Extras.Calculators.Localized;
using Terminaux.Shell.Commands;
using System.Collections.Generic;
using Nitrocid.Kernel.Extensions;
using Terminaux.Shell.Shells;
using System.Linq;
using Nitrocid.Languages;

namespace Nitrocid.Extras.Calculators
{
    internal class CalculatorsInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("calc", /* Localizable */ "NKS_CALCULATORS_COMMAND_CALC_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "expression", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_CALCULATORS_EXPRESSION"
                        }),
                    ], true)
                ], new CalcCommand()),

            new CommandInfo("imaginary", /* Localizable */ "NKS_CALCULATORS_COMMAND_IMAGINARY_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "real", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_CALCULATORS_REAL"
                        }),
                        new CommandArgumentPart(true, "imaginary", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_CALCULATORS_IMAG"
                        }),
                    ])
                ], new ImaginaryCommand()),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasCalculators);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        void IAddon.StartAddon()
        {
            LanguageTools.AddCustomAction("Nitrocid.Extras.Calculators", new(() => LocalStrings.Languages, () => LocalStrings.Localizations, LocalStrings.Translate, LocalStrings.CheckCulture, LocalStrings.ListLanguagesCulture, LocalStrings.Exists));
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
        }

        void IAddon.StopAddon()
        {
            LanguageTools.RemoveCustomAction("Nitrocid.Extras.Calculators");
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
        }

        void IAddon.FinalizeAddon()
        { }
    }
}
