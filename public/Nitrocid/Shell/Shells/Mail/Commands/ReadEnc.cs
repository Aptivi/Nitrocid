
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Text;
using KS.Network.Mail.Transfer;
using KS.Shell.ShellBase.Commands;
using System;

namespace KS.Shell.Shells.Mail.Commands
{
    /// <summary>
    /// Shows you the content of an encrypted mail
    /// </summary>
    /// <remarks>
    /// It shows you the content of a specified mail number, including whether or not it has attachments, sender and recipient information, time/date when the mail was sent, and the body.
    /// <br></br>
    /// This command will decrypt the mail message before displaying it, assuming that you have the public key.
    /// </remarks>
    class Mail_ReadEncCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Message number is numeric? {0}", TextTools.IsStringNumeric(parameters.ArgumentsList[0]));
            if (TextTools.IsStringNumeric(parameters.ArgumentsList[0]))
            {
                MailTransfer.MailPrintMessage(Convert.ToInt32(parameters.ArgumentsList[0]), true);
                return 0;
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Message number is not a numeric value."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Mail;
            }
        }

    }
}
