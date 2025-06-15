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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Files;
using Nitrocid.Files.Paths;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Kernel.Power;
using Nitrocid.Kernel.Threading;
using Nitrocid.Kernel.Time;
using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Languages;

namespace Nitrocid.Extras.Calendar.Calendar.Events
{
    /// <summary>
    /// Event management module
    /// </summary>
    public static class EventManager
    {

        internal static object EventManagerLock = new();
        internal static KernelThread EventThread = new("Event Thread", false, EventListen);
        internal static List<EventInfo> CalendarEvents = [];
        internal static readonly EventInfo[] baseEvents =
        [
            new(new(2018, 2, 22), LanguageTools.GetLocalized("NKS_CALENDAR_BUILTIN_NKSANNIVERSARY")
                , true, 2, 22, 2, 22, "Gregorian"),
            new(new(2018, 2, 22), LanguageTools.GetLocalized("NKS_CALENDAR_BUILTIN_RAMADAN")
                , true, 9, 1, 10, 1, "Hijri"),
        ];

        /// <summary>
        /// Listens for events and notifies the user if the date is due to the event
        /// </summary>
        private static void EventListen()
        {
            while (!PowerManager.KernelShutdown)
            {
                try
                {
                    SpinWait.SpinUntil(() => PowerManager.KernelShutdown, 100);
                    lock (EventManagerLock)
                    {
                        for (int EventIndex = 0; EventIndex <= CalendarEvents.Count - 1; EventIndex++)
                        {
                            var EventInstance = CalendarEvents[EventIndex];
                            if ((EventInstance.IsYearly && TimeDateTools.KernelDateTime >= EventInstance.Start && TimeDateTools.KernelDateTime <= EventInstance.End) ||
                                (!EventInstance.IsYearly && TimeDateTools.KernelDateTime == EventInstance.EventDate))
                            {
                                DebugWriter.WriteDebug(DebugLevel.I, "Event is due! {0} @ {1}", vars: [EventInstance.EventTitle, TimeDateRenderers.Render(EventInstance.EventDate)]);
                                EventInstance.NotifyEvent();
                            }
                        }
                    }
                }
                catch (ThreadInterruptedException)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Aborting event listener...");
                    return;
                }
            }
        }

        /// <summary>
        /// Adds the event to the list (calendar will mark the day with color)
        /// </summary>
        /// <param name="EventDate">Event date and time</param>
        /// <param name="EventTitle">Event title</param>
        public static void AddEvent(DateTime EventDate, string EventTitle)
        {
            if (string.IsNullOrWhiteSpace(EventTitle))
                EventTitle = LanguageTools.GetLocalized("NKS_CALENDAR_UNTITLEDEVENT");
            var EventInstance = new EventInfo(EventDate, EventTitle);
            DebugWriter.WriteDebug(DebugLevel.I, "Adding event {0} @ {1} to list...", vars: [EventInstance.EventTitle, TimeDateRenderers.Render(EventInstance.EventDate)]);
            AddEvent(EventInstance);
        }

        /// <summary>
        /// Adds the event to the list (calendar will mark the day with color)
        /// </summary>
        /// <param name="EventInstance">Event info instance</param>
        internal static void AddEvent(EventInfo EventInstance) =>
            CalendarEvents.Add(EventInstance);

