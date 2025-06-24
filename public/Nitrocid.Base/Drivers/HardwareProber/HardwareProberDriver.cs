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

using System.Collections;

namespace Nitrocid.Base.Drivers.HardwareProber
{
    /// <summary>
    /// Hardware prober driver module
    /// </summary>
    public static class HardwareProberDriver
    {
        /// <summary>
        /// Probes the graphics adapter
        /// </summary>
        /// <returns>An object representing the graphics adapter collection object</returns>
        public static IEnumerable? ProbeGraphics() =>
            DriverHandler.CurrentHardwareProberDriverLocal.ProbeGraphics();

        /// <summary>
        /// Probes the hard drives
        /// </summary>
        /// <returns>An object representing the hard drive collection object</returns>
        public static IEnumerable? ProbeHardDrive() =>
            DriverHandler.CurrentHardwareProberDriverLocal.ProbeHardDrive();

        /// <summary>
        /// Probes the PC memory
        /// </summary>
        /// <returns>An object representing the PC memory collection object</returns>
        public static IEnumerable ProbePcMemory() =>
            DriverHandler.CurrentHardwareProberDriverLocal.ProbePcMemory();

        /// <summary>
        /// Probes the processor
        /// </summary>
        /// <returns>An object representing the processor collection object</returns>
        public static IEnumerable ProbeProcessor() =>
            DriverHandler.CurrentHardwareProberDriverLocal.ProbeProcessor();

        /// <summary>
        /// Lists all the probed hardware. Please note that this overload probes all the hardware
        /// </summary>
        public static void ListHardware() =>
            DriverHandler.CurrentHardwareProberDriverLocal.ListHardware();

        /// <summary>
        /// Lists all the probed hardware. Please note that this overload probes all the hardware
        /// </summary>
        /// <param name="processors">List of processors returned from the same driver</param>
        /// <param name="memory">List of memory returned from the same driver</param>
        /// <param name="graphics">List of graphics adapters returned from the same driver</param>
        /// <param name="hardDrives">List of hard drives returned from the same driver</param>
        public static void ListHardware(IEnumerable? processors, IEnumerable? memory, IEnumerable? graphics, IEnumerable? hardDrives) =>
            DriverHandler.CurrentHardwareProberDriverLocal.ListHardware(processors, memory, graphics, hardDrives);

        /// <summary>
        /// Lists all the probed hardware. Please note that this overload probes all the hardware
        /// </summary>
        /// <param name="hardwareType">Hardware type supported by the prober driver. If "all", prints all information.</param>
        public static void ListHardware(string hardwareType) =>
            DriverHandler.CurrentHardwareProberDriverLocal.ListHardware(hardwareType);

        /// <summary>
        /// Lists the disk partitions
        /// </summary>
        /// <param name="diskIndex">The zero-based number of the disk number</param>
        public static string ListDiskPartitions(int diskIndex) =>
            DriverHandler.CurrentHardwareProberDriverLocal.ListDiskPartitions(diskIndex);

        /// <summary>
        /// Lists the disks
        /// </summary>
        public static string ListDisks() =>
            DriverHandler.CurrentHardwareProberDriverLocal.ListDisks();

        /// <summary>
        /// Prints the disk info
        /// </summary>
        /// <param name="diskIndex">The zero-based number of the disk number</param>
        public static string DiskInfo(int diskIndex) =>
            DriverHandler.CurrentHardwareProberDriverLocal.DiskInfo(diskIndex);

        /// <summary>
        /// Prints the disk partition info
        /// </summary>
        /// <param name="diskIndex">The zero-based number of the disk number</param>
        /// <param name="diskPartitionIndex">The zero-based number of the disk partition number</param>
        public static string DiskPartitionInfo(int diskIndex, int diskPartitionIndex) =>
            DriverHandler.CurrentHardwareProberDriverLocal.DiskPartitionInfo(diskIndex, diskPartitionIndex);
    }
}
