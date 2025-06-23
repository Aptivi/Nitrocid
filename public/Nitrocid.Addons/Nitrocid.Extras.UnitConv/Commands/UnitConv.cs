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

using System;
using System.Data;
using System.Linq;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Switches;
using UnitsNet;
using Nitrocid.Extras.UnitConv.Tools;

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

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            bool tuiMode = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-tui");
            if (tuiMode)
                UnitConvTools.OpenUnitConvTui();
            else
            {
                var parser = UnitsNetSetup.Default.UnitParser;
                string UnitType = parameters.ArgumentsList[0];
                int QuantityNum = Convert.ToInt32(parameters.ArgumentsList[1]);
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

        public override void HelpHelper()
        {
            var abbreviations = UnitsNetSetup.Default.UnitAbbreviations;
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_UNITCONV_LISTUNITS_AVAILABLETYPESUNITS"));
            foreach (QuantityInfo QuantityInfo in Quantity.Infos)
            {
                TextWriterColor.Write("- {0}:", true, ThemeColorType.ListEntry, QuantityInfo.Name);
                foreach (Enum UnitValues in QuantityInfo.UnitInfos.Select(x => x.Value))
                {
                    TextWriterColor.Write("  - {0}: ", false, ThemeColorType.ListEntry, string.Join(", ", abbreviations.GetDefaultAbbreviation(UnitValues.GetType(), Convert.ToInt32(UnitValues))));
                    TextWriterColor.Write(UnitValues.ToString(), true, ThemeColorType.ListValue);
                }
            }
        }

    }
}
