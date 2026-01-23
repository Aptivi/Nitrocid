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

using System;
using System.Collections.Generic;
using System.IO;
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
using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Languages;
using Nitrocid.Misc.Notifications;

namespace Nitrocid.Extras.Calendar.Calendar.Reminders
{
    /// <summary>
    /// Reminder management module
    /// </summary>
    public static class ReminderManager
    {

        /// <summary>
        /// Current reminder notification importance
        /// </summary>
        public static NotificationPriority CurrentReminderImportance { get; set; } = NotificationPriority.Low;
        internal static List<ReminderInfo> Reminders = [];
        internal static KernelThread ReminderThread = new("Reminder Thread", false, ReminderListen);
        internal static object ReminderManagerLock = new();

        /// <summary>
        /// Listens for reminders and notifies the user
        /// </summary>
        private static void ReminderListen()
        {
            while (!PowerManager.KernelShutdown)
            {
                try
                {
                    SpinWait.SpinUntil(() => PowerManager.KernelShutdown, 100);
                    lock (ReminderManagerLock)
                    {
                        for (int ReminderIndex = 0; ReminderIndex <= Reminders.Count - 1; ReminderIndex++)
                        {
                            var ReminderInstance = Reminders[ReminderIndex];
                            var CurrentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                            if (DateTime.Now >= ReminderInstance.ReminderDate)
                            {
                                DebugWriter.WriteDebug(DebugLevel.I, "Reminder is due! {0} @ {1}", vars: [ReminderInstance.ReminderTitle, TimeDateRenderers.Render(ReminderInstance.ReminderDate)]);
                                ReminderInstance.NotifyReminder();
                            }
                        }
                    }
                }
                catch (ThreadInterruptedException)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Aborting reminder listener...");
                    return;
                }
            }
        }

        /// <summary>
        /// Adds the reminder to the list (calendar will mark the day with parentheses)
        /// </summary>
        /// <param name="ReminderDate">Reminder date and time</param>
        /// <param name="ReminderTitle">Reminder title</param>
        public static void AddReminder(DateTime ReminderDate, string ReminderTitle) =>
            AddReminder(ReminderDate, ReminderTitle, CurrentReminderImportance);

        /// <summary>
        /// Adds the reminder to the list (calendar will mark the day with parentheses)
        /// </summary>
        /// <param name="ReminderDate">Reminder date and time</param>
        /// <param name="ReminderTitle">Reminder title</param>
        /// <param name="ReminderImportance">Reminder importance</param>
        public static void AddReminder(DateTime ReminderDate, string ReminderTitle, NotificationPriority ReminderImportance)
        {
            if (string.IsNullOrWhiteSpace(ReminderTitle))
                ReminderTitle = Translate.DoTranslation("Untitled reminder");
            var Reminder = new ReminderInfo(ReminderDate, ReminderTitle, ReminderImportance);
            DebugWriter.WriteDebug(DebugLevel.I, "Adding reminder {0} @ {1} to list...", vars: [Reminder.ReminderTitle, TimeDateRenderers.Render(Reminder.ReminderDate)]);
            AddReminder(Reminder);
        }

        /// <summary>
        /// Adds the reminder to the list (calendar will mark the day with parentheses)
        /// </summary>
        /// <param name="Reminder">Reminder info instance</param>
        internal static void AddReminder(ReminderInfo? Reminder)
        {
            if (Reminder is not null)
                Reminders.Add(Reminder);
        }

