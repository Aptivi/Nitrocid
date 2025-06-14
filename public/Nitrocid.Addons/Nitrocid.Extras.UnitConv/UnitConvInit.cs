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

using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Shell.ShellBase.Switches;
using Nitrocid.Extras.UnitConv.Commands;
using Nitrocid.Extras.UnitConv.Localized;
using Nitrocid.Shell.ShellBase.Commands;
using System.Collections.Generic;
using System.Linq;
using UnitsNet;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Extras.UnitConv.Tools;
using Nitrocid.Shell.Homepage;
using Nitrocid.Languages;

namespace Nitrocid.Extras.UnitConv
{
    internal class UnitConvInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("listunits", LanguageTools.GetLocalized("NKS_UNITCONV_COMMAND_LISTUNITS_DESC", "Nitrocid.Extras.UnitConv"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "type", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => Quantity.Infos.Select((src) => src.Name).ToArray(),
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_UNITCONV_COMMAND_ARGUMENT_UNITTYPE_DESC", "Nitrocid.Extras.UnitConv")
                        }),
                    ])
                ], new ListUnitsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("unitconv", LanguageTools.GetLocalized("NKS_UNITCONV_COMMAND_UNITCONV_DESC", "Nitrocid.Extras.UnitConv"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "unittype", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => Quantity.Infos.Select((src) => src.Name).ToArray(),
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_UNITCONV_COMMAND_ARGUMENT_UNITTYPE_DESC", "Nitrocid.Extras.UnitConv")
                        }),
                        new CommandArgumentPart(true, "quantity", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_UNITCONV_COMMAND_UNITCONV_ARGUMENT_QUANTITY_DESC", "Nitrocid.Extras.UnitConv")
                        }),
                        new CommandArgumentPart(true, "sourceunit", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_UNITCONV_COMMAND_UNITCONV_ARGUMENT_SOURCEUNIT_DESC", "Nitrocid.Extras.UnitConv")
                        }),
                        new CommandArgumentPart(true, "targetunit", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_UNITCONV_COMMAND_UNITCONV_ARGUMENT_TARGETUNIT_DESC", "Nitrocid.Extras.UnitConv")
                        }),
                    ],
                    [
                        new SwitchInfo("tui", LanguageTools.GetLocalized("NKS_UNITCONV_COMMAND_UNITCONV_SWITCH_TUI_DESC", "Nitrocid.Extras.UnitConv"), new SwitchOptions()
                        {
                            OptionalizeLastRequiredArguments = 4,
                            AcceptsValues = false
                        })
                    ])
                ], new UnitConvCommand()),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasUnitConv);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        void IAddon.StartAddon()
        {
            LanguageTools.AddCustomAction("Nitrocid.Extras.UnitConv", new(() => LocalStrings.Languages, () => LocalStrings.Localizations, LocalStrings.Translate, LocalStrings.CheckCulture, LocalStrings.ListLanguagesCulture, LocalStrings.Exists));
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);
        }

        void IAddon.StopAddon()
        {
            LanguageTools.RemoveCustomAction("Nitrocid.Extras.UnitConv");
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);
            HomepageTools.UnregisterBuiltinAction("Unit Converter");
        }

        void IAddon.FinalizeAddon()
        {
            // Add homepage entries
            HomepageTools.RegisterBuiltinAction("Unit Converter", UnitConvTools.OpenUnitConvTui);
        }
    }
}
