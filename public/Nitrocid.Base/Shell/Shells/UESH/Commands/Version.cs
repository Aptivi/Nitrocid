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
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Switches;
using Nitrocid.Base.Kernel;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// This command prints your current kernel or mod API version
    /// </summary>
    /// <remarks>
    /// This command prints your current kernel or mod API version. If invoked with -set, will also set the indicated variable to its decimal representation.
    /// </remarks>
    class VersionCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            Version? ver;

            // Get the version according to the switches provided
            if (parameters.SwitchesList.Length > 0 && SwitchManager.ContainsSwitch(parameters.SwitchesList, "-m"))
                ver = KernelReleaseInfo.ApiVersion;
            else
                ver = KernelReleaseInfo.Version;
            if (ver is null)
                return 5;

            // Now, provide the current version as a string and as a decimal
            string currentVersion = ver.ToString();
            bool result = long.TryParse($"{ver.Major:000}{ver.Minor:000}{ver.Build:000}{ver.Revision:000}", out long currentVersionDecimal);
            TextWriterColor.Write(currentVersion);
            if (!result)
                return 5;
            variableValue = $"{currentVersionDecimal}";
            return 0;
        }

    }
}
