
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

using KS.Files.Operations;
using KS.Files.Querying;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Runtime.Remoting;

namespace KS.Kernel.Administration.Journalling
{
    /// <summary>
    /// Kernel journalling manager
    /// </summary>
    public static class JournalManager
    {

        internal static string JournalPath = "";

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
}
