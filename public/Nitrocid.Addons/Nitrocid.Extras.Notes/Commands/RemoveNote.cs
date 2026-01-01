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

using FluentFTP.Helpers;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Extras.Notes.Management;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using System;

namespace Nitrocid.Extras.Notes.Commands
{
    internal class RemoveNote : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (parameters.ArgumentsList[0].IsNumeric())
                NoteManagement.RemoveNote(Convert.ToInt32(parameters.ArgumentsList[0]) - 1);
            else
            {
                TextWriters.Write(Translate.DoTranslation("The note ID must be a note number"), true, KernelColorType.Error);
                return 8;
            }
            return 0;
        }

    }
}
