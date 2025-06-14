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

using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Kernel.Journaling;
using Nitrocid.Languages;
using System;
using System.Collections.Generic;

namespace Nitrocid.Kernel.Events
{
    /// <summary>
    /// Kernel event management module
    /// </summary>
    public static class EventsManager
    {

        /// <summary>
        /// Recently fired events
        /// </summary>
        internal static Dictionary<string, object?[]?> FiredEvents { get; set; } = [];
        internal static Dictionary<EventType, List<Action<object?[]?>>> eventHandlers = [];

        /// <summary>
        /// Lists all the fired events with arguments
        /// </summary>
        public static Dictionary<string, object?[]?> ListAllFiredEvents() =>
            ListAllFiredEvents("");

        /// <summary>
        /// Lists all the fired events with arguments
        /// </summary>
        /// <param name="SearchTerm">The search term</param>
        public static Dictionary<string, object?[]?> ListAllFiredEvents(string SearchTerm)
        {
            var Events = new Dictionary<string, object?[]?>();

            // Enumerate all the fired events
            DebugWriter.WriteDebugConditional(Config.MainConfig.EventDebug, DebugLevel.I, "Searching events from search term {0}...", vars: [SearchTerm]);
            foreach (string FiredEvent in FiredEvents.Keys)
            {
                if (FiredEvent.Contains(SearchTerm))
                {
                    DebugWriter.WriteDebugConditional(Config.MainConfig.EventDebug, DebugLevel.I, "Got event {0} from search term {1}...", vars: [FiredEvent, SearchTerm]);
                    var EventArguments = FiredEvents[FiredEvent];
                    Events.Add(FiredEvent, EventArguments);
                }
            }
            DebugWriter.WriteDebugConditional(Config.MainConfig.EventDebug, DebugLevel.I, "{0} events.", vars: [Events.Count]);
            return Events;
        }

        /// <summary>
        /// Clears all the fired events
        /// </summary>
        public static void ClearAllFiredEvents() =>
            FiredEvents.Clear();

        /// <summary>
        /// Fires a kernel event
        /// </summary>
        /// <param name="Event">Event</param>
        /// <param name="Params">Parameters for event</param>
        public static void FireEvent(EventType Event, params object?[]? Params)
        {
            // Check to see if event exists
            if (!Enum.IsDefined(typeof(EventType), Event))
                throw new KernelException(KernelExceptionType.NoSuchEvent, LanguageTools.GetLocalized("NKS_KERNEL_EVENTS_EXCEPTION_EVENTNOTFOUND"), Event);

            // Add fired event to the list
            DebugWriter.WriteDebugConditional(Config.MainConfig.EventDebug, DebugLevel.I, $"Raising event {Event}...");
            FiredEvents.Add($"[{FiredEvents.Count}] {Event}", Params);
            JournalManager.WriteJournal(LanguageTools.GetLocalized("NKS_KERNEL_EVENTS_LISTING") + $" {Event} [{FiredEvents.Count}]");

            // Now, respond to the event
            if (!IsEventHandled(Event))
                eventHandlers.Add(Event, []);
            foreach (var handler in eventHandlers[Event])
            {
                try
                {
                    DebugWriter.WriteDebugConditional(Config.MainConfig.EventDebug, DebugLevel.I, "A mod responded to event {0}...", vars: [Event]);
                    handler.Invoke(Params);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebugConditional(Config.MainConfig.EventDebug, DebugLevel.E, "Error in event handler: {0}", vars: [ex.Message]);
                    DebugWriter.WriteDebugStackTraceConditional(Config.MainConfig.EventDebug, ex);
                }
            }
        }

        /// <summary>
        /// Registers the event handler
        /// </summary>
        /// <param name="eventType">An event type to handle</param>
        /// <param name="eventAction">An event action to add to the handler list</param>
        public static void RegisterEventHandler(EventType eventType, Action<object?[]?> eventAction)
        {
            if (eventAction == null)
                throw new KernelException(KernelExceptionType.NoSuchEvent, LanguageTools.GetLocalized("NKS_KERNEL_EVENTS_EXCEPTION_NEEDSACTION"));
            if (!IsEventFound(eventType))
                throw new KernelException(KernelExceptionType.NoSuchEvent, LanguageTools.GetLocalized("NKS_KERNEL_EVENTS_EXCEPTION_EVENTNOTFOUND"), eventType);
            if (!IsEventHandled(eventType))
                eventHandlers.Add(eventType, []);
            eventHandlers[eventType].Add(eventAction);
        }

        /// <summary>
        /// Unregisters the event handler
        /// </summary>
        /// <param name="eventType">An event type to handle</param>
        /// <param name="eventAction">An event action to remove from the handler list</param>
        public static void UnregisterEventHandler(EventType eventType, Action<object?[]?> eventAction)
        {
            if (eventAction == null)
                throw new KernelException(KernelExceptionType.NoSuchEvent, LanguageTools.GetLocalized("NKS_KERNEL_EVENTS_EXCEPTION_NEEDSACTION"));
            if (!IsEventFound(eventType))
                throw new KernelException(KernelExceptionType.NoSuchEvent, LanguageTools.GetLocalized("NKS_KERNEL_EVENTS_EXCEPTION_EVENTNOTFOUND"), eventType);
            if (!IsEventHandled(eventType))
                eventHandlers.Add(eventType, []);
            eventHandlers[eventType].Remove(eventAction);
        }

        /// <summary>
        /// Checks to see if the event is found
        /// </summary>
        /// <param name="eventType">An event type to query</param>
        /// <returns>True if defined. Otherwise, false.</returns>
        public static bool IsEventFound(EventType eventType) =>
            Enum.IsDefined(typeof(EventType), eventType);

        /// <summary>
        /// Checks to see if the event is handled
        /// </summary>
        /// <param name="eventType">An event type to query</param>
        /// <returns>True if handled. Otherwise, false.</returns>
        public static bool IsEventHandled(EventType eventType)
        {
            if (!IsEventFound(eventType))
                return false;
            return eventHandlers.ContainsKey(eventType);
        }

    }
}
