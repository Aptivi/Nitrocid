
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
using KS.Files.Operations;
using KS.Files.Querying;
using KS.Misc.Writers.ConsoleWriters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace KS.Kernel.Administration.Journalling
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

                // Make a new journal entry and store everything in it
                Message = string.Format(Message, Vars);
                var JournalEntry = 
                    new JObject(
                        new JProperty("date", TimeDate.TimeDateRenderers.RenderDate()),
                        new JProperty("time", TimeDate.TimeDateRenderers.RenderTime()),
                        new JProperty("status", Status.ToString()),
                        new JProperty("message", Message)
                    );

                // Open the journal and add the new journal entry to it
                var JournalFileObject = JArray.Parse(File.ReadAllText(JournalPath));
                JournalFileObject.Add(JournalEntry);

                // Save the journal with the changes in it
                File.WriteAllText(JournalPath, JsonConvert.SerializeObject(JournalFileObject, Formatting.Indented));
            }
        }

        /// <summary>
        /// Prints the current journal log
        /// </summary>
        public static void PrintJournalLog()
        {
            // If the journal path is null, bail
            if (string.IsNullOrEmpty(JournalPath))
                return;

            // Now, parse the journal
            var JournalFileObject = JArray.Parse(File.ReadAllText(JournalPath));
            for (int i = 0; i < JournalFileObject.Count; i++)
            {
                // Populate variables
                JToken journal = JournalFileObject[i];
                string Date = (string)journal["date"];
                string Time = (string)journal["time"];
                JournalStatus Status = (JournalStatus)Enum.Parse(typeof(JournalStatus), (string)journal["status"]);
                string Message = (string)journal["message"];

                // Now, print the entries
                TextWriterColor.Write($"[{Date} {Time}] [{i + 1}] [{Status}]: ", false, KernelColorType.ListEntry);
                TextWriterColor.Write(Message, true, KernelColorType.ListEntry);
            }
        }

    }
}
