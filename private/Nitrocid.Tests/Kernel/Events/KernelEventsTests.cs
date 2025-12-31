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

using Nitrocid.Base.Kernel.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Linq;

namespace Nitrocid.Tests.Kernel.Events
{

    [TestClass]
    public class KernelEventsTests
    {

        /// <summary>
        /// Tests raising an event and adding it to the fired events list
        /// </summary>
        [TestMethod]
        [Description("Misc")]
        public void TestRaiseEvent()
        {
            EventsManager.FireEvent(EventType.StartKernel);
            EventsManager.ListAllFiredEvents().ShouldContainKey("[" + (EventsManager.ListAllFiredEvents().Count - 1).ToString() + "] StartKernel");
        }

        /// <summary>
        /// Tests raising all events and adding them to the fired events list
        /// </summary>
        [TestMethod]
        [Description("Misc")]
        public void TestRaiseEvents()
        {
            var eventTypes = Enum.GetNames(typeof(EventType));
            foreach (var type in eventTypes)
            {
                var eventType = (EventType)Enum.Parse(typeof(EventType), type);
                EventsManager.FireEvent(eventType);
                EventsManager.ListAllFiredEvents().ShouldContainKey("[" + (EventsManager.ListAllFiredEvents().Count - 1).ToString() + $"] {type}");
            }
        }

        /// <summary>
        /// Tests clearing the events list
        /// </summary>
        [TestMethod]
        [Description("Misc")]
        public void TestClearEvents()
        {
            EventsManager.ClearAllFiredEvents();
            EventsManager.ListAllFiredEvents().ShouldBeEmpty();
        }

        /// <summary>
        /// Tests registering the event handler, testing it, and unregistering the handler
        /// </summary>
        [TestMethod]
        [Description("Misc")]
        public void TestEventHandler()
        {
            bool acknowledged = false;
            var acknowledge = new Action<object?[]?>((_) => acknowledged = true);
            EventsManager.RegisterEventHandler(EventType.ProcessError, acknowledge);
            EventsManager.FireEvent(EventType.ProcessError);
            EventsManager.UnregisterEventHandler(EventType.ProcessError, acknowledge);
            acknowledged.ShouldBeTrue();
        }

        /// <summary>
        /// Tests registering the event handler, testing it, and unregistering the handler
        /// </summary>
        [TestMethod]
        [Description("Misc")]
        public void TestEventHandlerParameterized()
        {
            bool acknowledged = false;
            var acknowledge = new Action<object?[]?>((parameters) =>
            {
                if (parameters is not null && parameters.Contains("Hello"))
                    acknowledged = true;
            });
            EventsManager.RegisterEventHandler(EventType.ProcessError, acknowledge);
            EventsManager.FireEvent(EventType.ProcessError, "Hello");
            EventsManager.UnregisterEventHandler(EventType.ProcessError, acknowledge);
            acknowledged.ShouldBeTrue();
        }

    }
}
