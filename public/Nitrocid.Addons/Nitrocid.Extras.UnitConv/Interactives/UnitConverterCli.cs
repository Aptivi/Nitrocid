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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnitsNet;
using FluentFTP.Helpers;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Styles.Infobox;
using Nitrocid.Languages;
using Magico.Enumeration;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Inputs.Styles.Infobox.Tools;

namespace Nitrocid.Extras.UnitConv.Interactives
{
    /// <summary>
    /// Unit converter TUI class
    /// </summary>
    public class UnitConverterCli : BaseInteractiveTui<object>, IInteractiveTui<object>
    {
        /// <inheritdoc/>
        public override bool SecondPaneInteractable =>
            true;

        /// <inheritdoc/>
        public override IEnumerable<object> PrimaryDataSource =>
            GetUnitTypeNames().OfType<object>();

        /// <inheritdoc/>
        public override IEnumerable<object> SecondaryDataSource =>
            GetUnits().OfType<object>();

        /// <inheritdoc/>
        public override string GetStatusFromItem(object item) =>
            $"{GetUnits().OfType<string>().Count()} " + Translate.DoTranslation("units to convert");

        /// <inheritdoc/>
        public override string GetEntryFromItem(object item) =>
            (string)item;

        internal void OpenConvert()
        {
            try
            {
                // Open a dialog box asking for number to convert
                string answer = InfoBoxInputColor.WriteInfoBoxInput(Translate.DoTranslation("Enter a number to convert..."), new InfoBoxSettings()
                {
                    ForegroundColor = KernelColorTools.GetColor(KernelColorType.TuiBoxForeground),
                    BackgroundColor = KernelColorTools.GetColor(KernelColorType.TuiBoxBackground),
                });
                if (string.IsNullOrEmpty(answer))
                {
                    InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("You haven't entered a number to convert."), new InfoBoxSettings()
                    {
                        ForegroundColor = KernelColorTools.GetColor(KernelColorType.TuiBoxForeground),
                        BackgroundColor = KernelColorTools.GetColor(KernelColorType.TuiBoxBackground),
                    });
                    return;
                }
                else if (!answer.IsNumeric())
                {
                    InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("The entered number is invalid."), new InfoBoxSettings()
                    {
                        ForegroundColor = KernelColorTools.GetColor(KernelColorType.TuiBoxForeground),
                        BackgroundColor = KernelColorTools.GetColor(KernelColorType.TuiBoxBackground),
                    });
                    return;
                }
                else
                {
                    var parser = UnitsNetSetup.Default.UnitParser;
                    var unitNames = GetUnitTypeNames();
                    var units = GetUnits();
                    string UnitType = (string?)unitNames.GetElementFromIndex(FirstPaneCurrentSelection - 1) ??
                        throw new Exception("The unit type is not known.");
                    int QuantityNum = Convert.ToInt32(answer);
                    string wholeUnit = units.OfType<string>().ElementAt(SecondPaneCurrentSelection - 1);
                    string SourceUnit = wholeUnit[..wholeUnit.IndexOf(' ')];
                    string TargetUnit = wholeUnit[(wholeUnit.LastIndexOf(' ') + 1)..];
                    var QuantityInfos = Quantity.Infos.Where(x => x.Name == UnitType).ToArray();
                    var TargetUnitInstance = parser.Parse(TargetUnit, QuantityInfos[0].UnitType);
                    var InitialUnit = Quantity.Parse(QuantityInfos[0].ValueType, $"{QuantityNum} {SourceUnit}");
                    var ConvertedUnit = InitialUnit.ToUnit(TargetUnitInstance);
                    InfoBoxModalColor.WriteInfoBoxModal("{0} => {1}", new InfoBoxSettings()
                    {
                        ForegroundColor = KernelColorTools.GetColor(KernelColorType.TuiBoxForeground),
                        BackgroundColor = KernelColorTools.GetColor(KernelColorType.TuiBoxBackground),
                    },
                        InitialUnit.ToString(CultureManager.CurrentCulture.NumberFormat),
                        ConvertedUnit.ToString(CultureManager.CurrentCulture.NumberFormat));
                }
            }
            catch (Exception ex)
            {
                InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("Can't convert unit.") + ex.Message, new InfoBoxSettings()
                {
                    ForegroundColor = KernelColorTools.GetColor(KernelColorType.TuiBoxForeground),
                    BackgroundColor = KernelColorTools.GetColor(KernelColorType.TuiBoxBackground),
                });
            }
        }

        internal IEnumerable GetUnitTypeNames() =>
            Quantity.Infos.Where((qi) => qi.UnitInfos.Length > 1).Select((qi) => qi.Name);

        internal IEnumerable GetUnitTypes() =>
            Quantity.Infos.Where((qi) => qi.UnitInfos.Length > 1);

        internal IEnumerable GetUnits()
        {
            var unitInfo = GetUnitTypes().Cast<QuantityInfo>().ToArray();
            var abbreviations = UnitsNetSetup.Default.UnitAbbreviations;
            for (int i = 0; i < unitInfo.Length; i++)
            {
                if (i != FirstPaneCurrentSelection - 1)
                    continue;

                QuantityInfo QuantityInfo = unitInfo[i];
                var unitValues = QuantityInfo.UnitInfos.Select(x => x.Value);
                foreach (Enum UnitValue in unitValues)
                {
                    var remainingUnitValues = unitValues.Except([UnitValue]);
                    foreach (Enum remainingUnitValue in remainingUnitValues)
                    {
                        string abbreviationSource = abbreviations.GetDefaultAbbreviation(UnitValue.GetType(), Convert.ToInt32(UnitValue));
                        string abbreviationTarget = abbreviations.GetDefaultAbbreviation(remainingUnitValue.GetType(), Convert.ToInt32(remainingUnitValue));
                        yield return $"{abbreviationSource} => {abbreviationTarget}";
                    }
                }
                break;
            }
        }
    }
}
