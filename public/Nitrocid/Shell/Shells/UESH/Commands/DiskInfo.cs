
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Exceptions;
using KS.Kernel.Hardware;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using System.Linq;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Shows hard disk info (for scripts)
    /// </summary>
    /// <remarks>
    /// This shows you a hard disk info.
    /// </remarks>
    class DiskInfoCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var driveKeyValues = HardwareProbe.HardwareInfo.Hardware.HDD;
            var hardDrives = HardwareProbe.HardwareInfo.Hardware.HDD.Keys.ToArray();
            bool isDriveNum = int.TryParse(parameters.ArgumentsList[0], out int driveNum);
            if (isDriveNum && driveNum <= hardDrives.Length)
            {
                // Get the drive index and get the partition info
                int driveIdx = driveNum - 1;
                var drive = driveKeyValues[hardDrives[driveIdx]];
                TextWriterColor.Write($"[{drive.ID}] {drive.Vendor} {drive.Model}");
                TextWriterColor.Write($"  - {drive.Size}, {drive.Speed}, {drive.Serial}");
                variableValue = $"[{drive.ID}] {drive.Vendor} {drive.Model} | {drive.Size}, {drive.Speed}, {drive.Serial}";
                return 0;
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Disk doesn't exist"));
                return 10000 + (int)KernelExceptionType.Hardware;
            }
        }

    }
}
