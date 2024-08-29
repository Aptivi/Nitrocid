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
using Nitrocid.Extras.Contacts.Contacts.Interactives;
using Nitrocid.Shell.ShellBase.Commands;
using VisualCard.Parts;
using System;
using Nitrocid.Languages;

namespace Nitrocid.Extras.Contacts.Contacts.Commands
{
    class ContactsCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var tui = new ContactsManagerCli();
            tui.Bindings.Add(new InteractiveTuiBinding<Card>(Translate.DoTranslation("Delete"), ConsoleKey.F1, (_, index, _, _) => tui.RemoveContact(index)));
            tui.Bindings.Add(new InteractiveTuiBinding<Card>(Translate.DoTranslation("Delete All"), ConsoleKey.F2, (_, _, _, _) => tui.RemoveContacts()));
            tui.Bindings.Add(new InteractiveTuiBinding<Card>(Translate.DoTranslation("Import"), ConsoleKey.F3, (_, _, _, _) => tui.ImportContacts(), true));
            tui.Bindings.Add(new InteractiveTuiBinding<Card>(Translate.DoTranslation("Import From"), ConsoleKey.F4, (_, _, _, _) => tui.ImportContactsFrom(), true));
            tui.Bindings.Add(new InteractiveTuiBinding<Card>(Translate.DoTranslation("Info"), ConsoleKey.F5, (_, index, _, _) => tui.ShowContactInfo(index)));
            tui.Bindings.Add(new InteractiveTuiBinding<Card>(Translate.DoTranslation("Search"), ConsoleKey.F6, (_, _, _, _) => tui.SearchBox()));
            tui.Bindings.Add(new InteractiveTuiBinding<Card>(Translate.DoTranslation("Search Next"), ConsoleKey.F7, (_, _, _, _) => tui.SearchNext()));
            tui.Bindings.Add(new InteractiveTuiBinding<Card>(Translate.DoTranslation("Search Back"), ConsoleKey.F8, (_, _, _, _) => tui.SearchPrevious()));
            tui.Bindings.Add(new InteractiveTuiBinding<Card>(Translate.DoTranslation("Raw Info"), ConsoleKey.F9, (_, index, _, _) => tui.ShowContactRawInfo(index)));
            tui.Bindings.Add(new InteractiveTuiBinding<Card>(Translate.DoTranslation("Import From MeCard"), ConsoleKey.F10, (_, _, _, _) => tui.ImportContactFromMeCard(), true));
            InteractiveTuiTools.OpenInteractiveTui(tui);
            return 0;
        }
    }
}
