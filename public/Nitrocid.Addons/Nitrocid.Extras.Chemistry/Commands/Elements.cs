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

using ChemiStar;
using ChemiStar.Data;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Extras.Chemistry.Commands
{
    class ElementsCommand : BaseCommand, ICommand
    {
        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Get all substances
            var substances = PeriodicTableParser.GetSubstances();
            for (int i = 0; i < substances.Length; i++)
            {
                SubstanceInfo substance = substances[i];

                // Print information
                SeparatorWriterColor.WriteSeparatorColor(substance.Name, KernelColorTools.GetColor(KernelColorType.ListTitle), true, substance.Name);
                TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_CHEMISTRY_ELEMENT_ATOMICNUMBER", "Nitrocid.Extras.Chemistry"), $"{substance.AtomicNumber}");
                TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_CHEMISTRY_ELEMENT_ATOMICMASS", "Nitrocid.Extras.Chemistry"), $"{substance.AtomicMass}");
                TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_CHEMISTRY_ELEMENT_SYMBOL", "Nitrocid.Extras.Chemistry"), substance.Symbol);
                TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_CHEMISTRY_ELEMENT_SUMMARY", "Nitrocid.Extras.Chemistry"), substance.Summary);
                TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_CHEMISTRY_ELEMENT_PHASE", "Nitrocid.Extras.Chemistry"), $"{substance.Phase}");
                TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_CHEMISTRY_ELEMENT_POSITIONPT", "Nitrocid.Extras.Chemistry"), $"{substance.Period}, {substance.Group}");
                TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_CHEMISTRY_ELEMENT_POSITIONCOORD", "Nitrocid.Extras.Chemistry"), $"{substance.PosX} (w: {substance.WPosX}), {substance.PosY} (w: {substance.WPosY})");
                TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_CHEMISTRY_ELEMENT_DISCOVERER", "Nitrocid.Extras.Chemistry"), substance.Discoverer);
                TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_CHEMISTRY_ELEMENT_NAMEDBY", "Nitrocid.Extras.Chemistry"), substance.NamedBy);
                TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_CHEMISTRY_ELEMENT_ELECTRON", "Nitrocid.Extras.Chemistry"), substance.ElectronConfiguration);
                if (i + 1 < substances.Length)
                    TextWriterRaw.Write();
            }
            return 0;
        }
    }
}
