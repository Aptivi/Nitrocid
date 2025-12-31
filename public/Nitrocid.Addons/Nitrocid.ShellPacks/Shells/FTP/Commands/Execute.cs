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

using FluentFTP;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Kernel.Exceptions;
using Terminaux.Shell.Commands;

namespace Nitrocid.ShellPacks.Shells.FTP.Commands
{
    /// <summary>
    /// Executes a server command
    /// </summary>
    /// <remarks>
    /// If you want to go advanced and execute a server command to the FTP server, you can use this command.
    /// </remarks>
    class ExecuteCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            TextWriterColor.Write("<<< C: {0}", parameters.ArgumentsText);
            var client = (FtpClient?)FTPShellCommon.ClientFTP?.ConnectionInstance;
            if (client is null)
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.FTPShell);
            var ExecutedReply = client.Execute(parameters.ArgumentsText);
            if (ExecutedReply.Success)
            {
                TextWriterColor.Write(">>> [{0}] M: {1}", true, ThemeColorType.Success, ExecutedReply.Code, ExecutedReply.Message);
                TextWriterColor.Write(">>> [{0}] I: {1}", true, ThemeColorType.Success, ExecutedReply.Code, ExecutedReply.InfoMessages);
                return 0;
            }
            else
            {
                TextWriterColor.Write(">>> [{0}] M: {1}", true, ThemeColorType.Error, ExecutedReply.Code, ExecutedReply.Message);
                TextWriterColor.Write(">>> [{0}] I: {1}", true, ThemeColorType.Error, ExecutedReply.Code, ExecutedReply.InfoMessages);
                TextWriterColor.Write(">>> [{0}] E: {1}", true, ThemeColorType.Error, ExecutedReply.Code, ExecutedReply.ErrorMessage);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.FTPShell);
            }
        }

    }
}
