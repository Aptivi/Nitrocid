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

using System;
using System.Collections;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Splash;
using Nitrocid.Base.Drivers.HardwareProber;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Users.Windows;
using Nitrocid.Base.Kernel.Events;

namespace Nitrocid.Base.Kernel.Hardware
{
    /// <summary>
    /// Hardware probe module
    /// </summary>
    public static class HardwareProbe
    {
        internal static IEnumerable? processors;
        internal static IEnumerable? pcMemory;
        internal static IEnumerable? hardDrive;
        internal static IEnumerable? graphics;

        /// <summary>
        /// Starts probing hardware
        /// </summary>
        public static void StartProbing()
        {
            // We will probe hardware
            EventsManager.FireEvent(EventType.HardwareProbing);
            try
            {
                if (!KernelPlatform.IsOnWindows() || KernelPlatform.IsOnWindows() && WindowsUserTools.IsAdministrator())
                {
                    processors = HardwareProberDriver.ProbeProcessor();
                    pcMemory = HardwareProberDriver.ProbePcMemory();
                    hardDrive = HardwareProberDriver.ProbeHardDrive();
                    graphics = HardwareProberDriver.ProbeGraphics();
                    DebugWriter.WriteDebug(DebugLevel.I, "Probe finished.");
                }
                else
                {
                    processors = Array.Empty<object>();
                    pcMemory = Array.Empty<object>();
                    hardDrive = Array.Empty<object>();
                    graphics = Array.Empty<object>();
                    SplashReport.ReportProgressWarning(LanguageTools.GetLocalized("NKS_KERNEL_HARDWARE_NEEDSELEVATION"));
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to probe hardware: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                KernelPanic.KernelError(KernelErrorLevel.F, true, 10L, LanguageTools.GetLocalized("NKS_KERNEL_HARDWARE_FAILED"), ex, ex.Message);
            }

            // Raise event
            EventsManager.FireEvent(EventType.HardwareProbed);
        }

    }
}
