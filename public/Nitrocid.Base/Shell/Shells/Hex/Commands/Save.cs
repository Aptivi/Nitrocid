//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;

namespace Nitrocid.Base.Shell.Shells.Hex.Commands
{
    /// <summary>
    /// Saves the file
    /// </summary>
    /// <remarks>
    /// This command will save any changes made to the binary file that is currently open.
    /// </remarks>
    class SaveCommand : BaseCommand, ICommand
    {

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            var hexShell = (HexShell?)shell ??
                throw new KernelException(KernelExceptionType.Archive, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_LASTSHELLTYPEMISMATCH"));
            hexShell.SaveBinaryFile();
            return 0;
        }
    }
}
