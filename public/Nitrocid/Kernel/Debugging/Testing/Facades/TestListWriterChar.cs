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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Languages;
using System.Collections.Generic;
using Terminaux.Writer.CyclicWriters;

namespace Nitrocid.Kernel.Debugging.Testing.Facades
{
    internal class TestListWriterChar : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Tests the list writer with the char and char array");
        public override TestSection TestSection => TestSection.ConsoleBase;
        public override void Run(params string[] args)
        {
            var NormalCharList = new List<char>() { '1', '2', '3' };
            var ArrayCharList = new List<char[]>() { { new char[] { '1', '2', '3' } }, { new char[] { '1', '2', '3' } }, { new char[] { '1', '2', '3' } } };
            TextWriterColor.Write(Translate.DoTranslation("Normal char list:"));
            var listing = new Listing()
            {
                Objects = NormalCharList,
            };
            TextWriterRaw.WriteRaw(listing.Render());
            TextWriterColor.Write(Translate.DoTranslation("Array char list:"));
            var listing2 = new Listing()
            {
                Objects = ArrayCharList,
            };
            TextWriterRaw.WriteRaw(listing2.Render());
        }
    }
}
