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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Files.Paths;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Finds a given file name from path lookup directories
    /// </summary>
    /// <remarks>
    /// If you are trying to find where in the $PATH a file is found, you can use this command.
    /// </remarks>
    class PathFindCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string? filePath = "";
            if (PathLookupTools.FileExistsInPath(parameters.ArgumentsList[0], ref filePath) && filePath is not null)
            {
                TextWriters.Write(Translate.DoTranslation("File found in path:") + " {0}", true, KernelColorType.Success, filePath);
                variableValue = filePath;
                return 0;
            }
            else
            {
                TextWriters.Write(Translate.DoTranslation("File not found in path lookup directories."), true, KernelColorType.Warning);
                variableValue = "";
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Filesystem);
            }
        }
    }
}
