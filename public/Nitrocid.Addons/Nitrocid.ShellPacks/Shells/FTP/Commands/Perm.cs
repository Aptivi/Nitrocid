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

using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.ShellPacks.Tools.Filesystem;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Commands;
using System;

namespace Nitrocid.ShellPacks.Shells.FTP.Commands
{
    /// <summary>
    /// Sets file permissions
    /// </summary>
    /// <remarks>
    /// If you have administrative access to the FTP server, you can set the remote file permissions. The permnumber argument is inherited from CHMOD's permission number.
    /// <br></br>
    /// The authenticated user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class PermCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (FTPFilesystem.FTPChangePermissions(parameters.ArgumentsList[0], Convert.ToInt32(parameters.ArgumentsList[1])))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_PERM_SETSUCCEEDED") + " {0}", true, ThemeColorType.Success, parameters.ArgumentsList[0]);
                return 0;
            }
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_PERM_SETFAILED"), true, ThemeColorType.Error, parameters.ArgumentsList[0], parameters.ArgumentsList[1]);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.FTPFilesystem);
            }
        }

    }
}
