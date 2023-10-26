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

using KS.ConsoleBase;
using KS.Drivers;
using KS.Drivers.Console;
using KS.Kernel.Debugging;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace KS.Kernel.Power
{
    internal static class PowerSignalHandlers
    {
        internal static List<PosixSignalRegistration> signalHandlers = new();
        internal static bool initialized = false;

        internal static void RegisterHandlers()
        {
            if (initialized)
                return;

            // Works on Windows and Linux
            signalHandlers.Add(PosixSignalRegistration.Create(PosixSignal.SIGINT, SigQuit));
            signalHandlers.Add(PosixSignalRegistration.Create(PosixSignal.SIGTERM, SigQuit));

            // Works on Linux only
            if (KernelPlatform.IsOnUnix())
            {
                signalHandlers.Add(PosixSignalRegistration.Create((PosixSignal)PowerSignals.SIGUSR1, SigReboot));
                signalHandlers.Add(PosixSignalRegistration.Create((PosixSignal)PowerSignals.SIGUSR2, SigReboot));
                ConsoleResizeListener.CurrentWindowWidth = ConsoleWrapper.WindowWidth;
                ConsoleResizeListener.CurrentWindowHeight = ConsoleWrapper.WindowHeight;
            }

            // Handle window change
            ConsoleResizeListener.usesSigWinch = KernelPlatform.IsOnUnix();
            if (ConsoleResizeListener.usesSigWinch)
                signalHandlers.Add(PosixSignalRegistration.Create((PosixSignal)PowerSignals.SIGWINCH, SigWindowChange));
            else
                ConsoleResizeListener.StartResizeListener();

            initialized = true;
        }

        internal static void DisposeHandlers()
        {
            foreach (var signalHandler in signalHandlers)
                signalHandler.Dispose();
        }

        private static void SigQuit(PosixSignalContext psc)
        {
            PowerManager.PowerManage(PowerMode.Shutdown);
            psc.Cancel = true;
        }

        private static void SigReboot(PosixSignalContext psc)
        {
            PowerManager.PowerManage(PowerMode.Reboot);
            psc.Cancel = true;
        }

        private static void SigWindowChange(PosixSignalContext psc)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "SIGWINCH recieved!");
            ConsoleResizeListener.ResizeDetected = true;
            var termDriver = DriverHandler.GetFallbackDriver<IConsoleDriver>();
            ConsoleResizeListener.CurrentWindowWidth = termDriver.WindowWidth;
            ConsoleResizeListener.CurrentWindowHeight = termDriver.WindowHeight;
            psc.Cancel = true;
        }
    }
}
