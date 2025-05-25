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

using SpecProbe.Parts.Types;
using System.Collections;
using System.Linq;
using HwProber = SpecProbe.HardwareProber;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Reflection;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Misc.Splash;
using Nitrocid.Languages;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Users.Windows;
using SpecProbe.Parts;

namespace Nitrocid.Drivers.HardwareProber
{
    /// <summary>
    /// Base Hardware prober driver
    /// </summary>
    public abstract class BaseHardwareProberDriver : IHardwareProberDriver
    {
        /// <inheritdoc/>
        public virtual string DriverName =>
            "Default";

        /// <inheritdoc/>
        public virtual DriverTypes DriverType =>
            DriverTypes.HardwareProber;

        /// <inheritdoc/>
        public virtual bool DriverInternal =>
            false;

        /// <inheritdoc/>
        public string[] SupportedHardwareTypes =>
            ["HDD", "CPU", "GPU", "RAM"];

        /// <inheritdoc/>
        public virtual IEnumerable? ProbeGraphics() =>
            HwProber.GetVideos();

        /// <inheritdoc/>
        public virtual IEnumerable? ProbeHardDrive() =>
            HwProber.GetHardDisks();

        /// <inheritdoc/>
        public virtual IEnumerable ProbePcMemory() =>
            new MemoryPart?[] { HwProber.GetMemory() };

        /// <inheritdoc/>
        public virtual IEnumerable ProbeProcessor() =>
            new ProcessorPart?[] { HwProber.GetProcessor() };

        /// <inheritdoc/>
        public virtual string DiskInfo(int diskIndex)
        {
            var hardDrives = ProbeHardDrive() as HardDiskPart[];
            if (hardDrives is not null && diskIndex < hardDrives.Length)
            {
                // Get the drive index and get the partition info
                var drive = hardDrives[diskIndex];
                TextWriterColor.Write($"[{drive.HardDiskNumber}] {drive.HardDiskSize.SizeString()}");
                return $"[{drive.HardDiskNumber}] {drive.HardDiskSize.SizeString()}";
            }
            else
                TextWriterColor.Write(Translate.DoTranslation("Disk doesn't exist"));
            return "";
        }

        /// <inheritdoc/>
        public virtual string DiskPartitionInfo(int diskIndex, int diskPartitionIndex)
        {
            var hardDrives = ProbeHardDrive() as HardDiskPart[];
            if (hardDrives is not null && diskIndex < hardDrives.Length)
            {
                // Get the drive index and get the partition info
                var parts = hardDrives[diskIndex].Partitions;
                if (diskPartitionIndex < parts.Length)
                {
                    // Get the part index and get the partition info
                    var part = parts[diskPartitionIndex];

                    // Write partition information
                    int id = part.PartitionNumber;
                    string size = part.PartitionSize.SizeString();
                    TextWriterColor.Write($"[{diskPartitionIndex + 1}] {id} - {size}");
                    return $"[{diskPartitionIndex + 1}] {id} - {size}";
                }
                else
                    TextWriterColor.Write(Translate.DoTranslation("Partition doesn't exist"));
            }
            else
                TextWriterColor.Write(Translate.DoTranslation("Disk doesn't exist"));
            return "";
        }

        /// <inheritdoc/>
        public virtual string ListDiskPartitions(int diskIndex)
        {
            var hardDrives = ProbeHardDrive() as HardDiskPart[];
            if (hardDrives is not null && diskIndex < hardDrives.Length)
            {
                // Get the drive index and get the partition info
                var parts = hardDrives[diskIndex].Partitions;
                int partNum = 1;
                foreach (var part in parts)
                {
                    // Write partition information
                    int id = part.PartitionNumber;
                    string size = part.PartitionSize.SizeString();
                    TextWriterColor.Write($"[{partNum}] {id}, {size}");
                    partNum++;
                }
                return $"[{string.Join(", ", parts.Select((pp) => pp.PartitionNumber))}]";
            }
            else
                TextWriterColor.Write(Translate.DoTranslation("Partition doesn't exist"));
            return "";
        }

        /// <inheritdoc/>
        public virtual string ListDisks()
        {
            var hardDrives = ProbeHardDrive() as HardDiskPart[];
            if (hardDrives is not null && hardDrives.Length == 0 || hardDrives is null)
            {
                // SpecProbe may have failed to parse hard disks due to insufficient permissions.
                TextWriters.Write(Translate.DoTranslation("The hardware probing library has failed to probe hard drives. If you're running Windows, the most likely cause is that you have insufficient permissions to access the hard drive information. Restart Nitrocid with elevated administrative privileges to be able to parse hard drives."), true, KernelColorType.Warning);
                return "";
            }
            for (int i = 0; i < hardDrives.Length; i++)
            {
                var hardDrive = hardDrives[i];
                TextWriterColor.Write($"- [{i + 1}] {hardDrive.HardDiskSize.SizeString()}");
            }
            return $"[{string.Join(", ", hardDrives.Select((hdp) => hdp.HardDiskNumber))}]";
        }

