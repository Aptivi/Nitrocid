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
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.ShellPacks.Tools.Transfer;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;

namespace Nitrocid.ShellPacks.Shells.SFTP.Commands
{
    /// <summary>
    /// Downloads a file from the current working directory
    /// </summary>
    /// <remarks>
    /// Downloads the binary or text file and saves it to the current working local directory for you to use the downloaded file that is provided in the SFTP server.
    /// </remarks>
    class GetCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            TextWriters.Write(Translate.DoTranslation("Downloading file {0}..."), false, KernelColorType.Progress, parameters.ArgumentsList[0]);
            if (SFTPTransfer.SFTPGetFile(parameters.ArgumentsList[0]))
            {
                TextWriterRaw.Write();
                TextWriters.Write(Translate.DoTranslation("Downloaded file {0}."), true, KernelColorType.Success, parameters.ArgumentsList[0]);
                return 0;
            }
            else
            {
                TextWriterRaw.Write();
                TextWriters.Write(Translate.DoTranslation("Download failed for file {0}."), true, KernelColorType.Error, parameters.ArgumentsList[0]);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.SFTPFilesystem);
            }
        }

    }
}