        /// <summary>
        /// Removes the reminder from the list
        /// </summary>
        /// <param name="ReminderDate">Reminder date and time</param>
        /// <param name="ReminderId">Reminder ID</param>
        public static void RemoveReminder(DateTime ReminderDate, int ReminderId)
        {
            int ReminderIndex = ReminderId - 1;
            if (ReminderIndex >= Reminders.Count)
                throw new KernelException(KernelExceptionType.Calendar, Translate.DoTranslation("There is no reminder."));
            var Reminder = Reminders[ReminderIndex];
            if (Reminder.ReminderDate == ReminderDate)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Removing reminder {0} @ {1} to list...", vars: [Reminder.ReminderTitle, TimeDateRenderers.Render(Reminder.ReminderDate)]);
                Reminders.Remove(Reminder);
            }
        }

        /// <summary>
        /// List all the reminders
        /// </summary>
        public static void ListReminders()
        {
            foreach (ReminderInfo Reminder in Reminders)
            {
                TextWriters.Write("- {0}: ", false, KernelColorType.ListEntry, Reminder.ReminderDate);
                TextWriters.Write(Reminder.ReminderTitle, true, KernelColorType.ListValue);
            }
        }

        /// <summary>
        /// Loads all the reminders from the KSReminders directory and adds them to the reminder list
        /// </summary>
        public static void LoadReminders()
        {
            FilesystemTools.MakeDirectory(PathsManagement.GetKernelPath(KernelPathType.Reminders), false);
            var ReminderFiles = FilesystemTools.GetFilesystemEntries(PathsManagement.GetKernelPath(KernelPathType.Reminders), "*", true);
            DebugWriter.WriteDebug(DebugLevel.I, "Got {0} reminders.", vars: [ReminderFiles.Length]);

            // Load all the reminders
            foreach (string ReminderFile in ReminderFiles)
            {
                var LoadedReminder = LoadReminder(ReminderFile);
                DebugWriter.WriteDebug(DebugLevel.I, "Loaded event is null? {0} | Loaded from file {1}", vars: [LoadedReminder is null, ReminderFile]);
                AddReminder(LoadedReminder);
            }
        }

        /// <summary>
        /// Loads an reminder file
        /// </summary>
        /// <param name="ReminderFile">Reminder file</param>
        /// <returns>A converted reminder info instance. null if unsuccessful.</returns>
        public static ReminderInfo? LoadReminder(string ReminderFile)
        {
            lock (ReminderManagerLock)
            {
                ReminderFile = FilesystemTools.NeutralizePath(ReminderFile);
                DebugWriter.WriteDebug(DebugLevel.I, "Loading reminder {0}...", vars: [ReminderFile]);

                // If file exists, convert the file to the reminder instance
                if (FilesystemTools.FileExists(ReminderFile))
                {
                    var reminderContents = FilesystemTools.ReadContentsText(ReminderFile);
                    ReminderInfo? ConvertedReminder = JsonConvert.DeserializeObject<ReminderInfo>(reminderContents);
                    DebugWriter.WriteDebug(DebugLevel.I, "Converted!");
                    return ConvertedReminder;
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "File doesn't exist!");
                }
                return null;
            }
        }

        /// <summary>
        /// Saves all the reminders from the reminder list to their individual files
        /// </summary>
        public static void SaveReminders() =>
            SaveReminders(PathsManagement.GetKernelPath(KernelPathType.Reminders), CalendarInit.CalendarConfig.SaveEventsRemindersDestructively);

        /// <summary>
        /// Saves all the reminders from the reminder list to their individual files
        /// </summary>
        public static void SaveReminders(string Path, bool Destructive)
        {
            Path = FilesystemTools.NeutralizePath(Path);
            DebugWriter.WriteDebug(DebugLevel.I, "Saving reminders to {0}...", vars: [Path]);

            // Remove all events from path, if running destructively
            if (Destructive)
            {
                string[] ReminderFiles = Directory.GetFiles(Path, "*", SearchOption.AllDirectories);
                string[] ReminderFolders = Directory.GetDirectories(Path, "*", SearchOption.AllDirectories);

                // First, remove all files
                foreach (string FilePath in ReminderFiles)
                    FilesystemTools.RemoveFile(FilePath);

                // Then, remove all empty folders
                foreach (string FolderPath in ReminderFolders)
                    FilesystemTools.RemoveDirectory(FolderPath);
            }

            // Enumerate through every reminder and save them
            for (int ReminderIndex = 0; ReminderIndex <= Reminders.Count - 1; ReminderIndex++)
            {
                var ReminderInstance = Reminders[ReminderIndex];
                string ReminderFileName = $"[{ReminderIndex}] {ReminderInstance.ReminderTitle}.json";
                DebugWriter.WriteDebug(DebugLevel.I, "Reminder file name: {0}...", vars: [ReminderFileName]);
                string ReminderFilePath = FilesystemTools.NeutralizePath(ReminderFileName, Path);
                DebugWriter.WriteDebug(DebugLevel.I, "Reminder file path: {0}...", vars: [ReminderFilePath]);
                SaveReminder(ReminderInstance, ReminderFilePath);
            }
        }

        /// <summary>
        /// Saves an reminder to a file
        /// </summary>
        public static void SaveReminder(ReminderInfo ReminderInstance) =>
            SaveReminder(ReminderInstance, PathsManagement.GetKernelPath(KernelPathType.Reminders));

        /// <summary>
        /// Saves an reminder to a file
        /// </summary>
        public static void SaveReminder(ReminderInfo ReminderInstance, string File)
        {
            File = FilesystemTools.NeutralizePath(File);
            DebugWriter.WriteDebug(DebugLevel.I, "Saving reminder to {0}...", vars: [File]);
            var contents = JsonConvert.SerializeObject(ReminderInstance, Formatting.Indented);
            FilesystemTools.WriteContentsText(File, contents);
        }

    }
}
