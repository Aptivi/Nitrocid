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

using Nitrocid.Base.Kernel;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Nitrocid.Base.Kernel.Power
{
    internal static class PowerSignalHandlers
    {
        internal static List<PosixSignalRegistration> signalHandlers = [];
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
            }
            initialized = true;
        }

        internal static void DisposeHandlers()
        {
            foreach (var signalHandler in signalHandlers)
                signalHandler.Dispose();
            initialized = false;
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
    }
}
