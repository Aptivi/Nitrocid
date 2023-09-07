
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Files.Operations;
using KS.Files.Querying;
using KS.Kernel.Debugging;
using KS.Kernel.Time.Renderers;
using KS.Misc.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Renci.SshNet.Messages;
using System;
using System.IO;

namespace KS.Kernel.Journaling
{
    /// <summary>
    /// Kernel journalling manager
    /// </summary>
    public static class JournalManager
    {

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
                if (!Checking.FileExists(JournalPath))
                    Making.MakeJsonFile(JournalPath, false, true);
                DebugWriter.WriteDebug(DebugLevel.I, "Opening journal {0}...", JournalPath);

                // Make a new journal entry and store everything in it
                Message = TextTools.FormatString(Message, Vars);
                DebugWriter.WriteDebug(DebugLevel.I, "Journal message {0}, status {1}.", Message, Status.ToString());
                var JournalEntry = new JournalEntry()
                {
                    date = TimeDateRenderers.RenderDate(),
                    time = TimeDateRenderers.RenderTime(),
                    status = Status.ToString(),
                    message = Message,
                };

                // Open the journal and add the new journal entry to it
                var JournalFileObject = JArray.Parse(File.ReadAllText(JournalPath));
                var journalObject = JObject.FromObject(JournalEntry);
                JournalFileObject.Add(journalObject);

                // Save the journal with the changes in it
                File.WriteAllText(JournalPath, JsonConvert.SerializeObject(JournalFileObject, Formatting.Indented));
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
                return Array.Empty<JournalEntry>();

            // Now, parse the journal
            var journals = JsonConvert.DeserializeObject<JournalEntry[]>(File.ReadAllText(JournalPath));
            return journals;
        }

        /// <summary>
        /// Prints the current journal log
        /// </summary>
        public static void PrintJournalLog()
        {
            // Parse the journal
            var journals = GetJournalEntries();
            for (int i = 0; i < journals.Length; i++)
            {
                // Populate variables
                var journal = journals[i];
                string Date = journal.Date;
                string Time = journal.Time;
                JournalStatus Status = (JournalStatus)Enum.Parse(typeof(JournalStatus), journal.Status);
                string Message = journal.Message;

                // Now, print the entries
                TextWriterColor.Write($"[{Date} {Time}] [{i + 1}] [{Status}]: ", false, KernelColorType.ListEntry);
                TextWriterColor.Write(Message, true, KernelColorType.ListEntry);
            }
        }

    }
}
