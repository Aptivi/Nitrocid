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
using Nitrocid.ShellPacks.Tools.Filesystem;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using Textify.General;

namespace Nitrocid.ShellPacks.Shells.FTP.Commands
{
    /// <summary>
    /// Moves a file or directory to another destination in the server
    /// </summary>
    /// <remarks>
    /// If you manage the FTP server and wanted to move a file or a directory from a remote directory to another remote directory, use this command.
    /// <br></br>
    /// The authenticated user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class MvCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_MOVING", "Nitrocid.ShellPacks"), true, KernelColorType.Progress, parameters.ArgumentsList[0], parameters.ArgumentsList[1]);
            if (FTPFilesystem.FTPMoveItem(parameters.ArgumentsList[0], parameters.ArgumentsList[1]))
            {
                TextWriters.Write(CharManager.NewLine + LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_MOVED", "Nitrocid.ShellPacks"), true, KernelColorType.Success);
                return 0;
            }
            else
            {
                TextWriters.Write(CharManager.NewLine + LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_MOVEFAILED", "Nitrocid.ShellPacks"), true, KernelColorType.Error, parameters.ArgumentsList[0], parameters.ArgumentsList[1]);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.FTPFilesystem);
            }
        }

    }
}
