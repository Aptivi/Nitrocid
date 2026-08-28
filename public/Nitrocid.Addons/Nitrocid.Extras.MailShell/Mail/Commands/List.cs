//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using Nitrocid.Extras.MailShell.Tools.Directory;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;

namespace Nitrocid.Extras.MailShell.Mail.Commands
{
    /// <summary>
    /// Lists all messages in the current folder
    /// </summary>
    /// <remarks>
    /// It allows you to list all the messages in the current working folder in pages. It lists 10 messages in a page, so you can optionally specify the page number.
    /// </remarks>
    class ListCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "list";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMAND_LIST_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(false, "pageNum", new CommandArgumentPartOptions()
                    {
                        IsNumeric = true,
                        ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_MAIL_COMMAND_LIST_ARGUMENT_PAGENUM_DESC"
                    })
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            var mailShell = (MailShell?)shell ??
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_LASTSHELLTYPEMISMATCH"));
            if (parameters.ArgumentsList.Length > 0)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Page is numeric? {0}", vars: [parameters.ArgumentsList[0].IsStringNumeric()]);
                if (parameters.ArgumentsList[0].IsStringNumeric())
                {
                    MailManager.MailListMessages(Convert.ToInt32(parameters.ArgumentsList[0]));
                    return 0;
                }
                else
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_PAGENOTNUMERIC"), true, ThemeColorType.Error);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.Mail);
                }
            }
            else
            {
                MailManager.MailListMessages(1);
                return 0;
            }
        }

    }
}