        /// <summary>
        /// Removes the event from the list
        /// </summary>
        /// <param name="EventDate">Event date and time</param>
        /// <param name="EventId">Event ID</param>
        public static void RemoveEvent(DateTime EventDate, int EventId)
        {
            int EventIndex = EventId - 1;
            if (EventIndex >= CalendarEvents.Count)
                throw new KernelException(KernelExceptionType.Calendar, LanguageTools.GetLocalized("NKS_CALENDAR_EXCEPTION_NOEVENT"));
            var EventInstance = CalendarEvents[EventIndex];
            if (EventInstance.EventDate == EventDate)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Removing event {0} @ {1} from list...", vars: [EventInstance.EventTitle, TimeDateRenderers.Render(EventInstance.EventDate)]);
                CalendarEvents.Remove(EventInstance);
            }
        }

        /// <summary>
        /// List all the events
        /// </summary>
        public static void ListEvents()
        {
            foreach (EventInfo EventInstance in CalendarEvents.Union(baseEvents))
            {
                TextWriters.Write("- {0}: ", false, KernelColorType.ListEntry, EventInstance.EventDate);
                TextWriters.Write($"{EventInstance.EventTitle}{(EventInstance.IsYearly ? $" [{EventInstance.StartMonth}/{EventInstance.StartDay} -> {EventInstance.EndMonth}/{EventInstance.EndDay}]" : "")}", true, KernelColorType.ListValue);
            }
        }

        /// <summary>
        /// Loads all the events from the KSEvents directory and adds them to the event list
        /// </summary>
        public static void LoadEvents()
        {
            FilesystemTools.MakeDirectory(PathsManagement.GetKernelPath(KernelPathType.Events), false);
            var EventFiles = FilesystemTools.GetFilesystemEntries(PathsManagement.GetKernelPath(KernelPathType.Events), "*", true);
            DebugWriter.WriteDebug(DebugLevel.I, "Got {0} events.", vars: [EventFiles.Length]);

            // Load all the events
            foreach (string EventFile in EventFiles)
            {
                var LoadedEvent = LoadEvent(EventFile);
                DebugWriter.WriteDebug(DebugLevel.I, "Loaded event is null? {0} | Loaded from file {1}", vars: [LoadedEvent is null, EventFile]);
                if (LoadedEvent is not null)
                    AddEvent(LoadedEvent);
            }
        }

        /// <summary>
        /// Loads an event file
        /// </summary>
        /// <param name="EventFile">Event file</param>
        /// <returns>A converted event info instance. null if unsuccessful.</returns>
        public static EventInfo? LoadEvent(string EventFile)
        {
            lock (EventManagerLock)
            {
                EventFile = FilesystemTools.NeutralizePath(EventFile);
                DebugWriter.WriteDebug(DebugLevel.I, "Loading event {0}...", vars: [EventFile]);

                // If file exists, convert the file to the event instance
                if (FilesystemTools.FileExists(EventFile))
                {
                    var eventContents = FilesystemTools.ReadContentsText(EventFile);
                    EventInfo? ConvertedEvent = JsonConvert.DeserializeObject<EventInfo>(eventContents);
                    DebugWriter.WriteDebug(DebugLevel.I, "Converted!");
                    return ConvertedEvent;
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "File doesn't exist!");
                }
                return null;
            }
        }

        /// <summary>
        /// Saves all the events from the event list to their individual files
        /// </summary>
        public static void SaveEvents() =>
            SaveEvents(PathsManagement.GetKernelPath(KernelPathType.Events), CalendarInit.CalendarConfig.SaveEventsRemindersDestructively);

        /// <summary>
        /// Saves all the events from the event list to their individual files
        /// </summary>
        public static void SaveEvents(string Path, bool Destructive)
        {
            Path = FilesystemTools.NeutralizePath(Path);
            DebugWriter.WriteDebug(DebugLevel.I, "Saving events to {0}...", vars: [Path]);

            // Remove all events from path, if running destructively
            if (Destructive)
            {
                var EventFiles = Directory.GetFiles(Path, "*", SearchOption.AllDirectories);
                var EventFolders = Directory.GetDirectories(Path, "*", SearchOption.AllDirectories);

                // First, remove all files
                foreach (string FilePath in EventFiles)
                    FilesystemTools.RemoveFile(FilePath);

                // Then, remove all empty folders
                foreach (string FolderPath in EventFolders)
                    FilesystemTools.RemoveDirectory(FolderPath);
            }

            // Enumerate through every event and save them
            for (int EventIndex = 0; EventIndex <= CalendarEvents.Count - 1; EventIndex++)
            {
                var EventInstance = CalendarEvents[EventIndex];
                string EventFileName = $"{EventIndex,0000}-{EventInstance.EventTitle}.json";
                DebugWriter.WriteDebug(DebugLevel.I, "Event file name: {0}...", vars: [EventFileName]);
                string EventFilePath = FilesystemTools.NeutralizePath(EventFileName, Path);
                DebugWriter.WriteDebug(DebugLevel.I, "Event file path: {0}...", vars: [EventFilePath]);
                SaveEvent(EventInstance, EventFilePath);
            }
        }

        /// <summary>
        /// Saves an event to a file
        /// </summary>
        public static void SaveEvent(EventInfo EventInstance) =>
            SaveEvent(EventInstance, PathsManagement.GetKernelPath(KernelPathType.Events));

        /// <summary>
        /// Saves an event to a file
        /// </summary>
        public static void SaveEvent(EventInfo EventInstance, string File)
        {
            File = FilesystemTools.NeutralizePath(File);
            DebugWriter.WriteDebug(DebugLevel.I, "Saving event to {0}...", vars: [File]);
            var contents = JsonConvert.SerializeObject(EventInstance, Formatting.Indented);
            FilesystemTools.WriteContentsText(File, contents);
        }

    }
}
