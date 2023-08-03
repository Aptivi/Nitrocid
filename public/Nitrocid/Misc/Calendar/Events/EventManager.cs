
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Files;
using KS.Files.Operations;
using KS.Files.Querying;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Kernel.Threading;
using KS.Languages;

namespace KS.Misc.Calendar.Events
{
    /// <summary>
    /// Event management module
    /// </summary>
    public static class EventManager
    {

        internal static object EventManagerLock = new();
        internal static KernelThread EventThread = new("Event Thread", false, EventListen);
        internal static List<EventInfo> CalendarEvents = new();

        /// <summary>
        /// Listens for events and notifies the user if the date is due to the event
        /// </summary>
        private static void EventListen()
        {
            while (!Flags.KernelShutdown)
            {
                try
                {
                    SpinWait.SpinUntil(() => Flags.KernelShutdown, 100);
                    lock (EventManagerLock)
                    {
                        for (int EventIndex = 0; EventIndex <= CalendarEvents.Count - 1; EventIndex++)
                        {
                            var EventInstance = CalendarEvents[EventIndex];
                            if (DateTime.Today == EventInstance.EventDate.Date)
                            {
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
                EventTitle = Translate.DoTranslation("Untitled event");
            var EventInstance = new EventInfo()
            {
                EventTitle = EventTitle,
                EventDate = EventDate
            };
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
                // TODO: Add appropriate exception type on Beta 3.
                throw new KernelException(KernelExceptionType.NoSuchEvent, Translate.DoTranslation("There is no event."));
            var EventInstance = CalendarEvents[EventIndex];
            if (EventInstance.EventDate == EventDate)
            {
                CalendarEvents.Remove(EventInstance);
            }
        }

        /// <summary>
        /// List all the events
        /// </summary>
        public static void ListEvents()
        {
            foreach (EventInfo EventInstance in CalendarEvents)
            {
                TextWriterColor.Write("- {0}: ", false, KernelColorType.ListEntry, EventInstance.EventDate);
                TextWriterColor.Write(EventInstance.EventTitle, true, KernelColorType.ListValue);
            }
        }

        /// <summary>
        /// Loads all the events from the KSEvents directory and adds them to the event list
        /// </summary>
        public static void LoadEvents()
        {
            Making.MakeDirectory(Paths.GetKernelPath(KernelPathType.Events), false);
            var EventFiles = Directory.EnumerateFileSystemEntries(Paths.GetKernelPath(KernelPathType.Events), "*", SearchOption.AllDirectories).ToList();
            DebugWriter.WriteDebug(DebugLevel.I, "Got {0} events.", EventFiles.Count);

            // Load all the events
            foreach (string EventFile in EventFiles)
            {
                var LoadedEvent = LoadEvent(EventFile);
                if (LoadedEvent is not null)
                    AddEvent(LoadedEvent);
            }
        }

        /// <summary>
        /// Loads an event file
        /// </summary>
        /// <param name="EventFile">Event file</param>
        /// <returns>A converted event info instance. null if unsuccessful.</returns>
        public static EventInfo LoadEvent(string EventFile)
        {
            lock (EventManagerLock)
            {
                Filesystem.ThrowOnInvalidPath(EventFile);
                EventFile = Filesystem.NeutralizePath(EventFile);
                DebugWriter.WriteDebug(DebugLevel.I, "Loading event {0}...", EventFile);

                // If file exists, convert the file to the event instance
                if (Checking.FileExists(EventFile))
                {
                    var Converter = new XmlSerializer(typeof(EventInfo));
                    var EventFileStream = new FileStream(EventFile, FileMode.Open);
                    DebugWriter.WriteDebug(DebugLevel.I, "Opened stream [{0}]. Converting...", EventFileStream.Length);
                    EventInfo ConvertedEvent = (EventInfo)Converter.Deserialize(EventFileStream);
                    DebugWriter.WriteDebug(DebugLevel.I, "Converted!");
                    EventFileStream.Close();
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
        public static void SaveEvents() => SaveEvents(Paths.GetKernelPath(KernelPathType.Events), Flags.SaveEventsRemindersDestructively);

        /// <summary>
        /// Saves all the events from the event list to their individual files
        /// </summary>
        public static void SaveEvents(string Path, bool Destructive)
        {
            Filesystem.ThrowOnInvalidPath(Path);
            Path = Filesystem.NeutralizePath(Path);
            DebugWriter.WriteDebug(DebugLevel.I, "Saving events to {0}...", Path);

            // Remove all events from path, if running destructively
            if (Destructive)
            {
                var EventFiles = Directory.EnumerateFiles(Path, "*", SearchOption.AllDirectories).ToArray();
                var EventFolders = Directory.EnumerateDirectories(Path, "*", SearchOption.AllDirectories).ToArray();

                // First, remove all files
                foreach (string FilePath in EventFiles)
                    Removing.RemoveFile(FilePath);

                // Then, remove all empty folders
                foreach (string FolderPath in EventFolders)
                    Removing.RemoveDirectory(FolderPath);
            }

            // Enumerate through every event and save them
            for (int EventIndex = 0; EventIndex <= CalendarEvents.Count - 1; EventIndex++)
            {
                var EventInstance = CalendarEvents[EventIndex];
                string EventFileName = $"[{EventIndex}] {EventInstance.EventTitle}.ksevent";
                DebugWriter.WriteDebug(DebugLevel.I, "Event file name: {0}...", EventFileName);
                string EventFilePath = Filesystem.NeutralizePath(EventFileName, Path);
                DebugWriter.WriteDebug(DebugLevel.I, "Event file path: {0}...", EventFilePath);
                SaveEvent(EventInstance, EventFilePath);
            }
        }

        /// <summary>
        /// Saves an event to a file
        /// </summary>
        public static void SaveEvent(EventInfo EventInstance) => SaveEvent(EventInstance, Paths.GetKernelPath(KernelPathType.Events));

        /// <summary>
        /// Saves an event to a file
        /// </summary>
        public static void SaveEvent(EventInfo EventInstance, string File)
        {
            Filesystem.ThrowOnInvalidPath(File);
            File = Filesystem.NeutralizePath(File);
            DebugWriter.WriteDebug(DebugLevel.I, "Saving event to {0}...", File);
            var Converter = new XmlSerializer(typeof(EventInfo));
            var EventFileStream = new FileStream(File, FileMode.OpenOrCreate);
            DebugWriter.WriteDebug(DebugLevel.I, "Opened stream with length {0}", EventFileStream.Length);
            Converter.Serialize(EventFileStream, EventInstance);
            EventFileStream.Close();
        }

    }
}
