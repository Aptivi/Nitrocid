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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel;
using Nitrocid.Kernel.Power;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using Nitrocid.Users.Windows;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// You can restart Nitrocid with elevated admin permissions
    /// </summary>
    /// <remarks>
    /// If you are on Windows, you can use this command to shut Nitrocid down and restart it with elevated admin permissions.
    /// </remarks>
    class WinElevateCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (KernelPlatform.IsOnWindows())
            {
                bool isAdmin = WindowsUserTools.IsAdministrator();
                if (isAdmin)
                    TextWriterColor.Write(Translate.DoTranslation("You're already running the elevated Nitrocid session!"));
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("Elevating your Nitrocid session..."));
                    PowerManager.elevating = true;
                    PowerManager.PowerManage(PowerMode.Shutdown);
                }
            }
            else
                TextWriterColor.Write(Translate.DoTranslation("This command is unavailable for your platform."));
            return 0;
        }
    }
}
