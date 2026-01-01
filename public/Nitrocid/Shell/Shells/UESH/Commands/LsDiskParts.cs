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
using Nitrocid.Drivers.HardwareProber;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Shows hard disk partitions (for scripts)
    /// </summary>
    /// <remarks>
    /// This shows you a list of hard disk partitions.
    /// </remarks>
    class LsDiskPartsCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            bool isDriveNum = int.TryParse(parameters.ArgumentsList[0], out int driveNum);
            if (isDriveNum)
            {
                // Get the drive index and get the disk info
                variableValue = HardwareProberDriver.ListDiskPartitions(driveNum - 1);
                return 0;
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Disk doesn't exist"));
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Hardware);
            }
        }

    }
}
