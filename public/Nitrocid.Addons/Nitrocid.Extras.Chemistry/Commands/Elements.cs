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
                TextWriters.WriteListEntry(Translate.DoTranslation("Atomic number"), $"{substance.AtomicNumber}");
                TextWriters.WriteListEntry(Translate.DoTranslation("Atomic mass"), $"{substance.AtomicMass}");
                TextWriters.WriteListEntry(Translate.DoTranslation("Symbol"), substance.Symbol);
                TextWriters.WriteListEntry(Translate.DoTranslation("Summary"), substance.Summary);
                TextWriters.WriteListEntry(Translate.DoTranslation("Phase"), $"{substance.Phase}");
                TextWriters.WriteListEntry(Translate.DoTranslation("Position in the periodic table"), $"{substance.Period}, {substance.Group}");
                TextWriters.WriteListEntry(Translate.DoTranslation("Position in coordinates"), $"{substance.PosX} (w: {substance.WPosX}), {substance.PosY} (w: {substance.WPosY})");
                TextWriters.WriteListEntry(Translate.DoTranslation("Discoverer"), substance.Discoverer);
                TextWriters.WriteListEntry(Translate.DoTranslation("Named by"), substance.NamedBy);
                TextWriters.WriteListEntry(Translate.DoTranslation("Electron configuration"), substance.ElectronConfiguration);
                if (i + 1 < substances.Length)
                    TextWriterRaw.Write();
            }
            return 0;
        }
    }
}
