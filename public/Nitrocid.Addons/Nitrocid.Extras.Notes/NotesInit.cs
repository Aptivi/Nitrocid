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

using KS.Kernel.Extensions;
using KS.Shell.ShellBase.Arguments;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using Nitrocid.Extras.Notes.Commands;
using Nitrocid.Extras.Notes.Management;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Nitrocid.Extras.Notes
{
    internal class NotesInit : IAddon
    {
        private readonly Dictionary<string, CommandInfo> addonCommands = new()
        {
            { "addnote",
                new CommandInfo("addnote", /* Localizable */ "Adds a note",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "noteContents...")
                        }),
                    ], new AddNote())
            },

            { "removenote",
                new CommandInfo("removenote", /* Localizable */ "Removes a note",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "noteNumber", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            })
                        }),
                    ], new RemoveNote())
            },

            { "removenotes",
                new CommandInfo("removenotes", /* Localizable */ "Removes all notes",
                    [
                        new CommandArgumentInfo(),
                    ], new RemoveNotes())
            },

            { "listnotes",
                new CommandInfo("listnotes", /* Localizable */ "Lists all notes",
                    [
                        new CommandArgumentInfo(),
                    ], new ListNotes())
            },

            { "savenotes",
                new CommandInfo("savenotes", /* Localizable */ "Saves all notes",
                    [
                        new CommandArgumentInfo(),
                    ], new SaveNotes())
            },

            { "reloadnotes",
                new CommandInfo("reloadnotes", /* Localizable */ "Reloads all notes",
                    [
                        new CommandArgumentInfo(),
                    ], new ReloadNotes())
            },

            { "notestui",
                new CommandInfo("notestui", /* Localizable */ "Notes viewer TUI",
                    [
                        new CommandArgumentInfo(),
                    ], new NotesTui())
            },
        };

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasNotes);

        AddonType IAddon.AddonType => AddonType.Optional;

        ReadOnlyDictionary<string, Delegate> IAddon.PubliclyAvailableFunctions => null;

        ReadOnlyDictionary<string, PropertyInfo> IAddon.PubliclyAvailableProperties => null;

        ReadOnlyDictionary<string, FieldInfo> IAddon.PubliclyAvailableFields => null;

        void IAddon.StartAddon() =>
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands.Values]);

        void IAddon.StopAddon() =>
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Keys]);

        void IAddon.FinalizeAddon() =>
            NoteManagement.LoadNotes();
    }
}
