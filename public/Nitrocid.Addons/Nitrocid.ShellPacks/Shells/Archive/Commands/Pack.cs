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

using Nitrocid.Files;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.ShellPacks.Tools;

namespace Nitrocid.ShellPacks.Shells.Archive.Commands
{
    /// <summary>
    /// Compresses a file to a ZIP archive
    /// </summary>
    /// <remarks>
    /// If you want to compress a single file from the ZIP archive, you can use this command.
    /// </remarks>
    class PackCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string Where = "";
            if (parameters.ArgumentsList.Length > 1)
            {
                Where = FilesystemTools.NeutralizePath(parameters.ArgumentsList[1], ArchiveShellCommon.CurrentDirectory);
            }
            ArchiveTools.PackFile(parameters.ArgumentsList[0], Where);
            return 0;
        }

    }
}