        /// <inheritdoc/>
        public virtual void ListHardware() =>
            ListHardware(ProbeProcessor(), ProbePcMemory(), ProbeGraphics(), ProbeHardDrive());

        /// <inheritdoc/>
        public virtual void ListHardware(IEnumerable? processors, IEnumerable? memory, IEnumerable? graphics, IEnumerable? hardDrives)
        {
            if (!WindowsUserTools.IsAdministrator())
            {
                SplashReport.ReportProgressError(Translate.DoTranslation("You'll need to restart the kernel as elevated in order to be able to show hardware information."));
                return;
            }

            // Verify the types
            if (processors is not ProcessorPart[] procDict)
            {
                SplashReport.ReportProgressError("CPU: " + Translate.DoTranslation("Failed to parse the CPU info. Ensure that it's a valid info list."));
                return;
            }
            if (memory is not MemoryPart[] memList)
            {
                SplashReport.ReportProgressError("RAM: " + Translate.DoTranslation("Failed to parse the RAM info. Ensure that it's a valid info list."));
                return;
            }
            if (graphics is not VideoPart[] gpuDict)
            {
                SplashReport.ReportProgressError("GPU: " + Translate.DoTranslation("Failed to parse the GPU info. Ensure that it's a valid info list."));
                return;
            }
            if (hardDrives is not HardDiskPart[] hddDict)
            {
                SplashReport.ReportProgressError("HDD: " + Translate.DoTranslation("Failed to parse the HDD info. Ensure that it's a valid info list."));
                return;
            }

            // Print information about the probed hardware, starting from the CPU info
            foreach (var cpu in procDict)
            {
                SplashReport.ReportProgress("CPU: " + Translate.DoTranslation("Processor name:") + " {0}", cpu.Name);
                SplashReport.ReportProgress("CPU: " + Translate.DoTranslation("Processor clock speed:") + " {0} MHz", cpu.Speed);
                SplashReport.ReportProgress("CPU: " + Translate.DoTranslation("Total number of processors:") + $" {cpu.LogicalCores} ({cpu.ProcessorCores} x{cpu.Cores})", 3);
            }
            PrintErrors(HardwarePartType.Processor);

            // Print RAM info
            var mem = memList[0];
            SplashReport.ReportProgress("RAM: " + Translate.DoTranslation("Total memory:") + " {0}", 2, mem.TotalMemory.SizeString());
            PrintErrors(HardwarePartType.Memory);

            // GPU info
            foreach (var gpu in gpuDict)
                SplashReport.ReportProgress("GPU: " + Translate.DoTranslation("Graphics card:") + " {0}", gpu.VideoCardName);
            PrintErrors(HardwarePartType.Video);

            // Drive Info
            foreach (var hdd in hddDict)
            {
                SplashReport.ReportProgress("HDD: " + Translate.DoTranslation("Disk size:") + " {0}", hdd.HardDiskSize.SizeString());

                // Partition info
                foreach (var part in hdd.Partitions)
                    SplashReport.ReportProgress("HDD [{0}]: " + Translate.DoTranslation("Partition size:") + " {1}", hdd.HardDiskNumber, part.PartitionSize);
            }
            PrintErrors(HardwarePartType.HardDisk);
        }

        /// <inheritdoc/>
        public virtual void ListHardware(string hardwareType)
        {
            if (!WindowsUserTools.IsAdministrator())
            {
                SplashReport.ReportProgressError(Translate.DoTranslation("You'll need to restart the kernel as elevated in order to be able to show hardware information."));
                return;
            }

            HardwarePartType[] supportedTypesEnum =
            [
                HardwarePartType.HardDisk,
                HardwarePartType.Processor,
                HardwarePartType.Video,
                HardwarePartType.Memory,
            ];
            if (hardwareType == "all")
            {
                for (int i = 0; i < SupportedHardwareTypes.Length; i++)
                {
                    string supportedType = SupportedHardwareTypes[i];
                    ListHardwareInternal(supportedType);
                    PrintErrors(supportedTypesEnum[i]);
                }
            }
            else
            {
                ListHardwareInternal(hardwareType);
                foreach (var supportedType in supportedTypesEnum)
                    PrintErrors(supportedType);
            }
        }

