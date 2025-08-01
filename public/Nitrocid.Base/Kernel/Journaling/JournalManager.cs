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

using Newtonsoft.Json;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Files;
using System;
using System.Collections.Generic;
using Textify.General;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Time;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Time.Renderers;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Files.Paths;

namespace Nitrocid.Base.Kernel.Journaling
{
    /// <summary>
    /// Kernel journalling manager
    /// </summary>
    public static class JournalManager
    {

        internal static List<JournalEntry> journalEntries = [];
        internal static string JournalPath = "";
        private static readonly object journalLock = new();

        /// <summary>
        /// Writes a message to the journal
        /// </summary>
        /// <param name="Message">Message to be written</param>
        /// <param name="Vars">Variables to format in message</param>
        public static void WriteJournal(string Message, params object[] Vars) =>
            WriteJournal(Message, JournalStatus.Info, Vars);

        /// <summary>
        /// Writes a message to the journal
        /// </summary>
        /// <param name="Message">Message to be written</param>
        /// <param name="Status">Journal status (Error, warning, ...)</param>
        /// <param name="Vars">Variables to format in message</param>
        public static void WriteJournal(string Message, JournalStatus Status, params object[] Vars)
        {
            // If the journal path is null, bail
            if (string.IsNullOrEmpty(JournalPath))
                return;

            lock (journalLock)
            {
                // If we don't have the target journal file, create it
                if (!FilesystemTools.FileExists(JournalPath))
                    SaveJournals();
                DebugWriter.WriteDebug(DebugLevel.I, "Opening journal {0}...", vars: [JournalPath]);

                // Make a new journal entry and store everything in it
                Message = TextTools.FormatString(Message, Vars);
                DebugWriter.WriteDebug(DebugLevel.I, "Journal message {0}, status {1}.", vars: [Message, Status.ToString()]);
                var JournalEntry = new JournalEntry()
                {
                    date = TimeDateRenderers.RenderDate(FormatType.Short),
                    time = TimeDateRenderers.RenderTime(FormatType.Short),
                    status = Status.ToString(),
                    message = Message,
                };

                // Add the new journal entry to it
                journalEntries.Add(JournalEntry);

                // Save the journal with the changes in it
                SaveJournals();
                DebugWriter.WriteDebug(DebugLevel.I, "Saved successfully!");
            }
        }

        /// <summary>
        /// Gets the journal entries
        /// </summary>
        /// <returns>An array of journal entries</returns>
        public static JournalEntry[] GetJournalEntries()
        {
            // If the journal path is null, bail
            if (string.IsNullOrEmpty(JournalPath))
                return [];

            // Now, return the journal entries
            return [.. journalEntries];
        }

        /// <summary>
        /// Gets the journal entries
        /// </summary>
        /// <param name="sessionNum">Session number</param>
        /// <returns>An array of journal entries</returns>
        public static JournalEntry[] GetJournalEntries(int sessionNum)
        {
            // If the journal path is null, bail
            if (string.IsNullOrEmpty(JournalPath))
                return [];
            if (sessionNum < 0)
                return [];

            // Now, formulate a valid journal path and check it for existence
            string journalPath = PathsManagement.JournalingPath.Insert(PathsManagement.JournalingPath.Length - 5, $"-{sessionNum}");
            if (!FilesystemTools.FileExists(journalPath))
                throw new KernelException(KernelExceptionType.Journaling, LanguageTools.GetLocalized("NKS_KERNEL_JOURNALING_EXCEPTION_FILENOTFOUND"), journalPath);

            // Assuming that the file exists (guarded by the above check), deserialize the contents
            string entriesValue = FilesystemTools.ReadContentsText(journalPath);
            JournalEntry[] entries = JsonConvert.DeserializeObject<JournalEntry[]>(entriesValue) ??
                throw new KernelException(KernelExceptionType.Journaling, LanguageTools.GetLocalized("NKS_KERNEL_JOURNALING_EXCEPTION_GETENTRIES"));

            // Now, return the journal entries
            return entries;
        }

        /// <summary>
        /// Prints the current journal log
        /// </summary>
        public static void PrintJournalLog()
        {
            var journals = GetJournalEntries();
            PrintJournalLog(journals);
        }

        /// <summary>
        /// Prints the current journal log
        /// </summary>
        public static void PrintJournalLog(JournalEntry[] entries)
        {
            // Parse the journal
            for (int i = 0; i < entries.Length; i++)
            {
                // Populate variables
                var journal = entries[i];
                string Date = journal.Date;
                string Time = journal.Time;
                JournalStatus Status = (JournalStatus)Enum.Parse(typeof(JournalStatus), journal.Status);
                string Message = journal.Message;

                // Now, print the entries
                var finalColor =
                    Status == JournalStatus.Error ? ThemeColorType.Error :
                    Status == JournalStatus.Warning ? ThemeColorType.Warning :
                    ThemeColorType.NeutralText;
                TextWriterColor.Write($"[{Date} {Time}] [{i + 1}] [{Status}] : ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write(Message, true, finalColor);
            }
        }

        /// <summary>
        /// Saves the journals
        /// </summary>
        public static void SaveJournals() =>
            FilesystemTools.WriteContentsText(JournalPath, JsonConvert.SerializeObject(journalEntries, Formatting.Indented));

    }
}
