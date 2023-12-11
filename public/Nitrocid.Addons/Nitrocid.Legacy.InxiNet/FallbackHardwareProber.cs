//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using InxiFrontend;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers.HardwareProber;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Misc.Splash;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.ConsoleBase.Writers;

namespace Nitrocid.Legacy.InxiNet
{
    internal class FallbackHardwareProber : BaseHardwareProberDriver, IHardwareProberDriver
    {
        internal IEnumerable processors;
        internal IEnumerable pcMemory;
        internal IEnumerable graphics;
        internal IEnumerable hardDrive;

        public override string DriverName =>
            "Fallback";

        /// <inheritdoc/>
        public override IEnumerable ProbeGraphics()
        {
            if (graphics is not null)
                return graphics;
            var hwInfoBase = new Inxi(InxiHardwareType.Graphics);
            graphics = hwInfoBase.Hardware.GPU;
            return graphics;
        }

        /// <inheritdoc/>
        public override IEnumerable ProbeHardDrive()
        {
            if (hardDrive is not null)
                return hardDrive;
            var hwInfoBase = new Inxi(InxiHardwareType.HardDrive);
            hardDrive = hwInfoBase.Hardware.HDD;
            return hardDrive;
        }

        /// <inheritdoc/>
        public override IEnumerable ProbePcMemory()
        {
            if (pcMemory is not null)
                return pcMemory;
            var hwInfoBase = new Inxi(InxiHardwareType.PCMemory);
            pcMemory = new[] { hwInfoBase.Hardware.RAM };
            return pcMemory;
        }

        /// <inheritdoc/>
        public override IEnumerable ProbeProcessor()
        {
            if (processors is not null)
                return processors;
            var hwInfoBase = new Inxi(InxiHardwareType.Processor);
            processors = hwInfoBase.Hardware.CPU;
            return processors;
        }

        /// <inheritdoc/>
        public override void ListHardware() =>
            ListHardware(ProbeProcessor(), ProbePcMemory(), ProbeGraphics(), ProbeHardDrive());

        /// <inheritdoc/>
        public override void ListHardware(IEnumerable processors, IEnumerable memory, IEnumerable graphics, IEnumerable hardDrives)
        {
            // Verify the types
            if (processors is not Dictionary<string, Processor> procDict)
            {
                SplashReport.ReportProgress("CPU: " + Translate.DoTranslation("Failed to parse the CPU info. Ensure that it's a valid info list."));
                return;
            }
            if (memory is not PCMemory[] memList)
            {
                SplashReport.ReportProgress("RAM: " + Translate.DoTranslation("Failed to parse the RAM info. Ensure that it's a valid info list."));
                return;
            }
            if (graphics is not Dictionary<string, Graphics> gpuDict)
            {
                SplashReport.ReportProgress("GPU: " + Translate.DoTranslation("Failed to parse the GPU info. Ensure that it's a valid info list."));
                return;
            }
            if (hardDrives is not Dictionary<string, HardDrive> hddDict)
            {
                SplashReport.ReportProgress("HDD: " + Translate.DoTranslation("Failed to parse the HDD info. Ensure that it's a valid info list."));
                return;
            }

            // Print information about the probed hardware, starting from the CPU info
            foreach (string cpuName in procDict.Keys)
            {
                var ProcessorInfo = procDict[cpuName];
                SplashReport.ReportProgress("CPU: " + Translate.DoTranslation("Processor name:") + " {0}", cpuName);
                SplashReport.ReportProgress("CPU: " + Translate.DoTranslation("Processor clock speed:") + " {0}", ProcessorInfo.Speed);
                SplashReport.ReportProgress("CPU: " + Translate.DoTranslation("Processor bits:") + $" {ProcessorInfo.Bits}-bit");
            }
            SplashReport.ReportProgress("CPU: " + Translate.DoTranslation("Total number of processors:") + $" {Environment.ProcessorCount}", 3);

            // Print RAM info
            var mem = memList[0];
            SplashReport.ReportProgress("RAM: " + Translate.DoTranslation("Total memory:") + " {0}", 2, KernelPlatform.IsOnWindows() ? ((long)Math.Round(Convert.ToDouble(mem.TotalMemory) * 1024d)).SizeString() : mem.TotalMemory);

            // GPU info
            foreach (string GPUInfo in gpuDict.Keys)
            {
                var TargetGraphics = gpuDict[GPUInfo];
                SplashReport.ReportProgress("GPU: " + Translate.DoTranslation("Graphics card:") + " {0}", TargetGraphics.Name);
            }

            // Drive Info
            foreach (string DriveInfo in hddDict.Keys)
            {
                var TargetDrive = hddDict[DriveInfo];
                string DriveModel = TargetDrive.Vendor == "(Standard disk drives)" ? $" {TargetDrive.Model}" : $" {TargetDrive.Vendor} {TargetDrive.Model}";
                SplashReport.ReportProgress("HDD: " + Translate.DoTranslation("Disk model:") + " {0}", DriveModel);
                SplashReport.ReportProgress("HDD: " + Translate.DoTranslation("Disk size:") + " {0}", KernelPlatform.IsOnWindows() ? Convert.ToInt64(TargetDrive.Size).SizeString() : TargetDrive.Size);

                // Partition info
                foreach (string PartInfo in TargetDrive.Partitions.Keys)
                {
                    var TargetPart = TargetDrive.Partitions[PartInfo];
                    SplashReport.ReportProgress("HDD [{0}]: " + Translate.DoTranslation("Partition size:") + " {1}", TargetPart.ID, KernelPlatform.IsOnWindows() ? Convert.ToInt64(TargetPart.Size).SizeString() : TargetPart.Size);
                    SplashReport.ReportProgress("HDD [{0}]: " + Translate.DoTranslation("Partition filesystem:") + " {1}", TargetPart.ID, TargetPart.FileSystem);
                }
            }
        }

