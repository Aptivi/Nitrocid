
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Kernel.Debugging;
using KS.Kernel.Events;
using KS.Misc.Threading;
using System;
using System.Threading;

namespace KS.ConsoleBase
{
    internal static class ConsoleResizeListener
    {
        private static int CurrentWindowWidth;
        private static int CurrentWindowHeight;
        private static readonly KernelThread ResizeListenerThread = new("Console Resize Listener Thread", true, PollForResize);

        private static void PollForResize()
        {
            try
            {
                while (ResizeListenerThread.IsAlive)
                {
                    Thread.Sleep(5);
                    if (CurrentWindowHeight != ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleWrapper.WindowWidth)
                    {
                        EventsManager.FireEvent("ResizeDetected", CurrentWindowWidth, CurrentWindowHeight, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
                        CurrentWindowWidth = ConsoleWrapper.WindowWidth;
                        CurrentWindowHeight = ConsoleWrapper.WindowHeight;
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
