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
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;

namespace Nitrocid.ShellPacks.Shells.Mail.Commands
{
    /// <summary>
    /// Moves a message to a folder
    /// </summary>
    /// <remarks>
    /// This command lets you move a message to a folder. It is especially useful when organizing mail messages manually.
    /// </remarks>
    class MvCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "mv";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMAND_MV_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "mailId", new CommandArgumentPartOptions()
                    {
                        IsNumeric = true,
                        ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_MAIL_COMMAND_ARGUMENT_MAILID_DESC"
                    }),
                    new CommandArgumentPart(true, "targetFolder", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_MAIL_COMMAND_ARGUMENT_FOLDER_DESC"
                    })
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            var mailShell = (MailShell?)shell ??
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_LASTSHELLTYPEMISMATCH"));
            DebugWriter.WriteDebug(DebugLevel.I, "Message number is numeric? {0}", vars: [parameters.ArgumentsList[0].IsStringNumeric()]);
            if (parameters.ArgumentsList[0].IsStringNumeric())
            {
                mailShell.MailMoveMessage(Convert.ToInt32(parameters.ArgumentsList[0]), parameters.ArgumentsList[1]);
                return 0;
            }
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_MESSAGENUMNOTNUMERIC"), true, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Mail);
            }
        }

    }
}
