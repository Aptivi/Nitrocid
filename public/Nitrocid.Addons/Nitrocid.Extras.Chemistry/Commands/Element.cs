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

using ChemiStar;
using ChemiStar.Data;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters;

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
                    TextWriters.Write(Translate.DoTranslation("There is no substance with atomic number") + $" {atomicNumber}", KernelColorType.Error);
                    return 44;
                }
            }
            else
            {
                // This is either a symbol or a name
                if (!PeriodicTableParser.IsSubstanceRegistered(representation, out substance) &&
                    !PeriodicTableParser.IsSubstanceRegisteredName(representation, out substance))
                {
                    TextWriters.Write(Translate.DoTranslation("There is no substance with symbol or name") + $" {representation}", KernelColorType.Error);
                    return 44;
                }
            }

            // Print information
            WriteListEntry(Translate.DoTranslation("Name"), substance.Name);
            WriteListEntry(Translate.DoTranslation("Atomic number"), $"{substance.AtomicNumber}");
            WriteListEntry(Translate.DoTranslation("Atomic mass"), $"{substance.AtomicMass}");
            WriteListEntry(Translate.DoTranslation("Symbol"), substance.Symbol);
            WriteListEntry(Translate.DoTranslation("Summary"), substance.Summary);
            WriteListEntry(Translate.DoTranslation("Phase"), $"{substance.Phase}");
            WriteListEntry(Translate.DoTranslation("Position in the periodic table"), $"{substance.Period}, {substance.Group}");
            WriteListEntry(Translate.DoTranslation("Position in coordinates"), $"{substance.PosX} (w: {substance.WPosX}), {substance.PosY} (w: {substance.WPosY})");
            WriteListEntry(Translate.DoTranslation("Discoverer"), substance.Discoverer);
            WriteListEntry(Translate.DoTranslation("Named by"), substance.NamedBy);
            WriteListEntry(Translate.DoTranslation("Electron configuration"), substance.ElectronConfiguration);
            return 0;
        }

        private void WriteListEntry(string entry, string value)
        {
            var listEntry = new ListEntry()
            {
                Entry = entry,
                Value = value,
                KeyColor = KernelColorTools.GetColor(KernelColorType.ListEntry),
                ValueColor = KernelColorTools.GetColor(KernelColorType.ListValue),
            };
            TextWriterRaw.WritePlain(listEntry.Render());
        }
    }
}