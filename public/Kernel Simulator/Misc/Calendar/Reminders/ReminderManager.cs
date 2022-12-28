
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Files.Operations;
using KS.Files.Querying;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;

namespace KS.Misc.Calendar.Reminders
{
    /// <summary>
    /// Reminder management module
    /// </summary>
    public static class ReminderManager
    {

        /// <summary>
        /// List of reminders
        /// </summary>
        public static List<ReminderInfo> Reminders = new();
        /// <summary>
        /// Current reminder notification importance
        /// </summary>
        public static Notifications.NotificationManager.NotifPriority CurrentReminderImportance = Notifications.NotificationManager.NotifPriority.Low;
        /// <summary>
        /// Reminder thread
        /// </summary>
        public static KernelThread ReminderThread = new("Reminder Thread", false, ReminderListen);
        internal static object ReminderManagerLock = new();

        /// <summary>
        /// Listens for reminders and notifies the user
        /// </summary>
        private static void ReminderListen()
        {
            while (!Flags.KernelShutdown)
            {
                try
                {
                    Thread.Sleep(100);
                    lock (ReminderManagerLock)
                    {
                        for (int ReminderIndex = 0; ReminderIndex <= Reminders.Count - 1; ReminderIndex++)
                        {
                            var ReminderInstance = Reminders[ReminderIndex];
                            var CurrentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                            if (DateTime.Now >= ReminderInstance.ReminderDate)
                            {
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
        public static void AddReminder(DateTime ReminderDate, string ReminderTitle) => AddReminder(ReminderDate, ReminderTitle, CurrentReminderImportance);

        /// <summary>
        /// Adds the reminder to the list (calendar will mark the day with parentheses)
        /// </summary>
        /// <param name="ReminderDate">Reminder date and time</param>
        /// <param name="ReminderTitle">Reminder title</param>
        /// <param name="ReminderImportance">Reminder importance</param>
        public static void AddReminder(DateTime ReminderDate, string ReminderTitle, Notifications.NotificationManager.NotifPriority ReminderImportance)
        {
            if (string.IsNullOrWhiteSpace(ReminderTitle))
                ReminderTitle = Translate.DoTranslation("Untitled reminder");
            var Reminder = new ReminderInfo()
            {
                ReminderTitle = ReminderTitle,
                ReminderImportance = ReminderImportance,
                ReminderDate = ReminderDate
            };
            AddReminder(Reminder);
        }

        /// <summary>
        /// Adds the reminder to the list (calendar will mark the day with parentheses)
        /// </summary>
        /// <param name="Reminder">Reminder info instance</param>
        internal static void AddReminder(ReminderInfo Reminder) => Reminders.Add(Reminder);

        /// <summary>
        /// Removes the reminder from the list
        /// </summary>
        /// <param name="ReminderDate">Reminder date and time</param>
        /// <param name="ReminderId">Reminder ID</param>
        public static void RemoveReminder(DateTime ReminderDate, int ReminderId)
        {
            int ReminderIndex = ReminderId - 1;
            var Reminder = Reminders[ReminderIndex];
            if (Reminder.ReminderDate == ReminderDate)
            {
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
                TextWriterColor.Write("- {0}: ", false, KernelColorType.ListEntry, Reminder.ReminderDate);
                TextWriterColor.Write(Reminder.ReminderTitle, true, KernelColorType.ListValue);
            }
        }

        /// <summary>
        /// Loads all the reminders from the KSReminders directory and adds them to the reminder list
        /// </summary>
        public static void LoadReminders()
        {
            Making.MakeDirectory(Paths.GetKernelPath(KernelPathType.Reminders), false);
            var ReminderFiles = Directory.EnumerateFileSystemEntries(Paths.GetKernelPath(KernelPathType.Reminders), "*", SearchOption.AllDirectories).ToList();
            DebugWriter.WriteDebug(DebugLevel.I, "Got {0} reminders.", ReminderFiles.Count);

            // Load all the reminders
            foreach (string ReminderFile in ReminderFiles)
            {
                var LoadedReminder = LoadReminder(ReminderFile);
                AddReminder(LoadedReminder);
            }
        }

        /// <summary>
        /// Loads an reminder file
        /// </summary>
        /// <param name="ReminderFile">Reminder file</param>
        /// <returns>A converted reminder info instance. null if unsuccessful.</returns>
        public static ReminderInfo LoadReminder(string ReminderFile)
        {
            lock (ReminderManagerLock)
            {
                Filesystem.ThrowOnInvalidPath(ReminderFile);
                ReminderFile = Filesystem.NeutralizePath(ReminderFile);
                DebugWriter.WriteDebug(DebugLevel.I, "Loading reminder {0}...", ReminderFile);

                // If file exists, convert the file to the reminder instance
                if (Checking.FileExists(ReminderFile))
                {
                    var Converter = new XmlSerializer(typeof(ReminderInfo));
                    var ReminderFileStream = new FileStream(ReminderFile, FileMode.Open);
                    DebugWriter.WriteDebug(DebugLevel.I, "Opened stream [{0}]. Converting...", ReminderFileStream.Length);
                    ReminderInfo ConvertedReminder = (ReminderInfo)Converter.Deserialize(ReminderFileStream);
                    DebugWriter.WriteDebug(DebugLevel.I, "Converted!");
                    ReminderFileStream.Close();
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
        public static void SaveReminders() => SaveReminders(Paths.GetKernelPath(KernelPathType.Reminders), Flags.SaveEventsRemindersDestructively);

        /// <summary>
        /// Saves all the reminders from the reminder list to their individual files
        /// </summary>
        public static void SaveReminders(string Path, bool Destructive)
        {
            Filesystem.ThrowOnInvalidPath(Path);
            Path = Filesystem.NeutralizePath(Path);
            DebugWriter.WriteDebug(DebugLevel.I, "Saving reminders to {0}...", Path);

            // Remove all events from path, if running destructively
            if (Destructive)
            {
                string[] ReminderFiles = (string[])Directory.EnumerateFiles(Path, "*", SearchOption.AllDirectories);
                string[] ReminderFolders = (string[])Directory.EnumerateDirectories(Path, "*", SearchOption.AllDirectories);

                // First, remove all files
                foreach (string FilePath in ReminderFiles)
                    Removing.RemoveFile(FilePath);

                // Then, remove all empty folders
                foreach (string FolderPath in ReminderFolders)
                    Removing.RemoveDirectory(FolderPath);
            }

            // Enumerate through every reminder and save them
            for (int ReminderIndex = 0; ReminderIndex <= Reminders.Count - 1; ReminderIndex++)
            {
                var ReminderInstance = Reminders[ReminderIndex];
                string ReminderFileName = $"[{ReminderIndex}] {ReminderInstance.ReminderTitle}.ksreminder";
                DebugWriter.WriteDebug(DebugLevel.I, "Reminder file name: {0}...", ReminderFileName);
                string ReminderFilePath = Filesystem.NeutralizePath(ReminderFileName, Path);
                DebugWriter.WriteDebug(DebugLevel.I, "Reminder file path: {0}...", ReminderFilePath);
                SaveReminder(ReminderInstance, ReminderFilePath);
            }
        }

        /// <summary>
        /// Saves an reminder to a file
        /// </summary>
        public static void SaveReminder(ReminderInfo ReminderInstance) => SaveReminder(ReminderInstance, Paths.GetKernelPath(KernelPathType.Reminders));

        /// <summary>
        /// Saves an reminder to a file
        /// </summary>
        public static void SaveReminder(ReminderInfo ReminderInstance, string File)
        {
            Filesystem.ThrowOnInvalidPath(File);
            File = Filesystem.NeutralizePath(File);
            DebugWriter.WriteDebug(DebugLevel.I, "Saving reminder to {0}...", File);
            var Converter = new XmlSerializer(typeof(ReminderInfo));
            var ReminderFileStream = new FileStream(File, FileMode.OpenOrCreate);
            DebugWriter.WriteDebug(DebugLevel.I, "Opened stream with length {0}", ReminderFileStream.Length);
            Converter.Serialize(ReminderFileStream, ReminderInstance);
            ReminderFileStream.Close();
        }

    }
}
