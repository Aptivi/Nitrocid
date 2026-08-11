//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Terminaux.Shell.Arguments;
using Nitrocid.Extras.Notes.Commands;
using Nitrocid.Extras.Notes.Management;
using Terminaux.Shell.Commands;
using System.Collections.Generic;
using Nitrocid.Base.Kernel.Extensions;
using System.Linq;
using Nitrocid.Base.Shell.Homepage;
using Nitrocid.Core.Languages;

namespace Nitrocid.Extras.Notes
{
    internal class NotesInit : IAddon
    {
        private readonly BaseCommand[] addonCommands =
        [
            new AddNote(),
            new RemoveNote(),
            new RemoveNotes(),
            new ListNotes(),
            new SaveNotes(),
            new ReloadNotes(),
            new NotesTui(),
        ];

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasNotes);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasNotes);

        public void StartAddon()
        {
            LanguageTools.AddCustomAction(AddonName, new("Nitrocid.Extras.Notes.Resources.Languages.Output.Localizations", typeof(NotesInit).Assembly));
            CommandManager.RegisterCustomCommands("Shell", addonCommands);

            // Add homepage entries
            HomepageTools.RegisterBuiltinAction(/* Localizable */ "NKS_NOTES_HOMEPAGE_NOTES", NoteManagement.OpenNotesTui);

            // Load notes
            NoteManagement.LoadNotes();
        }

        public void StopAddon()
        {
            LanguageTools.RemoveCustomAction(AddonName);
            CommandManager.UnregisterCustomCommands("Shell", addonCommands);
            HomepageTools.UnregisterBuiltinAction(/* Localizable */ "NKS_NOTES_HOMEPAGE_NOTES");
        }
    }
}
