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

using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.ShellPacks.Tools.Directory;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Commands;
using System;
using Textify.General;

namespace Nitrocid.ShellPacks.Shells.Mail.Commands
{
    /// <summary>
    /// Lists all messages in the current folder
    /// </summary>
    /// <remarks>
    /// It allows you to list all the messages in the current working folder in pages. It lists 10 messages in a page, so you can optionally specify the page number.
    /// </remarks>
    class ListCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
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
