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

using System.Collections.Generic;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.ShellPacks.Tools.Filesystem;
using Terminaux.Shell.Commands;

namespace Nitrocid.ShellPacks.Shells.FTP.Commands
{
    /// <summary>
    /// Lists the contents of the current folder or the folder provided
    /// </summary>
    /// <remarks>
    /// You can see the list of the files and sub-directories contained in the current working directory if no directories are specified, or in the specified directory, if specified.
    /// <br></br>
    /// You can also see the list of the files and sub-directories contained in the previous directory of your current position.
    /// <br></br>
    /// Unlike lsl, you should connect to the server to use this command, because it lists directories in the server, not in the local hard drive.
    /// <br></br>
    /// <list type="table">
    /// <listheader>
    /// <term>Switches</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>-showdetails</term>
    /// <description>Shows the details of the files and folders</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// </remarks>
    class LsrCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            bool ShowFileDetails = parameters.ContainsSwitch("-showdetails") || ShellsInit.ShellsConfig.FtpShowDetailsInList;
            var Entries = new List<string>();
            if (parameters.ArgumentsList.Length != 0)
            {
                foreach (string TargetDirectory in parameters.ArgumentsList)
                    Entries = FTPFilesystem.FTPListRemote(TargetDirectory, ShowFileDetails);
            }
            else
            {
                Entries = FTPFilesystem.FTPListRemote("", ShowFileDetails);
            }
            Entries.Sort();
            foreach (string Entry in Entries)
                TextWriterColor.Write(Entry, true, ThemeColorType.ListEntry);
            return 0;
        }

    }
}
