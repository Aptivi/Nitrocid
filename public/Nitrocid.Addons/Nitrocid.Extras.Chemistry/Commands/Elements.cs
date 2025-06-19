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
using Terminaux.Shell.Commands;
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
                TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_CHEMISTRY_ELEMENT_ATOMICNUMBER"), $"{substance.AtomicNumber}");
                TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_CHEMISTRY_ELEMENT_ATOMICMASS"), $"{substance.AtomicMass}");
                TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_CHEMISTRY_ELEMENT_SYMBOL"), substance.Symbol);
                TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_CHEMISTRY_ELEMENT_SUMMARY"), substance.Summary);
                TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_CHEMISTRY_ELEMENT_PHASE"), $"{substance.Phase}");
                TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_CHEMISTRY_ELEMENT_POSITIONPT"), $"{substance.Period}, {substance.Group}");
                TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_CHEMISTRY_ELEMENT_POSITIONCOORD"), $"{substance.PosX} (w: {substance.WPosX}), {substance.PosY} (w: {substance.WPosY})");
                TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_CHEMISTRY_ELEMENT_DISCOVERER"), substance.Discoverer);
                TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_CHEMISTRY_ELEMENT_NAMEDBY"), substance.NamedBy);
                TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_CHEMISTRY_ELEMENT_ELECTRON"), substance.ElectronConfiguration);
                if (i + 1 < substances.Length)
                    TextWriterRaw.Write();
            }
            return 0;
        }
    }
}
