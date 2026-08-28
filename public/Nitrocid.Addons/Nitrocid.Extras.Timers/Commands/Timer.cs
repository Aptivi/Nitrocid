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

using Nitrocid.Extras.Timers.Timers;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;

namespace Nitrocid.Extras.Timers.Commands
{
    /// <summary>
    /// The timer CLI
    /// </summary>
    /// <remarks>
    /// If you want to set the time limit, you can do so using this command.
    /// </remarks>
    class TimerCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "timer";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_DATES_COMMAND_TIMER_DESC");

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            TimerScreen.OpenTimer();
            return 0;
        }
    }
}
