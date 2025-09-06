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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Extras.MailShell.Tools.Directory;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;

namespace Nitrocid.Extras.MailShell.Mail.Commands
{
    /// <summary>
    /// Removes all mail from a specified recipient
    /// </summary>
    /// <remarks>
    /// If you no longer want all messages from a recipient in your mail account, use this command, assuming you know the full name of the recipient.
    /// </remarks>
    class RmAllCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (MailManager.MailRemoveAllBySender(parameters.ArgumentsList[0]))
            {
                TextWriters.Write(Translate.DoTranslation("All mail made by {0} are removed successfully."), true, KernelColorType.Success, parameters.ArgumentsList[0]);
                return 0;
            }
            else
            {
                TextWriters.Write(Translate.DoTranslation("Failed to remove all mail made by {0}."), true, KernelColorType.Error, parameters.ArgumentsList[0]);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Mail);
            }
        }

    }
}
