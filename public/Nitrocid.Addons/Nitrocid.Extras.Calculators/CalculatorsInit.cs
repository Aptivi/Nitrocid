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
using Nitrocid.Extras.Calculators.Commands;
using Terminaux.Shell.Commands;
using System.Collections.Generic;
using Nitrocid.Base.Kernel.Extensions;
using System.Linq;
using Nitrocid.Core.Languages;

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

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasCalculators);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasCalculators);

        public void StartAddon()
        {
            LanguageTools.AddCustomAction(AddonName, new("Nitrocid.Extras.Calculators.Resources.Languages.Output.Localizations", typeof(CalculatorsInit).Assembly));
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
        }

        public void StopAddon()
        {
            LanguageTools.RemoveCustomAction(AddonName);
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
        }

        public void FinalizeAddon()
        { }
    }
}
