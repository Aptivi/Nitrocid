
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Kernel.Events;
using NUnit.Framework;
using Shouldly;

namespace KSTests.KernelTests
{

    [TestFixture]
    public class KernelMiscTests
    {

        /// <summary>
        /// Tests raising an event and adding it to the fired events list
        /// </summary>
        [Test]
        [Description("Misc")]
        public void TestRaiseEvent()
        {
            EventsManager.FireEvent(EventType.StartKernel);
            EventsManager.ListAllFiredEvents().ShouldContainKey("[" + (EventsManager.ListAllFiredEvents().Count - 1).ToString() + "] StartKernel");
        }

    }
}
