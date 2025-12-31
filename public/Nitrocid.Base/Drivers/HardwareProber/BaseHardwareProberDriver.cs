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

using SpecProbe.Parts.Types;
using System.Collections;
using System.Linq;
using HwProber = SpecProbe.HardwareProber;
using Nitrocid.Base.Misc.Reflection;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors.Themes.Colors;
using SpecProbe.Parts;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Splash;
using Nitrocid.Base.Users.Windows;

namespace Nitrocid.Base.Drivers.HardwareProber
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
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_STORAGEFAILED"), true, ThemeColorType.Warning);
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
            SeparatorWriterColor.WriteSeparatorColor(hardwareType, ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
            switch (hardwareType)
            {
                case "CPU":
                    {
                        var hardwareList = ProbeProcessor() as ProcessorPart[];
                        if (hardwareList is not null)
                        {
                            foreach (var processor in hardwareList)
                            {
                                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_LISTING_CPUNAME"), false, ThemeColorType.ListEntry);
                                TextWriterColor.Write($" {processor.Name}", true, ThemeColorType.ListValue);
                                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_PROCESSORVENDOR"), false, ThemeColorType.ListEntry);
                                TextWriterColor.Write($" {processor.Vendor} [CPUID: {processor.CpuidVendor}]", true, ThemeColorType.ListValue);
                                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_CLOCKSPEED2"), false, ThemeColorType.ListEntry);
                                TextWriterColor.Write($" {processor.Speed} MHz", true, ThemeColorType.ListValue);
                                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_TOTALCORES"), false, ThemeColorType.ListEntry);
                                TextWriterColor.Write($" {processor.LogicalCores} ({processor.ProcessorCores} x{processor.Cores})", true, ThemeColorType.ListValue);
                                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_CACHEDSIZES"), false, ThemeColorType.ListEntry);
                                TextWriterColor.Write($" {processor.L1CacheSize.SizeString()} L1, {processor.L2CacheSize.SizeString()} L2, {processor.L3CacheSize.SizeString()} L3", true, ThemeColorType.ListValue);
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
                                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_TOTALUSABLEMEMORY"), false, ThemeColorType.ListEntry);
                                TextWriterColor.Write($" {ram.TotalMemory.SizeString()}", true, ThemeColorType.ListValue);
                                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_TOTALMEMORY"), false, ThemeColorType.ListEntry);
                                TextWriterColor.Write($" {ram.TotalPhysicalMemory.SizeString()}", true, ThemeColorType.ListValue);
                                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_TOTALRESERVEDMEMORY"), false, ThemeColorType.ListEntry);
                                TextWriterColor.Write($" {ram.SystemReservedMemory.SizeString()}", true, ThemeColorType.ListValue);
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
                                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_DISKNUM"), false, ThemeColorType.ListEntry);
                                TextWriterColor.Write($" {hdd.HardDiskNumber}", true, ThemeColorType.ListValue);
                                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_DISKSIZE"), false, ThemeColorType.ListEntry);
                                TextWriterColor.Write($" {hdd.HardDiskSize.SizeString()}", true, ThemeColorType.ListValue);
                                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_PARTITIONS"), false, ThemeColorType.ListEntry);
                                TextWriterColor.Write($" {hdd.PartitionCount}", true, ThemeColorType.ListValue);
                                foreach (var part in hdd.Partitions)
                                {
                                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_PARTITIONNUM"), false, ThemeColorType.ListEntry);
                                    TextWriterColor.Write($" {part.PartitionNumber}", true, ThemeColorType.ListValue);
                                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_LISTING_PARTITIONSIZE"), false, ThemeColorType.ListEntry);
                                    TextWriterColor.Write($" {part.PartitionSize.SizeString()}", true, ThemeColorType.ListValue);
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
                                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_GFXCARDNAME"), false, ThemeColorType.ListEntry);
                                TextWriterColor.Write($" {gpu.VideoCardName}", true, ThemeColorType.ListValue);
                            }
                        }
                        break;
                    }
                default:
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_TYPENOTFOUND"), true, ThemeColorType.Error, hardwareType);
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
