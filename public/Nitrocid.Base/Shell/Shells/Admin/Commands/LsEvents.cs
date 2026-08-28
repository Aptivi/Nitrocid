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

using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using Nitrocid.Base.Kernel.Events;
using Terminaux.Shell.Shells;
using Nitrocid.Base.Languages;

namespace Nitrocid.Base.Shell.Shells.Admin.Commands
{
    /// <summary>
    /// Shows the list of fired events
    /// </summary>
    /// <remarks>
    /// It shows you a detailed list of fired events with the arguments passed to each of them, if any.
    /// </remarks>
    class LsEventsCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "lsevents";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ADMIN_COMMAND_LSEVENTS_DESC");

        public override CommandFlags Flags =>
            CommandFlags.Wrappable | CommandFlags.RedirectionSupported;

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            var events = EventsManager.ListAllFiredEvents();
            ListWriterColor.WriteList(events);
            return 0;
        }

        public override int ExecuteDumb(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            var events = EventsManager.ListAllFiredEvents();
            foreach (string @event in events.Keys)
                TextWriterColor.Write(@event);
            return 0;
        }

    }
}
