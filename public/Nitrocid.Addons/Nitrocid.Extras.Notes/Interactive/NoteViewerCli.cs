//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using KS.ConsoleBase.Inputs.Styles.Infobox;
using KS.ConsoleBase.Interactive;
using KS.Languages;
using KS.Misc.Text;
using KS.Modifications.ManPages;
using Nitrocid.Extras.Notes.Management;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Nitrocid.Extras.Notes.Interactive
{
    /// <summary>
    /// Notes viewer class
    /// </summary>
    public class NoteViewerCli : BaseInteractiveTui, IInteractiveTui
    {
        /// <inheritdoc/>
        public override IEnumerable PrimaryDataSource =>
            NoteManagement.ListNotes();

        /// <inheritdoc/>
        public override string GetInfoFromItem(object item)
        {
            // Get some info from the note
            string noteInstance = (string)item;
            bool noteEmpty = string.IsNullOrEmpty(noteInstance);

            // Generate the rendered text
            string finalRenderedNote = noteEmpty ?
                Translate.DoTranslation("This note is empty") :
                $"{noteInstance}";

            // Render them to the second pane
            return finalRenderedNote;
        }

        /// <inheritdoc/>
        public override void RenderStatus(object item)
        {
            // Get some info from the note
            string noteInstance = (string)item;
            bool noteEmpty = string.IsNullOrEmpty(noteInstance);

            // Generate the rendered text
            string finalRenderedNote = noteEmpty ?
                Translate.DoTranslation("This note is empty") :
                $"{noteInstance}";

            // Render them to the status
            Status = finalRenderedNote;
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem(object item)
        {
            // Get some info from the note
            string noteInstance = (string)item;
            bool noteEmpty = string.IsNullOrEmpty(noteInstance);

            // Generate the rendered text
            string finalRenderedNote = noteEmpty ?
                Translate.DoTranslation("This note is empty") :
                $"{noteInstance}";

            // Render them to the second pane
            return finalRenderedNote;
        }
    }
}
