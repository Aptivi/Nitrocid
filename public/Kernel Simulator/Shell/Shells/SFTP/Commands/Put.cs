
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.Network.SFTP.Transfer;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.SFTP.Commands
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
    class SFTP_PutCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            TextWriterColor.Write(Translate.DoTranslation("Uploading file {0}..."), true, KernelColorType.Progress, ListArgsOnly[0]);

            // Begin the uploading process
            if (SFTPTransfer.SFTPUploadFile(ListArgsOnly[0]))
            {
                TextWriterColor.Write();
                TextWriterColor.Write(CharManager.NewLine + Translate.DoTranslation("Uploaded file {0}"), true, KernelColorType.Success, ListArgsOnly[0]);
            }
            else
            {
                TextWriterColor.Write();
                TextWriterColor.Write(CharManager.NewLine + Translate.DoTranslation("Failed to upload {0}"), true, KernelColorType.Error, ListArgsOnly[0]);
            }
        }

    }
}
