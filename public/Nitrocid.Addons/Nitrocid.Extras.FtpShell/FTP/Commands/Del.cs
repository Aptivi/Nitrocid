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

using System;
using Nitrocid.Extras.FtpShell.Tools.Filesystem;
using Terminaux.Shell.Commands;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using Terminaux.Inputs.Styles.Choice;
using Nitrocid.ConsoleBase.Colors;
using Textify.General;

namespace Nitrocid.Extras.FtpShell.FTP.Commands
{
    /// <summary>
    /// Removes files or folders
    /// </summary>
    /// <remarks>
    /// If you have logged in to a user that has administrative privileges, you can remove unwanted files, or extra folders, from the server.
    /// <br></br>
    /// If you deleted a file while there are transmissions going on in the server, people who tries to get the deleted file will never be able to download it again after their download fails.
    /// <br></br>
    /// The authenticated user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class DelCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Print a message
            TextWriters.Write(Translate.DoTranslation("Deleting {0}..."), true, KernelColorType.Progress, parameters.ArgumentsList[0]);

            // Make a confirmation message so user will not accidentally delete a file or folder
            string answer = ChoiceStyle.PromptChoice(TextTools.FormatString(Translate.DoTranslation("Are you sure you want to delete {0}?"), parameters.ArgumentsList[0]), [("y", "Yes"), ("n", "No")]);
            if (answer != "y")
                return 1;

            try
            {
                FTPFilesystem.FTPDeleteRemote(parameters.ArgumentsList[0]);
                return 0;
            }
            catch (Exception ex)
            {
                TextWriters.Write(ex.Message, true, KernelColorType.Error);
                return ex.GetHashCode();
            }
        }

    }
}
