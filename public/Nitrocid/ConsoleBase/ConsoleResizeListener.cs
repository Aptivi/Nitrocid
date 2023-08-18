
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

using KS.Drivers;
using KS.Drivers.Console;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Kernel.Events;
using KS.Kernel.Threading;
using KS.Misc.Screensaver;
using System;
using System.Threading;

namespace KS.ConsoleBase
{
    /// <summary>
    /// The console resize listener module
    /// </summary>
    public static class ConsoleResizeListener
    {
        private static int CurrentWindowWidth;
        private static int CurrentWindowHeight;
        private static readonly KernelThread ResizeListenerThread = new("Console Resize Listener Thread", true, PollForResize) { isCritical = true };
        private static bool ResizeDetected;

        /// <summary>
        /// This property checks to see if the console has been resized since the last time it has been called or the listener has started.
        /// </summary>
        /// <param name="reset">Reset the resized value once this is called</param>
        public static bool WasResized(bool reset = true)
        {
            if (ResizeDetected)
            {
                // The console has been resized.
                ResizeDetected = !reset;
                return true;
            }
            return false;
        }

        private static void PollForResize()
        {
            try
            {
                var termDriver = DriverHandler.GetDriver<IConsoleDriver>("Default");
                while (!Flags.KernelShutdown)
                {
                    SpinWait.SpinUntil(() => Flags.KernelShutdown, 100);

                    // We need to call the WindowHeight and WindowWidth properties on the Terminal console driver, because
                    // this polling works for all the terminals. Other drivers that don't use the terminal may not even
                    // implement these two properties.
                    if (CurrentWindowHeight != termDriver.WindowHeight | CurrentWindowWidth != termDriver.WindowWidth)
                    {
                        ResizeDetected = true;
                        DebugWriter.WriteDebug(DebugLevel.W, "Console resize detected! Old width x height: {0}x{1} | New width x height: {2}x{3}", CurrentWindowWidth, CurrentWindowHeight, termDriver.WindowWidth, termDriver.WindowHeight);
                        DebugWriter.WriteDebug(DebugLevel.W, "Userspace application will have to call Resized to set ResizeDetected back to false.");
                        EventsManager.FireEvent(EventType.ResizeDetected, CurrentWindowWidth, CurrentWindowHeight, termDriver.WindowWidth, termDriver.WindowHeight);
                        CurrentWindowWidth = termDriver.WindowWidth;
                        CurrentWindowHeight = termDriver.WindowHeight;

                        // Also, tell the screensaver application to refresh itself
                        if (ScreensaverManager.InSaver)
                            ScreensaverDisplayer.displayingSaver.ScreensaverResizeSync();
                    }
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to detect console resize: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }

        internal static void StartResizeListener()
        {
            CurrentWindowWidth = ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleWrapper.WindowHeight;
            if (!ResizeListenerThread.IsAlive)
                ResizeListenerThread.Start();
        }
    }
}