        /// <inheritdoc/>
        public override void ListHardware(string hardwareType)
        {
            string[] supportedTypes = ["CPU", "RAM", "HDD", "GPU"];
            if (hardwareType == "all")
            {
                foreach (string supportedType in supportedTypes)
                    ListHardwareInternal(supportedType);
            }
            else
                ListHardwareInternal(hardwareType);
        }

        /// <inheritdoc/>
        public override string ListDisks()
        {
            var hdds = ProbeHardDrive() as Dictionary<string, HardDrive>;
            var hardDrives = hdds.Keys.ToArray();
            for (int i = 0; i < hardDrives.Length; i++)
            {
                string hardDrive = hardDrives[i];
                TextWriterColor.Write($"- [{i + 1}] {hardDrive}");
            }
            return $"[{string.Join(", ", hardDrives)}]";
        }

        /// <inheritdoc/>
        public override string ListDiskPartitions(int diskIndex)
        {
            var hdds = ProbeHardDrive() as Dictionary<string, HardDrive>;
            var hardDrives = hdds.Keys.ToArray();
            if (diskIndex < hardDrives.Length)
            {
                // Get the drive index and get the partition info
                var parts = hdds[hardDrives[diskIndex]].Partitions;
                int partNum = 1;
                foreach (var part in parts.Values)
                {
                    // Write partition information
                    string id = part.ID;
                    string name = part.Name;
                    TextWriterColor.Write($"[{partNum}] {name}, {id}");
                    partNum++;
                }
                return $"[{string.Join(", ", parts.Keys)}]";
            }
            else
                TextWriterColor.Write(Translate.DoTranslation("Partition doesn't exist"));
            return "";
        }

        /// <inheritdoc/>
        public override string DiskInfo(int diskIndex)
        {
            var hdds = ProbeHardDrive() as Dictionary<string, HardDrive>;
            var hardDrives = hdds.Keys.ToArray();
            if (diskIndex < hardDrives.Length)
            {
                // Get the drive index and get the partition info
                var drive = hdds[hardDrives[diskIndex]];
                TextWriterColor.Write($"[{drive.ID}] {drive.Vendor} {drive.Model} - {drive.Size}, {drive.Speed}, {drive.Serial}");
                return $"[{drive.ID}] {drive.Vendor} {drive.Model} | {drive.Size}, {drive.Speed}, {drive.Serial}";
            }
            else
                TextWriterColor.Write(Translate.DoTranslation("Disk doesn't exist"));
            return "";
        }

        /// <inheritdoc/>
        public override string DiskPartitionInfo(int diskIndex, int diskPartitionIndex)
        {
            var hdds = ProbeHardDrive() as Dictionary<string, HardDrive>;
            var hardDrives = hdds.Keys.ToArray();
            if (diskIndex < hardDrives.Length)
            {
                // Get the drive index and get the partition info
                var partKeyValues = hdds[hardDrives[diskIndex]].Partitions;
                var parts = partKeyValues.Keys.ToArray();
                if (diskPartitionIndex < parts.Length)
                {
                    // Get the part index and get the partition info
                    var part = partKeyValues[parts[diskPartitionIndex]];

                    // Write partition information
                    string id = part.ID;
                    string name = part.Name;
                    string size = part.Size;
                    string used = part.Used;
                    string fileSystem = part.FileSystem;
                    TextWriterColor.Write($"[{diskPartitionIndex + 1}] {name}, {id} - {used} / {size}, {fileSystem}");
                    return $"[{diskPartitionIndex + 1}] {name}, {id} | {used} / {size}, {fileSystem}";
                }
                else
                    TextWriterColor.Write(Translate.DoTranslation("Partition doesn't exist"));
            }
            else
                TextWriterColor.Write(Translate.DoTranslation("Disk doesn't exist"));
            return "";
        }

