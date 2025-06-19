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
using Nitrocid.ShellPacks.Tools.Transfer;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using System;
using Textify.General;

namespace Nitrocid.ShellPacks.Shells.Mail.Commands
{
    /// <summary>
    /// Shows you the content of an encrypted mail
    /// </summary>
    /// <remarks>
    /// It shows you the content of a specified mail number, including whether or not it has attachments, sender and recipient information, time/date when the mail was sent, and the body.
    /// <br></br>
    /// This command will decrypt the mail message before displaying it, assuming that you have the public key.
    /// </remarks>
    class ReadEncCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Message number is numeric? {0}", vars: [parameters.ArgumentsList[0].IsStringNumeric()]);
            if (parameters.ArgumentsList[0].IsStringNumeric())
            {
                MailTransfer.MailPrintMessage(Convert.ToInt32(parameters.ArgumentsList[0]), true);
                return 0;
            }
            else
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_MESSAGENUMNOTNUMERIC"), true, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Mail);
            }
        }

    }
}
