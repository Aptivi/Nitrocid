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

using Terminaux.Shell.Arguments;
using Terminaux.Shell.Switches;
using Nitrocid.Extras.UnitConv.Commands;
using Terminaux.Shell.Commands;
using System.Collections.Generic;
using System.Linq;
using UnitsNet;
using Nitrocid.Base.Kernel.Extensions;
using Nitrocid.Extras.UnitConv.Tools;
using Nitrocid.Base.Shell.Homepage;
using Nitrocid.Core.Languages;

namespace Nitrocid.Extras.UnitConv
{
    internal class UnitConvInit : IAddon
    {
        private readonly BaseCommand[] addonCommands =
        [
            new ListUnitsCommand(),
            new UnitConvCommand(),
        ];

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasUnitConv);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasUnitConv);

        public void StartAddon()
        {
            LanguageTools.AddCustomAction(AddonName, new("Nitrocid.Extras.UnitConv.Resources.Languages.Output.Localizations", typeof(UnitConvInit).Assembly));
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
        }

        public void StopAddon()
        {
            LanguageTools.RemoveCustomAction(AddonName);
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
            HomepageTools.UnregisterBuiltinAction(/* Localizable */ "NKS_UNITCONV_HOMEPAGE_UNITCONV");
        }

        public void FinalizeAddon()
        {
            // Add homepage entries
            HomepageTools.RegisterBuiltinAction(/* Localizable */ "NKS_UNITCONV_HOMEPAGE_UNITCONV", UnitConvTools.OpenUnitConvTui);
        }
    }
}
