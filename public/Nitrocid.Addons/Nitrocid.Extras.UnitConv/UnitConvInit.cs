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
using Terminaux.Shell.Switches;
using Nitrocid.Extras.UnitConv.Commands;
using Nitrocid.Extras.UnitConv.Localized;
using Terminaux.Shell.Commands;
using System.Collections.Generic;
using System.Linq;
using UnitsNet;
using Nitrocid.Base.Kernel.Extensions;
using Nitrocid.Extras.UnitConv.Tools;
using Nitrocid.Base.Shell.Homepage;
using Nitrocid.Base.Languages;

namespace Nitrocid.Extras.UnitConv
{
    internal class UnitConvInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("listunits", /* Localizable */ "NKS_UNITCONV_COMMAND_LISTUNITS_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "type", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => Quantity.Infos.Select((src) => src.Name).ToArray(),
                            ArgumentDescription = /* Localizable */ "NKS_UNITCONV_COMMAND_ARGUMENT_UNITTYPE_DESC"
                        }),
                    ])
                ], new ListUnitsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("unitconv", /* Localizable */ "NKS_UNITCONV_COMMAND_UNITCONV_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "unittype", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => Quantity.Infos.Select((src) => src.Name).ToArray(),
                            ArgumentDescription = /* Localizable */ "NKS_UNITCONV_COMMAND_ARGUMENT_UNITTYPE_DESC"
                        }),
                        new CommandArgumentPart(true, "quantity", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_UNITCONV_COMMAND_UNITCONV_ARGUMENT_QUANTITY_DESC"
                        }),
                        new CommandArgumentPart(true, "sourceunit", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_UNITCONV_COMMAND_UNITCONV_ARGUMENT_SOURCEUNIT_DESC"
                        }),
                        new CommandArgumentPart(true, "targetunit", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_UNITCONV_COMMAND_UNITCONV_ARGUMENT_TARGETUNIT_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("tui", /* Localizable */ "NKS_UNITCONV_COMMAND_UNITCONV_SWITCH_TUI_DESC", new SwitchOptions()
                        {
                            OptionalizeLastRequiredArguments = 4,
                            AcceptsValues = false
                        })
                    ])
                ], new UnitConvCommand()),
        ];

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasUnitConv);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasUnitConv);

        public void StartAddon()
        {
            LanguageTools.AddCustomAction(AddonName, new(() => LocalStrings.Languages, () => LocalStrings.Localizations, LocalStrings.Translate, LocalStrings.CheckCulture, LocalStrings.ListLanguagesCulture, LocalStrings.Exists));
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
        }

        public void StopAddon()
        {
            LanguageTools.RemoveCustomAction(AddonName);
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
            HomepageTools.UnregisterBuiltinAction("NKS_UNITCONV_HOMEPAGE_UNITCONV");
        }

        public void FinalizeAddon()
        {
            // Add homepage entries
            HomepageTools.RegisterBuiltinAction("NKS_UNITCONV_HOMEPAGE_UNITCONV", UnitConvTools.OpenUnitConvTui);
        }
    }
}
