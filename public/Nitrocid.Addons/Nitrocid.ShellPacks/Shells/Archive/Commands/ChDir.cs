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
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.ShellPacks.Tools;

namespace Nitrocid.ShellPacks.Shells.Archive.Commands
{
    /// <summary>
    /// Changes current local directory
    /// </summary>
    /// <remarks>
    /// If you want to interact with the ZIP file in another local directory, you can use this command to change the current local directory. This change isn't applied to the main shell.
    /// </remarks>
    class ChDirCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (!ArchiveTools.ChangeWorkingArchiveLocalDirectory(parameters.ArgumentsList[0]))
            {
                TextWriters.Write(Translate.DoTranslation("Directory {0} doesn't exist"), true, KernelColorType.Error, parameters.ArgumentsList[0]);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Archive);
            }
            return 0;
        }

    }
}
