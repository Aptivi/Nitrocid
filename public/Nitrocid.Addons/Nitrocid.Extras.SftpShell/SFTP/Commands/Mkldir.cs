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

using Nitrocid.Files;
using Terminaux.Shell.Commands;

namespace Nitrocid.Extras.SftpShell.SFTP.Commands
{
    /// <summary>
    /// Makes your local directory
    /// </summary>
    /// <remarks>
    /// This command lets you create your local directory in your connected SFTP server.
    /// </remarks>
    class MkldirCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string targetDir = FilesystemTools.NeutralizePath(parameters.ArgumentsList[0], SFTPShellCommon.SFTPCurrDirect);
            FilesystemTools.MakeDirectory(targetDir);
            return 0;
        }
    }
}
