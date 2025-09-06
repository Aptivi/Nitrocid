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

using Terminaux.Inputs.Interactive;
using Terminaux.Shell.Commands;
using Nitrocid.Languages;
using System;
using Nitrocid.Extras.MailShell.Mail.Interactive;
using MailKit;
using MimeKit;

namespace Nitrocid.Extras.MailShell.Mail.Commands
{
    class TuiCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var tui = new MailManagerCli();
            tui.Bindings.Add(new InteractiveTuiBinding<MailFolder, MimeMessage>(Translate.DoTranslation("Open"), ConsoleKey.Enter, (entry1, _, entry2, _) => tui.Open(entry1, entry2)));
            tui.Bindings.Add(new InteractiveTuiBinding<MailFolder, MimeMessage>(Translate.DoTranslation("Create folder"), ConsoleKey.F1, (_, _, _, _) => tui.MakeFolder()));
            tui.Bindings.Add(new InteractiveTuiBinding<MailFolder, MimeMessage>(Translate.DoTranslation("Move..."), ConsoleKey.F2, (_, _, _, index) => tui.MoveMessage(index)));
            tui.Bindings.Add(new InteractiveTuiBinding<MailFolder, MimeMessage>(Translate.DoTranslation("Move all..."), ConsoleKey.F2, ConsoleModifiers.Shift, (_, _, _, index) => tui.MoveAllMessages(index)));
            tui.Bindings.Add(new InteractiveTuiBinding<MailFolder, MimeMessage>(Translate.DoTranslation("Rename folder"), ConsoleKey.F3, (entry1, _, _, _) => tui.RenameFolder(entry1)));
            tui.Bindings.Add(new InteractiveTuiBinding<MailFolder, MimeMessage>(Translate.DoTranslation("Remove folder"), ConsoleKey.F4, (entry1, _, _, _) => tui.RemoveFolder(entry1)));
            tui.Bindings.Add(new InteractiveTuiBinding<MailFolder, MimeMessage>(Translate.DoTranslation("Remove message"), ConsoleKey.F5, (_, _, _, index) => tui.RemoveMessage(index)));
            tui.Bindings.Add(new InteractiveTuiBinding<MailFolder, MimeMessage>(Translate.DoTranslation("Remove all..."), ConsoleKey.F5, ConsoleModifiers.Shift, (_, _, _, index) => tui.RemoveAllMessages(index)));
            InteractiveTuiTools.OpenInteractiveTui(tui);
            return 0;
        }
    }
}
