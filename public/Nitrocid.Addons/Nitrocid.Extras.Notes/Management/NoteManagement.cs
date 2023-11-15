//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.Files;
using KS.Files.Operations;
using KS.Files.Operations.Querying;
using KS.Kernel.Exceptions;
using KS.Languages;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Nitrocid.Extras.Notes.Management
{
    internal static class NoteManagement
    {
        internal static List<string> notes = [];

        /// <summary>
        /// Path to the notes file
        /// </summary>
        internal static string NotesPath =>
            Paths.AppDataPath + "/notes.json";

        /// <summary>
        /// Creates a new note
        /// </summary>
        /// <param name="contents">Contents of the new note.</param>
        internal static void NewNote(string contents)
        {
            // Add a note to the list
            notes.Add(contents);

            // Save the notes
            SaveNotes();
        }

        /// <summary>
        /// Removes a note
        /// </summary>
        /// <param name="noteIndex">Note index to remove (starts from zero)</param>
        /// <exception cref="KernelException"></exception>
        internal static void RemoveNote(int noteIndex)
        {
            // Check to see if the index is valid
            if (noteIndex < 0 || noteIndex >= notes.Count)
                throw new KernelException(KernelExceptionType.NoteManagement, Translate.DoTranslation("Can't remove non-existent note") + $" {noteIndex}.");
            
            // Now, remove the target note.
            notes.RemoveAt(noteIndex);
        }

        /// <summary>
        /// Removes all notes
        /// </summary>
        internal static void RemoveNotes() =>
            notes.Clear();

        /// <summary>
        /// Lists all notes
        /// </summary>
        /// <returns>An array of note contents</returns>
        internal static string[] ListNotes() =>
            notes.ToArray();

        /// <summary>
        /// Saves all notes
        /// </summary>
        internal static void SaveNotes()
        {
            // Check if the notes file exists
            if (!Checking.FileExists(NotesPath))
                Making.MakeJsonFile(NotesPath, true, true);

            // Now, serialize the array of notes to the JSON file
            string serialized = JsonConvert.SerializeObject(notes.ToArray(), Formatting.Indented);
            Writing.WriteContentsText(NotesPath, serialized);
        }

        /// <summary>
        /// Loads all notes
        /// </summary>
        internal static void LoadNotes()
        {
            // Check if the notes file exists
            if (!Checking.FileExists(NotesPath))
                SaveNotes();

            // Now, serialize the array of notes to the JSON file
            string serialized = Reading.ReadContentsText(NotesPath);
            var notesArray = JsonConvert.DeserializeObject<string[]>(serialized);
            notes = [.. notesArray];
        }
    }
}
