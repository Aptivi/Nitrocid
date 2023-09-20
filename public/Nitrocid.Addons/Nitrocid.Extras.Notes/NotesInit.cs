
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

using KS.Kernel.Extensions;
using KS.Shell.ShellBase.Arguments;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.ShellBase.Switches;
using Nitrocid.Extras.Notes.Commands;
using Nitrocid.Extras.Notes.Management;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Extras.Notes
{
    internal class NotesInit : IAddon
    {
        private readonly Dictionary<string, CommandInfo> addonCommands = new()
        {
            { "addnote",
                new CommandInfo("addnote", ShellType.Shell, /* Localizable */ "Adds a note",
                    new[]
                    {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "noteContents...")
                        }, Array.Empty<SwitchInfo>()),
                    }, new AddNote())
            },

            { "removenote",
                new CommandInfo("removenote", ShellType.Shell, /* Localizable */ "Removes a note",
                    new[]
                    {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "noteNumber")
                        }, Array.Empty<SwitchInfo>()),
                    }, new RemoveNote())
            },

            { "removenotes",
                new CommandInfo("removenotes", ShellType.Shell, /* Localizable */ "Removes all notes",
                    new[]
                    {
                        new CommandArgumentInfo(),
                    }, new RemoveNotes())
            },

            { "listnotes",
                new CommandInfo("listnotes", ShellType.Shell, /* Localizable */ "Lists all notes",
                    new[]
                    {
                        new CommandArgumentInfo(),
                    }, new ListNotes())
            },

            { "savenotes",
                new CommandInfo("savenotes", ShellType.Shell, /* Localizable */ "Saves all notes",
                    new[]
                    {
                        new CommandArgumentInfo(),
                    }, new SaveNotes())
            },

            { "reloadnotes",
                new CommandInfo("reloadnotes", ShellType.Shell, /* Localizable */ "Reloads all notes",
                    new[]
                    {
                        new CommandArgumentInfo(),
                    }, new ReloadNotes())
            },
        };

        string IAddon.AddonName => "Extras - Notes";

        AddonType IAddon.AddonType => AddonType.Optional;

        void IAddon.StartAddon() =>
            CommandManager.RegisterAddonCommands(ShellType.Shell, addonCommands.Values.ToArray());

        void IAddon.StopAddon() =>
            CommandManager.UnregisterAddonCommands(ShellType.Shell, addonCommands.Keys.ToArray());

        void IAddon.FinalizeAddon() =>
            NoteManagement.LoadNotes();
    }
}
