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

using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Files.Paths;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
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
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_PATHFIND_FOUND") + " {0}", true, ThemeColorType.Success, filePath);
                variableValue = filePath;
                return 0;
            }
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_PATHFIND_NOTFOUND"), true, ThemeColorType.Warning);
                variableValue = "";
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Filesystem);
            }
        }
    }
}
