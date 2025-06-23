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
using Nitrocid.ShellPacks.Tools.Transfer;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using Textify.General;

namespace Nitrocid.ShellPacks.Shells.SFTP.Commands
{
    /// <summary>
    /// Uploads the file to the server
    /// </summary>
    /// <remarks>
    /// If you need to add your local files in your current working directory to the current working server directory, you must have administrative privileges to add them.
    /// <br></br>
    /// For example, if you're adding the picture of the New Delhi city using the PNG format, you need to upload it to the server for everyone to see. Assuming that it's "NewDelhi.PNG", use "put NewDelhi.PNG."
    /// <br></br>
    /// The authenticated user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class PutCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_FS_UPLOADINGFILE"), true, ThemeColorType.Progress, parameters.ArgumentsList[0]);

            // Begin the uploading process
            if (SFTPTransfer.SFTPUploadFile(parameters.ArgumentsList[0]))
            {
                TextWriterRaw.Write();
                TextWriterColor.Write(CharManager.NewLine + LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_FS_UPLOADEDFILE"), true, ThemeColorType.Success, parameters.ArgumentsList[0]);
                return 0;
            }
            else
            {
                TextWriterRaw.Write();
                TextWriterColor.Write(CharManager.NewLine + LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_PUT_FAILED"), true, ThemeColorType.Error, parameters.ArgumentsList[0]);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.SFTPFilesystem);
            }
        }

    }
}
