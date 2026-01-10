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

using ChemiStar;
using ChemiStar.Data;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Commands;

namespace Nitrocid.Extras.Chemistry.Commands
{
    class ElementCommand : BaseCommand, ICommand
    {
        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Get substance from either the name, the symbol, or from the atomic number
            string representation = parameters.ArgumentsList[0];
            SubstanceInfo substance;
            if (int.TryParse(representation, out int atomicNumber))
            {
                // This is an atomic number.
                if (!PeriodicTableParser.IsSubstanceRegistered(atomicNumber, out substance))
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_CHEMISTRY_ELEMENT_NOSUBSTANCE_ATOMICNUM") + $" {atomicNumber}", ThemeColorType.Error);
                    return 44;
                }
            }
            else
            {
                // This is either a symbol or a name
                if (!PeriodicTableParser.IsSubstanceRegistered(representation, out substance) &&
                    !PeriodicTableParser.IsSubstanceRegisteredName(representation, out substance))
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_CHEMISTRY_ELEMENT_NOSUBSTANCE_SYMORNAME") + $" {representation}", ThemeColorType.Error);
                    return 44;
                }
            }

            // Print information
            ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_CHEMISTRY_ELEMENT_NAME"), substance.Name);
            ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_CHEMISTRY_ELEMENT_ATOMICNUMBER"), $"{substance.AtomicNumber}");
            ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_CHEMISTRY_ELEMENT_ATOMICMASS"), $"{substance.AtomicMass}");
            ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_CHEMISTRY_ELEMENT_SYMBOL"), substance.Symbol);
            ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_CHEMISTRY_ELEMENT_SUMMARY"), substance.Summary);
            ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_CHEMISTRY_ELEMENT_PHASE"), $"{substance.Phase}");
            ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_CHEMISTRY_ELEMENT_POSITIONPT"), $"{substance.Period}, {substance.Group}");
            ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_CHEMISTRY_ELEMENT_POSITIONCOORD"), $"{substance.PosX} (w: {substance.WPosX}), {substance.PosY} (w: {substance.WPosY})");
            ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_CHEMISTRY_ELEMENT_DISCOVERER"), substance.Discoverer);
            ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_CHEMISTRY_ELEMENT_NAMEDBY"), substance.NamedBy);
            ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_CHEMISTRY_ELEMENT_ELECTRON"), substance.ElectronConfiguration);
            return 0;
        }
    }
}
