
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
using System.Drawing;
using System.Linq;
using System.Xml.Linq;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Shows hard disk partition info (for scripts)
    /// </summary>
    /// <remarks>
    /// This shows you a hard disk partition info.
    /// </remarks>
    class PartInfoCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, ref string variableValue)
        {
            var driveKeyValues = HardwareProbe.HardwareInfo.Hardware.HDD;
            var hardDrives = driveKeyValues.Keys.ToArray();
            bool isDriveNum = int.TryParse(ListArgsOnly[0], out int driveNum);
            if (isDriveNum && driveNum <= hardDrives.Length)
            {
                // Get the drive index and get the partition info
                int driveIdx = driveNum - 1;
                var partKeyValues = driveKeyValues[hardDrives[driveIdx]].Partitions;
                var parts = partKeyValues.Keys.ToArray();
                bool isPartNum = int.TryParse(ListArgsOnly[1], out int partNum);
                if (isPartNum && partNum <= parts.Length)
                {
                    // Get the part index and get the partition info
                    int partIdx = partNum - 1;
                    var part = partKeyValues[parts[partIdx]];

                    // Write partition information
                    string id = part.ID;
                    string name = part.Name;
                    string size = part.Size;
                    string used = part.Used;
                    string fileSystem = part.FileSystem;
                    TextWriterColor.Write($"[{partNum}] {name}, {id}");
                    TextWriterColor.Write($"  - {used} / {size}, {fileSystem}");
                    variableValue = $"[{partNum}] {name}, {id} | {used} / {size}, {fileSystem}";
                    return 0;
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("Partition doesn't exist"));
                    return 10000 + (int)KernelExceptionType.Hardware;
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Disk doesn't exist"));
                return 10000 + (int)KernelExceptionType.Hardware;
            }
        }

    }
}
