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

using Terminaux.Shell.Arguments;
using Nitrocid.Extras.Notes.Commands;
using Nitrocid.Extras.Notes.Management;
using Terminaux.Shell.Commands;
using System.Collections.Generic;
using Nitrocid.Kernel.Extensions;
using Terminaux.Shell.Shells;
using System.Linq;
using Nitrocid.Shell.Homepage;

namespace Nitrocid.Extras.Notes
{
    internal class NotesInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("addnote", /* Localizable */ "Adds a note",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "noteContents...", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Note contents to add"
                        })
                    ]),
                ], new AddNote()),

            new CommandInfo("removenote", /* Localizable */ "Removes a note",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "noteNumber", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "Note number"
                        })
                    ]),
                ], new RemoveNote()),

            new CommandInfo("removenotes", /* Localizable */ "Removes all notes",
                [
                    new CommandArgumentInfo(),
                ], new RemoveNotes()),

            new CommandInfo("listnotes", /* Localizable */ "Lists all notes",
                [
                    new CommandArgumentInfo(),
                ], new ListNotes()),

            new CommandInfo("savenotes", /* Localizable */ "Saves all notes",
                [
                    new CommandArgumentInfo(),
                ], new SaveNotes()),

            new CommandInfo("reloadnotes", /* Localizable */ "Reloads all notes",
                [
                    new CommandArgumentInfo(),
                ], new ReloadNotes()),

            new CommandInfo("notestui", /* Localizable */ "Notes viewer TUI",
                [
                    new CommandArgumentInfo(),
                ], new NotesTui()),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasNotes);

        void IAddon.StartAddon() =>
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);

        void IAddon.StopAddon()
        {
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
            HomepageTools.UnregisterBuiltinAction("Notes");
        }

        void IAddon.FinalizeAddon()
        {
            // Add homepage entries
            HomepageTools.RegisterBuiltinAction(/* Localizable */ "Notes", NoteManagement.OpenNotesTui);

            // Load notes
            NoteManagement.LoadNotes();
        }
    }
}
