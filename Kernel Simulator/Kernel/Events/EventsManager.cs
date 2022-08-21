using System.Collections.Generic;

// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

namespace KS.Kernel.Events
{
    public static class EventsManager
    {

        /// <summary>
        /// Recently fired events
        /// </summary>
        internal static Dictionary<string, object[]> FiredEvents { get; set; } = new Dictionary<string, object[]>();

        /// <summary>
        /// Lists all the fired events with arguments
        /// </summary>
        public static Dictionary<string, object[]> ListAllFiredEvents()
        {
            return ListAllFiredEvents("");
        }

        /// <summary>
        /// Lists all the fired events with arguments
        /// </summary>
        /// <param name="SearchTerm">The search term</param>
        public static Dictionary<string, object[]> ListAllFiredEvents(string SearchTerm)
        {
            var Events = new Dictionary<string, object[]>();

            // Enumerate all the fired events
            foreach (string FiredEvent in FiredEvents.Keys)
            {
                if (FiredEvent.Contains(SearchTerm))
                {
                    var EventArguments = FiredEvents[FiredEvent];
                    Events.Add(FiredEvent, EventArguments);
                }
            }
            return Events;
        }

        /// <summary>
        /// Clears all the fired events
        /// </summary>
        public static void ClearAllFiredEvents()
        {
            FiredEvents.Clear();
        }

    }
}