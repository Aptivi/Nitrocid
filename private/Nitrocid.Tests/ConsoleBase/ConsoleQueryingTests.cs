//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using Terminaux.Base;
using Textify.General;

namespace Nitrocid.Tests.ConsoleBase
{

    [TestClass]
    public class ConsoleQueryingTests
    {

        /// <summary>
        /// Tests filtering the VT sequences that matches the regex
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestFilterVTSequences()
        {
            char BellChar = Convert.ToChar(7);
            char EscapeChar = CharManager.GetEsc();
            ConsoleExtensions.FilterVTSequences($"Hello!{EscapeChar}[38;5;43m").ShouldBe("Hello!");
            ConsoleExtensions.FilterVTSequences($"{EscapeChar}]0;This is the title{BellChar}Hello!").ShouldBe("Hello!");
        }

    }
}
