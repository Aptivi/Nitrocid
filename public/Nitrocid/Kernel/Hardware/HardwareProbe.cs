//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using InxiFrontend;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Splash;
using KS.Kernel.Events;
using KS.Kernel.Configuration;
using System.Collections;
using KS.Drivers.HardwareProber;

namespace KS.Kernel.Hardware
{
    /// <summary>
    /// Hardware probe module
    /// </summary>
    public static class HardwareProbe
    {
        internal static IEnumerable processors;
        internal static IEnumerable pcMemory;
        internal static IEnumerable hardDrive;
        internal static IEnumerable graphics;

        /// <summary>
        /// Probe the hardware quietly. This overrides the <see cref="VerboseHardwareProbe"/> flag.
        /// </summary>
        public static bool QuietHardwareProbe =>
            Config.MainConfig.QuietHardwareProbe;

        /// <summary>
        /// Makes the hardware prober a bit talkative
        /// </summary>
        public static bool VerboseHardwareProbe =>
            Config.MainConfig.VerboseHardwareProbe;

        /// <summary>
        /// Starts probing hardware
        /// </summary>
        public static void StartProbing()
        {
            // We will probe hardware
            EventsManager.FireEvent(EventType.HardwareProbing);
            try
            {
                InxiTrace.DebugDataReceived += WriteInxiDebugData;
                InxiTrace.HardwareParsed += WriteWhatProbed;
                processors = HardwareProberDriver.ProbeProcessor();
                pcMemory = HardwareProberDriver.ProbePcMemory();
                hardDrive = HardwareProberDriver.ProbeHardDrive();
                graphics = HardwareProberDriver.ProbeGraphics();
                DebugWriter.WriteDebug(DebugLevel.I, "Probe finished.");
                InxiTrace.DebugDataReceived -= WriteInxiDebugData;
                InxiTrace.HardwareParsed -= WriteWhatProbed;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to probe hardware: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                KernelPanic.KernelError(KernelErrorLevel.F, true, 10L, Translate.DoTranslation("There was an error when probing hardware: {0}"), ex, ex.Message);
            }

            // Raise event
            EventsManager.FireEvent(EventType.HardwareProbed);
        }

        /// <summary>
        /// Write Inxi.NET hardware parsing completion to debugger and, if quiet probe is disabled, the console
        /// </summary>
        private static void WriteWhatProbed(InxiHardwareType Hardware)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Hardware {0} ({1}) successfully probed.", Hardware, Hardware.ToString());
            if (!QuietHardwareProbe & VerboseHardwareProbe | SplashManager.EnableSplash)
                SplashReport.ReportProgress(Translate.DoTranslation("Successfully probed {0}."), 5, Hardware.ToString());
        }

        /// <summary>
        /// Write Inxi.NET debug data to debugger
        /// </summary>
        private static void WriteInxiDebugData(string Message, string PlainMessage) =>
            DebugWriter.WriteDebug(DebugLevel.I, PlainMessage);

    }
}
