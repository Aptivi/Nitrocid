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
using Nitrocid.Extras.Notes.Localized;
using Nitrocid.Extras.Notes.Management;
using Terminaux.Shell.Commands;
using System.Collections.Generic;
using Nitrocid.Kernel.Extensions;
using System.Linq;
using Nitrocid.Shell.Homepage;
using Nitrocid.Languages;

namespace Nitrocid.Extras.Notes
{
    internal class NotesInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("addnote", /* Localizable */ "NKS_NOTES_COMMAND_ADDNOTE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "noteContents...", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_NOTES_COMMAND_ADDNOTE_ARGUMENT_NOTECONTENTS_DESC"
                        })
                    ]),
                ], new AddNote()),

            new CommandInfo("removenote", /* Localizable */ "NKS_NOTES_COMMAND_REMOVENOTE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "noteNumber", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_NOTES_COMMAND_REMOVENOTE_ARGUMENT_NOTENUMBER_DESC"
                        })
                    ]),
                ], new RemoveNote()),

            new CommandInfo("removenotes", /* Localizable */ "NKS_NOTES_COMMAND_REMOVENOTES_DESC",
                [
                    new CommandArgumentInfo(),
                ], new RemoveNotes()),

            new CommandInfo("listnotes", /* Localizable */ "NKS_NOTES_COMMAND_LISTNOTES_DESC",
                [
                    new CommandArgumentInfo(),
                ], new ListNotes()),

            new CommandInfo("savenotes", /* Localizable */ "NKS_NOTES_COMMAND_SAVENOTES_DESC",
                [
                    new CommandArgumentInfo(),
                ], new SaveNotes()),

            new CommandInfo("reloadnotes", /* Localizable */ "NKS_NOTES_COMMAND_RELOADNOTES_DESC",
                [
                    new CommandArgumentInfo(),
                ], new ReloadNotes()),

            new CommandInfo("notestui", /* Localizable */ "NKS_NOTES_COMMAND_NOTESTUI_DESC",
                [
                    new CommandArgumentInfo(),
                ], new NotesTui()),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasNotes);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        void IAddon.StartAddon()
        {
            LanguageTools.AddCustomAction("Nitrocid.Extras.Notes", new(() => LocalStrings.Languages, () => LocalStrings.Localizations, LocalStrings.Translate, LocalStrings.CheckCulture, LocalStrings.ListLanguagesCulture, LocalStrings.Exists));
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
        }

        void IAddon.StopAddon()
        {
            LanguageTools.RemoveCustomAction("Nitrocid.Extras.Notes");
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
            HomepageTools.UnregisterBuiltinAction("NKS_NOTES_HOMEPAGE_NOTES");
        }

        void IAddon.FinalizeAddon()
        {
            // Add homepage entries
            HomepageTools.RegisterBuiltinAction("NKS_NOTES_HOMEPAGE_NOTES", NoteManagement.OpenNotesTui);

            // Load notes
            NoteManagement.LoadNotes();
        }
    }
}
