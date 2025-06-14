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
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_DISKNONEXISTENT"));
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
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_PARTNONEXISTENT"));
            }
            else
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_DISKNONEXISTENT"));
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
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_PARTNONEXISTENT"));
            return "";
        }

        /// <inheritdoc/>
        public virtual string ListDisks()
        {
            var hardDrives = ProbeHardDrive() as HardDiskPart[];
            if (hardDrives is not null && hardDrives.Length == 0 || hardDrives is null)
            {
                // SpecProbe may have failed to parse hard disks due to insufficient permissions.
                TextWriters.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_STORAGEFAILED"), true, KernelColorType.Warning);
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
                SplashReport.ReportProgressError(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_NEEDSELEVATION"));
                return;
            }

            // Verify the types
            if (processors is not ProcessorPart[] procDict)
            {
                SplashReport.ReportProgressError("CPU: " + LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_CPUPARSEFAILED"));
                return;
            }
            if (memory is not MemoryPart[] memList)
            {
                SplashReport.ReportProgressError("RAM: " + LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_RAMPARSEFAILED"));
                return;
            }
            if (graphics is not VideoPart[] gpuDict)
            {
                SplashReport.ReportProgressError("GPU: " + LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_GPUPARSEFAILED"));
                return;
            }
            if (hardDrives is not HardDiskPart[] hddDict)
            {
                SplashReport.ReportProgressError("HDD: " + LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_HDDPARSEFAILED"));
                return;
            }

            // Print information about the probed hardware, starting from the CPU info
            foreach (var cpu in procDict)
            {
                SplashReport.ReportProgress("CPU: " + LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_LISTING_CPUNAME") + " {0}", cpu.Name);
                SplashReport.ReportProgress("CPU: " + LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_CLOCKSPEED") + " {0} MHz", cpu.Speed);
                SplashReport.ReportProgress("CPU: " + LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_TOTALCPUS") + $" {cpu.LogicalCores} ({cpu.ProcessorCores} x{cpu.Cores})", 3);
            }
            PrintErrors(HardwarePartType.Processor);

            // Print RAM info
            var mem = memList[0];
            SplashReport.ReportProgress("RAM: " + LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_TOTALMEMORY") + " {0}", 2, mem.TotalMemory.SizeString());
            PrintErrors(HardwarePartType.Memory);

            // GPU info
            foreach (var gpu in gpuDict)
                SplashReport.ReportProgress("GPU: " + LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_GFXCARD") + " {0}", gpu.VideoCardName);
            PrintErrors(HardwarePartType.Video);

            // Drive Info
            foreach (var hdd in hddDict)
            {
                SplashReport.ReportProgress("HDD: " + LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_DISKSIZE") + " {0}", hdd.HardDiskSize.SizeString());

                // Partition info
                foreach (var part in hdd.Partitions)
                    SplashReport.ReportProgress("HDD [{0}]: " + LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_LISTING_PARTITIONSIZE") + " {1}", hdd.HardDiskNumber, part.PartitionSize);
            }
            PrintErrors(HardwarePartType.HardDisk);
        }

        /// <inheritdoc/>
        public virtual void ListHardware(string hardwareType)
        {
            if (!WindowsUserTools.IsAdministrator())
            {
                SplashReport.ReportProgressError(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_NEEDSELEVATION"));
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
                                TextWriters.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_LISTING_CPUNAME"), false, KernelColorType.ListEntry);
                                TextWriters.Write($" {processor.Name}", true, KernelColorType.ListValue);
                                TextWriters.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_PROCESSORVENDOR"), false, KernelColorType.ListEntry);
                                TextWriters.Write($" {processor.Vendor} [CPUID: {processor.CpuidVendor}]", true, KernelColorType.ListValue);
                                TextWriters.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_CLOCKSPEED"), false, KernelColorType.ListEntry);
                                TextWriters.Write($" {processor.Speed} MHz", true, KernelColorType.ListValue);
                                TextWriters.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_TOTALCORES"), false, KernelColorType.ListEntry);
                                TextWriters.Write($" {processor.LogicalCores} ({processor.ProcessorCores} x{processor.Cores})", true, KernelColorType.ListValue);
                                TextWriters.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_CACHEDSIZES"), false, KernelColorType.ListEntry);
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
                                TextWriters.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_TOTALUSABLEMEMORY"), false, KernelColorType.ListEntry);
                                TextWriters.Write($" {ram.TotalMemory.SizeString()}", true, KernelColorType.ListValue);
                                TextWriters.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_TOTALMEMORY"), false, KernelColorType.ListEntry);
                                TextWriters.Write($" {ram.TotalPhysicalMemory.SizeString()}", true, KernelColorType.ListValue);
                                TextWriters.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_TOTALRESERVEDMEMORY"), false, KernelColorType.ListEntry);
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
                                TextWriters.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_DISKNUM"), false, KernelColorType.ListEntry);
                                TextWriters.Write($" {hdd.HardDiskNumber}", true, KernelColorType.ListValue);
                                TextWriters.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_DISKSIZE"), false, KernelColorType.ListEntry);
                                TextWriters.Write($" {hdd.HardDiskSize.SizeString()}", true, KernelColorType.ListValue);
                                TextWriters.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_PARTITIONS"), false, KernelColorType.ListEntry);
                                TextWriters.Write($" {hdd.PartitionCount}", true, KernelColorType.ListValue);
                                foreach (var part in hdd.Partitions)
                                {
                                    TextWriters.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_PARTITIONNUM"), false, KernelColorType.ListEntry);
                                    TextWriters.Write($" {part.PartitionNumber}", true, KernelColorType.ListValue);
                                    TextWriters.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_LISTING_PARTITIONSIZE"), false, KernelColorType.ListEntry);
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
                                TextWriters.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_GFXCARDNAME"), false, KernelColorType.ListEntry);
                                TextWriters.Write($" {gpu.VideoCardName}", true, KernelColorType.ListValue);
                            }
                        }
                        break;
                    }
                default:
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_TYPENOTFOUND"), true, KernelColorType.Error, hardwareType);
                    break;
            }
        }

        private void PrintErrors(HardwarePartType type)
        {
            var errors = HwProber.GetParseExceptions(type);
            if (errors.Length > 0)
            {
                SplashReport.ReportProgressError(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_PARSEFAILED"));
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
