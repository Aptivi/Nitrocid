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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Languages;
using System.Collections.Generic;

namespace Nitrocid.Kernel.Debugging.Testing.Facades
{
    internal class TestListEntryWriter : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Tests the list entry writer");
        public override TestSection TestSection => TestSection.ConsoleBase;
        public override void Run(params string[] args)
        {
            var NormalStringListEntries = new List<string>() { "String 1", "String 2", "String 3" };
            var NormalStringListValues = new List<string>() { "Value 1", "Value 2", "Value 3" };
            TextWriterColor.Write(Translate.DoTranslation("Normal string list:"));
            for (int i = 0; i < NormalStringListEntries.Count; i++)
                ListEntryWriterColor.WriteListEntry(NormalStringListEntries[i], NormalStringListValues[i]);
            TextWriterColor.Write(Translate.DoTranslation("Indent 1:"));
            for (int i = 0; i < NormalStringListEntries.Count; i++)
                ListEntryWriterColor.WriteListEntry(NormalStringListEntries[i], NormalStringListValues[i], 1);
            TextWriterColor.Write(Translate.DoTranslation("Indent 2:"));
            for (int i = 0; i < NormalStringListEntries.Count; i++)
                ListEntryWriterColor.WriteListEntry(NormalStringListEntries[i], NormalStringListValues[i], 2);
        }
    }
}
