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

using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using Terminaux.Themes.Colors;
using Nitrocid.Base.Kernel;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Shells;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Returns true or false depending on the kernel mode
    /// </summary>
    class IsModeCommand : BaseCommand, ICommand
    {

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            bool safeModeCheck = parameters.ContainsSwitch("-s");
            bool debugModeCheck = parameters.ContainsSwitch("-d");
            bool maintenanceModeCheck = parameters.ContainsSwitch("-m");
            bool verbose = parameters.ContainsSwitch("-v");
            bool result = false;

            if (!safeModeCheck && !debugModeCheck && !maintenanceModeCheck)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_ISMODE_NOTSPECIFIED"), true, ThemeColorType.Error);
                return 46;
            }

            if (safeModeCheck)
                result = KernelEntry.SafeMode;
            if (debugModeCheck)
                result = KernelEntry.DebugMode;
            if (maintenanceModeCheck)
                result = KernelEntry.Maintenance;

            if (verbose)
                TextWriterColor.Write(result.ToString());
            return 0;
        }
    }
}
