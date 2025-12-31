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

using Terminaux.Inputs.Interactive;
using Nitrocid.Extras.Notes.Management;
using Nitrocid.Base.Languages;
using System.Collections.Generic;
using Textify.General;
using System.Linq;
using Terminaux.Inputs.Styles.Editor;

namespace Nitrocid.Extras.Notes.Interactive
{
    /// <summary>
    /// Notes viewer class
    /// </summary>
    public class NoteViewerCli : BaseInteractiveTui<string>, IInteractiveTui<string>
    {
        /// <inheritdoc/>
        public override InteractiveTuiHelpPage[] HelpPages =>
        [
            new()
            {
                HelpTitle = /* Localizable */ "NKS_NOTES_TUI_HELP01_TITLE",
                HelpDescription = /* Localizable */ "NKS_NOTES_TUI_HELP01_DESC",
                HelpBody =
                    LanguageTools.GetLocalized("NKS_NOTES_TUI_HELP01_BODY") + "\n\n" +
#pragma warning disable NLOC0001
                    LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_COMMON_HELP_MOREINFO") + ": https://aptivi.gitbook.io/aptivi/nitrocid-ks-manual/fundamentals/simulated-kernel-features/extra-features/common-programs/notes",
#pragma warning restore NLOC0001
            }
        ];

        /// <inheritdoc/>
        public override IEnumerable<string> PrimaryDataSource =>
            NoteManagement.ListNotes();

        /// <inheritdoc/>
        public override bool AcceptsEmptyData =>
            true;

        /// <inheritdoc/>
        public override string GetInfoFromItem(string item)
        {
            // Get some info from the note
            string noteInstance = item;
            bool noteEmpty = string.IsNullOrEmpty(noteInstance);

            // Generate the rendered text
            string finalRenderedNote = noteEmpty ?
                LanguageTools.GetLocalized("NKS_NOTES_TUI_EMPTYNOTE") :
                $"{noteInstance}";

            // Render them to the second pane
            return finalRenderedNote;
        }

        /// <inheritdoc/>
        public override string GetStatusFromItem(string item)
        {
            // Get some info from the note
            string noteInstance = item;
            bool noteEmpty = string.IsNullOrEmpty(noteInstance);

            // Generate the rendered text
            string finalRenderedNote = noteEmpty ?
                LanguageTools.GetLocalized("NKS_NOTES_TUI_EMPTYNOTE") :
                $"{noteInstance.SplitNewLines()[0]}";

            // Render them to the status
            return finalRenderedNote;
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem(string item)
        {
            // Get some info from the note
            string noteInstance = item;
            bool noteEmpty = string.IsNullOrEmpty(noteInstance);

            // Generate the rendered text
            string finalRenderedNote = noteEmpty ?
                LanguageTools.GetLocalized("NKS_NOTES_TUI_EMPTYNOTE") :
                $"{noteInstance.SplitNewLines()[0]}";

            // Render them to the second pane
            return finalRenderedNote;
        }

        internal void Add()
        {
            List<string> lines = [];
            TextEditInteractive.OpenInteractive(ref lines);
            NoteManagement.NewNote(string.Join('\n', lines));
            NoteManagement.SaveNotes();
        }

        internal void Edit(int noteIdx)
        {
            string note = NoteManagement.notes[noteIdx];
            var lines = note.SplitNewLines().ToList();
            TextEditInteractive.OpenInteractive(ref lines);
            NoteManagement.notes[noteIdx] = string.Join('\n', lines);
            NoteManagement.SaveNotes();
        }

        internal void Remove(int noteIdx)
        {
            NoteManagement.RemoveNote(noteIdx);
            NoteManagement.SaveNotes();
        }

        internal void RemoveAll()
        {
            NoteManagement.RemoveNotes();
            NoteManagement.SaveNotes();
        }

        internal void Load() =>
            NoteManagement.LoadNotes();

        internal void Save() =>
            NoteManagement.SaveNotes();
    }
}
