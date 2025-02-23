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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Languages;
using System.Collections.Generic;
using Terminaux.Writer.CyclicWriters;
using Nitrocid.ConsoleBase.Writers;

namespace Nitrocid.Kernel.Debugging.Testing.Facades
{
    internal class TestListWriterStr : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Tests the list writer with the string and string array");
        public override TestSection TestSection => TestSection.ConsoleBase;
        public override void Run()
        {
            var NormalStringList = new List<string>() { "String 1", "String 2", "String 3" };
            var ArrayStringList = new List<string[]>() { { new string[] { "String 1", "String 2", "String 3" } }, { new string[] { "String 1", "String 2", "String 3" } }, { new string[] { "String 1", "String 2", "String 3" } } };
            TextWriterColor.Write(Translate.DoTranslation("Normal string list:"));
            TextWriters.WriteList(NormalStringList);
            TextWriterColor.Write(Translate.DoTranslation("Array string list:"));
            TextWriters.WriteList(ArrayStringList);
        }
    }
}
