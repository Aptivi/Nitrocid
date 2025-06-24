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

using Nitrocid.Base.Kernel.Hardware;
using System.Collections;

namespace Nitrocid.Base.Drivers.HardwareProber
{
    /// <summary>
    /// Hardware prober driver interface for drivers
    /// </summary>
    public interface IHardwareProberDriver : IDriver
    {
        /// <summary>
        /// List of supported hardware types for <see cref="HardwareList"/>
        /// </summary>
        string[] SupportedHardwareTypes { get; }

        /// <summary>
        /// Probes the processor
        /// </summary>
        /// <returns>An object representing the processor collection object</returns>
        IEnumerable ProbeProcessor();

        /// <summary>
        /// Probes the PC memory
        /// </summary>
        /// <returns>An object representing the PC memory collection object</returns>
        IEnumerable ProbePcMemory();

        /// <summary>
        /// Probes the graphics adapter
        /// </summary>
        /// <returns>An object representing the graphics adapter collection object</returns>
        IEnumerable? ProbeGraphics();

        /// <summary>
        /// Probes the hard drives
        /// </summary>
        /// <returns>An object representing the hard drive collection object</returns>
        IEnumerable? ProbeHardDrive();

        /// <summary>
        /// Lists all the probed hardware. Please note that this overload probes all the hardware
        /// </summary>
        void ListHardware();

        /// <summary>
        /// Lists all the probed hardware.
        /// </summary>
        /// <param name="processors">List of processors returned from the same driver</param>
        /// <param name="memory">List of memory returned from the same driver</param>
        /// <param name="graphics">List of graphics adapters returned from the same driver</param>
        /// <param name="hardDrives">List of hard drives returned from the same driver</param>
        void ListHardware(IEnumerable? processors, IEnumerable? memory, IEnumerable? graphics, IEnumerable? hardDrives);

        /// <summary>
        /// Lists information about hardware
        /// </summary>
        /// <param name="hardwareType">Hardware type supported by the prober driver. If "all", prints all information.</param>
        void ListHardware(string hardwareType);

        /// <summary>
        /// Lists the disk partitions
        /// </summary>
        /// <param name="diskIndex">The zero-based number of the disk number</param>
        string ListDiskPartitions(int diskIndex);

        /// <summary>
        /// Lists the disks
        /// </summary>
        string ListDisks();

        /// <summary>
        /// Prints the disk info
        /// </summary>
        /// <param name="diskIndex">The zero-based number of the disk number</param>
        string DiskInfo(int diskIndex);

        /// <summary>
        /// Prints the disk partition info
        /// </summary>
        /// <param name="diskIndex">The zero-based number of the disk number</param>
        /// <param name="diskPartitionIndex">The zero-based number of the disk partition number</param>
        string DiskPartitionInfo(int diskIndex, int diskPartitionIndex);
    }
}
