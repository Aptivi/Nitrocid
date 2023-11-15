﻿//
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Shell.ShellBase.Commands;

namespace Nitrocid.Extras.ArchiveShell.Archive.Shell.Commands
{
    /// <summary>
    /// Changes current archive directory
    /// </summary>
    /// <remarks>
    /// If you want to go to a folder inside the ZIP archive, you can use this command to change the working archive directory.
    /// </remarks>
    class ChADirCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (!ArchiveTools.ChangeWorkingArchiveDirectory(parameters.ArgumentsList[0]))
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Archive directory {0} doesn't exist"), true, KernelColorType.Error, parameters.ArgumentsList[0]);
                return 10000 + (int)KernelExceptionType.Archive;
            }
            return 0;
        }

    }
}