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

using System;
using System.Data;
using System.Linq;
using Nitrocid.Languages;
using Nitrocid.Extras.UnitConv.Tools;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using UnitsNet;

namespace Nitrocid.Extras.UnitConv.Commands
{
    /// <summary>
    /// Unit conversion command
    /// </summary>
    /// <remarks>
    /// This command allows you to convert numbers from one unit to another compatible unit, provided that you've specified the unit type, like Length, Area, and so on.
    /// <br></br>
    /// If you want to see the full list of all supported units by the UnitsNet library, check out its help command where it lists all possible units.
    /// </remarks>
    class UnitConvCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "unitconv";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_UNITCONV_COMMAND_UNITCONV_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "unittype", new CommandArgumentPartOptions()
                    {
                        AutoCompleter = (_) => [.. Quantity.Infos.Select((src) => src.Name)],
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
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            bool tuiMode = parameters.ContainsSwitch("-tui");
            if (tuiMode)
                UnitConvTools.OpenUnitConvTui();
            else
            {
                var parser = UnitsNetSetup.Default.UnitParser;
                string UnitType = parameters.ArgumentsList[0];
                int QuantityNum = int.Parse(parameters.ArgumentsList[1]);
                string SourceUnit = parameters.ArgumentsList[2];
                string TargetUnit = parameters.ArgumentsList[3];
                var QuantityInfos = Quantity.Infos.Where(x => x.Name == UnitType).ToArray();
                var TargetUnitInstance = parser.Parse(TargetUnit, QuantityInfos[0].UnitType);
                var InitialUnit = Quantity.Parse(QuantityInfos[0].ValueType, $"{QuantityNum} {SourceUnit}");
                var ConvertedUnit = InitialUnit.ToUnit(TargetUnitInstance);
                TextWriterColor.Write("- {0} => ", false, ThemeColorType.ListEntry, InitialUnit.ToString(CultureManager.CurrentCulture.NumberFormat));
                TextWriterColor.Write(ConvertedUnit.ToString(CultureManager.CurrentCulture.NumberFormat), true, ThemeColorType.ListValue);
            }
            return 0;
        }

        public override void HelpHelper(IShell? shell)
        {
            var abbreviations = UnitsNetSetup.Default.UnitAbbreviations;
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_UNITCONV_LISTUNITS_AVAILABLETYPESUNITS"));
            foreach (QuantityInfo QuantityInfo in Quantity.Infos)
            {
                SeparatorWriterColor.WriteSeparator(QuantityInfo.Name);
                foreach (Enum UnitValues in QuantityInfo.UnitInfos.Select(x => x.Value))
                {
                    string abbreviationsStr = string.Join(", ", abbreviations.GetDefaultAbbreviation(UnitValues.GetType(), (int)(object)UnitValues));
                    ListEntryWriterColor.WriteListEntry(abbreviationsStr, UnitValues.ToString());
                }
            }
        }

    }
}
