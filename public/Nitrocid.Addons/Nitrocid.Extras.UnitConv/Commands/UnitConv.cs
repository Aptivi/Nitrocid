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

using System;
using System.Data;
using System.Linq;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Switches;
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
                var ConvertedUnit = Quantity.Parse(QuantityInfos[0].ValueType, $"{QuantityNum} {SourceUnit}").ToUnit(TargetUnitInstance);
                TextWriters.Write("- {0} => {1}: ", false, KernelColorType.ListEntry, SourceUnit, TargetUnit);
                TextWriters.Write(ConvertedUnit.ToString(CultureManager.CurrentCult.NumberFormat), true, KernelColorType.ListValue);
            }
            return 0;
        }

        public override void HelpHelper()
        {
            var abbreviations = UnitsNetSetup.Default.UnitAbbreviations;
            TextWriterColor.Write(Translate.DoTranslation("Available unit types and their units:"));
            foreach (QuantityInfo QuantityInfo in Quantity.Infos)
            {
                TextWriters.Write("- {0}:", true, KernelColorType.ListEntry, QuantityInfo.Name);
                foreach (Enum UnitValues in QuantityInfo.UnitInfos.Select(x => x.Value))
                {
                    TextWriters.Write("  - {0}: ", false, KernelColorType.ListEntry, string.Join(", ", abbreviations.GetDefaultAbbreviation(UnitValues.GetType(), Convert.ToInt32(UnitValues))));
                    TextWriters.Write(UnitValues.ToString(), true, KernelColorType.ListValue);
                }
            }
        }

    }
}
