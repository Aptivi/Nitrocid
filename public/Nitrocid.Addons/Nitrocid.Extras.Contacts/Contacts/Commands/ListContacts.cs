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

using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using System;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Kernel.Exceptions;
using VisualCard.Parts.Implementations;
using System.Text;
using VisualCard.Parts.Enums;

namespace Nitrocid.Extras.Contacts.Contacts.Commands
{
    class ListContactsCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            try
            {
                // Initiate listing process
                var contacts = ContactsManager.GetContacts();
                foreach (var contact in contacts)
                {
                    var finalNameRendered = new StringBuilder();
                    bool hasName = contact.GetPartsArray<NameInfo>().Length != 0;
                    bool hasFullName = contact.GetString(CardStringsEnum.FullName).Length != 0;

                    if (hasName || hasFullName)
                        finalNameRendered.Append(contact.GetString(CardStringsEnum.FullName)[0].Value);
                    else
                        finalNameRendered.Append(Translate.DoTranslation("No contact name"));
                    TextWriters.Write(finalNameRendered.ToString(), KernelColorType.NeutralText);
                }
                return 0;
            }
            catch (Exception ex)
            {
                TextWriters.Write(Translate.DoTranslation("Some of the contacts can't be listed.") + ex.Message, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Contacts);
            }
        }
    }
}
