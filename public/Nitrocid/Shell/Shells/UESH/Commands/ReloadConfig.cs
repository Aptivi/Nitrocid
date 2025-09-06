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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Reloads the kernel configuration
    /// </summary>
    /// <remarks>
    /// This command reloads the kernel settings and tries to conserve restarts and reflect the changes immediately. If the changes are not reflected, reboot the kernel.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class ReloadConfigCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            ConfigTools.ReloadConfig();
            TextWriterColor.Write(Translate.DoTranslation("Configuration reloaded. You might need to reboot the kernel for some changes to take effect."));
            return 0;
        }

        public override void HelpHelper() =>
            TextWriterColor.Write(Translate.DoTranslation("Colors don't require a restart, but most of the settings require a restart."));

    }
}
