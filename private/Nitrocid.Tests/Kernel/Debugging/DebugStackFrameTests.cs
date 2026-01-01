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

using Nitrocid.Kernel.Debugging.Trace;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Nitrocid.Tests.Kernel.Debugging
{

    [TestClass]
    public class DebugStackFrameTests
    {

        /// <summary>
        /// Tests making a new instance of the debug stack frame
        /// </summary>
        [TestMethod]
        [Description("Misc")]
        public void TestDebugStackFrame()
        {
            var frame = new DebugStackFrame();
            frame.ShouldNotBeNull();
            var frame2 = new DebugStackFrame(2);
            frame2.ShouldNotBeNull();
        }

    }
}
