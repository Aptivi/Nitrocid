
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Misc.Splash;
using SpecProbe.Hardware.Parts.Types;
using System.Collections;
using System.Linq;
using HwProber = SpecProbe.Hardware.HardwareProber;

namespace KS.Drivers.HardwareProber.Bases
{
    internal class SpecProbeHardwareProber : BaseHardwareProberDriver, IHardwareProberDriver
    {
        public override string DriverName =>
            "SpecProbe";

        /// <inheritdoc/>
        public override IEnumerable ProbeGraphics() =>
            HwProber.Video;

        /// <inheritdoc/>
        public override IEnumerable ProbeHardDrive() =>
            HwProber.HardDisk;

        /// <inheritdoc/>
        public override IEnumerable ProbePcMemory() =>
            HwProber.Memory;

        /// <inheritdoc/>
        public override IEnumerable ProbeProcessor() =>
            HwProber.Processors;

        public override string DiskInfo(int diskIndex)
        {
            var hardDrives = ProbeHardDrive() as HardDiskPart[];
            if (diskIndex < hardDrives.Length)
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

        public override string DiskPartitionInfo(int diskIndex, int diskPartitionIndex)
        {
            var hardDrives = ProbeHardDrive() as HardDiskPart[];
            if (diskIndex < hardDrives.Length)
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

        public override string ListDiskPartitions(int diskIndex)
        {
            var hardDrives = ProbeHardDrive() as HardDiskPart[];
            if (diskIndex < hardDrives.Length)
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

        public override string ListDisks()
        {
            var hardDrives = ProbeHardDrive() as HardDiskPart[];
            for (int i = 0; i < hardDrives.Length; i++)
            {
                var hardDrive = hardDrives[i];
                TextWriterColor.Write($"- [{i + 1}] {hardDrive.HardDiskSize.SizeString()}");
            }
            return $"[{string.Join(", ", hardDrives.Select((hdp) => hdp.HardDiskNumber))}]";
        }

        public override void ListHardware() =>
            ListHardware(ProbeProcessor(), ProbePcMemory(), ProbeGraphics(), ProbeHardDrive());

        public override void ListHardware(IEnumerable processors, IEnumerable memory, IEnumerable graphics, IEnumerable hardDrives)
        {
            // Verify the types
            if (processors is not ProcessorPart[] procDict)
            {
                SplashReport.ReportProgress("CPU: " + Translate.DoTranslation("Failed to parse the CPU info. Ensure that it's a valid info list."));
                return;
            }
            if (memory is not MemoryPart[] memList)
            {
                SplashReport.ReportProgress("RAM: " + Translate.DoTranslation("Failed to parse the RAM info. Ensure that it's a valid info list."));
                return;
            }
            if (graphics is not VideoPart[] gpuDict)
            {
                SplashReport.ReportProgress("GPU: " + Translate.DoTranslation("Failed to parse the GPU info. Ensure that it's a valid info list."));
                return;
            }
            if (hardDrives is not HardDiskPart[] hddDict)
            {
                SplashReport.ReportProgress("HDD: " + Translate.DoTranslation("Failed to parse the HDD info. Ensure that it's a valid info list."));
                return;
            }

            // Print information about the probed hardware, starting from the CPU info
            foreach (var cpu in procDict)
            {
                SplashReport.ReportProgress("CPU: " + Translate.DoTranslation("Processor name:") + " {0}", cpu.Name);
                SplashReport.ReportProgress("CPU: " + Translate.DoTranslation("Processor clock speed:") + " {0} MHz", cpu.Speed);
                SplashReport.ReportProgress("CPU: " + Translate.DoTranslation("Total number of processors:") + $" {cpu.TotalCores}", 3);
            }

            // Print RAM info
            var mem = memList[0];
            SplashReport.ReportProgress("RAM: " + Translate.DoTranslation("Total memory:") + " {0}", 2, mem.TotalMemory.SizeString());

            // GPU info
            foreach (var gpu in gpuDict)
                SplashReport.ReportProgress("GPU: " + Translate.DoTranslation("Graphics card:") + " {0}", gpu.VideoCardName);

            // Drive Info
            foreach (var hdd in hddDict)
            {
                SplashReport.ReportProgress("HDD: " + Translate.DoTranslation("Disk size:") + " {0}", hdd.HardDiskSize.SizeString());

                // Partition info
                foreach (var part in hdd.Partitions)
                    SplashReport.ReportProgress("HDD [{0}]: " + Translate.DoTranslation("Partition size:") + " {1}", hdd.HardDiskNumber, part.PartitionSize);
            }
            PrintErrors();
        }

        public override void ListHardware(string hardwareType)
        {
            string[] supportedTypes = new[] { "CPU", "RAM", "HDD", "GPU" };
            if (hardwareType == "all")
            {
                foreach (string supportedType in supportedTypes)
                    ListHardwareInternal(supportedType);
            }
            else
                ListHardwareInternal(hardwareType);
            PrintErrors();
        }

        private void ListHardwareInternal(string hardwareType)
        {
            SeparatorWriterColor.WriteSeparator(hardwareType, true);
            switch (hardwareType)
            {
                case "CPU":
                    {
                        var hardwareList = ProbeProcessor() as ProcessorPart[];
                        if (hardwareList is not null)
                        {
                            foreach (var processor in hardwareList)
                            {
                                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Processor name:"), false, KernelColorType.ListEntry);
                                TextWriterColor.WriteKernelColor($" {processor.Name}", true, KernelColorType.ListValue);
                                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Processor vendor:"), false, KernelColorType.ListEntry);
                                TextWriterColor.WriteKernelColor($" {processor.Vendor} [{processor.CpuidVendor}]", true, KernelColorType.ListValue);
                                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Clock speed:"), false, KernelColorType.ListEntry);
                                TextWriterColor.WriteKernelColor($" {processor.Speed} MHz", true, KernelColorType.ListValue);
                                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Total cores:"), false, KernelColorType.ListEntry);
                                TextWriterColor.WriteKernelColor($" {processor.TotalCores} ({processor.ProcessorCores} x{processor.CoresForEachCore})", true, KernelColorType.ListValue);
                                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Cache sizes:"), false, KernelColorType.ListEntry);
                                TextWriterColor.WriteKernelColor($" {processor.L1CacheSize.SizeString()} L1, {processor.L2CacheSize.SizeString()} L2, {processor.L3CacheSize.SizeString()} L3", true, KernelColorType.ListValue);
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
                                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Total usable memory:"), false, KernelColorType.ListEntry);
                                TextWriterColor.WriteKernelColor($" {ram.TotalMemory.SizeString()}", true, KernelColorType.ListValue);
                                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Total memory:"), false, KernelColorType.ListEntry);
                                TextWriterColor.WriteKernelColor($" {ram.TotalPhysicalMemory.SizeString()}", true, KernelColorType.ListValue);
                                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Reserved memory:"), false, KernelColorType.ListEntry);
                                TextWriterColor.WriteKernelColor($" {ram.SystemReservedMemory.SizeString()}", true, KernelColorType.ListValue);
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
                                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Disk number:"), false, KernelColorType.ListEntry);
                                TextWriterColor.WriteKernelColor($" {hdd.HardDiskNumber}", true, KernelColorType.ListValue);
                                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Disk size:"), false, KernelColorType.ListEntry);
                                TextWriterColor.WriteKernelColor($" {hdd.HardDiskSize.SizeString()}", true, KernelColorType.ListValue);
                                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Partitions:"), false, KernelColorType.ListEntry);
                                TextWriterColor.WriteKernelColor($" {hdd.PartitionCount}", true, KernelColorType.ListValue);
                                foreach (var part in hdd.Partitions)
                                {
                                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("Partition number:"), false, KernelColorType.ListEntry);
                                    TextWriterColor.WriteKernelColor($" {part.PartitionNumber}", true, KernelColorType.ListValue);
                                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("Partition size:"), false, KernelColorType.ListEntry);
                                    TextWriterColor.WriteKernelColor($" {part.PartitionSize.SizeString()}", true, KernelColorType.ListValue);
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
                                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Graphics card name:"), false, KernelColorType.ListEntry);
                                TextWriterColor.WriteKernelColor($" {gpu.VideoCardName}", true, KernelColorType.ListValue);
                            }
                        }
                        break;
                    }
                default:
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("Either the hardware type {0} is not probed, or is not valid."), true, KernelColorType.Error, hardwareType);
                    break;
            }
        }

        private void PrintErrors()
        {
            if (HwProber.Errors.Length > 0)
            {
                SplashReport.ReportProgressError(Translate.DoTranslation("SpecProbe failed to parse some of the hardware. Below are the errors reported by SpecProbe:"));
                DebugWriter.WriteDebug(DebugLevel.E, "SpecProbe failed to parse hardware due to the following errors:");
                foreach (var error in HwProber.Errors)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, $"- {error.Message}");
                    DebugWriter.WriteDebugStackTrace(error);
                    SplashReport.ReportProgressError(error.Message);
                }
            }
        }
    }
}
