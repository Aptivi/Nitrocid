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

using Nitrocid.Tests.Misc.Interactive.Interactives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;

namespace Nitrocid.Tests.Misc.Interactive
{

    [TestClass]
    public class InteractiveTests
    {

        /// <summary>
        /// Tests building the base interactive TUI
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void BuildBaseInteractiveTui()
        {
            var tui = new MyCustomInteractiveTui();
            tui.ShouldNotBeNull();
            tui.Bindings.ShouldNotBeNull();
            tui.Bindings.ShouldNotBeEmpty();
            tui.Bindings[0].ShouldNotBeNull();
            tui.Bindings[0].BindingName.ShouldBe("Test");
            tui.Bindings[0].BindingKeyName.ShouldBe(ConsoleKey.F1);
            tui.Bindings[0].BindingAction.ShouldNotBeNull();
        }

    }

}
