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
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using UnitsNet;

namespace Nitrocid.Extras.UnitConv.Commands
{
    /// <summary>
    /// Lists all units
    /// </summary>
    /// <remarks>
    /// If you don't know what units are there, you can use this command. If you don't know what unit types are there, use its help entry.
    /// </remarks>
    class ListUnitsCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var abbreviations = UnitsNetSetup.Default.UnitAbbreviations;
            var Quantities = Quantity.Infos.Where(x => x.Name == parameters.ArgumentsList[0]);
            if (Quantities.Any())
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_UNITCONV_LISTUNITS_AVAILABLETYPESUNITS"));
                foreach (QuantityInfo QuantityInfo in Quantities)
                {
                    TextWriters.Write("- {0}:", true, KernelColorType.ListEntry, QuantityInfo.Name);
                    foreach (Enum UnitValues in QuantityInfo.UnitInfos.Select(x => x.Value))
                    {
                        TextWriters.Write("  - {0}: ", false, KernelColorType.ListEntry, string.Join(", ", abbreviations.GetDefaultAbbreviation(UnitValues.GetType(), Convert.ToInt32(UnitValues))));
                        TextWriters.Write(UnitValues.ToString(), true, KernelColorType.ListValue);
                    }
                }
                return 0;
            }
            else
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_UNITCONV_LISTUNITS_NOUNITTYPE") + " {0}", true, KernelColorType.Error, parameters.ArgumentsList[0]);
                return 3;
            }
        }

        public override void HelpHelper()
        {
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_UNITCONV_LISTUNITS_AVAILABLETYPES"));
            foreach (QuantityInfo QuantityInfo in Quantity.Infos)
                TextWriters.Write("- {0}", true, KernelColorType.ListEntry, QuantityInfo.Name);
        }

    }
}
