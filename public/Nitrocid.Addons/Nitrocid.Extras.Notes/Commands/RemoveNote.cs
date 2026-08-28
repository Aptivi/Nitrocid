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

using FluentFTP.Helpers;
using Nitrocid.Languages;
using Nitrocid.Extras.Notes.Management;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Extras.Notes.Commands
{
    internal class RemoveNote : BaseCommand, ICommand
    {
        public override string Command => 
            "removenote";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_NOTES_COMMAND_REMOVENOTE_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
           [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "noteNumber", new CommandArgumentPartOptions()
                    {
                        IsNumeric = true,
                        ArgumentDescription = /* Localizable */ "NKS_NOTES_COMMAND_REMOVENOTE_ARGUMENT_NOTENUMBER_DESC"
                    })
                ]),
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string noteId = parameters.ArgumentsList[0];
            if (noteId.IsNumeric())
                NoteManagement.RemoveNote(int.Parse(noteId) - 1);
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_NOTES_NOTEIDNOTNUMERIC"), true, ThemeColorType.Error);
                return 8;
            }
            return 0;
        }

    }
}
