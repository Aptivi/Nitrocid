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
using Terminaux.Shell.Commands;
using Nitrocid.Base.Languages;
using System;
using Nitrocid.ShellPacks.Shells.Mail.Interactive;
using MailKit;
using MimeKit;

namespace Nitrocid.ShellPacks.Shells.Mail.Commands
{
    class TuiCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var tui = new MailManagerCli();
            tui.Bindings.Add(new InteractiveTuiBinding<MailFolder, MimeMessage>(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_KEYBINDING_OPEN"), ConsoleKey.Enter, (entry1, _, entry2, _) => tui.Open(entry1, entry2)));
            tui.Bindings.Add(new InteractiveTuiBinding<MailFolder, MimeMessage>(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_TUI_KEYBINDING_CREATEFOLDER"), ConsoleKey.F1, (_, _, _, _) => tui.MakeFolder()));
            tui.Bindings.Add(new InteractiveTuiBinding<MailFolder, MimeMessage>(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_TUI_KEYBINDING_MOVE"), ConsoleKey.F2, (_, _, _, index) => tui.MoveMessage(index)));
            tui.Bindings.Add(new InteractiveTuiBinding<MailFolder, MimeMessage>(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_TUI_KEYBINDING_MOVEALL"), ConsoleKey.F2, ConsoleModifiers.Shift, (_, _, _, index) => tui.MoveAllMessages(index)));
            tui.Bindings.Add(new InteractiveTuiBinding<MailFolder, MimeMessage>(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_TUI_KEYBINDING_RENAMEFOLDER"), ConsoleKey.F3, (entry1, _, _, _) => tui.RenameFolder(entry1)));
            tui.Bindings.Add(new InteractiveTuiBinding<MailFolder, MimeMessage>(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_TUI_KEYBINDING_REMOVEFOLDER"), ConsoleKey.F4, (entry1, _, _, _) => tui.RemoveFolder(entry1)));
            tui.Bindings.Add(new InteractiveTuiBinding<MailFolder, MimeMessage>(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_TUI_KEYBINDING_REMOVE"), ConsoleKey.F5, (_, _, _, index) => tui.RemoveMessage(index)));
            tui.Bindings.Add(new InteractiveTuiBinding<MailFolder, MimeMessage>(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_TUI_KEYBINDING_REMOVEALL"), ConsoleKey.F5, ConsoleModifiers.Shift, (_, _, _, index) => tui.RemoveAllMessages(index)));
            InteractiveTuiTools.OpenInteractiveTui(tui);
            return 0;
        }
    }
}
