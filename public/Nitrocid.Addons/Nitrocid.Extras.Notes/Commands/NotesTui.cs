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

using Terminaux.Inputs.Interactive;
using Nitrocid.Extras.Notes.Interactive;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Languages;
using System;

namespace Nitrocid.Extras.Notes.Commands
{
    internal class NotesTui : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var tui = new NoteViewerCli();
            tui.Bindings.Add(new InteractiveTuiBinding<string>(Translate.DoTranslation("Add"), ConsoleKey.F1, (_, _, _, _) => tui.Add(), true));
            tui.Bindings.Add(new InteractiveTuiBinding<string>(Translate.DoTranslation("Edit"), ConsoleKey.F2, (_, noteIdx, _, _) => tui.Edit(noteIdx)));
            tui.Bindings.Add(new InteractiveTuiBinding<string>(Translate.DoTranslation("Remove"), ConsoleKey.F3, (_, noteIdx, _, _) => tui.Remove(noteIdx)));
            tui.Bindings.Add(new InteractiveTuiBinding<string>(Translate.DoTranslation("Remove All"), ConsoleKey.F4, (_, _, _, _) => tui.RemoveAll()));
            tui.Bindings.Add(new InteractiveTuiBinding<string>(Translate.DoTranslation("Load"), ConsoleKey.F5, (_, _, _, _) => tui.Load()));
            tui.Bindings.Add(new InteractiveTuiBinding<string>(Translate.DoTranslation("Save"), ConsoleKey.F6, (_, _, _, _) => tui.Save()));
            InteractiveTuiTools.OpenInteractiveTui(tui);
            return 0;
        }

    }
}
