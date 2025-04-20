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

using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using System;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Kernel.Exceptions;

namespace Nitrocid.Extras.Contacts.Contacts.Commands
{
    class LoadContactsCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            try
            {
                // Initiate import process
                ContactsManager.ImportContacts();
                return 0;
            }
            catch (Exception ex)
            {
                TextWriters.Write(Translate.DoTranslation("Some of the contacts can't be imported.") + ex.Message, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Contacts);
            }
        }
    }
}
