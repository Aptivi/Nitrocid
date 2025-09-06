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
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using System;
using Textify.General;

namespace Nitrocid.Extras.MailShell.Mail.Commands
{
    /// <summary>
    /// Removes a mail
    /// </summary>
    /// <remarks>
    /// If you no longer want a message in your mail account, use this command to remove a mail permanently.
    /// </remarks>
    class RmCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Message number is numeric? {0}", vars: [TextTools.IsStringNumeric(parameters.ArgumentsList[0])]);
            if (TextTools.IsStringNumeric(parameters.ArgumentsList[0]))
            {
                MailManager.MailRemoveMessage(Convert.ToInt32(parameters.ArgumentsList[0]));
                return 0;
            }
            else
            {
                TextWriters.Write(Translate.DoTranslation("Message number is not a numeric value."), true, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Mail);
            }
        }

    }
}