        private void ListHardwareInternal(string hardwareType)
        {
            SeparatorWriterColor.WriteSeparatorColor(hardwareType, KernelColorTools.GetColor(KernelColorType.ListTitle));
            switch (hardwareType)
            {
                case "CPU":
                    {
                        var hardwareList = ProbeProcessor() as ProcessorPart[];
                        if (hardwareList is not null)
                        {
                            foreach (var processor in hardwareList)
                            {
                                TextWriters.Write(Translate.DoTranslation("Processor name:"), false, KernelColorType.ListEntry);
                                TextWriters.Write($" {processor.Name}", true, KernelColorType.ListValue);
                                TextWriters.Write(Translate.DoTranslation("Processor vendor:"), false, KernelColorType.ListEntry);
                                TextWriters.Write($" {processor.Vendor} [CPUID: {processor.CpuidVendor}]", true, KernelColorType.ListValue);
                                TextWriters.Write(Translate.DoTranslation("Clock speed:"), false, KernelColorType.ListEntry);
                                TextWriters.Write($" {processor.Speed} MHz", true, KernelColorType.ListValue);
                                TextWriters.Write(Translate.DoTranslation("Total cores:"), false, KernelColorType.ListEntry);
                                TextWriters.Write($" {processor.LogicalCores} ({processor.ProcessorCores} x{processor.Cores})", true, KernelColorType.ListValue);
                                TextWriters.Write(Translate.DoTranslation("Cache sizes:"), false, KernelColorType.ListEntry);
                                TextWriters.Write($" {processor.L1CacheSize.SizeString()} L1, {processor.L2CacheSize.SizeString()} L2, {processor.L3CacheSize.SizeString()} L3", true, KernelColorType.ListValue);
                            }
                        }
                        break;
                    }
                case "RAM":
                    {
                        var hardwareList = ProbePcMemory() as MemoryPart[];
                        if (hardwareList is not null)
                        {
                            foreach (var ram in hardwareList)
                            {
                                TextWriters.Write(Translate.DoTranslation("Total usable memory:"), false, KernelColorType.ListEntry);
                                TextWriters.Write($" {ram.TotalMemory.SizeString()}", true, KernelColorType.ListValue);
                                TextWriters.Write(Translate.DoTranslation("Total memory:"), false, KernelColorType.ListEntry);
                                TextWriters.Write($" {ram.TotalPhysicalMemory.SizeString()}", true, KernelColorType.ListValue);
                                TextWriters.Write(Translate.DoTranslation("Reserved memory:"), false, KernelColorType.ListEntry);
                                TextWriters.Write($" {ram.SystemReservedMemory.SizeString()}", true, KernelColorType.ListValue);
                            }
                        }
                        break;
                    }
                case "HDD":
                    {
                        var hardwareList = ProbeHardDrive() as HardDiskPart[];
                        if (hardwareList is not null)
                        {
                            foreach (var hdd in hardwareList)
                            {
                                TextWriters.Write(Translate.DoTranslation("Disk number:"), false, KernelColorType.ListEntry);
                                TextWriters.Write($" {hdd.HardDiskNumber}", true, KernelColorType.ListValue);
                                TextWriters.Write(Translate.DoTranslation("Disk size:"), false, KernelColorType.ListEntry);
                                TextWriters.Write($" {hdd.HardDiskSize.SizeString()}", true, KernelColorType.ListValue);
                                TextWriters.Write(Translate.DoTranslation("Partitions:"), false, KernelColorType.ListEntry);
                                TextWriters.Write($" {hdd.PartitionCount}", true, KernelColorType.ListValue);
                                foreach (var part in hdd.Partitions)
                                {
                                    TextWriters.Write(Translate.DoTranslation("Partition number:"), false, KernelColorType.ListEntry);
                                    TextWriters.Write($" {part.PartitionNumber}", true, KernelColorType.ListValue);
                                    TextWriters.Write(Translate.DoTranslation("Partition size:"), false, KernelColorType.ListEntry);
                                    TextWriters.Write($" {part.PartitionSize.SizeString()}", true, KernelColorType.ListValue);
                                }
                            }
                        }
                        break;
                    }
                case "GPU":
                    {
                        var hardwareList = ProbeGraphics() as VideoPart[];
                        if (hardwareList is not null)
                        {
                            foreach (var gpu in hardwareList)
                            {
                                TextWriters.Write(Translate.DoTranslation("Graphics card name:"), false, KernelColorType.ListEntry);
                                TextWriters.Write($" {gpu.VideoCardName}", true, KernelColorType.ListValue);
                            }
                        }
                        break;
                    }
                default:
                    TextWriters.Write(Translate.DoTranslation("Either the hardware type {0} is not probed, or is not valid."), true, KernelColorType.Error, hardwareType);
                    break;
            }
        }

        private void PrintErrors(HardwarePartType type)
        {
            var errors = HwProber.GetParseExceptions(type);
            if (errors.Length > 0)
            {
                SplashReport.ReportProgressError(Translate.DoTranslation("SpecProbe failed to parse some of the hardware. Below are the errors reported by SpecProbe:"));
                DebugWriter.WriteDebug(DebugLevel.E, "SpecProbe failed to parse hardware due to the following errors:");
                foreach (var error in errors)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, $"- {error.Message}");
                    DebugWriter.WriteDebugStackTrace(error);
                    SplashReport.ReportProgressError(error.Message);
                }
            }
        }
    }
}
