
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

using KS.Drivers.HardwareProber;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Misc.Splash;

namespace KS.Kernel.Hardware
{
    /// <summary>
    /// Hardware list module
    /// </summary>
    public static class HardwareList
    {

        /// <summary>
        /// Lists simple information about hardware
        /// </summary>
        internal static void ListHardware()
        {
            // Some variables
            var processors = HardwareProbe.processors;
            var pcMemory = HardwareProbe.pcMemory;
            var graphics = HardwareProbe.graphics;
            var hardDrive = HardwareProbe.hardDrive;

            // We are checking to see if any of the probers reported a failure starting with CPU
            if (processors is null || processors is not null && EnumerableTools.CountElements(processors) == 0)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "CPU failed to probe.");
                SplashReport.ReportProgressError(Translate.DoTranslation("One or more of the CPU cores failed to probe. Showing information anyway..."));
            }

            // then RAM
            if (pcMemory is null || pcMemory is not null && EnumerableTools.CountElements(pcMemory) == 0)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "RAM failed to probe.");
                SplashReport.ReportProgressError(Translate.DoTranslation("One or more of the RAM chips failed to probe. Showing information anyway..."));
            }

            // then GPU
            if (graphics is null || graphics is not null && EnumerableTools.CountElements(graphics) == 0)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "GPU failed to probe.");
                SplashReport.ReportProgressError(Translate.DoTranslation("One or more of the graphics cards failed to probe. Showing information anyway..."));
            }

            // and finally HDD
            if (hardDrive is null || hardDrive is not null && EnumerableTools.CountElements(hardDrive) == 0)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "HDD failed to probe.");
                SplashReport.ReportProgressError(Translate.DoTranslation("One or more of the hard drives failed to probe. Showing information anyway..."));
            }

            // Print information about the probed hardware, starting from the CPU info
            HardwareProberDriver.ListHardware(processors, pcMemory, graphics, hardDrive);
        }

        /// <summary>
        /// Lists information about hardware
        /// </summary>
        /// <param name="HardwareType">Hardware type defined by Inxi.NET. If "all", prints all information.</param>
        public static void ListHardware(string HardwareType) =>
            HardwareProberDriver.ListHardware(HardwareType);

    }
}
