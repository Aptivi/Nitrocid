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
using Nitrocid.ShellPacks.Tools.Directory;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;

namespace Nitrocid.ShellPacks.Shells.Mail.Commands
{
    /// <summary>
    /// Moves all messages from a recipient to a folder
    /// </summary>
    /// <remarks>
    /// It allows you to bulk move all messages from a recipient to a folder. Depending on the number of messages in the folder, it may take some time to move all messages.
    /// </remarks>
    class MvAllCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (MailManager.MailMoveAllBySender(parameters.ArgumentsList[0], parameters.ArgumentsList[1]))
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_MVALL_SUCCESS", "Nitrocid.ShellPacks"), true, KernelColorType.Success, parameters.ArgumentsList[0]);
                return 0;
            }
            else
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_MVALL_FAILURE", "Nitrocid.ShellPacks"), true, KernelColorType.Error, parameters.ArgumentsList[0]);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Mail);
            }
        }

    }
}
