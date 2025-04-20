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
using Nitrocid.ConsoleBase.Writers;

namespace Nitrocid.Kernel.Debugging.Testing.Facades
{
    internal class TestDictWriterStr : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Tests the dictionary writer with the string and string array");
        public override TestSection TestSection => TestSection.ConsoleBase;
        public override void Run()
        {
            var NormalStringDict = new Dictionary<string, string>() { { "One", "String 1" }, { "Two", "String 2" }, { "Three", "String 3" } };
            var ArrayStringDict = new Dictionary<string, string[]>() { { "One", new string[] { "String 1", "String 2", "String 3" } }, { "Two", new string[] { "String 1", "String 2", "String 3" } }, { "Three", new string[] { "String 1", "String 2", "String 3" } } };
            TextWriterColor.Write(Translate.DoTranslation("Normal string dictionary:"));
            TextWriters.WriteList(NormalStringDict);
            TextWriterColor.Write(Translate.DoTranslation("Array string dictionary:"));
            TextWriters.WriteList(ArrayStringDict);
        }
    }
}
