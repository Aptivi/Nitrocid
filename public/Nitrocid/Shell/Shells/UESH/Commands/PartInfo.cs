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
using Nitrocid.Drivers.HardwareProber;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Shows hard disk partition info (for scripts)
    /// </summary>
    /// <remarks>
    /// This shows you a hard disk partition info.
    /// </remarks>
    class PartInfoCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            bool isDriveNum = int.TryParse(parameters.ArgumentsList[0], out int driveNum);
            if (isDriveNum)
            {
                // Get the drive index and get the partition info
                int driveIdx = driveNum - 1;
                bool isPartNum = int.TryParse(parameters.ArgumentsList[1], out int partNum);
                if (isPartNum)
                {
                    // Get the part index and get the partition info
                    int partIdx = partNum - 1;
                    variableValue = HardwareProberDriver.DiskPartitionInfo(driveIdx, partIdx);
                    return 0;
                }
                else
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_PARTNONEXISTENT"));
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.Hardware);
                }
            }
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_DISKNONEXISTENT"));
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Hardware);
            }
        }

    }
}
