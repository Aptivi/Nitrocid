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

using Terminaux.Inputs.Interactive;
using Nitrocid.Base.Misc.Interactives;
using Terminaux.Shell.Commands;
using System;
using Nitrocid.Base.Languages;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    class TaskManCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var tui = new TaskManagerCli();
            tui.Bindings.Add(new InteractiveTuiBinding<(int, object)>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_TASKMANTUI_KEYBINDING_KILL"), ConsoleKey.F1, (thread, _, _, _) => tui.KillThread(thread)));
            tui.Bindings.Add(new InteractiveTuiBinding<(int, object)>(LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_KEYBINDING_SWITCH"), ConsoleKey.F2, (_, _, _, _) => tui.SwitchMode()));
            InteractiveTuiTools.OpenInteractiveTui(tui);
            return 0;
        }
    }
}