        internal void ResetAll()
        {
            processors = null;
            graphics = null;
            hardDrive = null;
            pcMemory = null;
        }

        private void ListHardwareInternal(string hardwareType)
        {
            string[] supportedTypes = ["CPU", "RAM", "HDD", "GPU"];
            IEnumerable hardwareList = default;
            switch (hardwareType)
            {
                case "CPU":
                    hardwareList = ProbeProcessor();
                    break;
                case "RAM":
                    hardwareList = ProbePcMemory();
                    break;
                case "HDD":
                    hardwareList = ProbeHardDrive();
                    break;
                case "GPU":
                    hardwareList = ProbeGraphics();
                    break;
            }
            SeparatorWriterColor.WriteSeparator(hardwareType, true);
            DebugWriter.WriteDebug(DebugLevel.I, "Got hardware type {0}.", hardwareList is not null ? hardwareType : "unknown");
            if (supportedTypes.Contains(hardwareType) && (hardwareList is not null || EnumerableTools.CountElements(hardwareList) > 0))
                ListHardwareProperties(hardwareList, hardwareType);
            else
                TextWriters.Write(Translate.DoTranslation("Either the hardware type {0} is not probed, or is not valid."), true, KernelColorType.Error, hardwareType);
        }

        private static void ListHardwareProperties(IEnumerable hardwareList, string hardwareType)
        {
            if (hardwareList is null)
            {
                TextWriters.Write(Translate.DoTranslation("The hardware type {0} is not probed yet. If you're sure that it's probed, restart the kernel with debugging enabled."), true, KernelColorType.Error, hardwareType);
                return;
            }

            IDictionary FieldValueDict = hardwareList as IDictionary;
            if (FieldValueDict is not null)
            {
                foreach (string HardwareKey in FieldValueDict.Keys)
                {
                    TextWriters.Write("- {0}: ", true, KernelColorType.ListEntry, HardwareKey);
                    foreach (PropertyInfo HardwareValuePropertyInfo in FieldValueDict[HardwareKey].GetType().GetProperties())
                    {
                        TextWriters.Write("  - {0}: ", false, KernelColorType.ListEntry, HardwareValuePropertyInfo.Name);
                        if (hardwareType == "HDD" & HardwareValuePropertyInfo.Name == "Partitions")
                        {
                            TextWriterColor.Write();
                            IDictionary Partitions = HardwareValuePropertyInfo.GetValue(FieldValueDict[HardwareKey]) as IDictionary;
                            if (Partitions is not null)
                            {
                                foreach (string PartitionKey in Partitions.Keys)
                                {
                                    TextWriters.Write("    - {0}: ", true, KernelColorType.ListEntry, PartitionKey);
                                    foreach (PropertyInfo PartitionValuePropertyInfo in Partitions[PartitionKey].GetType().GetProperties())
                                    {
                                        TextWriters.Write("      - {0}: ", false, KernelColorType.ListEntry, PartitionValuePropertyInfo.Name);
                                        TextWriters.Write(Convert.ToString(PartitionValuePropertyInfo.GetValue(Partitions[PartitionKey])), true, KernelColorType.ListValue);
                                    }
                                }
                            }
                            else
                            {
                                TextWriters.Write(Translate.DoTranslation("Partitions not parsed to list."), true, KernelColorType.Error);
                            }
                        }
                        else if (hardwareType == "CPU" & HardwareValuePropertyInfo.Name == "Flags")
                        {
                            TextWriters.Write(string.Join(", ", HardwareValuePropertyInfo.GetValue(FieldValueDict[HardwareKey]) as string[]), true, KernelColorType.ListValue);
                        }
                        else
                        {
                            TextWriters.Write(Convert.ToString(HardwareValuePropertyInfo.GetValue(FieldValueDict[HardwareKey])), true, KernelColorType.ListValue);
                        }
                    }
                }
            }
            else
            {
                foreach (var hardware in hardwareList)
                {
                    foreach (FieldInfo HardwareFieldInfo in hardware.GetType().GetFields())
                    {
                        TextWriters.Write("- {0}: ", false, KernelColorType.ListEntry, HardwareFieldInfo.Name);
                        TextWriters.Write(Convert.ToString(HardwareFieldInfo.GetValue(hardware)), true, KernelColorType.ListValue);
                    }
                }
            }
        }

    }
}
