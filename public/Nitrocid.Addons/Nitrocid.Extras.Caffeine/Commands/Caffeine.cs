//
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

using System;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using Nitrocid.Extras.Caffeine.Alarm;

namespace Nitrocid.Extras.Caffeine.Commands
{
    /// <summary>
    /// Sets up your coffee or tea alarm
    /// </summary>
    /// <remarks>
    /// This command schedules your tea or coffee alarm to make the kernel emit a sound when it's ready.
    /// </remarks>
    class CaffeineCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (!int.TryParse(parameters.ArgumentsList[0], out int alarmSeconds))
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("The seconds in which your cup will be ready is invalid."), KernelColorType.Error);
                return 26;
            }
            AlarmTools.StartAlarm(Translate.DoTranslation("Your cup is now ready!"), alarmSeconds);
            return 0;
        }

    }
}
