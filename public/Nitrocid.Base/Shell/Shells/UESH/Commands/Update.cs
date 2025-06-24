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

using Nitrocid.Base.Kernel.Updates;
using Terminaux.Shell.Commands;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Checks for the kernel update
    /// </summary>
    /// <remarks>
    /// It checks for the kernel update by fetching the release information from GitHub, taking the latest release group, and comparing between versions. It will not download the update automatically, only checks for it.
    /// <br></br>
    /// To download the update, download it using the link provided in this command, shutdown the kernel using shutdown command, and extract it to the executable directory, overwriting the old version with the latest version.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class UpdateCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            UpdateManager.CheckKernelUpdates();
            return 0;
        }

    }
}
