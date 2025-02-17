﻿//
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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Extras.Notes.Management;
using Nitrocid.Shell.ShellBase.Commands;
using Terminaux.Writer.CyclicWriters;
using Nitrocid.ConsoleBase.Colors;

namespace Nitrocid.Extras.Notes.Commands
{
    internal class ListNotes : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var listing = new Listing()
            {
                Objects = NoteManagement.notes,
                KeyColor = KernelColorTools.GetColor(KernelColorType.ListEntry),
                ValueColor = KernelColorTools.GetColor(KernelColorType.ListValue),
            };
            TextWriterRaw.WriteRaw(listing.Render());
            return 0;
        }

        public override int ExecuteDumb(CommandParameters parameters, ref string variableValue)
        {
            for (int i = 0; i < NoteManagement.notes.Count; i++)
            {
                string note = NoteManagement.notes[i];
                TextWriterColor.Write($"[{i + 1}] {note}");
            }

            return 0;
        }

    }
}
