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
using Nitrocid.Extras.FtpShell.Tools.Filesystem;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using Textify.General;

namespace Nitrocid.Extras.FtpShell.FTP.Commands
{
    /// <summary>
    /// Copies a file or directory to another destination in the server
    /// </summary>
    /// <remarks>
    /// If you manage the FTP server and wanted to copy a file or a directory from a remote directory to another remote directory, use this command. It might take a few minutes depending on the server, because it downloads the file to a temporary directory and uploads the file to another destination.
    /// <br></br>
    /// This is because FluentFTP doesn't support .CopyFile(Source, Destination) yet.
    /// <br></br>
    /// The authenticated user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class CpCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            TextWriterColor.Write(Translate.DoTranslation("Copying {0} to {1}..."), parameters.ArgumentsList[0], parameters.ArgumentsList[1]);
            if (FTPFilesystem.FTPCopyItem(parameters.ArgumentsList[0], parameters.ArgumentsList[1]))
            {
                TextWriters.Write(CharManager.NewLine + Translate.DoTranslation("Copied successfully"), true, KernelColorType.Success);
                return 0;
            }
            else
            {
                TextWriters.Write(CharManager.NewLine + Translate.DoTranslation("Failed to copy {0} to {1}."), true, KernelColorType.Error, parameters.ArgumentsList[0], parameters.ArgumentsList[1]);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Filesystem);
            }
        }

    }
}
